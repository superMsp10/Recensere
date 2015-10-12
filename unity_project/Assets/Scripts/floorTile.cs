using UnityEngine;
using System.Collections;

public class floorTile : Tile
{
		public wallTile yTile;
		public wallTile xTile;

		public floorTile (int size) : base (size)
		{
		}



		public	override void takeDamage (float damage, string attacker)
		{
//				Debug.Log ("take dmg");
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}

				GameManeger.thisM.sendFloorTileDamage (damage, attacker, xPos, yPos);
		
		}

		public	override void syncDamage (float damage, string attacker)
		{
//				Debug.Log ("sync dmg");

				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}

		}
}
