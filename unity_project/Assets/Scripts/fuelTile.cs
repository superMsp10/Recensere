using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
		public JetpackFuelDisplay display;

		public PhotonView v;
		public float _fuel;
		public float maxFuel;
		playerMove curr;
		float fuelRate;
		//Tube FX
		public MeshRenderer tube;
		public Color tubeOrg;
		public Color tubeFuel;
		float timeStarted;
		bool startUp = false;

		public float fuel {
				get {
						return _fuel; 
				}
				set {
						_fuel = value; 
						display.setFuelY (value / maxFuel);
				}
		}

		void Start ()
		{
				tube.material.color = tubeOrg;
				fuelRate = GameManager.thisM.currLevel.fuelRate;

		}

		void Update ()
		{
				if (startUp) {
						tube.material.color = Color.Lerp (tubeOrg, tubeFuel, (Time.time - timeStarted) / fuelRate);
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
						fuel += fuelRate;
				v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
		}

		void OnTriggerEnter (Collider other)
		{
				playerMove m = other.GetComponent<playerMove> ();
				if (m != null) {
						curr = m;
						timeStarted = Time.time;
						startUp = true;
						InvokeRepeating ("inputFuel", fuelRate, fuelRate);
				}
		}

		void OnTriggerExit (Collider other)
		{
				if (other.gameObject == curr.gameObject) {
						CancelInvoke ();
						startUp = false;
						tube.material.color = tubeOrg;
				}
		}

		public void inputFuel ()
		{

				startUp = false;

				if (fuel >= 0) {
						curr.fuel ++;
						fuel--;
						v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
				}
		}


}

