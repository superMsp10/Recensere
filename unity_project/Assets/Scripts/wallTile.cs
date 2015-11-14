using UnityEngine;
using System.Collections;

public class wallTile : Tile
{
		public bool yWall = false;

		public wallTile (int size) : base (size)
		{

		}

		public	override bool takeDamage (float damage, string attacker)
		{


				return base.takeDamage (damage, attacker);
				GameManeger.thisM.sendWallTileDamage (damage, attacker, xPos, yPos, yWall);
		
		}

		public	override void Destroy ()
		{
				DestroyedTileManager.thisWall.addDestroyedWall (gameObject.transform.position, gameObject.transform.rotation);
				Destroy (gameObject);
		
		}
	
}

