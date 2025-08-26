using Niantic.Lightship.AR.LocationAR;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    [SerializeField] private string prefabName;
    private string uuID;
    public string PrefabName{
        get { return prefabName; }
        set { prefabName = value; }
    }
    public string UuID
    {
        get { return uuID; }
        set { uuID = value; }
    }
    public static event Action<persistentObjectData> myPersistantData;
    public void Start()
    {
        if(string.IsNullOrEmpty(uuID))
        {
            uuID = CreateUuID();
        }
        SaveAndLoad.saveEvent += SaveData;
    }
    private void OnDestroy()
    {
        SaveAndLoad.saveEvent -= SaveData;
    }
    string CreateUuID()
    {
        return Guid.NewGuid().ToString();
    }
    public void SaveData(string ARLocationName)
    {
        persistentObjectData data = new persistentObjectData(ARLocationName,
            prefabName, uuID, transform.position, transform.rotation, transform.localScale);

        myPersistantData?.Invoke(data);
    }
    public struct persistentObjectData
    {
        public string _arLocation;
        public string _prefabName;
        public string _uuID;

        public Vector3 _position;
        public Quaternion _rotation;
        public Vector3 _scale;

        public string getARLocation()
        {
            return _arLocation;
        }
        public persistentObjectData(string arLocation, string prefabName, string uuID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _arLocation = arLocation;
            _prefabName = prefabName;
            _uuID = uuID;

            _position = position;
            _rotation = rotation;
            _scale = scale;
        }
    }
}
