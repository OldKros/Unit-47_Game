using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 200.0f;
    [SerializeField] float curHP = 200.0f;
    [SerializeField] int score = 0;
    [SerializeField] [Range(0, 5)] int lives = 3;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float xPadding = 0.5f;
    [SerializeField] float yPadding = 0.5f;

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
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    public int GetScore() { return score; }

    public void AddScore(int score) { this.score += score; }

    public int GetLivesLeft() { return lives; }

    public float GetHealthPercent() { return curHP / maxHP; }

    private void Fire()
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
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
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
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
            StartCoroutine(ProcessHit(damageDealer));
    }

    IEnumerator ProcessHit(DamageDealer damageDealer)
    {
        if (!invincible)
        {
            curHP -= damageDealer.GetDamage();
            damageDealer.Hit();
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
                FindObjectOfType<GameUI>().RemoveLife(lives);
                lives--;
                yield return StartCoroutine(BlinkAndRespawn());
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
