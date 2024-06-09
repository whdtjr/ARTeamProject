using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScriptablePrefabID", menuName = "ScriptableObjects/ScriptablePrefabID")]
public class CreateGaneObject : ScriptableObject
{
    [SerializeField] private List<GameObject> persistantObjects;
    private Dictionary<string, GameObject> keyToObject = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    public void OnEnable()
    {
        if(persistantObjects.Count == 0)
        {
            Debug.Log("No Persistant Objects Found");
            return;
        }

        foreach(var item in persistantObjects)
        {
            keyToObject.Add(item.GetComponent<PersistentObjects>().PrefabName, item);
            Debug.Log("added " + item.GetComponent<PersistentObjects>().PrefabName);
        }
    }

    public GameObject GetObject(string prefabID)
    {
        return keyToObject[prefabID];
    }
}
