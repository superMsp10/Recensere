

public interface Health
{

		bool takeDamage (float damage, string attacker);
		void syncDamage (float damage, string attacker);

		void Destroy (bool local);
		string lastDamageBy ();

		float HP {
				get;
				set;

		}

		float Sturdyness {
				get;
		}

}
