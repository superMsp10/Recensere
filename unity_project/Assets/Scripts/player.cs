using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour, Health
{
    GameManager thisM = GameManager.thisM;
    //Health
    public float health;
    public float originalHealth;
    public string lastAttacker;
    public bool takeDmg = true;
    public float Sturdy = 10f;
    public LayerMask damagedBy;

    //healthUI
    private Text HPText;

    //Network
    public MonoBehaviour[] networkSet;
    public int playerID;
    public PhotonView thisView;

    public GameObject thisCam;
    public MouseLook look;
    public MouseLook look2;

    //Items
    public Transform left_hand;
    public Transform right_hand;
    public string handLayer;

    //Animation
    public Animator anim;
    public Rigidbody r;
    public GameObject animModel;

    public Transform spwanPos;
    public PlayerStructure spwanRoom;


    //SFX
    public AudioSource source;
    public AudioClip pickupOpeningClip;
    public AudioClip damageStart;
    public AudioClip damageFinal;

    public float soundMaxLength, soundMinLength;
    List<string> deathMessages = new List<string>() { "You hit it too <color=red>hard</color>",
        "<color=purple>Sufaces: 1, You : 0</color>", "<color=grey>Who put that there?</color>", "<color=green>Splat!</color>",
        "<size=36>STOP, Hammer time!</size>", "<color=orange>Collision velocity > Health</color>" };

    void FixedUpdate()
    {
        anim.SetFloat("YVelo", r.velocity.y);
    }

    [PunRPC]
    public void PlayRandomPickup()
    {
        source.clip = pickupOpeningClip;
        source.time = Random.Range(0, source.clip.length);
        source.Play();
        Invoke("StopSFX", Random.Range(soundMinLength, soundMaxLength));
    }

    public void StopSFX()
    {
        source.Stop();
    }


    public virtual bool takeDamage(float damage, string attacker)
    {
        if (thisView.isMine)
        {
            if (health <= 0)
                return false;
            HP -= damage;
            lastAttacker = attacker;
            if (health <= 0)
            {
                SoundManager.thisM.sourceFX.PlayOneShot(damageFinal);
                thisView.RPC("playHealthSound", PhotonTargets.Others, true);

                Destroy(true, true);
                return true;
            }
            else
            {
                SoundManager.thisM.sourceFX.PlayOneShot(damageStart);
                thisView.RPC("playHealthSound", PhotonTargets.Others, false);

            }
        }
        else
        {

            //Debug.Log("Damage by syncing for  " + attacker);
            thisView.RPC("syncDamage", thisView.owner, damage, attacker);
        }
        return false;

    }



    [PunRPC]
    public virtual void syncDamage(float damage, string attacker)
    {
        if (thisView.isMine)
        {
            if (health <= 0)
                return;

            HP -= damage;
            lastAttacker = attacker;
            if (health <= 0)
            {
                SoundManager.thisM.sourceFX.PlayOneShot(damageFinal);

                Destroy(true, true);
            }
            else
            {
                SoundManager.thisM.sourceFX.PlayOneShot(damageStart);
            }
        }
    }

    [PunRPC]
    public void playHealthSound(bool dead)
    {
        if (dead)
        {
            source.PlayOneShot(damageFinal);
        }
        else
            source.PlayOneShot(damageStart);
    }


    public virtual void Destroy(bool local, bool effects)
    {


        if (lastAttacker.Contains("player"))
        {
            PhotonPlayer p = PhotonPlayer.Find(int.Parse(lastAttacker.Replace("player", "")));

            if (p != null)
            {
                if (!p.IsLocal)
                    thisM.view.RPC("addKills", p);
            }

        }

        Invoke("respawn", 0.1f);

    }

    public void OnDestroy()
    {
            thisM.players.Remove(this);
    }

    void respawn()
    {
        thisM.NetworkDisable();
        StartCoroutine(tileDictionary.thisM.pauseUI.GetComponent<pauseUI>().Respawn(5f));
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
            health = value;
            if (thisView.isMine)
                HPText.text = "Health: " + value;
        }
    }

    public float Sturdyness
    {
        get
        {
            return Sturdy;
        }

    }

    public void networkInit()
    {

        thisM.ChangeCam(thisCam);
        //				thisCam.gameObject.SetActive (true);
        foreach (MonoBehaviour m in networkSet)
        {
            m.enabled = true;
        }
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = new Vector3(0, 0, 0);
        r.useGravity = true;

        playerMove pm = GetComponent<playerMove>();
        pm.Start();

        HPText = tileDictionary.thisM.HPText;
        HP = originalHealth;
    }

    public void networkDisable()
    {
        //				thisCam.gameObject.SetActive (false);
        foreach (MonoBehaviour m in networkSet)
        {
            m.enabled = false;
        }

    }




    public static Color getPlayerColour(int playerID)
    {
        int team = playerID % 4;
        switch (team)
        {
            case 0:
                return Color.blue;

            case 1:
                return Color.red;

            case 2:
                return Color.green;

            case 3:
                return Color.magenta;


            default:
                break;
        }

        return Color.blue;

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Enter at Player");
        if (damagedBy == (damagedBy | (1 << collision.gameObject.layer)))
        {
            //Debug.Log("Damage Layer Collision at Player");
            float sdm = GameManager.speedToDamageMultiplier;
            float dmg = Mathf.Pow(collision.relativeVelocity.magnitude, sdm);
            if (takeDmg && collision.relativeVelocity.magnitude > Sturdy)
            {

                //Debug.Log("Engough Damage at Player");
                if (takeDamage(dmg, collision.collider.name))
                {
                    tileDictionary.thisM.pauseUI.GetComponent<pauseUI>().deathMessage = deathMessages[UnityEngine.Random.Range(0, deathMessages.Count)];

                    if (collision.collider.attachedRigidbody != null)
                    {
                        collision.collider.attachedRigidbody.velocity *= sdm;
                    }
                }


            }
        }
    }
}
