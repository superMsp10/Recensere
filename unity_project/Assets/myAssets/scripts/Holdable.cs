using UnityEngine;
using System.Collections;

public class Holdable : MonoBehaviour
{

		public Sprite holdUI;
		public GameManager thisManage;
		public pickups phisical;
		public int stackSize;
		public bool weapon = false;
		public string description;
		public void Start ()
		{
				thisManage = GameManager.thisM;

		}

		

		public virtual bool  onUse ()
		{
				return false;

		}
		public virtual void  onSelect ()
		{
		}
		public virtual void  onDeselect ()
		{
		}
		public virtual void  onPickup ()
		{

		}

		public virtual void  onDrop ()
		{
				
		}


}
