using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour, Poolable, Timer
{
    public PhotonView thisPV;
    public LaserGun thisPooler;
    public LineRenderer lineRenderer;


    public GameObject gameobject
    {
        get
        {
            return gameObject;
        }
    }

    public virtual void reset(bool on)
    {
        gameObject.SetActive(on);

        if (on)
        {

        }
        else
        {

        }
        thisPV.RPC("NetworkReset", PhotonTargets.Others, on);
    }

    [PunRPC]
    protected void NetworkReset(bool on)
    {
        gameObject.SetActive(on);
    }

    public void StartTimer(float time)
    {
        Invoke("TimerComplete", time);

    }

    public void CancelTimer()
    {
        CancelInvoke("TimerComplete");

    }
    public void TimerComplete()
    {
        if (thisPooler != null)
        {
            thisPooler.detach(gameobject);
        }
    }

}
