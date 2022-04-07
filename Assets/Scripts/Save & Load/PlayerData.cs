using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] numberOfTriesList = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public PlayerData(LevelManager levelManager)
    {
        numberOfTriesList[levelManager.activeLevelIndex - 1] = PlayerPrefs.GetInt("level number of tries");
    }
}
