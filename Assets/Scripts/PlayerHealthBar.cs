using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    GameObject unit;
    Transform healthBar;

    // Start is called before the first frame update
    void Start()
    {
        unit = FindObjectOfType<Player>().gameObject;
        healthBar = transform.Find("Bar");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        if (unit != null)
        {
            float hp = unit.GetComponent<Player>().GetHealthPercent();
            healthBar.localScale = new Vector3(hp, 1f, 1f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
