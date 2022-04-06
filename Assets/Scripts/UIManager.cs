using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _gasBar;
    [SerializeField] private Rocket _rocket;

    private void Start()
    {
        _gasBar.maxValue = _rocket.GasMax;
        UpdateUI(_rocket.GasMax);
    }

    public void UpdateUI(float gasTotal)
    {
        _gasBar.value = gasTotal;
        _gasBar.fillRect.gameObject.SetActive(_gasBar.value > 0);
    }
}
