using UnityEngine;
using System.Collections;

public class JetpackFuelDisplay : MonoBehaviour
{
		public float maxDisplay = 0.48f;

	
		public void setFuel (float fuelOutOfMax)
		{
				transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (0, maxDisplay, fuelOutOfMax), transform.localScale.z);
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localScale.y / 2);
		}

		public void setFuelY (float fuelOutOfMax)
		{
				transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (0, maxDisplay, fuelOutOfMax), transform.localScale.z);
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localScale.y / 2, transform.localPosition.z);
		}
}
