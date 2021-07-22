using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string playerName;

    public int[] highscores = {5,10,20};
    public string[] highscoreNames = {"STILL", "NOT", "TAKEN"};

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighscores();
        
    }

    [System.Serializable]
    class SaveData
    {
        public int[] highscores;
        public string[] highscoreNames;
    }

    public void SaveHighscores()
    {
        SaveData data = new SaveData();
        data.highscores = highscores;
        data.highscoreNames = highscoreNames;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscores()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highscores = data.highscores;
            highscoreNames = data.highscoreNames;
        }
    }
}
