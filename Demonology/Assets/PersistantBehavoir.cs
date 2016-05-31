using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistantBehavoir : MonoBehaviour {

    public static int levelsEarned = 0;
	// Use this for initialization
	void Start () {
        if (levelsEarned < Application.loadedLevel && Application.loadedLevelName != "LoadingScreen" && Application.loadedLevelName != "Credits") 
        {
            
            levelsEarned = Application.loadedLevel;
            Save();
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Save() 
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/levelInfo.dat");

        PlayerData data = new PlayerData();
        data.levelsEarned = levelsEarned;

        bf.Serialize(file, data);
        file.Close();
    }

    void Load() 
    {
        if (File.Exists(Application.persistentDataPath + "/levelInfo.dat")) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/levelInfo.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            levelsEarned = data.levelsEarned;
        }
    }

    [System.Serializable]
    class PlayerData     
    {
        public int levelsEarned;
    }

}
