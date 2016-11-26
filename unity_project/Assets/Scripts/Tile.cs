using UnityEngine;
using System.Collections.Generic;
using Boomlagoon.JSON;
using System;

public abstract class Tile : MonoBehaviour, Health, Attachable, IJSON
{
    //Health
    public float health;
    public bool takeDmg = true;
    public float Sturdy = 10f;
    [SerializeField]
    private LayerMask damagedBy;

    protected float orgHealth;
    string lastAttacker;


    //Health Representation
    public Renderer thisRender;
    public Color damaged;

    public int tileSize = 4;
    Structure s;


    //Decal
    int decalLimit = 10;
    List<Poolable> Attached;

    public Structure thisStructure;

    public string prefabName;

    void Start()
    {
        Attached = new List<Poolable>();
        orgHealth = health;
        thisRender = GetComponent<Renderer>();
        if (thisRender == null)
            Debug.Log("No renderer found on this object, damage color representation will not suceed");

    }

    public virtual bool takeDamage(float damage, string attacker)
    {
        //Debug.Log("Take Damage From Tile");
        GameManager.thisM.SendTileDamage(damage, attacker, thisStructure.name, name);

        if (takeDmg)
        {
            if (health <= 0)
                return false;
            HP -= damage;
            lastAttacker = attacker;
            if (health <= 0)
            {
                Destroy(true, true);
                return true;
            }
        }
        return false;


    }

    public virtual void syncDamage(float damage, string attacker)
    {
        //				Debug.Log ("Sync Damage From Tile");
        if (takeDmg)
        {
            HP -= damage;
            lastAttacker = attacker;
            if (health <= 0)
            {
                Destroy(false, true);
            }
        }
    }

    public void syncDamageWithoutEffects(float damage, string attacker)
    {
        if (takeDmg)
        {
            HP -= damage;
            lastAttacker = attacker;
            if (health <= 0)
            {
                Destroy(false, false);
            }
        }
    }


    public virtual void Destroy(bool local, bool effects)
    {
        if (Attached != null)
        {
            for (int i = 0; i < Attached.Count; i++)
            {
                detach(Attached[0].gameobject);
            }
        }
        thisStructure.editedTiles.Remove(this);
        thisStructure.DestroyTile(this);
        GameObject.Destroy(gameObject);

    }
    public string lastDamageBy()
    {
        return lastAttacker;
    }

    public float HP
    {
        get
        {
            return health;
        }
        set
        {
            if (health == orgHealth)
                thisStructure.editedTiles.Add(this);

            health = value;
            thisRender.material.color = Color.Lerp(damaged, Color.white, health / orgHealth);
        }
    }

    public float Sturdyness
    {
        get
        {
            return Sturdy;
        }

    }

    public int limit
    {
        get
        {
            return decalLimit;
        }

    }

    public List<Poolable> attached
    {
        get
        {
            return Attached;
        }

    }

    public void attach(GameObject g)
    {

        if (g != null)
        {
            Attached.Add(g.GetComponent<Poolable>());
        }
        else
            Debug.Log("Game object you are trying to attach is null");
    }

    public void detach(GameObject g)
    {
        Attached.Remove(g.GetComponent<Poolable>());
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Enter at Tile");

        if (damagedBy == (damagedBy | (1 << collision.gameObject.layer)))
        {
            //Debug.Log("Damage Layer Collision at Player");
            float sdm = GameManager.speedToDamageMultiplier;
            float dmg = Mathf.Pow(collision.relativeVelocity.magnitude, sdm);
            if (collision.relativeVelocity.magnitude > Sturdy)
            {
                //Debug.Log("Engough Damage at Player");
                if (takeDamage(dmg, collision.collider.name))
                {
                    if (collision.collider.attachedRigidbody != null)
                    {
                        collision.collider.attachedRigidbody.velocity *= sdm;
                    }
                }
                else
                {
                    addFX(collision, dmg);
                }


            }
        }
    }

    public virtual void addFX(Collision c, float dmg)
    {
        //Debug.Log("FX Call at Tile");
        EffectsManager.thisM.AddCracksFX(c.contacts[0].normal, c.contacts[0].point, this, dmg / HP);

    }


    public JSONObject ToJSON()
    {
        JSONObject ret = new JSONObject();
        ret.Add("Position", transform.position.ToString());
        ret.Add("Rotation", transform.rotation.ToString());
        ret.Add("Health", health);
        ret.Add("PrefabName", prefabName);
        ret.Add("Name", name);
        ret.Add("Destroyed", false);


        return ret;
    }

    public void FromJSON(JSONObject JSON)
    {
        transform.position = JSONObject.StringToVector3(JSON.GetString("Position"));
        transform.rotation = JSONObject.StringToQuaternion(JSON.GetString("Rotation"));

        health = (float)JSON.GetNumber("Health");

        Attached = new List<Poolable>();
        //orgHealth = health;

        if (thisRender == null)
            Debug.Log("No renderer found on this object, damage color representation will not suceed");
        thisRender.material.color = Color.Lerp(damaged, Color.white, health / orgHealth);

    }
}
