using UnityEngine;
using System.Collections;

public class Item_Throwable : MonoBehaviour, Holdable
{


    //Holdable Stuff
    public Sprite _holdUI;
    public bool _pickable;
    public int _amount;
    public int _stackSize = 16;
    public string _description = "<b>Hello! <color=red>My name is, </color> Cube, </b>I will be <i><color=blue>helping you</color> test!</i>";
    bool pickedBefore = false;


    //Pooler
    [HideInInspector]
    public NetworkPooler
            projectilePooler = null;
    public float itemReset;
    public int maxItems;
    public GameObject projectile;
    public float throwMultiplier;
    public string projectileLayer;
    public string itemLayer;

    //Item Stuff
    public Rigidbody r;
    public player thisPlayer;
    public Color normal;
    public Color highlighted;
    float timeStarted;
    float timeEnded;
    public float maximumHeldTime;
    public float minimumHeldTime = 0.5f;
    public float defaultHeldTime;
    public PhotonView thisView;
    protected bool selected = false;
    bool startedHold = false;
    Renderer ren;

    void Start()
    {
        projectilePooler = new NetworkPooler(maxItems, projectile);
        ren = GetComponent<Renderer>();
        if (thisView != null)
        {
            if (!GameManager.thisM.loaded && !PhotonNetwork.isMasterClient)
                thisView.RPC("getInit", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
        }
    }



    //Item------------------------------------------//
    public void Update()
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
            }
            else
            {
                ren.material.color = normal;
            }
        }
    }

    public void detach(GameObject g)
    {
        projectilePooler.disposeObject(g.GetComponent<Poolable>());

    }

    void OnCollisionEnter(Collision collision)
    {
        if (pickable)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer(GameManager.thisM.PlayerLayer))
            {
                //Debug.Log("OnCollisionEnter, Cube and Player Amount: " + _amount);
                //								p = collision.collider.gameObject.GetComponent<player> ();
                if (invManager.thisInv.addHoldable(this, _amount) <= 0)
                {
                    if (thisView != null)
                    {
                        thisView.RPC("pickedUpBy", PhotonTargets.All, collision.collider.gameObject.GetComponent<player>().playerID);

                    }
                    else
                    {
                        pickedUpBy(collision.collider.gameObject.GetComponent<player>().playerID);
                    }
                    _pickable = false;
                }
            }
        }
    }

    //Holdable------------------------------------------//
    public Sprite holdUI
    {
        get
        {
            return _holdUI;
        }

    }
    public int stackSize
    {
        get
        {
            return _stackSize;
        }

    }
    public string description
    {
        get
        {
            return _description;
        }

    }
    public bool pickable
    {
        get
        {
            return _pickable;
        }

    }
    public int amount
    {
        get
        {
            return _amount;
        }
        set
        {
            _amount = value;
        }
    }

    public bool buttonDown()
    {
        if (thisView != null)
        {
            thisView.RPC("buttonDownBy", PhotonTargets.All, null);
        }
        else
        {
            buttonDownBy();
        }


        return false;
    }
    public bool buttonUP()
    {
        if (thisView != null)
        {
            thisView.RPC("buttonUpBy", PhotonTargets.All, null);
        }
        else
        {
            buttonUpBy();
        }


        //Apply Force
        float force = 0f;
        float heldTime = Time.time - timeStarted;

        if (heldTime <= minimumHeldTime)
            return false;

        //Projectile Stuff
        GameObject g = projectilePooler.getObject();
        //Set Transform to this and reset Timer
        g.transform.position = transform.position;
        g.transform.rotation = transform.rotation;
        g.GetComponent<Timer>().StartTimer(itemReset);
        Item_Throwable_Projectile c = g.GetComponent<Item_Throwable_Projectile>();
        c.thisPooler = this;
        c.belongsTo = thisPlayer;
        //Arming Object
        c.armed = true;
        c.SetLocal();

        if (heldTime > maximumHeldTime)
            force = throwMultiplier;
        else
            force = ((heldTime + defaultHeldTime) / maximumHeldTime) * throwMultiplier;
        g.GetComponent<Rigidbody>().AddForce(thisPlayer.left_hand.forward * force);

        amount--;
        return true;


    }
    public void onSelect()
    {
        if (thisView != null)
        {
            thisView.RPC("selectedBy", PhotonTargets.All, null);

        }
        else
        {
            selectedBy();
        }
    }
    public void onDeselect()
    {
        if (thisView != null)
        {

            thisView.RPC("deselectedBy", PhotonTargets.All, null);
        }
        else
        {
            deselectedBy();
        }
    }
    public void onPickup()
    {
        //				Debug.Log ("onPickup by Cube");
        if (thisView != null)
        {

            thisView.TransferOwnership(PhotonNetwork.player.ID);
        }

        if (!pickedBefore)
        {
            pickedBefore = true;
            GameManager.thisM.addNewItemsPicked();
        }
    }
    public void onDrop()
    {
        if (thisView != null)
        {

            thisView.RPC("droppedBy", PhotonTargets.All, null);
        }
        else
        {
            droppedBy();
        }

    }

    public void onRemove()
    {
        gameObject.SetActive(false);
        Invoke("DestroyItem", 5f);
    }

    void DestroyItem()
    {
        if (thisView != null)
            PhotonNetwork.Destroy(gameObject);
        projectilePooler.OnDestroy();
    }

    public void resetPick()
    {
        thisView.RPC("resetPickBy", PhotonTargets.All, null);
    }

    //RPCs------------------------------------------//

    [PunRPC]
    public void getInit(int clientPlayerID)
    {
        //Debug.Log("getInit on" + name);
        if (thisPlayer != null)
        {
            thisView.RPC("setInit", PhotonPlayer.Find(clientPlayerID), thisPlayer.playerID, selected);
        }
    }


    [PunRPC]
    public void setInit(int setPlayerID, bool _selected)
    {
        Debug.Log("setInit on" + name);
        pickedUpBy(setPlayerID);
        if (_selected)
        {
            selectedBy();
        }
    }

    [PunRPC]
    protected virtual void buttonDownBy()
    {
        //				Debug.Log ("buttonDown by Cube");
        timeStarted = Time.time;
        startedHold = true;
    }
    [PunRPC]
    protected virtual void buttonUpBy()
    {
        timeEnded = Time.time;
        startedHold = false;
        //Debug.Log("buttonUP by Cube");
    }
    [PunRPC]
    protected virtual void selectedBy()
    {
        //Debug.Log("onSelected by Cube");
        gameObject.SetActive(true);
        selected = true;
    }
    [PunRPC]
    protected virtual void deselectedBy()
    {
        //Debug.Log("onDeselect by Cube");
        gameObject.SetActive(false);
        selected = false;

    }
    [PunRPC]
    protected virtual void pickedUpBy(int playerID)
    {
        GetComponent<Renderer>().material.color = normal;
        thisPlayer = GameManager.thisM.getPlayerByPlayerID(playerID);
        _pickable = false;
        r.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer(thisPlayer.handLayer);
        transform.parent = thisPlayer.right_hand;
        transform.position = thisPlayer.right_hand.position;
        transform.rotation = thisPlayer.right_hand.rotation;
        if (!selected)
            gameObject.SetActive(false);

        if (thisView.isMine)
        {
            enabled = true;
        }
        else
            enabled = false;
    }
    [PunRPC]
    protected virtual void droppedBy()
    {
        //				Debug.Log ("onDrop by Cube");
        GetComponent<Renderer>().material.color = normal;
        r.velocity = Vector3.zero;
        r.isKinematic = false;
        transform.parent = GameManager.thisM.currLevel.items;
        gameObject.layer = LayerMask.NameToLayer(itemLayer);

        gameObject.SetActive(true);
        //				transform.position = p.transform.position;
        Invoke("resetPick", 5.0f);
    }
    [PunRPC]
    protected virtual void resetPickBy()
    {
        //				Debug.Log ("ResetPick by Cube");
        _pickable = true;

    }





}
