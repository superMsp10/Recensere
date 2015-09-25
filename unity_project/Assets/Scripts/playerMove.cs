using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour
{

		public float speed;
		Rigidbody rigidbod;

		void Start ()
		{
				rigidbod = GetComponent<Rigidbody> ();
		}

		void FixedUpdate ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
				float moveVertical = Input.GetAxis ("Vertical");
		
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
				rigidbod.velocity = movement * speed;
		
				rigidbod.position = new Vector3 (rigidbod.position.x, 0.0f, rigidbod.position.z);
		
				//	rigidbod.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbod.velocity.x * -tilt);
		}
}
