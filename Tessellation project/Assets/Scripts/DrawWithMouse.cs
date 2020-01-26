using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public Camera mainCamera;
    private RenderTexture splatMap;

    public Shader drawShader;

    private Material snowMaterial;
    private Material drawMaterial;
    private RaycastHit hit;
    [Range(1,500)]
    public float brushSize;
    [Range(0,1)]
    public float brushStrength;
    bool GUI_on = false;
    // Start is called before the first frame update
    void Start()
    {
        drawMaterial=new Material(drawShader);
        drawMaterial.SetVector("_Color",Color.red);

        snowMaterial=GetComponent<MeshRenderer>().material;
        splatMap=new RenderTexture(1024,1024,0,RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat",splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            splatMap.Release();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GUI_on = !GUI_on;
        }
    }
    private void OnGUI()
    {
        if (GUI_on)
        {
            GUI.DrawTexture(new Rect(0, 0, 256, 256), splatMap, ScaleMode.ScaleToFit, false, 1);
        }
        
    }
}
