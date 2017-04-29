using UnityEngine;
using System.Collections;

public class EndGameLevel : Level
{
    public Transform center;
    public GameObject centerCircle;
    public float spaceBetPlayers;
    public GameObject ins;
    public new void Start()
    {
        base.Start();
    }

    public override void OnConnected()
    {

        if (PhotonNetwork.isMasterClient)
        {

            //int playerNum = PhotonNetwork.playerList.Length;
            int playerNum = 3;
            float diameter = ((playerNum - 1) * spaceBetPlayers) / Mathf.PI;
            centerCircle.transform.localScale = new Vector3(diameter, centerCircle.transform.localScale.y, diameter);
            float rotBy = 360 / (playerNum - 1);
            Vector3 rotated;

            for (int i = 0; i < playerNum - 1; i++)
            {
                center.Rotate(0, rotBy * i, 0);
                rotated = center.forward * diameter;
                Instantiate(ins, new Vector3(rotated.x, -2.7f, rotated.z), Quaternion.identity, transform);
            }
        }

        foreach (PhotonPlayer item in PhotonNetwork.playerList)
        {

        }

        base.OnConnected();

    }
}

