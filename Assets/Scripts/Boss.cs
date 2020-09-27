using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 6000.0f;
    [SerializeField] float curHP = 6000.0f;
    [SerializeField] float maxShield = 3000.0f;
    [SerializeField] float curShield = 3000.0f;
    [SerializeField] int scoreWorth = 10;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10.0f;
    [SerializeField] int laserDamage = 50;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3.0f;
    float laserTimer;
    bool shootLeft = true;

    [Header("Rocket")]
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float rocketSpeed = 10.0f;
    [SerializeField] int rocketDamage = 50;
    [SerializeField] float minTimeBetweenRockets = 4.0f;
    [SerializeField] float maxTimeBetweenRockets = 15.0f;
    float rocketTimer;

    [Header("Audio")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0f, 1f)] float laserSoundVol = 0.75f;
    [SerializeField] AudioClip rocketSound;
    [SerializeField] [Range(0f, 1f)] float rocketSoundVol = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVolume = 0.5f;

    GameObject shield;
    GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        shield = transform.Find("Shield").gameObject;
        laserTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        rocketTimer = Random.Range(minTimeBetweenRockets, maxTimeBetweenRockets);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    public float GetShieldPercent() { return (float)curShield / maxShield; }

    public float GetHealthPercent() { return (float)curHP / maxHP; }

    void CountDownAndShoot()
    {
        laserTimer -= Time.deltaTime;
        rocketTimer -= Time.deltaTime;
        if (laserTimer <= 0.0f)
        {
            Vector2 shootPos;
            if (shootLeft)
            {
                shootPos = new Vector2(transform.position.x + -1.1f, transform.position.y - 0.515f);
                shootLeft = false;
            }
            else
            {
                shootPos = new Vector2(transform.position.x + 1.1f, transform.position.y - 0.515f);
                shootLeft = true;
            }
            Fire(shootPos);
            laserTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }

        if (rocketTimer <= 0.0f)
        {
            FireRocket();
            rocketTimer = Random.Range(minTimeBetweenRockets, maxTimeBetweenRockets);
        }
    }

    void Fire(Vector2 shootPos)
    {
        GameObject laser = Instantiate(laserPrefab, shootPos, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        laser.GetComponent<Projectile>().SetDamage(laserDamage);
        AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVol);
    }

    void FireRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rocketSpeed);
        rocket.GetComponent<Projectile>().SetDamage(rocketDamage);
        AudioSource.PlayClipAtPoint(rocketSound, transform.position, rocketSoundVol);
    }

    void DeactivateShield()
    {
        // AudioSource.PlayClipAtPoint(shieldsDownSound, transform.position, shieldsDownVol);
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

    void ProcessHit(float damage)
    {
        if (curShield >= damage)
        {
            int temp = (int)curShield;
            curShield -= damage;
            damage -= temp;
            if (curShield <= 0f)
            {
                damage -= (int)curShield;
                StartCoroutine(ShieldDestroy());
                curHP -= damage;
            }
        }
        else
        {
            curHP -= damage;
        }
        // var healthBar = transform.Find("enemyHealthBar").GetComponent<EnemyHealthBar>();
        // healthBar.gameObject.SetActive(true);
        // healthBar.ShowHealth(GetHealthPercent());

        if (curHP <= 0.0f)
        {
            // AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            gameState.AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }

    IEnumerator ShieldDestroy()
    {
        curHP = maxHP;
        for (int i = 0; i <= 3; i++)
        {
            shield.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.2f);
            shield.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(0.2f);
        }
        DeactivateShield();
    }
}
