using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour
{

    public Vector3 offSet;
    int playersHere = 0;

    public Vector3 getSpawnPoint()
    {
        playersHere++;
        return new Vector3(transform.position.x + (offSet.x * playersHere),
                           transform.position.y + (offSet.y * playersHere),
                           transform.position.z + (offSet.z * playersHere));
    }

}
