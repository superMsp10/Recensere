using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour
{

    public int teamId = 0;
    public int playerCount = 0;
    public Vector3 offSet;

    public Vector3 getSpawnPoint()
    {
        playerCount++;

        return new Vector3(transform.position.x + (offSet.x * playerCount),
                           transform.position.y + (offSet.y * playerCount),
                           transform.position.z + (offSet.z * playerCount));
    }

}
