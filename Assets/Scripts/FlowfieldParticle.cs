﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowfieldParticle : MonoBehaviour
{
    public float moveSpeed;
    public int audioBand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void ApplyRotation(Vector3 rotation, float rotateSpeed)
    {
        Quaternion targetRot = Quaternion.LookRotation(rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            targetRot, 
            rotateSpeed * Time.deltaTime);
    }
}
