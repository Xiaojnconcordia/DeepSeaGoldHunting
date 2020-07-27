using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    [SerializeField]
    private GameObject PlayerPrefab;

    public Vector3 offset = new Vector3(0, 30, 0);

    void Start()
    {
              
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerPrefab.transform.position + offset;
    }
}
