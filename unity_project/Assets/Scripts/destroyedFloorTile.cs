using UnityEngine;
using System.Collections;

public class destroyedFloorTile : destroyedTile
{
		public	override void reset (bool on)
		{
				base.reset (on);
				if (!on) {
						Transform t;
						Transform prefabTransform = tileDictionary.thisM.destroyedFloorTile.transform;
						for (int i = 0; i <transform.childCount; i++) {
								t = transform.GetChild (i);
								t.GetComponent<Rigidbody> ().velocity = Vector3.zero;
								t.localPosition = prefabTransform.GetChild (i).localPosition;
								t.localRotation = prefabTransform.GetChild (i).localRotation;

						}
				}
		
		}

		public override void TimerComplete ()
		{
				DestroyedTileManager.thisFloor.destroyedWallPooler.disposeObject (this);
		}
}

