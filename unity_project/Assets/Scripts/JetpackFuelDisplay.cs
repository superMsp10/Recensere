using UnityEngine;
using System.Collections;

public class JetpackFuelDisplay : MonoBehaviour
{
		public float maxDisplay = 0.48f;

	
		public void setFuel (float fuelOutOfMax)
		{
				transform.localScale = new Vector3 (maxDisplay, Mathf.Lerp (0, maxDisplay, fuelOutOfMax), maxDisplay);
				transform.localPosition = new Vector3 (0, 0, transform.localScale.y / 2);
		}
}
