using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayerLives : MonoBehaviour
{
    [SerializeField] GameObject[] lives;

    // Start is called before the first frame update
    void Start()
    {
        SetLives();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetLives()
    {
        int livesLeft = FindObjectOfType<Player>().GetLivesLeft();
        // Debug.Log($"Lives left: {livesLeft}");
        for (int i = 0; i < livesLeft; i++)
        {
            if (i < livesLeft)
                lives[i].SetActive(true);
        }
    }

    public void RemoveLife(int life)
    {
        lives[life - 1].SetActive(false);
    }

    public void AddLife(int life)
    {
        lives[life - 1].SetActive(true);
    }
}
