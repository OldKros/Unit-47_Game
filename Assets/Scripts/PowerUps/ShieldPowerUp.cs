using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] AudioClip pickupSoundEffect;
    [SerializeField] float pickupSEVol = 0.75f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Player>().ActivateShield();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
