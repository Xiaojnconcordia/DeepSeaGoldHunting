using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archimedes : MonoBehaviour
{
    public float waterLevel = 100;    
    public float floatHeight = 2; // how high the boat will float
    public float bounceDamp = 0.05f;    
    
    public Vector3 buoyancyCenterOffset;

    //the transform.position.y when the object just totally immerse in water
    // when an object sinks down, before reach this level, the buoyance is increasing 
    // because more and more part of the object immerse into water
    // after the object reaches this level, no matter how deep the object gets, the buoyance remains the same
    // because the volume immersed in water does not change from this point.
    private float allInWaterLevel = 98; 
    private float objectHeight; // how high the object is
    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLiftForce;
    private Rigidbody floatRb;

    // Start is called before the first frame update
    void Start()
    {
        floatRb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        actionPoint = transform.position + transform.TransformDirection(buoyancyCenterOffset);
        if (actionPoint.y > waterLevel)
            forceFactor = 0; //no buoyance above water
        else if (actionPoint.y > allInWaterLevel)
        {
            // before totally immerse into water, the buoyance gets bigger when the object gets deeper
            forceFactor = 1.0f - ((actionPoint.y - waterLevel) / floatHeight);
        }
        else // after totally immerse into water, the buoyance remains the same
            forceFactor = 1.0f - ((allInWaterLevel - waterLevel) / floatHeight); 

        if(forceFactor > 0)
        {
            upLiftForce = -Physics.gravity * (forceFactor - floatRb.velocity.y * bounceDamp);
            floatRb.AddForceAtPosition(upLiftForce, actionPoint);
        }
        
    }
}
