using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    [SerializeField] string startMenu = "Start Menu";
    [SerializeField] string nextLevel;
    [SerializeField] string winScene = "Win Scene";
    [SerializeField] string gameOver = "Game Over";


    void Start()
    {

    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(startMenu);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextLevel);
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
