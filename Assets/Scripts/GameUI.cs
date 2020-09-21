using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    GameObject countdownSprite;
    float levelTimer;

    void Start()
    {
        countdownSprite = transform.Find("Countdown Sprite").gameObject;
    }

    public void SetLevelTimer(float timer)
    {
        levelTimer = timer;
    }

    public IEnumerator FinishLevelAndCountdown(int seconds)
    {
        SetLevelText();
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

    private void SetLevelText()
    {
        string text = $"Level Complete ! \nLevel took: {levelTimer:0.00}s\nNext Level in";
        transform.Find("Level Complete").gameObject
        .GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
}
