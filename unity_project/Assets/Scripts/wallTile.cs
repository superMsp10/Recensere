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
				
				base.takeDamage (damage, attacker);
				GameManeger.thisM.sendWallTileDamage (damage, attacker, xPos, yPos, yWall);
		
		}



}

