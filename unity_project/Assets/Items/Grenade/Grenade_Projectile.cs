using UnityEngine;
using System.Collections;


class Grenade_Projectile : Item_Throwable_Projectile
{
    public float explosionDelay = 2f;
    public ParticleSystem[] explosionFX;
    public Renderer thisRenderer;
    public Collider thisCollider;
    public GameObject destroyedGrenade;
    private Transform prefabTransform;

    public float explosionRadius, explosionForce;
    public LayerMask explodingLayers;
    public float minDamage, maxDamage;

    //SFX
    public AudioClip explosionSFX;



    public override void reset(bool on)
    {
        base.reset(on);
        transform.SetParent(tileDictionary.thisM.projectiles, true);
        if (!on)
        {
            //Debug.Log("Grenade projectile enabled");
            thisRigid.velocity = Vector3.zero;

            thisCollider.enabled = true;
            thisRigid.isKinematic = false;
            thisRenderer.enabled = true;
            destroyedGrenade.SetActive(false);

            prefabTransform = tileDictionary.thisM.destroyedGrenadeProjectile.transform;
            for (int i = 0; i < destroyedGrenade.transform.childCount; i++)
            {
                Transform t = destroyedGrenade.transform.GetChild(i);
                t.GetComponent<Rigidbody>().velocity = Vector3.zero;
                t.transform.localPosition = prefabTransform.GetChild(i).localPosition;
                t.transform.localRotation = prefabTransform.GetChild(i).localRotation;
            }

            CancelInvoke();
        }
        else
        {
            Invoke("_explode", explosionDelay);
        }


    }

    void _explode()
    {
        //Debug.Log("Grenade explosion client");
        thisPV.RPC("explode", PhotonTargets.All, null);

        foreach (Collider c in Physics.OverlapSphere(transform.position, explosionRadius, explodingLayers))
        {
            Health h = c.GetComponent<Health>();
            if (h != null)
            {
                float dmg = Mathf.Lerp(maxDamage, minDamage, Vector3.Distance(c.transform.position, transform.position) / explosionRadius);
                h.takeDamage(dmg, "player" + belongsTo.playerID.ToString());
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
        //Debug.Log("Grenade explosion rpc");
        foreach (ParticleSystem s in explosionFX)
        {
            s.Play();
        }

        thisCollider.enabled = false;
        thisRigid.isKinematic = true;
        thisRenderer.enabled = false;
        destroyedGrenade.SetActive(true);

        AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
    }

}

