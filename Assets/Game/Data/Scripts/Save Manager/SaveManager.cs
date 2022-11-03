using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILENAME = "save.dat";

    private string _saveFilePath;

    public Player       player;
    //public CameraRig    cameraRig;

    //
    private string[] _SavedFiles;


    [System.Serializable]
    private struct GameSaveData
    {
        public Player.SaveData      playerSaveData;
        //public CameraRig.SaveData   cameraRigSaveData;
    }

    void Start()
    {
        _saveFilePath = Application.persistentDataPath + "/" + SAVE_FILENAME;

        //LoadGame();
    }

    
    void Update()
    {

        /*
        if (Input.GetButtonDown("QuickSave"))
            SaveGame();
        else if (Input.GetButtonDown("QuickLoad"))
            LoadGame();
        */

          
    }

    public void SaveButton()
    {
        SaveGame();
    }

    public void LoadButton()
    {
        LoadGame();
    }

    public void GetSaveData()
    {
        SaveGame();
    }

    public void GetLastloadedData()
    {
        LoadGame();
    }


    public void AutoSave()
    {
        SaveGame();
    }

   

    public void SaveGame()
    {
        GameSaveData saveData;

        saveData.playerSaveData     = player.GetSaveData();
        //saveData.cameraRigSaveData  = cameraRig.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_saveFilePath, jsonSaveData);

        print("Game saved.");
    }

    public void LoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string jsonSaveData = File.ReadAllText(_saveFilePath);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonSaveData);

            player.LoadSaveData(saveData.playerSaveData);
            //cameraRig.LoadSaveData(saveData.cameraRigSaveData);

            print("Game loaded.");
        }
    }
}
