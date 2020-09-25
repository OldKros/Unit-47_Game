using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int maxHP = 500;
    [SerializeField] int curHP = 500;
    [SerializeField] int maxShield = 200;
    [SerializeField] int curShield = 200;
    [SerializeField] [Range(0, 3)] int lives = 3;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float leftPadding = 0.5f;
    [SerializeField] float rightPadding = 0.5f;
    [SerializeField] float topPadding = 5.5f;
    [SerializeField] float bottomPadding = 1.5f;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] int laserDamage = 50;
    [SerializeField] float laserFireSpeed = 0.2f; // in seconds

    [Header("Rocket")]
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] int rocketCount = 5;
    [SerializeField] float rocketSpeed = 15f;
    [SerializeField] int rocketDamage = 150;

    [Header("Audio")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] float laserSoundVol = 0.75f;
    [SerializeField] AudioClip rocketSound;
    [SerializeField] float rocketSoundVol = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVol = 0.75f;

    [SerializeField] AudioClip shieldsDownSound;
    [SerializeField] float shieldsDownVol = 0.75f;
    [SerializeField] AudioClip shieldsUpSound;
    [SerializeField] float shieldsUpVol = 0.75f;

    Coroutine laserFiringCoroutine;
    float xMin, xMax, yMin, yMax;
    bool invincible = false;
    GameObject shield;

    // Start is called before the first frame update
    void Awake()
    {
        shield = transform.Find("Shield").gameObject;
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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetLivesLeft() { return lives; }

    public int GetRocketCount() { return rocketCount; }

    public float GetShieldPercent() { return (float)curShield / maxShield; }

    public float GetHealthPercent() { return (float)curHP / maxHP; }

    public void Heal(int amount) { curHP = maxHP; }

    public void AddShield() { curShield = maxShield; }

    public void AddRockets(int rocketsToGive)
    {
        rocketCount += rocketsToGive;
        if (rocketCount <= 99) rocketCount = 99;
    }

    public void AddLife() { if (lives < 3) lives++; }

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

        if (Input.GetButtonDown("Fire2"))
        {
            if (rocketCount == 0)
                return;

            GameObject rocket = Instantiate(rocketPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
            rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rocketSpeed);
            rocket.GetComponent<Projectile>().SetDamage(rocketDamage);
            AudioSource.PlayClipAtPoint(rocketSound, transform.position, rocketSoundVol);
            rocketCount--;
        }
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            laser.GetComponent<Projectile>().SetDamage(laserDamage);
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

    public void ActivateShield()
    {
        AudioSource.PlayClipAtPoint(shieldsUpSound, transform.position, shieldsUpVol);
        curShield = maxShield;
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        AudioSource.PlayClipAtPoint(shieldsDownSound, transform.position, shieldsDownVol);
        curShield = 0;
        shield.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
            ProcessHit(projectile.GetDamage());
            projectile.Hit();
        }
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
            ProcessHit(damageDealer.GetDamage());
    }

    void ProcessHit(int damage)
    {
        if (!invincible)
        {
            if (curShield >= damage)
            {
                int temp = (int)curShield;
                curShield -= damage;
                damage -= temp;
                if (curShield <= 0f)
                {
                    damage -= (int)curShield;
                    DeactivateShield();
                    curHP -= damage;
                }
            }
            else
            {
                curHP -= damage;
            }
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
                FindObjectOfType<GUIPlayerLives>().RemoveLife(lives);
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
