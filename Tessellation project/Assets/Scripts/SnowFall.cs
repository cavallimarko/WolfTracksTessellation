using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFall : MonoBehaviour
{
    public Shader snowfallShader;
    private Material snowFallMaterial;
    private MeshRenderer meshRenderer;
    [Range(0.001f,0.3f)]
    public float flakeAmount;
    [Range(0, 1)]
    public float flakeOpacity;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        snowFallMaterial = new Material(snowfallShader);
    }

    // Update is called once per frame
    void Update()
    {
        snowFallMaterial.SetFloat("_FlakeAmount", flakeAmount);

        snowFallMaterial.SetFloat("_FlakeOpacity", flakeOpacity);
        RenderTexture snow = (RenderTexture)meshRenderer.material.GetTexture("_Splat");
        RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0,RenderTextureFormat.ARGBFloat);
        Graphics.Blit(snow, temp, snowFallMaterial);
        Graphics.Blit(temp,snow);
        meshRenderer.material.SetTexture("_Splat",snow);
        RenderTexture.ReleaseTemporary(temp);

    }
}
