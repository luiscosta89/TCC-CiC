using UnityEngine;
using System.Collections;

public class Description : MonoBehaviour 
{
	private bool isHit = false;
	private Color startcolor;
	private GameObject selected;

	RaycastHit hit; 
	Ray ray;

	/*void Update(){

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {

			//if(Input.GetMouseButtonDown(0)) {
				startcolor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
				hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.magenta;
				selected = hit.collider.gameObject;
				isHit = true;
			
				Debug.Log ("You selected the " + hit.transform.tag); // ensure you picked right object
			//}

			//else if (Input.GetMouseButtonDown (1)) {
				//hit.collider.gameObject.GetComponent<Renderer> ().material.color = startcolor;
				//isHit = false;
			//}
		} 			
	}*/

	void OnGUI(){
		if(isHit)
			GUI.Label (new Rect (5, 5, 400, 100), selected.name);
	}
}

