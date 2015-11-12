using UnityEngine;
using System.Collections;

public class destroyedWallTile : destroyedTile
{
		public ParticleSystem dust;
		public	override void reset (bool on)
		{
				base.reset (on);
				if (!on) {
						dust.Stop ();
						Transform t;
						Transform prefabTransform = tileDictionary.thisM.destroyedWallTile.transform;
						for (int i = 0; i <transform.childCount; i++) {
								t = transform.GetChild (i);
								Rigidbody r = t.GetComponent<Rigidbody> ();
								if (r != null) {
										r.velocity = Vector3.zero;
										t.localPosition = prefabTransform.GetChild (i).localPosition;
										t.localRotation = prefabTransform.GetChild (i).localRotation;
								}

						}
				} else
						dust.Play ();

		
		}
}

