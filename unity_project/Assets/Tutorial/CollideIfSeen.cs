using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideIfSeen : MonoBehaviour
{

    public Collider c;
    public float resetTime, lastTime;
    public bool reset = false;
    public Color trigger, collision;
    public Renderer ren;

    private void OnBecameVisible()
    {
        if (reset)
            c.isTrigger = false;
    }

    private void OnBecameInvisible()
    {
        if (reset)
        {
            c.isTrigger = true;
            lastTime = Time.time;
            reset = false;
            StartCoroutine(resetAnimation());
        }

    }

    IEnumerator resetAnimation()
    {
        float delta = Time.time - lastTime;
        while (delta < resetTime)
        {
            ren.material.SetColor("_TintColor", Color.Lerp(trigger, collision, delta / resetTime));
            delta = Time.time - lastTime;
            yield return new WaitForSeconds(0.2f);
        }
        reset = true;
        c.isTrigger = false;
    }
}
