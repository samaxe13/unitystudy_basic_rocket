using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider gasBar;
    [SerializeField] private GameObject gasBarFill;
    [SerializeField] private Rocket rocket;

    private void Start()
    {
        gasBar.maxValue = rocket.gasMax;
        UpdateUI(rocket.gasMax);
    }

    public void UpdateUI(float gasTotal)
    {
        gasBar.value = gasTotal;
        if (gasBar.value <= 0) gasBarFill.SetActive(false);
        else gasBarFill.SetActive(true);
    }
}
