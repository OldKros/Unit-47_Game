﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBossHealthBar : MonoBehaviour
{
    GameObject unit;
    Transform healthBar;
    Transform shieldBar;

    // Start is called before the first frame update
    void Start()
    {
        unit = FindObjectOfType<Boss>().gameObject;
        healthBar = transform.Find("HealthBar");
        shieldBar = transform.Find("ShieldBar");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();
    }

    void UpdateBars()
    {
        if (unit != null)
        {
            float hp = unit.GetComponent<Boss>().GetHealthPercent();
            healthBar.localScale = new Vector3(hp, 1f, 1f);
            float sp = unit.GetComponent<Boss>().GetShieldPercent();
            shieldBar.localScale = new Vector3(sp, 1f, 1f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
