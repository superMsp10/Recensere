using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager thisM;
    public static byte Version = 9;
    public string PlayerLayer;
    public string GhostLayer;

    public string endGameScene, startGameScene, startUIScene = "Photoshoot", tutorialScene = "Tutorial";

    //PlayerStuff------------------------------------------//

    public List<player> players;
    public GameObject playerInstantiate;
    public player myPlayer;
    public bool dead = true;
    bool pause = false;
    public bool doObjectives = true;


    //LevelStuf------------------------------------------//
    public Level currLevel;
    public PhotonView view;
    public static float speedToDamageMultiplier = 1f;
    public bool loaded = false;
    public GameObject currCam;
    private bool Master;

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
        GameObject g = PhotonNetwork.InstantiateSceneObject(prefabName, position, Quaternion.identity, 0, null);
        g.SetActive(true);
    }

    //Player------------------------------------------//
    public void spawnPlayer()
    {

        int playerID = PhotonNetwork.player.ID;
        SpawnSpot[] spawns = currLevel.spawns;
        playerSetup(spawns[playerID % spawns.Length]);
        loaded = true;

    }

    public void playerSetup(SpawnSpot thisSpawn)
    {
        GameObject p = null;
        int playerID = PhotonNetwork.player.ID;

        if (currLevel.spawnStructure != null)
        {
            GameObject g = Instantiate(currLevel.spawnStructure, thisSpawn.getSpawnPoint(), thisSpawn.transform.rotation, currLevel.StructuresTransform);
            g.name = "PlayerSpawn: " + Persistent.thisPersist.Username + playerID;

            PlayerStructure s = g.GetComponent<PlayerStructure>();
            if (s != null)
            {
                s.isLocal = true;
                currLevel.structures.Add(s);
                p = PhotonNetwork.Instantiate(playerInstantiate.name,
                             s.spawnPosition.position,
                                s.spawnPosition.rotation, 0, null);
                myPlayer = p.GetComponent<player>();
                view.RPC("setStructure", PhotonTargets.Others, s.GenerateJSON().ToString());
                myPlayer.spwanRoom = s;
                myPlayer.spwanPos = s.spawnPosition;
            }
            else
            {
                Debug.Log("The currLevel spawnStructure does not have a player sturcture comp. attached");
                return;
            }
        }
        else
        {
            p = PhotonNetwork.Instantiate(playerInstantiate.name,
                          thisSpawn.transform.position,
                             thisSpawn.transform.rotation, 0, null);
            myPlayer = p.GetComponent<player>();
            myPlayer.spwanPos = thisSpawn.transform;
        }


        myPlayer.playerID = playerID;
        myPlayer.name = PhotonNetwork.player.NickName = Persistent.thisPersist.Username;
        //				myPlayer.transform.FindChild ("Graphics").GetComponent<Renderer> ().material.color = player.getPlayerColour (playerID);

        p.layer = LayerMask.NameToLayer(PlayerLayer);
        myPlayer.animModel.layer = LayerMask.NameToLayer(GhostLayer);
        p.GetComponent<Rigidbody>().isKinematic = false;

        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", 0 }, { "Placed", 0 }, { "NewItemsPicked", 0 }, { "Destroyed", 0 }, { "Deaths", 0 } });
        if (doObjectives)
            ObjectivesManeger.thisM.Initialize();
        NetworkEnable();

        view.RPC("updatePlayers", PhotonTargets.AllBuffered, myPlayer.thisView.viewID, myPlayer.name, myPlayer.playerID);

    }

    [PunRPC]
    void updatePlayers(int id, string name, int playerID)
    {
        players = new List<player>(FindObjectsOfType<player>());
        player p = getPlayerByViewID(id);
        if (p != null)
        {
            p.name = "Player: " + name;
            p.playerID = playerID;
            Debug.Log("Updating players for player: " + p.name + ", Total:" + players.Count);
        }
        else
        {
            Debug.Log("No player for: " + id);

        }
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        SceneManager.LoadScene(startUIScene);
        DatabaseConnect.thisM.GameMessage_HostExited();
        PhotonNetwork.Disconnect();
    }


    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        players = new List<player>(FindObjectsOfType<player>());
        Debug.Log("Removing : " + player.NickName);
    }

    public void OnConnected()
    {
        currLevel.OnConnected();
    }

    public player getPlayerByViewID(int viewID)
    {
        foreach (player p in players)
        {
            if (p.thisView.viewID == viewID)
            {
                return p;
            }
        }

        Debug.Log("No player w/ viewID" + viewID);
        return null;
    }

    public player getPlayerByPlayerID(int playerID)
    {

        foreach (player p in players)
        {
            if (p.playerID == playerID)
            {
                return p;
            }
        }

        Debug.Log("No player w/ playerID" + playerID);
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
        if (doObjectives)
            ObjectivesManeger.thisM.updateObjectives();
        myPlayer.networkInit();
        dead = false;
        UIManager.thisM.changeUI(tileDictionary.thisM.inGameUI);

    }

    public void NetworkDisable()
    {

        ChangeCam(currLevel.cam);
        currLevel.playerDeath();
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

    public void StartStrucutresSync()
    {
        view.RPC("getStructuresNext", PhotonTargets.MasterClient, PhotonNetwork.player.ID, 0);
    }

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
        currLevel.OnLoaded();
    }

    [PunRPC]
    public void setStructure(String StructuresJSON)
    {
        currLevel.InitStrucutre(JSONObject.Parse(StructuresJSON));
    }



    //UI------------------------------------------//

    int getHashInt(object o)
    {
        return int.Parse(o.ToString());
    }

    [PunRPC]
    public void addKills()
    {
        PhotonNetwork.player.CustomProperties["Kills"] = getHashInt(PhotonNetwork.player.CustomProperties["Kills"]) + 1;
    }

    public void addPlaced()
    {
        if (doObjectives)
            PhotonNetwork.player.CustomProperties["Placed"] = getHashInt(PhotonNetwork.player.CustomProperties["Placed"]) + 1;
    }

    public void addNewItemsPicked()
    {
        if (doObjectives)
            PhotonNetwork.player.CustomProperties["NewItemsPicked"] = getHashInt(PhotonNetwork.player.CustomProperties["NewItemsPicked"]) + 1;
    }

    public void addDestroyed()
    {
        if (doObjectives)
            PhotonNetwork.player.CustomProperties["Destroyed"] = getHashInt(PhotonNetwork.player.CustomProperties["Destroyed"]) + 1;
    }

    public void addDeaths()
    {
        if (doObjectives)
            PhotonNetwork.player.CustomProperties["Deaths"] = getHashInt(PhotonNetwork.player.CustomProperties["Deaths"]) + 1;
    }

    void updateAllProperties()
    {
        PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.CustomProperties);
    }

    public void endGame()
    {
        view.RPC("changeToEndSceneMaster", PhotonTargets.MasterClient);
    }

    [PunRPC]
    public void changeToEndSceneMaster()
    {
        view.RPC("changeToEndScene", PhotonTargets.All);
        PhotonNetwork.LoadLevel(endGameScene);
    }

    [PunRPC]
    public void changeToEndScene()
    {
        ObjectivesManeger.thisM.setCompleted();
        updateAllProperties();
    }

    public void restartgame()
    {
        PhotonNetwork.LoadLevel(startGameScene);
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
                if (myPlayer != null)
                {
                    foreach (MonoBehaviour m in myPlayer.networkSet)
                    {
                        m.enabled = false;
                    }
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
            else
            {
                if (myPlayer != null)
                {
                    foreach (MonoBehaviour m in myPlayer.networkSet)
                    {
                        m.enabled = true;
                    }
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
        view.RPC("SyncTileDamage", PhotonTargets.Others, damage, attacker, structureName, tileName);

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
        Debug.Log("Tile Request for Name: " + TileName + " does not exist for structure" + StructureName);

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
