using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTracks : MonoBehaviour
{
    private RenderTexture splatMap;
    public Shader drawShader;

    private Material snowMaterial;
    private Material drawMaterial;
    public GameObject terrain;
    public Transform[] paws;
    public float rayCastLength=0.1f;
    RaycastHit groundhit;
    int layerMask;


    private int clipIndex;
    public float pitchMin, pitchMax, volumeMin, volumeMax;
    public AudioClip[] footStepsClipArray;
    private AudioSource[] sources;
    public int audioSourcesPoolCount = 4;
    [Range(0,10)]
    public float brushSize;
    [Range(0, 1)]
    public float brushStrength;
    public bool GUI_on = false;
    public int GUI_Scale = 1;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        

        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
        sources = new AudioSource[audioSourcesPoolCount];
        int i = 0;

        while (i < sources.Length)

        {

            GameObject child = new GameObject("AudioSource");

            child.transform.parent = gameObject.transform;

            sources[i] = child.AddComponent<AudioSource>();
            

            i++;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < paws.Length; i++)
        {
            if (Physics.Raycast(paws[i].position,Vector3.down,out groundhit, rayCastLength, layerMask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(groundhit.textureCoord.x, groundhit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
                float speed = GetComponent<WolfMoveController>().currentSpeeed;
                PlayRandom(footStepsClipArray, speed,i);
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
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            GUI_Scale += 1;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            GUI_Scale -= 1;
        }
    }
    void PlayRandom(AudioClip[] clipArray, float volumeModifier,int pawIndex)
    {
        clipIndex = RepeatCheck(clipIndex, clipArray.Length);
        
       // foreach (AudioSource source in sources)
       // {
            if (!sources[pawIndex].isPlaying)
            {
                sources[pawIndex].pitch = Random.Range(pitchMin, pitchMax);
                sources[pawIndex].volume = Random.Range(volumeMin, volumeMax) * volumeModifier;
                sources[pawIndex].PlayOneShot(clipArray[clipIndex]);
            //break;
                return;
            }
        if (!sources[pawIndex+4].isPlaying)
        {
            sources[pawIndex+4].pitch = Random.Range(pitchMin, pitchMax);
            sources[pawIndex+4].volume = Random.Range(volumeMin, volumeMax) * volumeModifier;
            sources[pawIndex+4].PlayOneShot(clipArray[clipIndex]);
            //break;
        }
        // }


    }
    int RepeatCheck(int previousIndex, int range)
    {
        if (range == 0)
        {
            return 0;
        }
        int index = Random.Range(0, range);

        while (index == previousIndex)
        {
            index = Random.Range(0, range);
        }
        return index;
    }
    private void OnGUI()
    {
        if (GUI_on)
        {
            GUI.DrawTexture(new Rect(0, 0, 256*GUI_Scale, 256*GUI_Scale), splatMap, ScaleMode.ScaleToFit, false, 1);
        }

    }
}
