using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int maxHP = 300;
    [SerializeField] int curHP = 300;
    [SerializeField] int scoreWorth = 200;
    [SerializeField] Sprite[] crackSprites;

    [Header("Death")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVol = 0.5f;

    [Header("Power Ups")]
    [SerializeField] [Range(0.0f, 1.0f)] float powerUpChance = 0.2f;
    GameObject[] powerUps;
    [SerializeField] float spinSpeed;

    GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, spinSpeed);
        ChooseDamageSprite();
    }

    public void SetPowerUps(GameObject[] powerUps)
    {
        this.powerUps = powerUps;
    }

    public void SetSpinSpeed(float spinSpeed) { this.spinSpeed = spinSpeed; }

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
        curHP -= damage;

        if (curHP <= 0)
        {
            // AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            gameState.AddScore(scoreWorth);
            GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);

            SpawnPowerUp();
            Destroy(gameObject);
        }
    }

    void SpawnPowerUp()
    {
        if (Random.Range(0f, 1f) <= powerUpChance)
        {
            int powerUpToSpawn = Random.Range(0, powerUps.Length - 1);
            var powerup = Instantiate(powerUps[powerUpToSpawn], transform.position, Quaternion.identity);
            powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
        }
    }

    void ChooseDamageSprite()
    {
        float hp = (float)curHP / maxHP;
        if (hp <= 0.25f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[2];
        else if (hp <= 0.5f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[1];
        else if (hp <= 0.75f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[0];
    }
}
