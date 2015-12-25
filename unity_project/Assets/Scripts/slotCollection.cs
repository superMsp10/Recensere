using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class slotCollection : MonoBehaviour
{
		public List<UIslot> slots;
		protected invManager player;

		void Start ()
		{
				player = invManager.thisInv;

		}

		public bool changeNextSlot (Holdable h)
		{

				for (int i = 0; i <  slots.Count; i ++) {

						if (slots [i].holding == null) {
								slots [i].changeHolding (h);
								return true;
						}
				}
				return false;
		}

		public int addHoldable (Holdable h, int amount)
		{
				
				for (int i = 0; i <  slots.Count; i ++) {

						if (slots [i].holding == null || slots [i].holding == h && slots [i].amount < h.stackSize) {
								int total = slots [i].amount + amount;
								if (total > h.stackSize) {
										amount = total - h.stackSize;
										slots [i].changeHolding (h, h.stackSize);
									
								} else {
										slots [i].changeHolding (h, total);
										amount = 0;
										return 0; 
								}
						}
						

				}
				return amount;
		}

		public int takeItem (Holdable h, int amount)
		{
				
				foreach (UIslot u in slots) {
						if (u.holding == h) {
								if (u.amount > amount) {
										u.changeHolding (u.holding, u.amount - amount);
										return 0;
								} else if (u.amount < amount) {
										int am = u.amount;
										
										return amount - am;
								} else {

										u.changeHolding (null);
										return 0;
								}
						}
				}
				return amount;

		}
		public void changeSlot (Holdable h, int i)
		{

				slots [i].changeHolding (h);
			

		}

}

