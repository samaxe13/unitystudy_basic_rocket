using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider gasBar;
    public GameObject gasBarFill;
    public Rocket rocket;

    void Start() 
    {
        rocket.gasChanged.AddListener(UpdateUI);
        gasBar.maxValue = rocket.gasTotal;
        UpdateUI();
    }

    public void UpdateUI() 
    {
        gasBar.value = rocket.gasTotal;
        if (gasBar.value <= 0) gasBarFill.SetActive(false);
        else gasBarFill.SetActive(true);
    }
}
