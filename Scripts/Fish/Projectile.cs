using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25.0f;
    private float xRange = 93.0f;
    private float zRange = 93.0f;
    private float yRange = 100.5f;
    // Start is called before the first frame update

    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        // destroy outranged projectiles
        if (transform.position.x > xRange
            || transform.position.x < -xRange
            || transform.position.y > yRange
            || transform.position.y < -yRange
            || transform.position.z > zRange
            || transform.position.z < -zRange)
        {
            Destroy(gameObject);
        }
    }
}
