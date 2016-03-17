using UnityEngine;
using System.Collections;

public interface Holdable
{

		Sprite holdUI {
				get;
		}
		int  stackSize {
				get;
		}
		string description {
				get;
		}
		bool pickable {
				get;
		}
		int amount {
				get;
				set;
		}



	
		bool  buttonDown ();
		void  buttonUP ();

		void  onSelect ();
		void  onDeselect ();
		void  onPickup ();
		void  onDrop (
//		if (thisM.myPlayer != null) {
//		playerPos = thisM.myPlayer.transform.position;
//		
//		GameObject p = (GameObject)Instantiate (holding.phisical.gameObject, playerPos, Quaternion.identity);
//		tmp = p.GetComponent<pickups> ();
//		//								tmp.thisLevel = thisM.currentLevel;
//		tmp.pickable = false;
//		tmp.amount = amount;
//		Invoke ("resetPickable", tmp.resetPickup);
//		changeHolding (null);
//	}
		);

		void resetPick ();


}
