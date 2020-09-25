using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage;

    public void Hit()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int damage) { this.damage = damage; }
    public int GetDamage() { return damage; }
}
