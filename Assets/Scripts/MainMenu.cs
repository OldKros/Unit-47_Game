using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject resManager;
    [SerializeField] GameObject startmenu;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowOptions()
    {
        startmenu.SetActive(false);
        resManager.SetActive(true);
    }

    public void ShowMenu()
    {
        startmenu.SetActive(true);
        resManager.SetActive(false);
    }
}
