using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    
    private GameObject mTarget;
    private GameObject mPlayer;
    [SerializeField]
    float mFollowSpeed;
    [SerializeField]
    float mFollowRange;
    float mArriveThreshold = 0.05f;

    private PlayerMovement playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindWithTag("Player");
        //playerControllerScript = mPlayer.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        mTarget = GameObject.FindWithTag("Figurine");
        if (mTarget != null)
        {            
            Vector3 direction = mTarget.transform.position - transform.position;
            if (direction.magnitude <= mFollowRange)
            {
                if (direction.magnitude > mArriveThreshold)
                {
                    transform.Translate(direction.normalized * mFollowSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.position = mTarget.transform.position;
                }
            }
        }

        if (playerMovementScript.carryingGoldTime > playerMovementScript.attackLimitTime)
        {
            //if the player carrys a gold after a limited time, the sharks will follow the player
            Vector3 direction = mPlayer.transform.position - transform.position;
            transform.Translate(direction.normalized * mFollowSpeed * Time.deltaTime, Space.World);
        }
    }
}
