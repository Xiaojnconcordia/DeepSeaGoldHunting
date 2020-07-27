using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusMovement : MonoBehaviour
{
       
    private float yRange = 95.0f;
    //private float zRange = 95.0f;
    private int level = 1;
    private float levelDuration = 8.0f;
    private float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveOctopus();
    }

    private void moveOctopus()
    {
        Vector3 startPos = new Vector3(transform.position.x, yRange, transform.position.z);
        Vector3 endPos = new Vector3(transform.position.x, -yRange, transform.position.z);
        // https://docs.unity3d.com/ScriptReference/Mathf.PingPong.html
        float t = Mathf.PingPong(Time.time, 10.0f);
        transform.position = Vector2.Lerp(startPos, endPos, t * 0.1f * level);
        // If we multiply t with level in Lerp, as level increases, Octopus interpolate faster
        currentTime += Time.deltaTime;
        if (currentTime >= levelDuration)
        {
            level++;
            currentTime = 0;
        }
    }
}
