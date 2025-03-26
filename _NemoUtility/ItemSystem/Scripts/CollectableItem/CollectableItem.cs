using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableItem : MonoBehaviour, ICollectable
{
    public Item Item;
    public int ItemCount;

    [SerializeField] private Image itemImage;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private AudioClip collectAudioClip;

    public List<GameObject> DestroyObjects;

    [HideInInspector] public bool CanCollectable;
    private void Start()
    {
        itemImage.sprite = Item.Sprite;
    }
    private void LateUpdate()
    {
        itemImage.transform.Rotate(0, 30 * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        var hitLayerMask = 1 << other.gameObject.layer;
        if (hitLayerMask == layerMask)
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
            CanCollectable = true;
        }
    }

    public void Collect(Inventory inventory)
    {
    }

    public void TouchEnter(Inventory inventory)
    {
        if (CanCollectable)
        {
            foreach (var item in DestroyObjects)
            {
                Destroy(item);
            }
            SoundsManager.Instance.InstantiateSound(collectAudioClip, transform.position, 1, 0.5f);
            inventory.AddItem(Item, ItemCount);
            Destroy(gameObject);
        }
    }

    public void TouchExit(Inventory inventory)
    {
    }
}
