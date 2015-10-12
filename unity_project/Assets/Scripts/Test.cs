using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
		public Texture2D overlay ;
		void Start ()
		{
				Renderer rend = GetComponent<Renderer> ();
		
				// duplicate the original texture and assign to the material
				var texture = Instantiate (rend.material.mainTexture) as Texture2D;
				rend.material.mainTexture = texture;
		

		
				var cols = texture.GetPixels ();
				var overlayPixels = overlay.GetPixels ();

				for (int w = 0; w < cols.Length; w++) {
						//								Debug.Log (cols [i]);
						cols [w] = new Color (Random.Range (0.0f, 99f), Random.Range (0.0f, 99f), Random.Range (0.0f, 99f));
				
						
			
				}
				texture.SetPixels (cols);
				
				// actually apply all SetPixels, don't recalculate mip levels
				texture.Apply (false);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
