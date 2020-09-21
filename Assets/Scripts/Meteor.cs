using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxHP = 300;
    [SerializeField] float curHP = 300;
    [SerializeField] int scoreWorth = 200;

    [Header("Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVol = 0.5f;

    [Header("Power Ups")]
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] [Range(0.0f, 1.0f)] float chanceToSpawn = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

        if (curHP <= 0.0f)
        {
            // AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            FindObjectOfType<GameState>().AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }

    void ProcessHit(DamageDealer damageDealer)
    {
        curHP -= damageDealer.GetDamage();

        if (curHP <= 0.0f)
        {
            // AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            FindObjectOfType<GameState>().AddScore(scoreWorth);
            Destroy(gameObject);
        }
    }
}
