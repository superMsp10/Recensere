using UnityEngine;
using System.Collections;

public class wallTile : Tile
{


		public	override bool takeDamage (float damage, string attacker)
		{
				GameManeger.thisM.sendWallTileDamage (damage, attacker, xPos, yPos, yWall);
				return base.takeDamage (damage, attacker);

		}

		public	override void Destroy ()
		{
//				Debug.Log (gameObject);
				DestroyedTileManager.thisWall.addDestroyedWall (gameObject.transform.position, gameObject.transform.rotation);
				base.Destroy ();
		}
	
}

