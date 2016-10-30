using UnityEngine;
using System.Collections;

public class floorTile : Tile
{

    public override void Destroy()
    {
        DestroyedTileManager.thisFloor.addDestroyedWall(gameObject.transform.position, gameObject.transform.rotation);
        base.Destroy();

    }
}
