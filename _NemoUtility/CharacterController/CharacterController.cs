using UnityEngine;

namespace NemoUtility
{
    [RequireComponent(typeof(BoxCollider))]
    public class CharacterController : MonoBehaviour
    {
        [Header("SpeedCollisionControl")]
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private int _rayCountPerSide = 3; // her eksende kaç ray atsın
        [SerializeField] private float _skinWidth = 0.01f;
        private BoxCollider _collider; // karakterin BoxCollider'ı
        public BoxCollider GetBoxCollider { get { return _collider; } }
        public float HeadPoundingRayDistance { get { return _headPoundingRayDistance; } set { Debug.Log(_headPoundingRayDistance); _headPoundingRayDistance = value; } }


        [Header("GroundCollisionControl")]
        [SerializeField] private float _groundRayDistance = 1f;
        [SerializeField] private LayerMask _groundCheckLayerMask;

        [Header("GroundCollisionControl")]
        [SerializeField] private float _headPoundingRayDistance = 1f;
        [SerializeField] private LayerMask _headPoundingCheckLayerMask;



        private Vector3 _moveDirection;
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            Vector3 finalMove = Vector3.zero;
            finalMove.y = _moveDirection.y;

            Vector3 moveX = new Vector3(_moveDirection.x, 0, 0);
            Vector3 moveZ = new Vector3(0, 0, _moveDirection.z);

            /*
            // X
            if (Mathf.Abs(_moveDirection.x) > 0.001f && CheckCollision(_moveDirection, Vector3.right))
            {
                finalMove.x = 0;
            }

            // Z
            if (Mathf.Abs(_moveDirection.z) > 0.001f && CheckCollision(_moveDirection, Vector3.forward))
            {
                finalMove.z = 0;
            }*/
            // +Y
            if (_moveDirection.y > 0.001f && IsHeadPounding())
            {
                finalMove.y = 0f;
            }

            if (IsGrounded() && _moveDirection.y < 0f)
            {
                finalMove.y = 0;
            }

            if (!CheckDirectionalCollision(moveX))
                finalMove += moveX;

            if (!CheckDirectionalCollision(moveZ))
                finalMove += moveZ;

            transform.position += finalMove;
            _moveDirection = Vector3.zero;
        }

        private bool CheckCollision(Vector3 move, Vector3 axis)
        {
            Bounds bounds = _collider.bounds;
            bounds.Expand(-_skinWidth * 2f);

            int rayCount = _rayCountPerSide;
            float rayLength = Mathf.Abs(Vector3.Dot(move, axis)) + _skinWidth;
            Vector3 direction = axis.normalized * Mathf.Sign(Vector3.Dot(move, axis));

            for (int i = 0; i < rayCount; i++)
            {
                Vector3 rayOrigin = bounds.center;

                if (axis == Vector3.right || axis == Vector3.left)
                {
                    rayOrigin.x = direction.x > 0 ? bounds.max.x : bounds.min.x;
                    rayOrigin.y = bounds.min.y + (bounds.size.y / (rayCount - 1)) * i;
                    rayOrigin.z = bounds.center.z;
                }
                else if (axis == Vector3.up || axis == Vector3.down)
                {
                    rayOrigin.y = direction.y > 0 ? bounds.max.y : bounds.min.y;
                    rayOrigin.x = bounds.min.x + (bounds.size.x / (rayCount - 1)) * i;
                    rayOrigin.z = bounds.center.z;
                }
                else if (axis == Vector3.forward || axis == Vector3.back)
                {
                    rayOrigin.z = direction.z > 0 ? bounds.max.z : bounds.min.z;
                    rayOrigin.y = bounds.min.y + (bounds.size.y / (rayCount - 1)) * i;
                    rayOrigin.x = bounds.center.x;
                }

                Debug.DrawRay(rayOrigin, direction * rayLength, Color.red);

                if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, rayLength, _layerMask) &&
                    !HasParent(hit.collider.gameObject, gameObject))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckDirectionalCollision(Vector3 move)
        {
            Bounds bounds = _collider.bounds;
            bounds.Expand(-_skinWidth * 2f);

            Vector3 direction = move.normalized;
            float rayLength = move.magnitude + _skinWidth;

            int rayCount = _rayCountPerSide;

            for (int i = 0; i < rayCount; i++)
            {
                for (int j = 0; j < rayCount; j++)
                {
                    for (int k = 0; k < rayCount; k++)
                    {
                        Vector3 rayOrigin = new Vector3(
                            Mathf.Lerp(bounds.min.x, bounds.max.x, i / (float)(rayCount - 1)),
                            Mathf.Lerp(bounds.min.y, bounds.max.y, j / (float)(rayCount - 1)),
                            Mathf.Lerp(bounds.min.z, bounds.max.z, k / (float)(rayCount - 1))
                        );

                        Debug.DrawRay(rayOrigin, direction * rayLength, Color.green);

                        if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, rayLength, _layerMask) &&
                            !HasParent(hit.collider.gameObject, gameObject))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Move(Vector3 direction)
        {
            _moveDirection += direction;
        }

        private bool HasParent(GameObject child, GameObject targetParent)
        {
            Transform current = child.transform.parent;
            while (current != null)
            {
                if (current.gameObject == targetParent)
                    return true;
                current = current.parent;
            }
            return false;
        }

        public bool IsGrounded()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _groundRayDistance, _groundCheckLayerMask) && !HasParent(hit.collider.transform.gameObject, gameObject))
            {
                return true;
            }
            return false;
        }

        public bool IsHeadPounding()
        {
            //return CheckCollision(direction * Time.deltaTime, Vector3.up);
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, _headPoundingRayDistance, _headPoundingCheckLayerMask) && !HasParent(hit.collider.transform.gameObject, gameObject))
            {
                return true;
            }
            return false;
        }


        void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, transform.position + (-transform.up * _groundRayDistance), Color.red);
            Debug.DrawLine(transform.position, transform.position + (transform.up * _headPoundingRayDistance), Color.blue);
        }
    }
}