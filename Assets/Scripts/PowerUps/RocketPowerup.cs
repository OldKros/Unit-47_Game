using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPowerup : MonoBehaviour
{
    [SerializeField] int rocketsToGive = 5;
    [SerializeField] AudioClip pickupSoundEffect;
    [SerializeField] [Range(0f, 1f)] float pickupSEVol = 0.75f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Player>().AddRockets(rocketsToGive);
        AudioSource.PlayClipAtPoint(pickupSoundEffect, transform.position, pickupSEVol);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
