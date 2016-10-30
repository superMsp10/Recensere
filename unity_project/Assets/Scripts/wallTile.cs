using UnityEngine;
using System.Collections;

public class wallTile : Tile
{

    public override void Destroy()
    {
        DestroyedTileManager.thisWall.addDestroyedWall(gameObject.transform.position, gameObject.transform.rotation);
        base.Destroy();
    }

}

