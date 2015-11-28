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

		public override void addFX (Collision collision, float dmg)
		{
				EffectsManager.thisM.addWallCracks (collision.contacts [0].normal, collision.contacts [0].point, this, dmg / HP);
		}
	
}

