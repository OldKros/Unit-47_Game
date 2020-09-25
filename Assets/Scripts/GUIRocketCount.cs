using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRocketCount : MonoBehaviour
{
    [SerializeField] Sprite[] numerals;
    GameObject units;
    GameObject tens;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        units = transform.Find("Units").gameObject;
        tens = transform.Find("Tens").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRockets();
    }

    void UpdateRockets()
    {
        var rocketCount = player.GetRocketCount();
        units.GetComponent<Image>().sprite = numerals[rocketCount % 10];
        tens.GetComponent<Image>().sprite = numerals[rocketCount / 10];
    }
}
