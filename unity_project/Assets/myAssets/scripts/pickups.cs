using UnityEngine;
using System.Collections;

public class pickups : Entity
{

		invManager thisInv;
		public Holdable thisHolding;
		public bool pickable = true;
		public int amount = 1;
		public float resetPickup = 5;
		public AudioClip pickup;
		public bool dropRandom;


		void Start ()
		{
//				if (thisLevel == null)
//						Debug.LogError ("no Level referenced for this entity: " + gameObject.name);
				thisManage = GameManager.thisM;
				thisInv = invManager.thisInv;
//				thisLevel.addEntity (this);
				if (dropRandom)
						amount = Random.Range (1, amount);
				pickable = false;
				Invoke ("resetPick", 0.5f);
	
		}
		void resetPick ()
		{
				pickable = true;
		}

		
}
