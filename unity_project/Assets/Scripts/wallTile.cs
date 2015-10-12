using UnityEngine;
using System.Collections;

public class wallTile : Tile
{
		public bool yWall = false;

		public wallTile (int size) : base (size)
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

				

				GameManeger.thisM.sendWallTileDamage (damage, attacker, xPos, yPos, yWall);
		
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

