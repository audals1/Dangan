using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class  Location
{
    public string name;
    public Transform tf_SpawnLocation;
}

public class TransferSpawnManager : MonoBehaviour
{
    [SerializeField] Location[] locations;
    Dictionary<string, Transform> locationDic = new Dictionary<string, Transform>();

    public static bool canSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locationDic.Add(locations[i].name, locations[i].tf_SpawnLocation);
        }

        if (canSpawn)
        {
            TransferManager transferManager = FindObjectOfType<TransferManager>();
            string t_LocationName = transferManager.GetLocationName();
            Transform t_SpawnLocation = locationDic[t_LocationName];
            PlayerController.instance.transform.position = t_SpawnLocation.position;
            PlayerController.instance.transform.rotation = t_SpawnLocation.rotation;
            Camera.main.transform.localPosition = new Vector3(0, 1, 0);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            PlayerController.instance.ResetAngle();
            canSpawn = false;

            StartCoroutine(transferManager.Done());
        }
    }
}
