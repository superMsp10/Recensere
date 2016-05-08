using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour
{
		public float rotSpeed;
		public Transform target;
		public bool rotate;
		
		
	
		// Update is called once per frame
		void Update ()
		{
				if (rotate) {
						if (Vector3.Distance (target.localRotation.eulerAngles, transform.localRotation.eulerAngles) > 10)
								gameObject.transform.Rotate (target.localRotation.eulerAngles * (rotSpeed * Time.deltaTime));

				}
				

		}

		public	void Rotate (Transform rot)
		{
				target = rot;
				rotate = true;
		}

}
