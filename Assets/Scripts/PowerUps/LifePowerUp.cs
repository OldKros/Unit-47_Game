using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePowerUp : MonoBehaviour
{
    [SerializeField] AudioClip pickupSoundEffect;
    [SerializeField] [Range(0f, 1f)] float pickupSEVol = 0.75f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Player>().AddLife();
        AudioSource.PlayClipAtPoint(pickupSoundEffect, transform.position, pickupSEVol);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
