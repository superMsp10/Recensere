using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class EndGameLevel : Level
{
    public Transform center;
    public GameObject centerCircle;
    public float spaceBetPlayers;
    public GameObject ins;
    public SpawnSpot centerSpot;
    public PhotonView p;
    bool setSpawn = false;

    public new void Start()
    {
        base.Start();
    }

    public override void OnConnected()
    {
        //int playerNum = PhotonNetwork.playerList.Length;
        int playerNum = 3;
        float diameter = ((playerNum - 1) * spaceBetPlayers) / Mathf.PI;
        centerCircle.transform.localScale = new Vector3(diameter, centerCircle.transform.localScale.y, diameter);

        if (PhotonNetwork.isMasterClient)
        {
            float rotBy = 360 / (playerNum - 1);
            Vector3 rotated;

            sS = new SpawnSpot[playerNum - 1];
            for (int i = 0; i < playerNum - 1; i++)
            {
                center.Rotate(0, rotBy * i, 0);
                rotated = center.forward * diameter;
                sS[i] = ((GameObject)Instantiate(ins, new Vector3(rotated.x, -2.7f, rotated.z), Quaternion.identity, transform)).GetComponent<SpawnSpot>();
            }


            int topScore = int.MinValue;
            PhotonPlayer topPlayer = null;
            foreach (PhotonPlayer item in PhotonNetwork.playerList)
            {
                int score = item.GetScore();
                if (score > topScore)
                {
                    topScore = score;
                    topPlayer = item;
                }
            }

            if (topPlayer != null)
            {
                p.RPC("SpawnCenter", topPlayer);

            }
            else
            {
                Debug.LogError("No top player found");
            }
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
        p.RPC("SpawnDefault", PhotonTargets.Others);

    }
}

