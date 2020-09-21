using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject unit;
    Transform healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.Find("Bar");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }
    void UpdateHealth()
    {
        float hp = unit.GetComponent<Player>().GetHealthPercent();
        healthBar.localScale = new Vector3(hp, 1f, 1f);
    }
}
