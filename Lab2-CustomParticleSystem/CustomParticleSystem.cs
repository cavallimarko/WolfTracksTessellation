using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomParticleSystem : MonoBehaviour
{
    public Mesh Quad;
    public Material material;
    private Camera mainCamera;
    public bool running = false;
    public bool drawInstancedParticlesBool=false;
    public int size = 200;
    List<Matrix4x4> matrices;
    List<Particle> particles;
    [Header("Particle Settings")]
    public float maxLifetime;
    private Vector3 startPosition;
    private Vector3 startRotation;
    public Vector3 startSpeed;
    public int spawnPerTick = 10;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        matrices = new List<Matrix4x4>(1023);
        particles = new List<Particle>(size);
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (particles.Count < size)
            {
                for (int i = 0; i < spawnPerTick; i++)
                {
                    particles.Add(new Particle(maxLifetime, transform.position, startRotation, startSpeed));
                }
               
            }
            foreach (var particle in particles)
            {
                particle.lifetime += Time.deltaTime;
                if (particle.lifetime < particle.maxLiftetime)
                {
                    updateParticle(particle);
                    drawParticle(particle);
                }
                else
                {
                    particle.reset(maxLifetime, transform.position, startRotation, startSpeed);
                }
            }
            if (drawInstancedParticlesBool)
            { 
                drawInstancedParticles();
            }
        }
    }

    
    private void drawInstancedParticles()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {         
            count++;
            if (count == 1023)
            {
                drawInstanced();
                count = 0;
            }
        }
        drawInstanced();
    }
    private void drawInstanced()
    {
        if (matrices.Count < 1024)
        {
            Graphics.DrawMeshInstanced(Quad, 0, material, matrices);
            matrices.Clear();
        }
    }
    private void updateParticle(Particle particle)
    {
        particle.position += particle.speed * Time.deltaTime;
    }

    private void drawParticle(Particle particle)
    {
        var lookPos = -mainCamera.transform.position + particle.position;
        //lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        if (drawInstancedParticlesBool)
        {
            matrices.Add(Matrix4x4.TRS(particle.position, rotation, Vector3.one));
            if (matrices.Count == 1023)
            {
                drawInstanced();
            }
        }
        else
        {
            Graphics.DrawMesh(Quad, particle.position, rotation, material, 0, null,0, null, false, false,true);
        }
        
    }
    
    
    [ContextMenu("StartParticleSystem")]
    public void StartSystem()
    {
        running= true;
    }
    [ContextMenu("StopParticleSystem")]
    public void StopSystem()
    {
        running = false;
    }
    private void drawInstancedArray()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector3 position = new Vector3(i, j, 0);
                var lookPos = -mainCamera.transform.position + position;
                //lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);

                matrices.Add(Matrix4x4.TRS(position, rotation, Vector3.one));
                //Graphics.DrawMesh(Quad, position, rotation, material, 0, null,0, null, false, false,true);
                count++;
                if (count == 1023)
                {
                    drawInstanced();
                    count = 0;
                }
            }

        }
        drawInstanced();

    }
}
