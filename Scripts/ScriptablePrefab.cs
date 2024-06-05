using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "scriptablePrefabID", menuName = "scriptableObjects/scriptablePrefabID")]
public class ScriptablePrefab : ScriptableObject
{
    [SerializeField] private List<GameObject> persistentObjects;
    private Dictionary<string, GameObject> keyValuePairs;
    public void OnEnable()
    {
        foreach(var objects in persistentObjects)
        {
            keyValuePairs.Add(objects.GetComponent<PersistantObjects>().PrefabID, objects);
            Debug.Log($"Adding to Dictionary {objects.GetComponent<PersistantObjects>().PrefabID}, {objects.name}");
        }
    }

    public GameObject ReturnObjectByID(string id)
    {
        return keyValuePairs[id];
    }
}
