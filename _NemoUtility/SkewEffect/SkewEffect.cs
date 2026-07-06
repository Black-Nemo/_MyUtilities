using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Skew Effect")]
[RequireComponent(typeof(Graphic))]
public class SkewEffect : BaseMeshEffect
{
    [Tooltip("Sağa veya sola eğim açısı (Brawl Stars tarzı için genelde -10 ile -15 arası idealdir)")]
    public float skewAngleX = -12f;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        UIVertex vert = new UIVertex();
        float tanAngle = Mathf.Tan(skewAngleX * Mathf.Deg2Rad);

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vert, i);
            // Y eksenindeki pozisyona göre X eksenini kaydır (Eğme işlemi)
            vert.position.x += vert.position.y * tanAngle;
            vh.SetUIVertex(vert, i);
        }
    }
}