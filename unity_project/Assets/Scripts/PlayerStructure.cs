using UnityEngine;
using System.Collections;

public class PlayerStructure : Structure
{
    public Transform spawnPosition;
    public Transform itemSpot;
    public string spawnItem;

    void Start()
    {
        GameManager.thisM.view.RPC("spawnSceneObject", PhotonTargets.MasterClient, spawnItem, itemSpot.position);
    }
}
