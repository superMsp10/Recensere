using UnityEngine;
using System.Collections;

public class PlayerStructure : Structure
{
    public Transform spawnPosition;
    public Transform itemSpot;
    public string spawnItem;

    void Start()
    {
       //PhotonNetwork.InstantiateSceneObject(spawnItem, itemSpot.position, Quaternion.identity, 0, null);
    }
}
