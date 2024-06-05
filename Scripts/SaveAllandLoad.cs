using Niantic.Lightship.AR.LocationAR;
using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.PersistentAnchors;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SaveAllandLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button saveButton;
    public static event Action<string> saveEvent;
    private ARLocationManager _arLocationManager;
    private ARLocation _lastARLocation;
    void Start()
    {
        _arLocationManager = FindObjectOfType<ARLocationManager>();
        saveButton.onClick.AddListener(() => saveEvent.Invoke(_arLocationManager.ARLocations[0].Payload.ToBase64()));
        PersistantObjects.myPersistantObjectData += SaveData;
        _arLocationManager.locationTrackingStateChanged += CheckShouldLoadData;
    }
    void CheckShouldLoadData(ARLocationTrackedEventArgs eventArgs)
    {
        if(_lastARLocation == null || eventArgs.ARLocation == _lastARLocation) //first visit, re open location same place load data
        {
            LoadData(eventArgs.ARLocation);
        }
        else
        {
            Debug.Log("Location was already loaded");
        }
    }
    void SaveData(PersistantObjects.PersistantObjectData objectData)
    {
        string pathToSave = Application.persistentDataPath;
        string folderToSave = objectData._arLocation;
        string combinePath = Path.Combine(pathToSave,folderToSave);

        if (Directory.Exists(combinePath))
        {
            File.WriteAllText(combinePath +"/"+"ARLocation.json", JsonUtility.ToJson(objectData));
        } else
        {
            Directory.CreateDirectory(combinePath);
            File.WriteAllText(combinePath +"/" +  "ARLocation.json", JsonUtility.ToJson(objectData));
        }
#if UNITY_EDITOR
        OpenFolderFinder(pathToSave);
#endif

    }
    void LoadData(ARLocation location)
    {
        string pathToLoad = Application.persistentDataPath;
        string folderToLoad = location.Payload.ToBase64();
        string combinePath = Path.Combine(pathToLoad, folderToLoad);

        List<PersistantObjects.PersistantObjectData> objectToSpawn = new();
        if (Directory.Exists(combinePath))
        {
            foreach(var file in Directory.GetFiles(combinePath))
            {
                string readFile = File.ReadAllText(file);
                objectToSpawn.Add(JsonUtility.FromJson<PersistantObjects.PersistantObjectData>(readFile));
            }
            foreach(var spwanObject in objectToSpawn)
            {
                SpawnObjectFromPersistantObjectData(spwanObject);
            }
        } else
        {
            Debug.Log("Nothing has been placed here yet");
        }
    }
    void SpawnObjectFromPersistantObjectData(PersistantObjects.PersistantObjectData data)
    {
        GameObject toSpawn;
    }
    public void OpenFolderFinder(string folderPath)
    {
        if(System.IO.Directory.Exists(folderPath))
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = folderPath,
                UseShellExecute = true,
                Verb = "open"
                
            });
        } else
        {
            Debug.Log("Folder not found:" + folderPath);
        }
    }
    // Update is called once per frame
}
