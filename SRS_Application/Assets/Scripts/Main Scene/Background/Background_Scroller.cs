using System.Threading;
using UnityEngine;

public class Background_Scroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float speed = 0.5f;
    private float offset;
    private Material mat;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    void Update()
    {
        offset += (Time.deltaTime * speed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
