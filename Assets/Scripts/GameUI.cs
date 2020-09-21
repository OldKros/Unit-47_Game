using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] List<Sprite> countdownSprites;
    [SerializeField] List<GameObject> lives;
    GameObject countdownSprite;
    GameObject levelComplete;
    GameObject scoreText;

    float levelTimer;

    void Start()
    {
        countdownSprite = transform.Find("Countdown Sprite").gameObject;
        levelComplete = transform.Find("Level Complete").gameObject;
        scoreText = transform.Find("Score").gameObject;
    }

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text =
            FindObjectOfType<Player>().GetScore().ToString();
    }

    public void RemoveLife(int life)
    {
        lives[life - 1].SetActive(false);
    }

    public void AddLife(int life)
    {
        lives[life - 1].SetActive(true);
    }

    public void SetLevelTimer(float timer)
    {
        levelTimer = timer;
    }

    public IEnumerator FinishLevelAndCountdown(int seconds)
    {
        SetLevelText();

        levelComplete.SetActive(true);
        countdownSprite.SetActive(true);

        while (seconds >= 0)
        {
            countdownSprite.GetComponent<Image>().sprite = countdownSprites[seconds];
            seconds--;
            yield return new WaitForSeconds(1);
        }
    }

    private void SetLevelText()
    {
        int minutes = (int)levelTimer / 60;
        int seconds = (int)levelTimer % 60;

        string text = $"Level Complete ! \nLevel took: {minutes}min {seconds}sec\nNext Level in";
        levelComplete.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
}
