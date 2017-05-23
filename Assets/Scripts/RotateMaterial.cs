﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMaterial : MonoBehaviour {

    public float rotateSpeed = 30f;
    public void Update()
    {
        // Construct a rotation matrix and set it for the shader
        Quaternion rot = Quaternion.Euler(0, 0, Time.time * rotateSpeed);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        GetComponent<Renderer>().material.SetMatrix("_TextureRotation", m);
    }
}
