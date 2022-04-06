using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Rocket rocket;
    private int _activeLevelIndex;
    private readonly string _currentLevel = "current level";

    private void Start()
    {
        _activeLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt(_currentLevel) != _activeLevelIndex) PlayerPrefs.SetInt(_currentLevel, _activeLevelIndex);
        rocket.Death.AddListener(LevelLoader);
        rocket.Finish.AddListener(LevelLoader);
    }

    private void LevelLoader(string eventName)
    {
        switch (eventName)
        {
            case "death":
                Invoke(nameof(LoadCurrentLevel), 2f);
                break;
            case "finish":
                Invoke(nameof(LoadNextLevel), 2f);
                break;
        }
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(_currentLevel));
    }

    private void LoadNextLevel()
    {
        if (_activeLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _activeLevelIndex = -1;
            PlayerPrefs.SetInt(_currentLevel, 1);
        }
        SceneManager.LoadScene(_activeLevelIndex + 1);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
