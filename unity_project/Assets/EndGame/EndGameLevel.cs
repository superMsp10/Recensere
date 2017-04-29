using UnityEngine;
using System.Collections;

public class EndGameLevel : Level
{
    public Transform center;
    public GameObject centerCircle;
    public float spaceBetPlayers;
    public new void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int playerNum = PhotonNetwork.playerList.Length;
            float diameter = ((playerNum - 1) * spaceBetPlayers) / Mathf.PI;
            centerCircle.transform.localScale = new Vector3(diameter, centerCircle.transform.localScale.y, diameter);
        }
        base.Start();
    }
}

