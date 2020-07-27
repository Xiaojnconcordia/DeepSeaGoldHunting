using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // movement variables
    public float speed = 50.0f;
    public float originalSpeed = 50.0f;
    public float swimUpDownForce = 5;
    private float waterDrag = 1.5f;    
    private float keyDelay = 1.0f;
    private float timePassed = 1.0f;
    private Rigidbody playerRb;
    private bool speedUpModeOn = false;
    private bool isInWater = true;
    private float xRange = 93.0f;
    private float zRange = 93.0f;
    private float yRange = 98;
    private float seaLevel = 100;
    private float rotation_left = -90f;
    private float rotation_right = 90f;
    private float rotation_backward = 180f;
    private float rotation_downward = 180f;
    private float rotation_corner = 45f;
    public Transform playerChild;
    public Transform camera;




    // variables related to gold
    public bool hasGold1 = false;
    public bool hasGold2 = false;
    public bool hasGold10 = false;
    private bool isCarrying = false; //public to be used in SharkMovement script, only for a convenient test
    private int score;
    public float carryingGoldTime = 0; //public to be used in SharkMovement script, only for a convenient test
    public float attackLimitTime = 5;  //public to be used in SharkMovement script, only for a convenient test

    // variables related to health and life
    private float life;
    private float health;
    private float fullHealth = 100;
    private float oxygenLeft;
    private float fullOxygen = 100;


    private Image healthBar;
    private Image goldBar;
    private Image oxygenBar;
    private int damage = 30;
    
    private float timePassedOxygen = 0;
    private float oxygenConsumeTime = 2.0f;
    private float kInvincibilityDuration = 2.5f;
    private float mInvincibleTimer;
    public bool mInvincible; //public to be used in SharkMovement script
    private bool gameOver;

    Vector3 playerSpawnPos = new Vector3(0, 105, -40);
    private float respawnDelay = 1.5f;

    // shoot projectile variables
    public GameObject projectileFb;
    private float projectileOffset = 2f;


    // UI variables
    public Text lifeText;
    public Text carryingGoldTimeText;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public GameObject completeMenu;
    public static bool isPaused = false;

    //SFX variables
    private AudioSource playerAudio;
    [SerializeField]
    private AudioClip shootFigurineSound;
    [SerializeField]
    private AudioClip damageSound;
    [SerializeField]
    private AudioClip pickGoldSound;
    [SerializeField]
    private AudioClip scoreSound;
    [SerializeField]
    private AudioClip bubbleSound;
    [SerializeField]
    private AudioClip gameoverMusic;
    [SerializeField]
    private AudioClip completeMusic;

    //Animator 
    private Animator anim;
    private bool isMoving;





    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.2f, 0.4f, 0.8f, 0.2f);
        RenderSettings.fogDensity = 0.01f;
        Time.timeScale = 1;        
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();        
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        goldBar = GameObject.FindGameObjectWithTag("GoldBar").GetComponent<Image>();
        oxygenBar = GameObject.FindGameObjectWithTag("OxygenBar").GetComponent<Image>();
    }


    void Start()
    {        
        score = 0;
        life = 2;
        health = fullHealth;
        oxygenLeft = fullOxygen;     
    }
    
    void Update()
    {           
        ShootFigurine();
        InvincibleControl();
        consumeOxygen();
        goldTimer();
        DetectWinOrLose();
        displayUI();
        RenderSettings.fog = isInWater;
        CheckPause();
    }
    private void FixedUpdate()
    {
        MovePlayer();        
    }


    private void MovePlayer()
    {
        
        DetectMoving();
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        

        if (isInWater)
        {
            transform.Translate(Vector3.right * Time.deltaTime * (speed / waterDrag) * horizontal, Space.Self);
            transform.Translate(Vector3.forward * Time.deltaTime * (speed / waterDrag) * vertical, Space.Self);

            //playerRb.velocity = new Vector3(
            //    horizontal * speed,
            //    playerRb.velocity.y,
            //    vertical * speed
            //    );
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed / 2 * horizontal, Space.Self);
            transform.Translate(Vector3.forward * Time.deltaTime * speed/2 * vertical, Space.Self);
            
            //playerRb.velocity = new Vector3(
            //    horizontal * speed * 0.5f,
            //    playerRb.velocity.y,
            //    vertical * speed * 0.5f                
            //    );
        }        



        timePassed += Time.deltaTime;
        if (isInWater && Input.GetKey(KeyCode.Q) && timePassed >= keyDelay)// delay when rising
        {
            playerRb.AddForce(-Physics.gravity * speed * swimUpDownForce);
            timePassed = 0;
        }

        if (Input.GetKey(KeyCode.E) && timePassed >= keyDelay)// delay when diving 
        {
            playerRb.AddForce(Physics.gravity * speed * swimUpDownForce);
            timePassed = 0;
        }

        // press and hold Z to speed up, press again to the original speed.
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (speedUpModeOn == false)
            {
                speedUpModeOn = true;
                speed *= 3;
            }
            else
            {
                speedUpModeOn = false;
                speed /= 3;
            }
        }

        // constrain the player position
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

        //drag
        if (isInWater)
            playerRb.drag = waterDrag;
        else
            playerRb.drag = 0;

        // whether the player is in water    
        if (transform.position.y >= seaLevel)
        {
            isInWater = false;
        }
        else
            isInWater = true;

        if (transform.position.y > yRange)
        {
            transform.position = new Vector3(transform.position.x, yRange, transform.position.z);
        }
    } //end of the MovePlayer method

    private void ShootFigurine()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isPaused == false)
            {
                Vector3 shootPos = transform.localPosition;
                shootPos.y += projectileOffset;
                Instantiate(projectileFb, shootPos, transform.rotation);
                // if the 3rd parameter is projectileFb.transform.rotation, 
                // the projectile will always go the same direction but not the direction the player faces.


                anim.SetTrigger("Shoot"); //play animation           
                playerAudio.PlayOneShot(shootFigurineSound, 0.5f);
            }
                  
        }
    }

    private void consumeOxygen()
    {
        timePassedOxygen += Time.deltaTime;
        // consume 1 unity of oxygenLeft every oxygenConsumeTime seconds
        if (isInWater && timePassedOxygen >= oxygenConsumeTime)
        {
            oxygenLeft--;
            timePassedOxygen = 0;
        }
        if (oxygenLeft <= 0)
        {
            Die();
            oxygenLeft = fullOxygen;
        }
        if (!isInWater)
        {
            if (oxygenLeft < fullOxygen)
            {
                oxygenLeft += 10;
            }
            else
            {
                oxygenLeft = fullOxygen;
            }
        }

        //displayOxygen(oxygenLeft);
    }

    public void TakeDamage()
    {
        if (!mInvincible)
        {
            health -= damage;

            // display health reduction in UI
            //displayHealth(health);

            // play being damaged sound
            playerAudio.PlayOneShot(damageSound, 0.7f);

            mInvincible = true;
            if(health > 0)
            {
                anim.SetBool("Invincible", true);
            }
            
            //Debug.Log("INVINCIBLE.");
        }
    }

    private void Die()
    {
        life--;
        // wait two seconds and player death animation before respawn
        anim.SetTrigger("Dead");
        if (life == -1)
        {
            GameOver();
        }
        Invoke("Respawn", respawnDelay);
        hasGold1 = false;
        hasGold2 = false;
        hasGold10 = false;
        speed = originalSpeed;
        health = fullHealth;

        mInvincible = true;
        Debug.Log("NOT INVINCIBLE.");
    }

    private void InvincibleControl()
    {

        if (mInvincible)
        {
            mInvincibleTimer += Time.deltaTime;
            if (mInvincibleTimer >= kInvincibilityDuration)
            {
                mInvincible = false;
                anim.SetBool("Invincible", false);
                Debug.Log("NOT INVINCIBLE.");
                mInvincibleTimer = 0.0f;
            }
        }
    }

    private void goldTimer()
    {
        if (hasGold1 == true || hasGold2 == true || hasGold10 == true)
            carryingGoldTime += Time.deltaTime;
        if (carryingGoldTime > attackLimitTime)
            //shark attack
            Debug.Log("Warning! The shark is coming for your GOLD.");
    }

    private void GameOver()
    {
        if (!gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            gameOver = true;
            playerAudio.Stop();
            playerAudio.PlayOneShot(gameoverMusic, 0.5f);
            gameOverMenu.SetActive(true);


            
            Invoke("PauseGame", 2.0f);
            isPaused = true;
        }
    }

    private void ComepleteLevel()
    {
        if (!gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            completeMenu.SetActive(true);
            Invoke("PauseGame", 3.0f);
            isPaused = true;
        }
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {                
                isPaused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {                
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
        }

    }

    private void displayUI()
    {
        lifeText.text = life.ToString();
        carryingGoldTimeText.text = carryingGoldTime.ToString();
        displayGold(score);
        displayHealth(health);
        displayOxygen(oxygenLeft);
    }


    private void OnTriggerEnter(Collider other)
    {
        float w1 = 1.5f;
        float w2 = 2.0f;
        float w3 = 3.0f;

        if (other.gameObject.CompareTag("Gold1") && isCarrying == false)
        {
            Destroy(other.gameObject);
            hasGold1 = true;
            isCarrying = true;
            speed = speed / w1; //speed is down when carrying gold       
            playerAudio.PlayOneShot(pickGoldSound, 1.0f);
            //Debug.Log("Player got gold 1 and the swim speed is: " + speed);
        }
        if (other.gameObject.CompareTag("Gold2") && isCarrying == false)
        {
            Destroy(other.gameObject);
            hasGold2 = true;
            isCarrying = true;
            speed = speed / w2;
            playerAudio.PlayOneShot(pickGoldSound, 1.0f);
            //Debug.Log("Player got gold 2 and the swim speed is: " + speed);
        }
        if (other.gameObject.CompareTag("Gold10") && isCarrying == false)
        {
            Destroy(other.gameObject);
            hasGold10 = true;
            isCarrying = true;
            speed = speed / w3;
            playerAudio.PlayOneShot(pickGoldSound, 1.0f);
            //Debug.Log("Player got gold 10 and the swim speed is: " + speed);
        }

        if (other.gameObject.CompareTag("Boat") && isCarrying == true)
        {
            
            
            if (hasGold1 == true)
            {
                hasGold1 = false;
                isCarrying = false;
                score += 1;
                speed = speed * w1; // speed is back to normal after the player releases the gold  
                carryingGoldTime = 0;
                playerAudio.PlayOneShot(scoreSound, 1.0f);
                //Debug.Log("Player sent gold 1 back and the swim speed is: " + speed + ", score is: " + score);
            }
            if (hasGold2 == true)
            {
                hasGold2 = false;
                isCarrying = false;
                score += 2;
                speed = speed * w2;
                carryingGoldTime = 0;
                playerAudio.PlayOneShot(scoreSound, 1.0f);
                //Debug.Log("Player sent gold 2 back and the swim speed is: " + speed + ", score is: " + score);
            }
            if (hasGold10 == true)
            {
                hasGold10 = false;
                isCarrying = false;
                score += 10;
                speed = speed * w3;
                carryingGoldTime = 0;
                playerAudio.PlayOneShot(scoreSound, 1.0f);
                //Debug.Log("Player sent gold 10 back and the swim speed is: " + speed + ", score is: " + score);
            }
            
        }

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {            
            TakeDamage();     
            
        }
        if (other.gameObject.CompareTag("Bubble"))
        {
            playerAudio.PlayOneShot(bubbleSound, 1.0f);
        }
    } //OnTriggerEnter

    void DetectWinOrLose()
    {
        if (health <= 0)
            Die();

        if (life == 0)
        {
            if (health <= 0)
            {
                anim.SetTrigger("Dead");
                GameOver();
            }

        }

        if (score >= 100)
        {
            ComepleteLevel();
        }
    }

    private void DetectMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    void Respawn()
    {
        transform.position = playerSpawnPos;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void displayHealth(float value)
    {
        value /= 100f;
        if (value < 0f)
        {
            value = 0f;            
        }
        healthBar.fillAmount = value;
    }

    void displayGold(float value)
    {
        value /= 100f;
        if (value > 100f)
        {
            value = 1f;
        }
        goldBar.fillAmount = value;
    }

    void displayOxygen(float value)
    {
        value /= 100f;
        if (value < 0f)
        {
            value = 0f;
        }
        if (value > 100f)
        {
            value = 1f;
        }

        oxygenBar.fillAmount = value;
    }

    void RotateAnimationToSwimDirection()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerRb.rotation = Quaternion.Euler(0, rotation_right, 0f); //face to right side
        }

        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            playerRb.rotation = Quaternion.Euler(0, rotation_left, 0f); 
        }

        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            playerRb.rotation = Quaternion.Euler(0, 0, 0f); 
        }

        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            playerRb.rotation = Quaternion.Euler(0, rotation_backward, 0f); 
        }

        else if (Input.GetKey(KeyCode.E))
        {
            playerRb.rotation = Quaternion.Euler(rotation_downward, rotation_backward, 0f); 
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            playerRb.rotation = Quaternion.Euler(0, 180f, 0f); 
        }
    } // rotation
}


