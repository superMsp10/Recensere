using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
		public JetpackFuelDisplay display;
		public static float fuelRate = 2f;
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
				fuel += fuelRate;
		}

}

