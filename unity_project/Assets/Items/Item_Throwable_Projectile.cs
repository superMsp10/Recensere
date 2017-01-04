using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Item_Throwable_Projectile : MonoBehaviour, Poolable, Timer
{
    public Rigidbody thisRigid;
    public Item_Throwable thisPooler;
    public int hitDamage;
    public PhotonView thisPV;
    public player belongsTo;

    public string damageLayer;
    public string ignoreDamgeLayer;
    public List<Transform> changeLayer = new List<Transform>();

    public bool armed = false;
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
            foreach (Transform t in changeLayer)
            {
                t.gameObject.layer = LayerMask.NameToLayer(ignoreDamgeLayer);
            }

        }
        thisPV.RPC("NetworkReset", PhotonTargets.Others, on);
    }

    [PunRPC]
    protected void NetworkReset(bool on)
    {
        gameObject.SetActive(on);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (armed)
        {
            player p = collision.gameObject.GetComponent<player>();

            if (p != null)
            {

                if (collision.relativeVelocity.magnitude > p.Sturdy)
                {
                    if (p.takeDamage(hitDamage, collision.collider.name))
                    {
                        //Debug.Log("Player died at cube");

                    }
                    armed = false;
                }


            }
        }
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

    public void SetLocal()
    {
        foreach (Transform t in changeLayer)
        {
            t.gameObject.layer = LayerMask.NameToLayer(damageLayer);
        }
    }

}
