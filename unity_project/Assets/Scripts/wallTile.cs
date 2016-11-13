using UnityEngine;
using System.Collections;

public class wallTile : Tile
{
    public override void Destroy(bool local, bool effects )
    {
        if (effects)
            DestroyedTileManager.thisWall.AddDestroyedTile(gameObject.transform.position, gameObject.transform.rotation, local);
        base.Destroy(local, effects);
    }
}

