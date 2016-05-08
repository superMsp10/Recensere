using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
		public JetpackFuelDisplay display;

		public PhotonView v;
		private float _fuel;
		public float maxFuel;
		playerMove curr;
		float fuelRate;
		//Tube FX
		public MeshRenderer tube;
		public Color tubeOrg;
		public Color tubeFuel;
		float timeStarted;
		bool startUp = false;
		//Vale&Lid FX
		public ConstantForce force;
		public float rotSpeed;
		public int startRot;
		public int endRot;
		bool playerOn = false;



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
				force.torque = Vector3.zero;

		}

		void Update ()
		{
				if (startUp) {
						tube.material.color = Color.Lerp (tubeOrg, tubeFuel, (Time.time - timeStarted) / fuelRate);
					
			
				} 
				if (playerOn) {
						if (force.gameObject.transform.localRotation.x > endRot) {
								force.torque = new Vector3 (-1 * rotSpeed, 0, 0);
						} else {
								force.torque = new Vector3 (0, 0, 0);
						}
				} else {

						if (force.gameObject.transform.localRotation.x < startRot) {
								force.torque = new Vector3 (1 * rotSpeed, 0, 0);
						} else {
								force.torque = new Vector3 (0, 0, 0);
						}
				}

		}

		[PunRPC]
		public void syncFuel (float f)
		{
				fuel = f;
		}

		public override	void generateLoot ()
		{
				if ((fuel + fuelRate) <= maxFuel)
						fuel += fuelRate;
				v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
		}

		void OnTriggerEnter (Collider other)
		{
				playerMove m = other.GetComponent<playerMove> ();
				if (m != null && fuel > 0) {
						curr = m;
						InvokeRepeating ("inputFuel", fuelRate, fuelRate);
						v.RPC ("startFuelFX", PhotonTargets.All, null);

				}
		}

		[PunRPC]
		public void startFuelFX ()
		{
				timeStarted = Time.time;
				startUp = true;
				playerOn = true;
		}

		void OnTriggerExit (Collider other)
		{
				if (other.gameObject == curr.gameObject) {
						stopFX ();
				}
		}

		void stopFX ()
		{
				CancelInvoke ();
				v.RPC ("stopFuelFX", PhotonTargets.All, null);
		}

		[PunRPC]
		public void stopFuelFX ()
		{
				
				startUp = false;
				tube.material.color = tubeOrg;
				playerOn = false;
				
		}

		public void inputFuel ()
		{

				startUp = false;

				if ((fuel + fuelRate) >= 0) {
						curr.fuel ++;
						fuel--;
						v.RPC ("syncFuel", PhotonTargets.Others, _fuel);
				} else {
						stopFX ();
				}
		}


}

