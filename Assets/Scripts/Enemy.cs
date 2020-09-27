﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 100.0f;
    [SerializeField] float curHP = 100.0f;
    [SerializeField] int scoreWorth = 10;
    [SerializeField] List<Sprite> sprites;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10.0f;
    [SerializeField] int laserDamage = 50;
    float shotTimer;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3.0f;

    [Header("Audio")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0f, 1f)] float laserSoundVol = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVolume = 0.5f;

    GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        int i = Random.Range(0, sprites.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[i];
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
        GameObject laser = Instantiate(laserPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        laser.GetComponent<Projectile>().SetDamage(laserDamage);
        AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVol);
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
        curHP -= damage;
        var healthBar = transform.Find("enemyHealthBar").GetComponent<EnemyHealthBar>();
        healthBar.gameObject.SetActive(true);
        healthBar.ShowHealth(GetHealthPercent());

        if (curHP <= 0.0f)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            gameState.AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }


}
