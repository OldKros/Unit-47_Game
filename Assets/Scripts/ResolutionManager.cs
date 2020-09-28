using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{

    int[] vertical = new int[] { 640, 960 };
    int[] horizontal = new int[] { 360, 540 };
    int i = 0;

    int maxHeight;


    TMPro.TextMeshProUGUI verRes;
    TMPro.TextMeshProUGUI horRes;

    // Start is called before the first frame update
    void Start()
    {
        verRes = transform.Find("verRes").GetComponent<TMPro.TextMeshProUGUI>();
        horRes = transform.Find("horRes").GetComponent<TMPro.TextMeshProUGUI>();
        maxHeight = Screen.height;
        horRes.text = horizontal[i].ToString();
        verRes.text = vertical[i].ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetRes()
    {
        Screen.SetResolution(horizontal[i], vertical[i], false);
    }

    public void Next()
    {
        i++;
        if (i >= vertical.Length)
        {
            i = 0;
        }

        horRes.text = horizontal[i].ToString();
        verRes.text = vertical[i].ToString();
    }

}
