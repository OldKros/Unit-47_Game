using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVol = 0.75f;
    int damage;

    public void Hit()
    {
        Die();
    }

    public void SetDamage(int damage) { this.damage = damage; }
    public int GetDamage() { return damage; }

    void Die()
    {
        if (deathVFX)
        {
            GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }
        if (deathSound)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVol);
        }

        Destroy(gameObject);
    }
}
