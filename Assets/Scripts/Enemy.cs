using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 100.0f;
    [SerializeField] float curHP = 100.0f;
    [SerializeField] int scoreWorth = 10;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10.0f;
    float shotTimer;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3.0f;

    [Header("Audio")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] float laserSoundVol = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVolume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    public float GetHealthPercent() { return curHP / maxHP; }
    void CountDownAndShoot()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0.0f)
        {
            Fire();
            shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    void Fire()
    {
        GameObject projectile = Instantiate(laserPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);

        AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVol);
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
        curHP -= projectile.GetDamage();
        projectile.Hit();
        var healthBar = transform.Find("enemyHealthBar").GetComponent<EnemyHealthBar>();
        healthBar.gameObject.SetActive(true);
        healthBar.ShowHealth(GetHealthPercent());

        if (curHP <= 0.0f)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            FindObjectOfType<GameState>().AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }

    void ProcessHit(DamageDealer damageDealer)
    {
        curHP -= damageDealer.GetDamage();
        // damageDealer.Hit(gameObject.GetComponent<DamageDealer>().GetDamage());
        var healthBar = transform.Find("enemyHealthBar").GetComponent<EnemyHealthBar>();
        healthBar.gameObject.SetActive(true);
        healthBar.ShowHealth(GetHealthPercent());

        if (curHP <= 0.0f)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            FindObjectOfType<GameState>().AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }
}
