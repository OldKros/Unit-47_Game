using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 300;
    [SerializeField] float curHP = 300;
    [SerializeField] int scoreWorth = 200;
    [SerializeField] List<Sprite> crackSprites;

    [Header("Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVol = 0.5f;

    [Header("Power Ups")]
    [SerializeField] [Range(0.0f, 1.0f)] float powerUpChance = 0.2f;
    List<GameObject> powerUps;
    [SerializeField] float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, spinSpeed);
        ChooseDamageSprite();
    }

    public void SetPowerUps(List<GameObject> powerUps)
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

    void ProcessHit(float damage)
    {
        curHP -= damage;

        if (curHP <= 0.0f)
        {
            // AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            FindObjectOfType<GameState>().AddScore(scoreWorth);
            gameObject.SetActive(false);
            SpawnPowerUp();
            Destroy(gameObject);
        }
    }

    void SpawnPowerUp()
    {
        if (Random.Range(0f, 1f) <= powerUpChance)
        {
            int powerUpToSpawn = Random.Range(0, powerUps.Count - 1);
            var powerup = Instantiate(powerUps[powerUpToSpawn], transform.position, Quaternion.identity);
            powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.2f);
        }
    }

    void ChooseDamageSprite()
    {
        float hp = curHP / maxHP;
        if (hp <= 0.25f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[2];
        else if (hp <= 0.5f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[1];
        else if (hp <= 0.75f)
            transform.Find("Cracks").GetComponent<SpriteRenderer>().sprite = crackSprites[0];
    }
}
