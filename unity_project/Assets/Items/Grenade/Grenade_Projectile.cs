using UnityEngine;
using System.Collections;


class Grenade_Projectile : Item_Throwable_Projectile
{
    public float explosionDelay = 2f;
    public ParticleSystem explosionFX_Fireball;
    public ParticleSystem explosionFX_Dust;


    public override void reset(bool on)
    {
        gameObject.SetActive(on);
        r.isKinematic = !on;
        transform.SetParent(tileDictionary.thisM.projectiles, true);


        if (on)
        {
            Debug.Log("Grenade projectile enabled");
            r.velocity = Vector3.zero;
            Invoke("_explode", explosionDelay);

        }


    }

    void _explode()
    {
        Debug.Log("Grenade explosion client");
        thisPV.RPC("explode", PhotonTargets.All, null);
    }

    [PunRPC]
    void explode()
    {
        Debug.Log("Grenade explosion rpc");
        explosionFX_Dust.Play();
        explosionFX_Fireball.Play();
    }

}

