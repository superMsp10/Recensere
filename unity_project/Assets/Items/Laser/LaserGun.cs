using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Item_Throwable
{
    public Transform laserStart;
    public float shootRange = 5f;
    public float dmg = 10f;
    public float knockback;

    public new void Update()
    {
        if (selected)
        {
            if (ren == null)
            {
                Debug.Log("No renderer on this object", this);
                return;
            }
            if (startedHold)
            {
                ren.material.color = Color.Lerp(normal, highlighted, (Time.time - timeStarted) / maximumHeldTime);
                ren.material.SetColor("_EmissionColor", Color.Lerp(normal, highlighted, (Time.time - timeStarted) / maximumHeldTime));
            }
            else
            {
                ren.material.color = normal;
                ren.material.SetColor("_EmissionColor", normal);

            }
        }
    }

    public override bool buttonUP()
    {
        float heldTime = Time.time - timeStarted;

        if (heldTime <= minimumHeldTime)
        {
            startedHold = false;
            return false;
        }

        if (thisView != null)
        {
            thisView.RPC("buttonUpBy", PhotonTargets.All, null);
        }
        else
        {
            buttonUpBy();
        }



        //Projectile Stuff
        GameObject g = projectilePooler.getObject();
        //Set Transform to this and reset Timer
        g.transform.position = laserStart.position;
        LaserProjectile c = g.GetComponent<LaserProjectile>();
        c.thisPooler = this;
        c.render(shoot(laserStart.position, Camera.main.transform.forward, 0, new List<Vector3>() { laserStart.TransformPoint(Vector3.zero) }, c.lineRenderer),itemReset);

        return false;
    }

    List<Vector3> shoot(Vector3 startPoint, Vector3 direction, float distanceCovered, List<Vector3> points, LineRenderer renderer)
    {
        RaycastHit info;
        if (Physics.Raycast(startPoint, direction, out info, 500f))
        {
            points.Add(info.point);

            Rigidbody r = info.collider.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.AddForceAtPosition(direction * knockback, info.point, ForceMode.Impulse);
            }

            Health h = info.collider.GetComponent<Health>();
            if (h != null)
            {
                h.takeDamage(dmg, "player" + thisPlayer.playerID.ToString());
            }

            distanceCovered += Vector3.Distance(startPoint, info.point);
            if (distanceCovered < shootRange)
            {
                shoot(info.point, Vector3.Reflect(direction, info.normal), distanceCovered, points, renderer);
            }
            else
            {
                return points;
            }

        }
        else
        {
            points.Add(renderer.transform.TransformPoint(direction * shootRange));
        }
        return points;
    }

}
