using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string directory = "/SaveData/";
    public static string fileName = "score.txt";
    public static void Save(SaveData saveData)
    {
        string dir = Application.persistentDataPath + directory;

        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(dir + fileName, json);
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        
        SaveData sd = new SaveData();
        if(File.Exists(fullPath))
        {
            string data = File.ReadAllText(fullPath);
            sd = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }


        return sd;
    }
}
