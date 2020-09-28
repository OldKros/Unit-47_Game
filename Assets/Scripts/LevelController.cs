using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    [SerializeField] string startMenu = "Start Menu";
    [SerializeField] string nextLevel = "0";
    [SerializeField] string winScene = "Win Scene";
    [SerializeField] string gameOver = "Game Over";


    void Start()
    {

    }

    public void LoadStartMenu()
    {
        FindObjectOfType<GameState>().ResetScore();
        SceneManager.LoadScene(startMenu);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadNextScene()
    {
        if (nextLevel == "0")
        {
            LoadWinScene();
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene(winScene);
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitBeforeLoadScene(2, gameOver));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator WaitBeforeLoadScene(int delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

}
