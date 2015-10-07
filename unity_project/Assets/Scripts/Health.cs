

public interface Health
{

		void takeDamage (float damage, string attacker);
		void syncDamage (float damage, string attacker);

		void Destroy ();
		string lastDamageBy ();
		float HP {
				get;
				set;

		}

}
