using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PersistantObjects : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string prefabID;
    public string PrefabID
    {
        get => prefabID;
        set => prefabID = value;
    }
    private string uuID;
    public static event Action<PersistantObjectData> myPersistantObjectData;
    void Start()
    {
        if(string.IsNullOrEmpty(uuID))
        {
            uuID = CreateUUID();
        }
        SaveAllandLoad.saveEvent += saveData;
    }
    // Update is called once per frame
    string CreateUUID()
    {
        return Guid.NewGuid().ToString();
    }
    void saveData(string ARLocation)
    {
        PersistantObjectData persistentObjects = new PersistantObjectData(
            prefabID,
            uuID,
            ARLocation,
            transform.rotation,
            transform.localScale,
            transform.localPosition
        );

        myPersistantObjectData?.Invoke(persistentObjects);
    }
    public struct PersistantObjectData
    {
        public string _prefabID;
        public string _uuID;
        public string _arLocation;
        public Quaternion _rotation;
        public Vector3 _scale;
        public Vector3 _location;

        public PersistantObjectData(string prefabID, string uuID, string ARLocation, Quaternion rotation, 
            Vector3 scale, Vector3 location )
        {
            _arLocation = ARLocation;
            _prefabID = prefabID;
            _uuID = uuID;
            _arLocation = ARLocation;
            _rotation = rotation;
            _scale = scale;
            _location = location;
        }

        
    }
}
