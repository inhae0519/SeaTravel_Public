using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody Rigidbody;

    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate()
    {
        Rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        
        float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            Rigidbody.AddForceAtPosition(new Vector3(0f , Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), 
                transform.position, ForceMode.Acceleration);
            Rigidbody.AddForce(-Rigidbody.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
            Rigidbody.AddTorque(-Rigidbody.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime),
                ForceMode.VelocityChange);
            
            
        }
    }
}
