using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour
{

    public Vector3 offSet;
    int playersHere = 0;
    public PhotonView thisV;

    public void NetworkInit()
    {
        Debug.Log("SetUP");
        thisV.RPC("GetPlayers", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
    }

    [PunRPC]
    public void GetPlayers(int playerID)
    {
        Debug.Log("Get players called on master");
        thisV.RPC("SetPlayers", PhotonPlayer.Find(playerID), playersHere);
    }

    [PunRPC]
    public void SetPlayers(int playerNum)
    {
        Debug.Log(name + ": Set players called on client: " + playerNum);
        playersHere = playerNum;
    }

    public Vector3 getSpawnPoint()
    {
        playersHere++;
        thisV.RPC("SetPlayers", PhotonTargets.MasterClient, playersHere);

        return new Vector3(transform.position.x + (offSet.x * playersHere),
                           transform.position.y + (offSet.y * playersHere),
                           transform.position.z + (offSet.z * playersHere));
    }

}
