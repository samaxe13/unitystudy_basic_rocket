using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
   public static void SaveData(LevelManager levelManager)
    {
        BinaryFormatter formatter = new();
        string path = $"{Application.persistentDataPath}/data.bruh";
        FileStream stream = new(path, FileMode.Create);
        PlayerData data = new(levelManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadData()
    {
        string path = $"{Application.persistentDataPath}/data.bruh";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
