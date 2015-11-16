using UnityEngine;
using System.Collections;

public class floorTile : Tile
{
		public wallTile yTile;
		public wallTile xTile;

		public floorTile (int size) : base (size)
		{
		}

		public	override void Destroy ()
		{
				DestroyedTileManager.thisFloor.addDestroyedWall (gameObject.transform.position, gameObject.transform.rotation);
				Destroy (gameObject);
		
		}
	
		public	override bool takeDamage (float damage, string attacker)
		{
//				Debug.Log ("Take Damage From Floor Tile");

				GameManeger.thisM.sendFloorTileDamage (damage, attacker, xPos, yPos);
				return base.takeDamage (damage, attacker);

		
		}
	
}
