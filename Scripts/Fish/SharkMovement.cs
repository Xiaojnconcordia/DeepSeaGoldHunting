using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    //move variables
    float wanderTimePassed = 4;
    float delay = 4;
    public float wanderSpeed = 10.0f;
    Quaternion lookWhereYoureGoing;    
    Vector3 goalFacing;
    float rotationDegreesPerSecond = 240;

    private float xRange = 90.0f;
    private float zRange = 90.0f;
    private float speedLevel = 1f;
    private float levelDuration = 8.0f;
    private float currentTime;
    private float directionX;
    private float directionZ;    
    Vector3 wanderDirection;
    public bool isFollowingProjectile;
    public bool isFollowingPlayer;


    //follow variables    
    private GameObject mTarget;
    [SerializeField]
    private GameObject mPlayer;
    [SerializeField]
    float mFollowSpeed = 5f;
    [SerializeField]
    float mFollowRange = 16f;
    [SerializeField]
    float mFollowPlayerRange;
    float mArriveThreshold = 5;
    private PlayerMovement playerMovementScript;
    private Rigidbody sharkRb;
    //Rigidbody sharkRb;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindWithTag("Player");
        playerMovementScript = mPlayer.GetComponent<PlayerMovement>();
        sharkRb = GetComponent<Rigidbody>();
        //shark move horizontally facing a random direction in x-z plane  
        //directionX = Random.Range(-1.0f, 1.0f);
        //directionZ = Random.Range(-1.0f, 1.0f);
        //wanderDirection = new Vector3(directionX, 0, directionZ);


    }

    // Update is called once per frame
    void Update()
    {

        moveShark();
        FollowFigurine();
        FollowPlayer();
    }


    private void moveShark()
    {
        
        if (isFollowingPlayer == false && isFollowingProjectile == false)
        {
            wanderTimePassed += Time.deltaTime;
            if (wanderTimePassed > delay)
            {
                wanderDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                wanderTimePassed = 0;
            }
            goalFacing = wanderDirection.normalized;
            lookWhereYoureGoing = Quaternion.LookRotation(goalFacing, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookWhereYoureGoing, rotationDegreesPerSecond * Time.deltaTime);

            GetComponent<Rigidbody>().velocity = (wanderDirection.normalized * wanderSpeed * speedLevel);


            currentTime += Time.deltaTime;
            if (currentTime >= levelDuration)
            {
                speedLevel += 0.1f;
                currentTime = 0;
            }

            // constrain the shark position
            // leaves one side, return on the opposite side. 
            // line from exit to reentry point passes through the center of cube
            // if exits at (x, y, z), should reenter at (-x, -y, -z)
            if (transform.position.x > xRange)
            {
                transform.position = new Vector3(-xRange, transform.position.y, -transform.position.z);
            }
            if (transform.position.x < -xRange)
            {
                transform.position = new Vector3(xRange, transform.position.y, -transform.position.z);
            }

            if (transform.position.z > zRange)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, -zRange);
            }
            if (transform.position.z < -zRange)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, zRange);
            }
        }        
    }

    private void FollowFigurine()
    {
        // follow the figurine
        mTarget = GameObject.FindWithTag("Figurine");
        if (mTarget != null)
        {            
            float distanceBetweenSharkAndFigurine = (mTarget.transform.position - transform.position).magnitude;
            if (distanceBetweenSharkAndFigurine <= mFollowRange)
            {
                isFollowingProjectile = true;
                transform.LookAt(mTarget.transform);
                sharkRb.velocity = (transform.forward * mFollowSpeed);
            }
            else
            {
                isFollowingProjectile = false;
            }                
        }
        else
        {
            isFollowingProjectile = false;
        }   
    }

    private void FollowPlayer()
    {
        // player shoot figurine to lure shark away, so shark should chase figurine as priority
        if (isFollowingProjectile == false)
        {
            if (mPlayer != null)
            {
                //if the player carrys some gold, then after a limited time, the sharks will follow the player
                if ((playerMovementScript.hasGold1 ||
                    playerMovementScript.hasGold2 ||
                    playerMovementScript.hasGold10
                    ) &&
                playerMovementScript.mInvincible == false &&
                playerMovementScript.carryingGoldTime > playerMovementScript.attackLimitTime)
                {

                    isFollowingPlayer = true;                    
                    transform.LookAt(mPlayer.transform);
                    sharkRb.velocity = (transform.forward * mFollowSpeed);

                }
                else
                {
                    isFollowingPlayer = false;
                }
            }
        }       
    }
}
