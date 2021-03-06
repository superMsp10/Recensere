﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DefaultMapPlayerStructure : PlayerStructure
{

    public Transform itemSpot;
    public string spawnItem;

    public Motor shaft;
    public Vector3 open, closed;
    public float totalTime;
    public Text objectiveText;

    public new void Start()
    {
        base.Start();
        if (PhotonNetwork.connected && isLocal)
        {
            GameManager.thisM.view.RPC("spawnSceneObject", PhotonTargets.MasterClient, spawnItem, itemSpot.position);
        }
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

    public void stopMoving()
    {
        shaft.moving = false;
    }

  

}
