using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class GameManager : MonoBehaviour
{
    public static GameManager thisM;
    public static byte Version = 6;
    public string PlayerLayer;
    public string GhostLayer;

    //PlayerStuff------------------------------------------//

    public player[] players;
    public GameObject playerInstantiate;
    public player myPlayer;
    public bool dead = true;
    bool pause = false;



    //LevelStuf------------------------------------------//
    public Level currLevel;
    public PhotonView view;
    public static float speedToDamageMultiplier = 1f;
    public bool loaded = false;
    public GameObject currCam;



    void Awake()
    {
        thisM = this;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
        {
            Application.CaptureScreenshot("Snapshots/Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", 2);
        }
    }



    //Player------------------------------------------//
    public void instantiatePlayer()
    {

        GameObject p;
        int playerID = PhotonNetwork.countOfPlayers;
        SpawnSpot[] spawns = currLevel.sS;
        SpawnSpot thisSpawn = spawns[playerID % spawns.Length];
        GameObject g = (GameObject)GameObject.Instantiate(currLevel.spawnStructure, thisSpawn.getSpawnPoint(), Quaternion.identity, currLevel.StructuresTransform);
        g.name = "PlayerSpawn: " + Persistent.thisPersist.Username + playerID;

        PlayerStructure s = g.GetComponent<PlayerStructure>();
        if (s != null)
        {
            currLevel.structures.Add(s);
            p = PhotonNetwork.Instantiate(playerInstantiate.name,
                         s.spawnPosition.position,
                            s.spawnPosition.rotation, 0, null);
            view.RPC("setStructure", PhotonTargets.Others, s.GenerateJSON().ToString());

        }
        else
        {
            Debug.Log("The currLevel spawnStructure does not have a player sturcture comp. attached");
            return;
        }

        view.RPC("updatePlayers", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);


        myPlayer = p.GetComponent<player>();
        myPlayer.spwanPos = s.spawnPosition.position;
        myPlayer.playerID = playerID;
        //				myPlayer.transform.FindChild ("Graphics").GetComponent<Renderer> ().material.color = player.getPlayerColour (playerID);

        p.layer = LayerMask.NameToLayer(PlayerLayer);
        myPlayer.animModel.layer = LayerMask.NameToLayer(GhostLayer);
        p.GetComponent<Rigidbody>().isKinematic = false;
        NetworkEnable();

    }

    [PunRPC]
    void updatePlayers(int id)
    {
        players = FindObjectsOfType<player>();
        Debug.Log("Updating players for player: " + id + ", Total:" + players.Length);
    }

    public void OnConnected()
    {
        if (PhotonNetwork.isMasterClient)
        {
            currLevel.OnConnected();
            loaded = true;
        }
        else
        {
            view.RPC("getStructuresInit", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
            Debug.Log("Sent(Client) Structures Request");
        }
    }

    //Structure------------------------------------------//
    [PunRPC]
    public void getStructuresInit(int playerId)
    {
        view.RPC("setStructuresInit", PhotonPlayer.Find(playerId), currLevel.getStrucutresInit());
        Debug.Log("Got and Sent(Master) Structures Request");

    }

    [PunRPC]
    public void setStructuresInit(string StructuresJSON)
    {
        Debug.Log("Got(Client) Structures Response");
        PhotonNetwork.isMessageQueueRunning = false;
        currLevel.InitStrucutres(StructuresJSON);
        currLevel.OnConnected();


        loaded = true;
        PhotonNetwork.isMessageQueueRunning = true;

    }

    [PunRPC]
    public void setStructure(String StructuresJSON)
    {
        currLevel.InitStrucutre(JSONObject.Parse(StructuresJSON));
    }

    //Player------------------------------------------//

    public player getPlayerByViewID(int viewID)
    {
        foreach (player p in players)
        {
            if (p.GetComponent<PhotonView>().viewID == viewID)
            {
                return p;
            }
        }
        return null;
    }

    public void ChangeCam(GameObject c)
    {
        if (c != null)
        {
            if (currCam != null)
                currCam.SetActive(false);
            currCam = c;
            currCam.SetActive(true);
        }
        else
        {
            Debug.Log("Tried to change to null camera");
        }
    }

    public void NetworkEnable()
    {
        int playerID = PhotonNetwork.countOfPlayers;
        myPlayer.transform.position = myPlayer.spwanPos;
        myPlayer.transform.rotation = Quaternion.identity;
        //				currLevel.cam.SetActive (false);
        myPlayer.networkInit();
        dead = false;
        UIManager.thisM.changeUI(tileDictionary.thisM.inGameUI);

    }

    public void NetworkDisable()
    {

        ChangeCam(currLevel.cam);
        myPlayer.networkDisable();
        dead = true;
        UIManager.thisM.changeUI(tileDictionary.thisM.pauseUI);
    }

    public void NetworkDisconnect()
    {
        PhotonNetwork.Disconnect();
    }

    //UI------------------------------------------//
    public bool paused
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;
            if (value == true)
            {
                foreach (MonoBehaviour m in myPlayer.networkSet)
                {
                    m.enabled = false;
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
            else
            {
                foreach (MonoBehaviour m in myPlayer.networkSet)
                {
                    m.enabled = true;
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }

        }
    }

    //Tile------------------------------------------//
    public void SendTileDamage(float damage, string attacker, string structureName, string tileName)
    {
        //Debug.Log("send tile dmg");
        view.RPC("SyncTileDamage", PhotonTargets.OthersBuffered, damage, attacker, structureName, tileName);

    }



    [PunRPC]
    public void SyncTileDamage(float damage, string attacker, string structureName, string tileName)
    {
        //Debug.Log("received sync dmg");
        if (!loaded) return;

        Tile thisTile = GetTile(structureName, tileName);
        if (thisTile != null)
        {
            thisTile.syncDamage(damage, attacker);
        }
        else
        {
            Debug.Log("Tile Damage Sync Request for Tile: " + tileName + " does not exist in Structure: " + structureName);
        }
    }


    public Tile GetTile(string StructureName, string TileName)
    {
        Tile thisTile = GetStructure(StructureName).tiles.Find(t => t.name == TileName);
        if (thisTile != null)
        {
            return thisTile;
        }
        Debug.Log("Tile Request for Name: " + TileName + " does not exist");

        return null;
    }

    public Structure GetStructure(string StructureName)
    {
        Structure thisStructure = currLevel.structures.Find(s => s.name == StructureName);
        if (thisStructure != null)
        {
            return thisStructure;
        }
        Debug.Log("Structure Request for Name: " + StructureName + " does not exist");

        return null;
    }
}
