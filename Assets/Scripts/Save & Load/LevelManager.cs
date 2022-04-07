using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Rocket rocket;
    [HideInInspector] public int activeLevelIndex;
    private readonly string _currentLevel = "current level";
    private readonly string _levelNumberOfTries = "level number of tries";

    private void Start()
    {
        PlayerData data = SaveSystem.LoadData();
        activeLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt(_currentLevel) != activeLevelIndex)
        {
            PlayerPrefs.SetInt(_currentLevel, activeLevelIndex);
            PlayerPrefs.SetInt(_levelNumberOfTries, data.numberOfTriesList[activeLevelIndex - 1]);
        }
        rocket.Death.AddListener(LevelLoader);
        rocket.Finish.AddListener(LevelLoader);
    }

    private void LevelLoader(string eventName)
    {
        switch (eventName)
        {
            case "death":
                Invoke(nameof(RestartLevel), 2f);
                break;
            case "finish":
                Invoke(nameof(LoadNextLevel), 2f);
                break;
        }
    }

    private void RestartLevel()
    {
        PlayerPrefs.SetInt(_levelNumberOfTries, PlayerPrefs.GetInt(_levelNumberOfTries) + 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt(_currentLevel));
    }

    private void LoadNextLevel()
    {
        if (activeLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            activeLevelIndex = -1;
            PlayerPrefs.SetInt(_currentLevel, 1);
        }
        PlayerPrefs.SetInt(_levelNumberOfTries, PlayerPrefs.GetInt(_levelNumberOfTries) + 1);
        SaveSystem.SaveData(this);
        SceneManager.LoadScene(activeLevelIndex + 1);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
