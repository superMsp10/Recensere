using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour
{
		public float rotSpeed;
		public Vector3 target;
		Vector3 startRot;
		float time;
		public bool rotate;
		
		
	
		// Update is called once per frame
		void Update ()
		{
				if (rotate) {

						Vector3 myEuler = Vector3.zero;

						myEuler.y = Mathf.MoveTowardsAngle (myEuler.y, target.y, Time.deltaTime * rotSpeed);
						if (myEuler.y >= target.y) {
								myEuler.y = 0;
								rotate = false;
						}
						transform.eulerAngles = myEuler;
				}
				

		}

		public	void Rotate (Transform rot)
		{
				target = rot.localRotation.eulerAngles;
				time = Time.time;
				rotate = true;
				startRot = transform.localRotation.eulerAngles;
		}

}
