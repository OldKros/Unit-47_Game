using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] AudioClip introClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip level1Clip;
    [SerializeField] AudioClip level2Clip;
    [SerializeField] AudioClip level3Clip;

    [SerializeField] [Range(0f, 1f)] float volume = 0.6f;
    AudioSource audioSource;
    string lastScene = "0";
    string currentScene;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = volume;
        lastScene = SceneManager.GetActiveScene().name;
        PlayClip(lastScene);
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != lastScene)
        {
            lastScene = currentScene;
            PlayClip(currentScene);
        }
    }

    void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayClip(string scene)
    {
        audioSource.Stop();
        switch (scene)
        {
            case "Start Menu":
                audioSource.loop = false;
                audioSource.clip = introClip;
                break;
            case "Game Over":
                audioSource.loop = false;
                audioSource.clip = gameOverClip;
                break;
            case "Win Scene":
                audioSource.loop = false;
                audioSource.clip = winClip;
                break;
            case "Level 1":
                audioSource.loop = true;
                audioSource.clip = level1Clip;
                break;
            case "Level 2":
                audioSource.loop = true;
                audioSource.clip = level2Clip;
                break;
            case "Level 3":
                audioSource.loop = true;
                audioSource.clip = level3Clip;
                break;
            default:
                audioSource.clip = level1Clip;
                break;
        }
        audioSource.Play();

    }
}
