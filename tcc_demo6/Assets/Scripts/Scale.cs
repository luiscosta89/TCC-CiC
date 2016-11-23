using UnityEngine;
using System.Collections;

public class Scale : MonoBehaviour {

	//Vector3 originalSize = GameObject.transform.localScale;

	void Update () {

		//Decrease size of atoms
		if (SixenseInput.Controllers [0].GetButton (SixenseButtons.BUMPER))
		{
			transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
		}

		//Increase size of atoms
		if (SixenseInput.Controllers [1].GetButton (SixenseButtons.BUMPER))
		{
			if (transform.localScale != Vector3.zero) {
				transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			}
		}
			
	
	}
}
