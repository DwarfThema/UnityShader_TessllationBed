using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithObject : MonoBehaviour
{
    private RenderTexture splatmap;
    public Shader drawShader;
    private Material drawMaterial;
    private Material myMaterial;
    public GameObject _terrain;
    public Transform _object;
    RaycastHit _groundHit;
    int _layerMask;

    [Range(0, 500)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;


    // Start is called before the first frame update
    void Start()
    {
        _object = this.gameObject.transform;
        _layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        myMaterial = _terrain.GetComponent<MeshRenderer>().material;
        myMaterial.SetTexture("_Splat", splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(_object.position, -Vector3.up, out _groundHit, 1f, _layerMask))
        {
            drawMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));
            drawMaterial.SetFloat("_Strength", _brushStrength);
            drawMaterial.SetFloat("_Size", _brushSize);
            RenderTexture temp = RenderTexture.GetTemporary(splatmap.width, splatmap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatmap, temp);
            Graphics.Blit(temp, splatmap, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
