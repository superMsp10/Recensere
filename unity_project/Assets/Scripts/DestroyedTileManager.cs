using UnityEngine;
using System.Collections.Generic;

public class DestroyedTileManager : MonoBehaviour
{

    public static DestroyedTileManager thisWall;
    public static DestroyedTileManager thisFloor;

    private GameObject destroyedWallFX;

    [HideInInspector]
    public Pooler
            destroyedWallPooler = null;

    public float wallReset;
    public int maxWalls;
    public bool floor;
    public string damageLayer;
    public string nonDamageLayer;


    void Awake()
    {
        if (!floor)
        {
            if (thisWall == null)
                thisWall = this;
        }
        else
        {
            if (thisFloor == null)
                thisFloor = this;
        }

    }

    void Start()
    {
        if (!floor)
        {
            destroyedWallFX = tileDictionary.thisM.destroyedWallTile;

        }
        else
        {
            destroyedWallFX = tileDictionary.thisM.destroyedFloorTile;

        }
        destroyedWallPooler = new Pooler(maxWalls, destroyedWallFX);

    }


    public void AddDestroyedTile(Vector3 pos, Quaternion rotation, bool local)
    {
        //Debug.Log("AddDestroyed tile at DestroyedTileManager");


        if (destroyedWallFX == null)
        {
            Debug.Log("No destoyedWallFX");
            return;
        }


        GameObject g = destroyedWallPooler.getObject();
        g.transform.parent = transform;

        g.transform.position = pos;
        g.transform.rotation = rotation;
        g.GetComponent<Timer>().StartTimer(wallReset);

        //Debug.Log("Local: " + local);
        if (local)
        {
            g.layer = LayerMask.NameToLayer(damageLayer);
            foreach (Transform t in g.transform.GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer(damageLayer);
        }
        else
        {
            g.layer = LayerMask.NameToLayer(nonDamageLayer);
            foreach (Transform t in g.transform.GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer(nonDamageLayer);
        }

    }
}
