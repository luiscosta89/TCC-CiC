using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

public class Move : MonoBehaviour {

	//Hydra
	SixenseInput.Controller leftController;
	SixenseInput.Controller rightController;

	private GameObject originalFather = null;
	private GameObject newFather = null;
	private GameObject lefthandFather = null;
	private GameObject righthandFather = null;

	private Vector3 originalPos;
	private Vector3 originalScale;
	private Vector3 newPos;

	private Color startColor;

	private GameObject hands;
	private GameObject grabbedObject = null;

	private Quaternion originalRotation;
	private Quaternion newRotation;

	private Quaternion angles;

	private GameObject cam;

	// Use this for initialization
	void Start () {
		newFather = GameObject.Find ("FPSController");
		startColor = gameObject.GetComponent<Renderer> ().material.color;
		originalFather = GameObject.Find (transform.parent.name);
		lefthandFather = GameObject.Find ("Hand - Left");
		righthandFather = GameObject.Find ("Hand - Right");
		hands = GameObject.Find ("Cube");
		originalPos = transform.position;
		originalRotation = originalFather.transform.rotation;
		cam = GameObject.Find ("FirstPersonCharacter");
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (VRSettings.enabled == true) {
			angles = InputTracking.GetLocalRotation (VRNode.Head);
			hands.transform.rotation = Quaternion.Euler (0, angles.eulerAngles.y + 90.0f, 0);
		} else {
			hands.transform.rotation = Quaternion.Euler (0, cam.transform.eulerAngles.y + 90.0f, 0);
		}

		//This test must always be done
		if (leftController == null || rightController == null) {
			leftController = SixenseInput.GetController (SixenseHands.LEFT);
			rightController = SixenseInput.GetController (SixenseHands.RIGHT);
		}

		if ((leftController != null && leftController.Enabled) || (rightController != null && rightController.Enabled)) {

			if ((leftController.GetButton (SixenseButtons.THREE) || rightController.GetButton (SixenseButtons.FOUR)) && tag == "Unselected") {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			}

			if ((leftController.GetButton (SixenseButtons.THREE) || rightController.GetButton (SixenseButtons.FOUR)) && grabbedObject != null) {
				grabbedObject.transform.position = newPos;
				grabbedObject = null;
			}

			if ((leftController.GetButton (SixenseButtons.THREE) || rightController.GetButton (SixenseButtons.FOUR)) && (tag == "Selected" || transform.position != originalPos)) {

				//Moves to original father
				transform.parent = originalFather.transform;
					
				//Moves to original position
				transform.position = originalPos;

				//Changes tag
				tag = "Unselected";

				gameObject.GetComponent<Renderer> ().material.color = startColor;
				gameObject.GetComponent<MeshRenderer> ().enabled = true;

				//Returns rotation to original father					
				if (originalFather.GetComponent ("RotObj") as RotObj  == null) {
					originalFather.AddComponent<RotObj> ();
				}					
			}


			if (leftController.GetButtonDown (SixenseButtons.TRIGGER) && gameObject.GetComponent<Renderer> ().material.color == Color.magenta) {
				//Saves original position
				newPos = transform.position;

				//Saves original scale
				originalScale = transform.localScale;
			
				transform.parent = lefthandFather.transform;
				grabbedObject = this.gameObject;
				grabbedObject.transform.localScale = originalScale - new Vector3(2.0f,2.0f,2.0f);
				grabbedObject.GetComponent<Renderer> ().material.color = startColor;
				Destroy (originalFather.GetComponent <RotObj> ());

			}

			if (leftController.GetButtonUp (SixenseButtons.TRIGGER) && grabbedObject != null) {
				grabbedObject.transform.parent = originalFather.transform;
				grabbedObject.transform.localScale = originalScale;

				//grabbedObject.tag = "Unselected";

				if (originalFather.GetComponent ("RotObj") as RotObj  == null) {
					originalFather.AddComponent<RotObj> ();
				}
			}

			if (rightController.GetButtonDown (SixenseButtons.TRIGGER) && gameObject.GetComponent<Renderer> ().material.color == Color.magenta) {
				//Saves original position
				newPos = transform.position;

				//Saves original scale
				originalScale = transform.localScale;

				transform.parent = righthandFather.transform;
				grabbedObject = this.gameObject;
				grabbedObject.transform.localScale = originalScale - new Vector3(2.0f,2.0f,2.0f);			
				grabbedObject.GetComponent<Renderer> ().material.color = startColor;
				Destroy (originalFather.GetComponent <RotObj> ());

			}

			if (rightController.GetButtonUp (SixenseButtons.TRIGGER) && grabbedObject != null) {
				grabbedObject.transform.parent = originalFather.transform;		
				grabbedObject.transform.localScale = originalScale;

				//grabbedObject.tag = "Unselected";

				if (originalFather.GetComponent ("RotObj") as RotObj  == null) {
					originalFather.AddComponent<RotObj> ();
				}
			}
			}
		}
	}



