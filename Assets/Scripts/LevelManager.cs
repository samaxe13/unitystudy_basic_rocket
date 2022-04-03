using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int activeLevelIndex;
    public Rocket rocket;

    private void Start() 
    {
        activeLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt("current level") != activeLevelIndex) PlayerPrefs.SetInt("current level", activeLevelIndex);
        rocket.death.AddListener(LoadHandler);
        rocket.finish.AddListener(LoadHandler);
    }

    private void LoadHandler(string eventName)
    {
        switch(eventName)
        {
            case "death":
                Invoke("LoadCurrentLevel", 2f);
                break;
            case "finish":
                Invoke("LoadNextLevel", 2f);
                break;
        }
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("current level"));
    }

    private void LoadNextLevel()
    {
        if (activeLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            activeLevelIndex = -1;
            PlayerPrefs.SetInt("current level", 1);
        }
        SceneManager.LoadScene(activeLevelIndex + 1);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
