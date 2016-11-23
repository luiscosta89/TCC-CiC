using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Select : MonoBehaviour {

	//Hydra
	SixenseInput.Controller leftController;
	SixenseInput.Controller rightController; 

	private GameObject selected;

	private GameObject handLeft;
	private GameObject handRight;

	private GameObject pointerLeft;
	private GameObject pointerRight;

	private GameObject hands;

	//Colors
	private Color carbonColor;
	private Color oxygenColor;
	private Color nitrogenColor;
	private Color hydrogenColor;
	private Color sulfurColor;
	private Color ironColor;
	private Color phosphorusColor;

	private LineRenderer lineLeft;
	private LineRenderer lineRight;


	public int numError = 0;

	// Use this for initialization
	void Start () {

		handLeft = GameObject.Find ("Hand - Left");
		handRight = GameObject.Find ("Hand - Right");
		pointerLeft = GameObject.Find ("JNT_PointerTipL");
		pointerRight = GameObject.Find("JNT_PointerTipR");
		hands = GameObject.Find ("HandsController");

		//Aim for left hand
		handLeft.AddComponent<LineRenderer> ();

		lineLeft = handLeft.GetComponent <LineRenderer> ();

		lineLeft.SetVertexCount (2);
		lineLeft.SetWidth (0.1f, 0.25f);
		lineLeft.enabled = true;
		lineLeft.SetColors (Color.green, Color.green);

		//Aim for right hand
		handRight.AddComponent<LineRenderer> ();

		lineRight = handRight.GetComponent <LineRenderer> ();

		lineRight.SetVertexCount (2);
		lineRight.SetWidth (0.1f, 0.25f);
		lineRight.enabled = true;
		lineRight.SetColors (Color.green, Color.green);
	}	

	// Update is called once per frame
	void Update () {

		//get the controller for both hands
		if (leftController == null || rightController == null) {
			leftController = SixenseInput.GetController (SixenseHands.LEFT);
			rightController = SixenseInput.GetController (SixenseHands.RIGHT);
		}

		if ((leftController != null && leftController.Enabled) && (rightController != null && rightController.Enabled)) {

			//Aim for left hand
			//handLeft.AddComponent<LineRenderer> ();

			//LineRenderer lineLeft = handLeft.GetComponent <LineRenderer> ();

			//lineLeft.SetVertexCount (2);
			//lineLeft.SetWidth (0.1f, 0.25f);
			//lineLeft.enabled = true;
			//lineLeft.SetColors (Color.green, Color.green);
			lineLeft.SetPosition (0, pointerLeft.transform.position);
			lineLeft.SetPosition (1, pointerLeft.transform.TransformDirection (Vector3.forward) * 1000.0f);

			//Aim for right hand
			//handRight.AddComponent<LineRenderer> ();

			//LineRenderer lineRight = handRight.GetComponent <LineRenderer> ();

			//lineRight.SetVertexCount (2);
			//lineRight.SetWidth (0.1f, 0.25f);
			//lineRight.enabled = true;
			//lineRight.SetColors (Color.green, Color.green);
			lineRight.SetPosition (0, pointerRight.transform.position);
			lineRight.SetPosition (1, pointerRight.transform.TransformDirection (Vector3.forward) * 1000.0f);


			if (leftController.GetButtonDown (SixenseButtons.TWO)) {

				Vector3 directionLeft = pointerLeft.transform.TransformDirection (Vector3.forward) * 1000.0f;
				RaycastHit hit;

				if (Physics.Raycast (pointerLeft.transform.position, directionLeft, out hit)) {

					//Deselect
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.color == Color.magenta) {

						switch (hit.collider.gameObject.name) {
						case "Carbon":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = carbonColor;
							break;
						case "Oxygen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = oxygenColor;
							break;
						case "Nitrogen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = nitrogenColor;
							break;
						case "Hydrogen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = hydrogenColor;
							break;
						case "Iron":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = ironColor;
							break;
						case "Sulfur":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = sulfurColor;
							break;
						case "Phosphorus":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = phosphorusColor;
							break;
						}

						numError++;

						hit.collider.gameObject.tag = "Unselected";

					} else {
						
						switch (hit.collider.gameObject.name) {
						case "Carbon":
							carbonColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Oxygen":
							oxygenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Nitrogen":
							hydrogenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Hydrogen":
							nitrogenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Iron":
							ironColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Sulfur":
							sulfurColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Phosphorus":
							phosphorusColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						}

						hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.magenta;
						hit.collider.gameObject.tag = "Selected";
						
						selected = hit.collider.gameObject;
					}
				}
			}

			if (rightController.GetButtonDown (SixenseButtons.ONE)) {

				Vector3 directionRight = pointerRight.transform.TransformDirection (Vector3.forward) * 1000.0f;
				RaycastHit hit;

				if (Physics.Raycast (pointerRight.transform.position, directionRight, out hit)) {

					//Deselect
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.color == Color.magenta) {

						switch (hit.collider.gameObject.name) {
						case "Carbon":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = carbonColor;
							break;
						case "Oxygen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = oxygenColor;
							break;
						case "Nitrogen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = nitrogenColor;
							break;
						case "Hydrogen":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = hydrogenColor;
							break;
						case "Iron":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = ironColor;
							break;
						case "Sulfur":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = sulfurColor;
							break;
						case "Phosphorus":
							hit.collider.gameObject.GetComponent<Renderer> ().material.color = phosphorusColor;
							break;
						}

						numError++;

						hit.collider.gameObject.tag = "Unselected";
					} else {

						switch (hit.collider.gameObject.name) {
						case "Carbon":
							carbonColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Oxygen":
							oxygenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Nitrogen":
							hydrogenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Hydrogen":
							nitrogenColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Iron":
							ironColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Sulfur":
							sulfurColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						case "Phosphorus":
							phosphorusColor = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
							break;
						}

						hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.magenta;
						hit.collider.gameObject.tag = "Selected";

						selected = hit.collider.gameObject;
					}

				}
			}
		}

		//Highlight atoms
		if ((leftController.GetButton (SixenseButtons.FOUR) || rightController.GetButton (SixenseButtons.THREE)) && selected != null) {

			foreach (GameObject atom in GameObject.FindGameObjectsWithTag("Unselected")) {
				if (atom.name != selected.gameObject.name) {
					atom.GetComponent<MeshRenderer> ().enabled = false;
				} else if (atom.name == selected.gameObject.name) {
					atom.tag = "Selected";
				}
			}
		}
	}
}
