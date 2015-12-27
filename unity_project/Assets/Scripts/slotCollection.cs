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

		bool changeNextSlot (Holdable h)
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

//		check for existing stacks
				List<UIslot> returnSlots = SlotsWithHoldable (h);
				if (returnSlots.Count > 0) {
						foreach (var s in returnSlots) {
								int space = h.stackSize - s.amount;
								if (space > amount) {
										s.amount += amount;
										Debug.Log ("Slot collection add holdable item exists \n only changeing amount. Can hold all");

										return 0;
								} else {
										s.amount += space;
										amount -= space;
										Debug.Log ("Slot collection add holdable item exists \n only changeing amount. Can not hold all");

								}
						}
				}

//		create new stacks
				foreach (var s in slots) {
						if (s.holding == null) {
								if (amount > h.stackSize) {
										Debug.Log ("Slot collection add holdable creating stack. \n Can not hold all");
										s.changeHolding (h, h.stackSize);
										amount -= h.stackSize;

								} else {
										Debug.Log ("Slot collection add holdable creating stack. \n Can hold all");
										s.changeHolding (h, amount);
										return 0;										


								}
						}
				}
				Debug.Log ("Slot collection add holdable exiting with " + amount + " objects");

				return amount;
		}

		public int takeItem (Holdable h, int amount)
		{
				List<UIslot> returnSlots = SlotsWithHoldable (h);
				if (returnSlots.Count > 0) {
						foreach (var s in returnSlots) {
								int left = amount - s.amount;
								if (left <= 0) {
										s.amount -= amount;
										return 0;
								} else {
										s.changeHolding (null);
										amount -= left;
								}
						}
				}

				return amount;

		}

		public void removeItemBySlot (int slotNumber)
		{
				slots [slotNumber].clearHolding ();
		}

		

		void changeSlot (Holdable h, int i)
		{
				slots [i].changeHolding (h);
		}

		public List<UIslot> SlotsWithHoldable (Holdable h)
		{

				List<UIslot> returnSlots = new List<UIslot> ();

				foreach (var s in slots) {
						if (s.holding == h)
								returnSlots.Add (s);
				}

				return returnSlots;

		}

}

