using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
		public JetpackFuelDisplay display;


		public float _fuel;
		public float maxFuel;

		public float fuel {
				get {
						return _fuel; 
				}
				set {
						_fuel = value; 
						display.setFuelY (value / maxFuel);
				}
		}

		public override	void generateLoot ()
		{
				if (fuel <= maxFuel)
						fuel += GameManager.thisM.currLevel.fuelRate;
		}

}

