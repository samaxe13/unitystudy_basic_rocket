using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Rocket rocket;
    [HideInInspector] public int activeLevelIndex;
    private readonly string _currentLevel = "current level";
    private readonly string _numberOfTries = "level number of tries";

    private void Start()
    {
        activeLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt(_currentLevel) != activeLevelIndex) PlayerPrefs.SetInt(_currentLevel, activeLevelIndex);
        rocket.Death.AddListener(LevelLoader);
        rocket.Finish.AddListener(LevelLoader);
    }

    private void LevelLoader(string eventName)
    {
        if (eventName == nameof(rocket.Death)) Invoke(nameof(RestartLevel), 2f);
        else if (eventName == nameof(rocket.Finish)) Invoke(nameof(LoadNextLevel), 2f);
    }

    private void RestartLevel()
    {
        PlayerPrefs.SetInt(_numberOfTries, PlayerPrefs.GetInt(_numberOfTries) + 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt(_currentLevel));
    }

    private void LoadNextLevel()
    {
        if (activeLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            activeLevelIndex = -1;
            PlayerPrefs.SetInt(_currentLevel, 1);
        }
        SceneManager.LoadScene(activeLevelIndex + 1);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
