using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

class Particle 
{
    public float lifetime;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 speed;

    public float maxLiftetime;
    public Vector3 startPosition;
    public Vector3 startRotation;
    public Vector3 startSpeed;

    public Particle(float maxLiftetime, Vector3 startPosition, Vector3 startRotation, Vector3 startSpeed)
    {
        this.maxLiftetime = maxLiftetime;
        this.lifetime = 0;
        this.startPosition = startPosition;
        this.startRotation = startRotation;
        if (startSpeed.magnitude > 0)
        {
            this.startSpeed = startSpeed;
        }
        else
        {
            this.startSpeed = Random.insideUnitSphere.normalized;
        }
        
        this.position = startPosition;
        this.rotation = startRotation;
        this.speed = this.startSpeed;
    }

    internal void reset()
    {
        this.lifetime = 0;
        this.position = startPosition;
        this.rotation = startRotation;
        this.speed = startSpeed;
    }

    internal void reset(float maxLiftetime, Vector3 startPosition, Vector3 startRotation, Vector3 startSpeed)
    {
        this.maxLiftetime = maxLiftetime;
        this.lifetime = 0;
        this.startPosition = startPosition;
        this.startRotation = startRotation;
        if (startSpeed.magnitude > 0)
        {
            this.startSpeed = startSpeed;
        }
        else
        {
            this.startSpeed = Random.insideUnitSphere.normalized;
        }
        this.position = startPosition;
        this.rotation = startRotation;
        this.speed = this.startSpeed;
    }
}
