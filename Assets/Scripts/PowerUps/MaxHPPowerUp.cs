﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPPowerUp : MonoBehaviour
{
    [SerializeField] int amountToAdd = 100;
    [SerializeField] AudioClip pickupSoundEffect;
    [SerializeField] [Range(0f, 1f)] float pickupSEVol = 0.75f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Player>().Heal(amountToAdd);
        AudioSource.PlayClipAtPoint(pickupSoundEffect, transform.position, pickupSEVol);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
