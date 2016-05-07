using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
		public JetpackFuelDisplay display;

		public PhotonView v;
		public float _fuel;
		public float maxFuel;
		playerMove curr;

		public float fuel {
				get {
						return _fuel; 
				}
				set {
						_fuel = value; 
						display.setFuelY (value / maxFuel);
				}
		}

		[PunRPC]
		public void syncFuel (float f)
		{
				fuel = f;
		}

		public override	void generateLoot ()
		{
				if (fuel <= maxFuel)
						fuel += GameManager.thisM.currLevel.fuelRate;
				v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
		}

		void OnTriggerEnter (Collider other)
		{
				playerMove m = other.GetComponent<playerMove> ();
				if (m != null) {
						curr = m;
						InvokeRepeating ("inputFuel", GameManager.thisM.currLevel.fuelRate, GameManager.thisM.currLevel.fuelRate);
				}
		}

		void OnTriggerExit (Collider other)
		{
				if (other.gameObject == curr.gameObject) {
						CancelInvoke ();
				}
		}

		public void inputFuel ()
		{
				if (fuel >= 0) {
						curr.fuel ++;
						fuel--;
						v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
				}
		}


}

