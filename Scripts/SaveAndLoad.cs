using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    private ARLocationManager arLocationManager;
    public static event Action<string> saveEvent;
    private Transform currentParaentTransform;
    [SerializeField] private CreateGaneObject createGaneObject;
    // Start is called before the first frame update
    void Start()
    {
        arLocationManager = FindObjectOfType<ARLocationManager>();
        saveButton.onClick.AddListener(saveeee);
        PersistentObjects.myPersistantData += saveData;
        arLocationManager.locationTrackingStateChanged += LoadData;
    }
    private void OnDisable()
    {
        PersistentObjects.myPersistantData -= saveData;
        arLocationManager.locationTrackingStateChanged -= LoadData;
        saveButton.onClick.RemoveListener(saveeee);
    }
    void saveData(PersistentObjects.persistentObjectData data)
    {
        string filePath = Application.persistentDataPath;
        string folderPath = (data._arLocation);
        string combinePath = Path.Combine(filePath ,folderPath);

        if (!Directory.Exists(combinePath))
        {
            Directory.CreateDirectory(combinePath);
        }
        string fileName = Path.Combine(combinePath, $"{data._uuID}.json");
        File.WriteAllText(fileName, JsonUtility.ToJson(data));

#if UNITY_EDITOR
        FolderFinder(filePath);
#endif
    }
    void saveeee()
    {
        string arlocationName = arLocationManager.ARLocations[0].Payload.ToBase64();
        saveEvent?.Invoke(arlocationName);
    }
    void LoadData(ARLocationTrackedEventArgs eventArgs)
    {
        string filePath = Application.persistentDataPath;
        string folderPath = (eventArgs.ARLocation.Payload.ToBase64());
        string combinePath = Path.Combine(filePath, folderPath);

        currentParaentTransform = FindObjectOfType<ARLocation>()?.transform;     
        if(currentParaentTransform == null)
        {
            Debug.Log("No ARLocation Found");
            return;
        }

        List<PersistentObjects.persistentObjectData> data = new List<PersistentObjects.persistentObjectData>();

        if (Directory.Exists(combinePath))
        {
            foreach(var file in Directory.GetFiles(combinePath))
            {
                string readFile = File.ReadAllText(file);
                data.Add(JsonUtility.FromJson<PersistentObjects.persistentObjectData>(readFile));
            }
            foreach(var spawnData in data)
            {
                SpawnDataFromPersistentObject(spawnData);
            }
        }
        else
        {
            Debug.Log("Load Error: No Folder Found");
        }
    }
    void SpawnDataFromPersistentObject(PersistentObjects.persistentObjectData data)
    { 
        GameObject gameObject = Instantiate(createGaneObject.GetObject(data._prefabName), data._position, data._rotation);
        gameObject.GetComponent<PersistentObjects>().UuID = data._uuID;
        if(currentParaentTransform != null)
        {
            gameObject.transform.SetParent(currentParaentTransform);
        }   
    }
    // Update is called once per frame
    void FolderFinder(string filePath)
    {
        if(Directory.Exists(filePath))
        {
            Process.Start(new ProcessStartInfo()
            {   
                FileName = filePath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        else
        {
            UnityEngine.Debug.Log("Save Error: No Folder Found");
        }   
    }
  
}
