using UnityEngine;
using System.Collections;
using System;

public class Item_Throwable_Projectile : MonoBehaviour, Poolable, Timer
{
    public Rigidbody thisRigid;
    public Item_Throwable thisPooler;
    public int hitDamage;
    public PhotonView thisPV;

    public string damageLayer;
    public string ignoreDamgeLayer;

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
            gameObject.layer = LayerMask.NameToLayer(ignoreDamgeLayer);
            foreach (Transform t in gameObject.transform.GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer(ignoreDamgeLayer);
        }

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
                        Debug.Log("Player died at cube");

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
        gameObject.layer = LayerMask.NameToLayer(damageLayer);
        foreach (Transform t in gameObject.transform.GetComponentsInChildren<Transform>())
            t.gameObject.layer = LayerMask.NameToLayer(damageLayer);
    }
}
