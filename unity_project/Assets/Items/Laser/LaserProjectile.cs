using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour, Poolable, Timer
{
    public PhotonView thisPV;
    public LaserGun thisPooler;
    public LineRenderer lineRenderer;
    public float stepsDuration = 0.5f, startTime, waitTime = 1f;

    private void Start()
    {
        transform.SetParent(tileDictionary.thisM.projectiles, true);
    }

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

    protected IEnumerator step()
    {

        Debug.Log("hello");
        while (Time.time - startTime > waitTime)
        {
            Debug.Log(Time.time - startTime);
            yield return new WaitForSeconds(stepsDuration);
        }
        TimerComplete();
    }

    public void StartTimer(float time)
    {
        startTime = Time.time;
        waitTime = time;
        StartCoroutine(step());
    }

    public void CancelTimer()
    {
        StopAllCoroutines();
    }

    public void TimerComplete()
    {
        if (thisPooler != null)
        {
            thisPooler.detach(gameobject);
        }
    }

}
