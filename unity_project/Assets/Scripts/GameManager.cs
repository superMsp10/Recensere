using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class GameManager : MonoBehaviour
{
    public static GameManager thisM;
    public static byte Version = 8;
    public string PlayerLayer;
    public string GhostLayer;

    public string endGameScene;

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

    //Items------------------------------------------//
    [PunRPC]
    void spawnSceneObject(string prefabName, Vector3 position)
    {
        GameObject g = (GameObject)PhotonNetwork.InstantiateSceneObject(prefabName, position, Quaternion.identity, 0, null);
        g.SetActive(true);
    }

    //Player------------------------------------------//
    public void instantiatePlayer()
    {

        GameObject p;
        int playerID = PhotonNetwork.player.ID;
        SpawnSpot[] spawns = currLevel.sS;
        SpawnSpot thisSpawn = spawns[playerID % spawns.Length];
        GameObject g = (GameObject)GameObject.Instantiate(currLevel.spawnStructure, thisSpawn.getSpawnPoint(), thisSpawn.transform.rotation, currLevel.StructuresTransform);
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
        myPlayer.spwanRoom = s;
        myPlayer.spwanPos = s.spawnPosition;
        myPlayer.playerID = playerID;
        //				myPlayer.transform.FindChild ("Graphics").GetComponent<Renderer> ().material.color = player.getPlayerColour (playerID);

        p.layer = LayerMask.NameToLayer(PlayerLayer);
        myPlayer.animModel.layer = LayerMask.NameToLayer(GhostLayer);
        p.GetComponent<Rigidbody>().isKinematic = false;

        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", 0 }, { "Placed", 0 }, { "NewItemsPicked", 0 }, { "Destroyed", 0 }, { "Deaths", 0 } });
        ObjectivesManeger.thisM.Initialize();
        NetworkEnable();
    }

    [PunRPC]
    public void addKills()
    {
        PhotonNetwork.player.customProperties["Kills"] = getHashInt(PhotonNetwork.player.customProperties["Kills"]) + 1;
    }

    public void addPlaced()
    {
        PhotonNetwork.player.customProperties["Placed"] = getHashInt(PhotonNetwork.player.customProperties["Placed"]) + 1;
    }

    public void addNewItemsPicked()
    {
        PhotonNetwork.player.customProperties["NewItemsPicked"] = getHashInt(PhotonNetwork.player.customProperties["NewItemsPicked"]) + 1;
    }

    public void addDestroyed()
    {
        PhotonNetwork.player.customProperties["Destroyed"] = getHashInt(PhotonNetwork.player.customProperties["Destroyed"]) + 1;
    }

    public void addDeaths()
    {
        PhotonNetwork.player.customProperties["Deaths"] = getHashInt(PhotonNetwork.player.customProperties["Deaths"]) + 1;
    }

    [PunRPC]
    void updateAllProperties()
    {
        PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.customProperties);
    }


    int getHashInt(object o)
    {
        return int.Parse(o.ToString());
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
            view.RPC("getStructuresNext", PhotonTargets.MasterClient, PhotonNetwork.player.ID, 0);
            Debug.Log("(Client)Sent Structures Request");
        }

    }

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
        myPlayer.transform.position = myPlayer.spwanPos.position;
        myPlayer.transform.rotation = myPlayer.spwanPos.rotation;
        //				currLevel.cam.SetActive (false);
        ObjectivesManeger.thisM.updateObjectives();
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
        addDeaths();
    }

    public void NetworkDisconnect()
    {
        PhotonNetwork.Disconnect();
    }


    //Structure------------------------------------------//

    [PunRPC]
    public void getStructuresNext(int playerId, int count)
    {
        if (count == currLevel.structures.Count - 1)
        {
            view.RPC("setStructureLast", PhotonPlayer.Find(playerId), currLevel.structures[count].GenerateJSON().ToString());
        }
        else
        {
            view.RPC("setStructuresNext", PhotonPlayer.Find(playerId), currLevel.structures[count].GenerateJSON().ToString(), count);
        }
        //Debug.Log("(Master)Got Request and Sent Structures Response");

    }

    [PunRPC]
    public void setStructuresNext(string StructuresJSON, int count)
    {
        //Debug.Log("(Client)Got Structure");
        currLevel.InitStrucutre(JSONObject.Parse(StructuresJSON));
        count++;
        view.RPC("getStructuresNext", PhotonTargets.MasterClient, PhotonNetwork.player.ID, count);

    }


    [PunRPC]
    public void setStructureLast(String StructuresJSON)
    {
        Debug.Log("(Client)Finished Structure");

        currLevel.InitStrucutre(JSONObject.Parse(StructuresJSON));
        loaded = true;
        PhotonNetwork.isMessageQueueRunning = true;
        currLevel.OnConnected();

    }

    [PunRPC]
    public void setStructure(String StructuresJSON)
    {
        currLevel.InitStrucutre(JSONObject.Parse(StructuresJSON));
    }



    //UI------------------------------------------//

    public void endGame()
    {
        view.RPC("updateAllProperties", PhotonTargets.All);

        ObjectivesManeger.thisM.setCompleted();
        view.RPC("changeToEndScene", PhotonTargets.MasterClient);

    }
    
    [PunRPC]
    public void changeToEndScene()
    {
        PhotonNetwork.LoadLevel(endGameScene);
    }

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

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

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
