using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Item_Throwable {

    public override bool buttonUP()
    {
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
        g.transform.position = transform.position;
        g.GetComponent<Timer>().StartTimer(itemReset);

        LaserProjectile c = g.GetComponent<LaserProjectile>();
        c.thisPooler = this;

        return false;


    }

}
