using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] GameObject lazerPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float pudding = 1f;
    [SerializeField] float projectFireSpeed = 0.1f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject explosureVFX;

    Coroutine fireCour;

    float xMin;
    float xMax;

    float yMin;
    float yMax;

    public int Health { get => health; set => health = value; }

    void Start()
    {
        SetUpMoveBoundries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           fireCour = StartCoroutine(FireContinuesly());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCour);
        }
    }

    private IEnumerator FireContinuesly()
    {
        while(true)
        {
            GameObject laser = Instantiate(lazerPrefab, transform.position, Quaternion.identity) as GameObject;
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, 0.25f);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            yield return new WaitForSeconds(projectFireSpeed);
        }
        
    }


    private void SetUpMoveBoundries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + pudding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - pudding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + pudding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - pudding;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal")* Time.deltaTime * playerSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin , xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        
     

        transform.position = new Vector2(newXPos, newYPos);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        HitProsecc(damageDealer);
    }

    private void HitProsecc(DamageDealer damageDealer)
    {
        Health -= damageDealer.Damage;
        damageDealer.Hit();
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        var expVFX = Instantiate(explosureVFX, transform.position, transform.rotation);
        Destroy(expVFX, 1f);
    }
}
