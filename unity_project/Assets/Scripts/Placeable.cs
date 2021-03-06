﻿using UnityEngine;
using System.Collections;
using System;

public class Placeable : MonoBehaviour, Holdable
{
    //Holdable Stuff
    public Sprite _holdUI;
    public bool _pickable;
    public int _amount;
    public int _stackSize = 16;
    public string _description = "<b>Hello! <color=red>My name is, </color> Placeable, </b>I can be <i><color=blue>placed</color> test!</i>";
    bool pickedBefore = false;


    //Placeable Stuff
    public float range;
    public Collider trigger;
    public Color valid, invalid, normal;
    bool inverted = false;
    public Renderer[] thisRender;
    public LayerMask rangeRayHits;
    public Vector3 defaultPos;
    protected bool collided = false;
    public GameObject instantiateObject;
    public float dockingOffset = 0.85f;
    public float perpendicularOffset = 0.85f;


    public PhotonView thisView;
    protected bool selected = false;
    public player thisPlayer;
    public string itemLayer;
    GameManager thisM;
    Camera thisC;
    bool docked = false;
    bool perpendicular = false;

    // Use this for initialization
    void Start()
    {
        thisM = GameManager.thisM;
        if (!GameManager.thisM.loaded && !PhotonNetwork.isMasterClient)
            thisView.RPC("getInit", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
    }



    // Update is called once per frame
    void Update()
    {

        if (selected)
        {
            if (thisC == null)
                thisC = thisM.currCam.GetComponent<Camera>();

            RaycastHit hit;
            Ray ray = thisC.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));


            if (Physics.Raycast(ray, out hit, range, rangeRayHits))
            {
                if (perpendicular)
                {
                    Vector3 perpendi = Vector3.Cross(hit.normal, thisC.transform.right);
                    transform.rotation = Quaternion.LookRotation(new Vector3(perpendi.x, perpendi.y, perpendi.z));
                    transform.position = hit.point + (hit.normal * perpendicularOffset);

                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(hit.normal);
                    transform.position = hit.point + (hit.normal * dockingOffset);
                }
                docked = true;
            }
            else
            {
                if (docked)
                {
                    transform.localPosition = defaultPos;
                    transform.localRotation = Quaternion.identity;

                    docked = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (!docked)
                    transform.Rotate(45, 0, 0);
                else
                    perpendicular = !perpendicular;
            }



        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (pickable)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer(GameManager.thisM.PlayerLayer))
            {
                if (invManager.thisInv.addHoldable(this, _amount) <= 0)
                {
                    thisView.RPC("pickedUpBy", PhotonTargets.All, collision.collider.gameObject.GetComponent<player>().playerID);
                    _pickable = false;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (collided == false && selected == true)
        {
            collided = true;
            validityColours(!collided);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (selected)
        {
            collided = true;
            validityColours(!collided);
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (selected)
        {
            collided = false;
            validityColours(!collided);
        }
    }

    void validityColours(bool validity)
    {
        foreach (Renderer item in thisRender)
        {
            if (validity)
            {

                item.material.color = valid;
            }
            else
            {
                item.material.color = invalid;

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
        thisView.RPC("buttonDownBy", PhotonTargets.All, null);

        return false;
    }
    public virtual bool buttonUP()
    {
        thisView.RPC("buttonUpBy", PhotonTargets.All, null);
        if (!collided)
        {
            Instantiate(instantiateObject, transform.position, transform.rotation, null);
            amount--;
            return true;

        }
        return false;
    }
    public void onSelect()
    {
        thisView.RPC("selectedBy", PhotonTargets.All, null);
    }
    public void onDeselect()
    {
        thisView.RPC("deselectedBy", PhotonTargets.All, null);
    }
    public void onPickup()
    {
        thisView.TransferOwnership(PhotonNetwork.player.ID);

        if (!pickedBefore)
        {
            pickedBefore = true;
            GameManager.thisM.addNewItemsPicked();
        }
    }

    public void onDrop()
    {
        thisView.RPC("droppedBy", PhotonTargets.All, null);

    }

    public void onRemove()
    {
        PhotonNetwork.Destroy(gameObject);
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

    }
    [PunRPC]
    protected virtual void buttonUpBy()
    {

        //Debug.Log("buttonUP by Cube");
    }
    [PunRPC]
    protected virtual void selectedBy()
    {
        //Debug.Log("onSelected by Cube");
        gameObject.SetActive(true);
        selected = true;
        collided = false;
        validityColours(!collided);
    }
    [PunRPC]
    protected virtual void deselectedBy()
    {
        //Debug.Log("onDeselect by Cube");
        gameObject.SetActive(false);
        selected = false;

    }
    [PunRPC]
    protected virtual void pickedUpBy(int viewID)
    {
        thisPlayer = GameManager.thisM.getPlayerByPlayerID(viewID);
        _pickable = false;
        //gameObject.layer = LayerMask.NameToLayer(thisPlayer.handLayer);
        transform.parent = thisPlayer.right_hand;
        transform.localPosition = defaultPos;
        transform.localRotation = Quaternion.identity;

        if (!selected)
            gameObject.SetActive(false);
        trigger.isTrigger = true;

        foreach (Renderer item in thisRender)
        {
            item.material.color = valid;
        }

        if (thisView.isMine)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }

    }
    [PunRPC]
    protected virtual void droppedBy()
    {
        //				Debug.Log ("onDrop by Cube");
        transform.parent = GameManager.thisM.currLevel.items;
        gameObject.layer = LayerMask.NameToLayer(itemLayer);

        gameObject.SetActive(true);
        Invoke("resetPick", 5.0f);
        trigger.isTrigger = false;
        selected = false;

        foreach (Renderer item in thisRender)
        {
            item.material.color = normal;
        }

    }
    [PunRPC]
    protected virtual void resetPickBy()
    {
        //				Debug.Log ("ResetPick by Cube");
        _pickable = true;

    }

}
