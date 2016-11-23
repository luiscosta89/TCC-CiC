using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(BoxCollider))]

public class mouseDragObject : MonoBehaviour 
{ 
	private Vector3 screenPoint;
	private Vector3 offset;

	private Color startColor; 
	private Vector3 originalPos;

	private GameObject player;
	private string fatherName;

	private GameObject father;

	void Start()
	{		
		originalPos = transform.position;
		startColor = GetComponent<Renderer> ().material.color;
		player = GameObject.Find ("FPSController");
	}

	void Update(){
		//Returns to original position
		if (Input.GetKeyDown(KeyCode.Z) && gameObject.tag == "Selected") {
			transform.position = originalPos;
			gameObject.tag = "Unselected";
		}

		//Returns original color
		if (Input.GetMouseButton(1) && gameObject.tag == "Selected") {
			GetComponent<Renderer> ().material.color = startColor;
		}

		//Returns to original father, color and position
		if (Input.GetKeyDown(KeyCode.X) && gameObject.tag == "Selected") {
			transform.parent = father.transform;

			GetComponent<Renderer> ().material.color = startColor;
			tag = "Unselected";
			transform.position = originalPos;
		}
	}

	void OnMouseDown() {		

			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

			GetComponent<Renderer> ().material.color = Color.magenta;
			
			tag = "Selected";
	}

	void OnMouseDrag() 
	{ 
		float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
	}

	void OnMouseUp()
	{
		Cursor.visible = true;

	}
}


