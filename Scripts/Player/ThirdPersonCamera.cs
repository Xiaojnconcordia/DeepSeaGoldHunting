using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float sensitivity = 4f;
    
    public Transform shotTarget; //at what point the camera will shoot
    public Transform player;
    float mouseX, mouseY;    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraControll();      
    }


    void CameraControll()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -45, 70); //limit the camera rotation angle

        transform.LookAt(shotTarget);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shotTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            
        }
        else
        {
            shotTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            player.rotation = Quaternion.Euler(0, mouseX, 0);
        }


    }


}
