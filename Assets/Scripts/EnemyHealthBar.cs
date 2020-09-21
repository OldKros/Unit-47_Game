using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    GameObject unit;
    Transform healthBar;

    // Start is called before the first frame update
    void Start()
    {
        unit = transform.root.gameObject;
        healthBar = transform.Find("Bar");
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateHealth();
    }
    public void ShowHealth(float hp)
    {
        healthBar.localScale = new Vector3(hp, 1f, 1f);
    }
}
