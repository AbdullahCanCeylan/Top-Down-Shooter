using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    //Gun variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private float fireTimer;

    private Vector2 mousePos;

    public AudioClip gunSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - 
        transform.position.x) * Mathf.Rad2Deg - 90f;

        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if(Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
            PlayGunSound();
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }

    private void PlayGunSound()
    {
        // Play the assigned gun sound
        audioSource.PlayOneShot(gunSound);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(mx, my).normalized * speed;
      
    }

    
    private void Shoot()
    {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            LevelManager.manager.GameOver();
            Destroy(gameObject);
        }
    }
}
