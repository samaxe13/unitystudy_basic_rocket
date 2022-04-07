using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _gasBar;
    [SerializeField] private Rocket _rocket;
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _triesText;

    private readonly string _currentLevel = "current level";
    private readonly string _levelNumberOfTries = "level number of tries";

    private void Start()
    {
        _gasBar.maxValue = _rocket.GasMax;
        _levelName.text = LevelNames.Names[PlayerPrefs.GetInt(_currentLevel) - 1];
        _triesText.text = $"tries: {PlayerPrefs.GetInt(_levelNumberOfTries)}";
        UpdateUI(_rocket.GasMax);
    }

    public void UpdateUI(float gasTotal)
    {
        _gasBar.value = gasTotal;
        _gasBar.fillRect.gameObject.SetActive(_gasBar.value > 0);
    }
}
