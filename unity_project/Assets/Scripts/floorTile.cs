using UnityEngine;
using System.Collections;

public class floorTile : Tile
{
		public wallTile yTile;
		public wallTile xTile;



		public	override void Destroy ()
		{
				DestroyedTileManager.thisFloor.addDestroyedWall (gameObject.transform.position, gameObject.transform.rotation);
				base.Destroy ();

		}
	
		public	override bool takeDamage (float damage, string attacker)
		{
//				Debug.Log ("Take Damage From Floor Tile");

				GameManager.thisM.sendFloorTileDamage (damage, attacker, xPos, yPos);
				return base.takeDamage (damage, attacker);

		
		}

		public override void addFX (Collision collision, float dmg)
		{
				EffectsManager.thisM.addFloorCracks (collision.contacts [0].normal, collision.contacts [0].point, this, dmg / HP);
		}
	
}
