using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToPlayer : MonoBehaviour
{
    [SerializeField]
    Projectile projectilePrefab;
    private Transform targetPlayer;
    
    float fireCooldown = 3.0f;
    float timer = 2.0f;
    bool isCooldown;
    private AudioSource octopusAudio;
    public AudioClip throwBombSound;

    


    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        octopusAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        timer += Time.deltaTime;
        
        if (targetPlayer != null)
        {
            transform.LookAt(targetPlayer);
            if (timer > fireCooldown)
            {
                Instantiate(projectilePrefab, transform.position, transform.rotation);
                octopusAudio.PlayOneShot(throwBombSound, 0.3f);
                timer = 0f;
            }              
        }
    }
}
