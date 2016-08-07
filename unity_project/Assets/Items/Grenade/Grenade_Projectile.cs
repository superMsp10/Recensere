using UnityEngine;
using System.Collections;


class Grenade_Projectile : Item_Throwable_Projectile
{
    public float explosionDelay = 2f;
    public ParticleSystem explosionFX_Fireball;
    public ParticleSystem explosionFX_Dust;
    public Renderer thisRenderer;
    public Collider thisCollider;
    public Rigidbody thisRigid;
    public GameObject destroyedGrenade;

    public float explosionRadius, explosionForce;
    public LayerMask explodingLayers;
    public float minDamage, maxDamage;



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

            thisCollider.enabled = true;
            thisRenderer.enabled = true;
            thisRigid.isKinematic = false;
            destroyedGrenade.SetActive(false);

        }


    }

    void _explode()
    {
        Debug.Log("Grenade explosion client");
        thisPV.RPC("explode", PhotonTargets.All, null);

        foreach (Collider c in Physics.OverlapSphere(transform.position, explosionRadius, explodingLayers))
        {
            Health h = c.GetComponent<Health>();
            if (h != null)
            {
                float dmg = Mathf.Lerp(minDamage, maxDamage, Vector3.Distance(c.transform.position, transform.position) / explosionRadius);
                h.takeDamage(dmg, "GrenadeExplosion");
            }
        }

        Invoke("explosionKnockback", 0.1f);


    }

    void explosionKnockback()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, explosionRadius, explodingLayers);
        foreach (Collider c in colls)
        {
            Rigidbody r = c.GetComponent<Rigidbody>();
            if (r != null && !r.isKinematic)
            {
                r.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    [PunRPC]
    void explode()
    {
        Debug.Log("Grenade explosion rpc");
        explosionFX_Dust.Play();
        explosionFX_Fireball.Play();

        thisCollider.enabled = false;

        thisRenderer.enabled = false;
        destroyedGrenade.SetActive(true);
        thisRigid.isKinematic = true;
    }

}

