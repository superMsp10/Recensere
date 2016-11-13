using UnityEngine;
using System.Collections;

public class floorTile : Tile
{
    public override void Destroy(bool local, bool effects)
    {
        if (effects)
            DestroyedTileManager.thisFloor.AddDestroyedTile(gameObject.transform.position, gameObject.transform.rotation, local);
        base.Destroy(local, effects);
    }
}
