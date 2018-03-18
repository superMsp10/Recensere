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
    public Color startC, endC;
    public Renderer ren;

    private void Start()
    {
        transform.SetParent(tileDictionary.thisM.projectiles, true);
    }

    public void render(List<Vector3> points, float t)
    {
        thisPV.RPC("renderLine", PhotonTargets.All, points.ToArray(), t);
    }

    [PunRPC]
    void renderLine(Vector3[] points, float time)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        StartTimer(time);
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
        while (Time.time - startTime < waitTime)
        {
            ren.material.SetColor("_TintColor", Color.Lerp(startC, endC, (Time.time - startTime) / waitTime));
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
