using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStructure : Structure
{
    public Transform spawnPosition;
    public Transform itemSpot;
    public string spawnItem;

    public Motor shaft;
    public Vector3 open, closed;
    public float totalTime;
    public Text objectiveText;


    void Start()
    {
        if (PhotonNetwork.connected)
            GameManager.thisM.view.RPC("spawnSceneObject", PhotonTargets.MasterClient, spawnItem, itemSpot.position);
    }

   public void closeLid()
    {
        shaft.moving = true;
        shaft.totalTime = totalTime;
        shaft.start = shaft.lookAt.localPosition;
        shaft.end = closed;
        shaft.startedTime = Time.time;

        Invoke("stopMoving", totalTime);
    }

    public void openLid()
    {
        shaft.moving = true;
        shaft.totalTime = totalTime;
        shaft.start = shaft.lookAt.localPosition;
        shaft.end = open;
        shaft.startedTime = Time.time;

        Invoke("stopMoving", totalTime);
    }

    void stopMoving()
    {
        shaft.moving = false;
    }

}
