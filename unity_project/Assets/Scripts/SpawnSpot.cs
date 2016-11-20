using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour
{

    public int teamId = 0;
    public Vector3 offSet;

    public Vector3 getSpawnPoint()
    {
        int playerCount = PhotonNetwork.countOfPlayers / GameManager.thisM.currLevel.sS.Length;

        return new Vector3(transform.position.x + (offSet.x * playerCount),
                           transform.position.y + (offSet.y * playerCount),
                           transform.position.z + (offSet.z * playerCount));
    }

}
