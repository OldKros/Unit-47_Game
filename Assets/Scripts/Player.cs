using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 200.0f;
    [SerializeField] float curHP = 200.0f;

    [SerializeField] [Range(0, 3)] int lives = 3;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float leftPadding = 0.5f;
    [SerializeField] float rightPadding = 0.5f;
    [SerializeField] float topPadding = 5.5f;
    [SerializeField] float bottomPadding = 1.5f;

    [Header("Projectile/s")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float laserFireSpeed = 0.2f; // in seconds

    [Header("Audio")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] float laserSoundVol = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVol = 0.75f;

    Coroutine laserFiringCoroutine;
    float xMin, xMax, yMin, yMax;
    bool invincible = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpMoveBoundaries();
        SetUpSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetLivesLeft() { return lives; }

    public float GetHealthPercent() { return curHP / maxHP; }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            laserFiringCoroutine = StartCoroutine(FireLaser());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(laserFiringCoroutine);
        }
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVol);
            yield return new WaitForSeconds(laserFireSpeed);
        }
    }

    void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + leftPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - rightPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + bottomPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - topPadding;
    }

    void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();
        if (projectile)
            ProcessHit(projectile);
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
            ProcessHit(damageDealer);
    }

    void ProcessHit(Projectile projectile)
    {
        if (!invincible)
        {
            curHP -= projectile.GetDamage();
            projectile.Hit();
        }

        if (curHP <= 0.0f)
        {
            if (lives <= 0)
            {
                FindObjectOfType<LevelController>().LoadGameOver();
                Debug.Log("Dedded");
                Destroy(gameObject);
            }
            else
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVol);
                FindObjectOfType<PlayerLives>().RemoveLife(lives);
                lives--;
                StartCoroutine(BlinkAndRespawn());
            }
        }
    }

    void ProcessHit(DamageDealer damageDealer)
    {
        if (!invincible)
        {
            curHP -= damageDealer.GetDamage();
            // damageDealer.Hit(gameObject.GetComponent<DamageDealer>().GetDamage());
        }

        if (curHP <= 0.0f)
        {
            if (lives <= 0)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVol);
                FindObjectOfType<LevelController>().LoadGameOver();
                Debug.Log("Dedded");
                Destroy(gameObject);
            }
            else
            {
                FindObjectOfType<PlayerLives>().RemoveLife(lives);
                lives--;
                StartCoroutine(BlinkAndRespawn());
            }
        }
    }

    IEnumerator BlinkAndRespawn()
    {
        invincible = true;
        curHP = maxHP;
        transform.position = new Vector2(0, -6);
        for (int i = 0; i <= 3; i++)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(0.2f);
        }
        invincible = false;
    }

}
