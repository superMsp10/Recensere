using UnityEngine;
using System.Collections;

public class Cube_Projectile : Item_Throwable_Projectile
{
    public override void reset(bool on)
    {
        base.reset(on);
        thisRigid.isKinematic = !on;

        transform.SetParent(tileDictionary.thisM.projectiles, true);
        if (!on)
        {
            thisRigid.velocity = Vector3.zero;
        }
      


    }


}
