using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using System.Collections.Generic;

public class EndGameLevel : Level
{
    public Transform center;
    public GameObject centerCircle;
    public float spaceBetPlayers;
    public GameObject ins;
    public SpawnSpot centerSpot;
    public PhotonView p;
    bool setSpawn = false;
    int[] players;
    int topPlayer;

    public new void Start()
    {
        base.Start();
    }

    public override void OnConnected()
    {
        int playerNum = PhotonNetwork.playerList.Length;
        players = new int[playerNum];
        float diameter = ((playerNum - 1) * spaceBetPlayers) / Mathf.PI;
        centerCircle.transform.localScale = new Vector3(diameter, centerCircle.transform.localScale.y, diameter);

        if (PhotonNetwork.isMasterClient)
        {
            float rotBy = 360 / (playerNum - 1);
            Vector3 rotated;

            for (int i = 0; i < playerNum - 1; i++)
            {
                center.Rotate(0, rotBy * i, 0);
                rotated = center.forward * diameter;
                ((GameObject)PhotonNetwork.InstantiateSceneObject("SpawnSpot", new Vector3(rotated.x, -2.7f, rotated.z), Quaternion.identity, 0, null)).GetComponent<SpawnSpot>();
            }


            int topScore = int.MinValue;
            int topPlayer = -1;
            for (int i = 0; i < playerNum; i++)
            {
                PhotonPlayer item = PhotonNetwork.playerList[i];
                players[i] = item.ID;


                //int score = item.GetScore();
                //if (score > topScore)
                //{
                //    topScore = score;
                //    topPlayer = item.ID;
                //}
            }

            if (topPlayer == -1)
            {
                Debug.LogError("No top player found");
            }

            JoinRequest(PhotonNetwork.player.ID);
        }
        else
        {
            p.RPC("JoinRequest", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
        }
    }

    public override void OnLoaded()
    {
    }

    [PunRPC]
    public void JoinRequest(int n)
    {
        if (n == topPlayer)
        {
            p.RPC("SpawnCenter", PhotonPlayer.Find(n));
        }
        else if (players.Contains(n))
        {
            p.RPC("SpawnDefault", PhotonPlayer.Find(n));
        }
        else
        {
            Debug.Log("Spectating");
        }
    }

    [PunRPC]
    public void SpawnDefault()
    {
        thisM.instantiatePlayer();
    }

    [PunRPC]
    public void SpawnCenter()
    {
        thisM.playerSetup(centerSpot);
    }


}

