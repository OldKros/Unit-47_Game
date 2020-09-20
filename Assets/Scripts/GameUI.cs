using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    GameObject countdownSprite;

    void Start()
    {
        countdownSprite = transform.Find("Countdown Sprite").gameObject;
    }

    public IEnumerator Countdown(int seconds)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        while (seconds >= 0)
        {
            countdownSprite.GetComponent<Image>().sprite = sprites[seconds];
            seconds--;
            yield return new WaitForSeconds(1);
        }
    }
}
