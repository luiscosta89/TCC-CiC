/*using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HydraController : MonoBehaviour
{

	public static HydraController instance {
		get { return FindObjectOfType<HydraController>(); }
	}

	SixenseInput.Controller hydraControllerLeft = null;
	SixenseInput.Controller hydraControllerRight = null;
	float positiveRange;
	float negativeRange;
	float zeroPositionL;
	float zeroPositionR;
	float HydContRingJy;
	float HydContLeftJy;

	public GameObject rightHand;
	public GameObject leftHand;
	public GameObject rightPointer;
	public GameObject leftPointer;

	float yMovement;

	Vector3 lastAngles;
	Vector3 lastDiff; // used for LOW PASS using IIR
	bool initialPosFixed = false;
	string activeScene;
	Vector3 iphead; //initial position head
	bool training = false;

	bool experimentTime = false;
	bool fix = false;
	public bool interaction = false;
	public bool locomotion = false;

	//public override void Setup() {

	//}

	void Start(){
		
	}

	void Update() {
		if (hydraControllerLeft == null || hydraControllerRight == null) {
			hydraControllerLeft = SixenseInput.GetController(SixenseHands.LEFT);
			hydraControllerRight = SixenseInput.GetController(SixenseHands.RIGHT);
		} 
			
		else if (hydraControllerLeft.Enabled && !hydraControllerLeft.Docked && 
			hydraControllerRight.Enabled && !hydraControllerRight.Docked) {

			NormalInteraction ();
//			switch (interaction) {
//			case Interaction.Normal:
//				NormalInteraction ();
//				break;
//
//			case Interaction.HeadTracking:
//				HeadTrackingInteraction ();
//				break;
//			}
		}
	}

	public void NormalInteraction(){
		if (locomotion) {
			//if (hydraControllerLeft.JoystickY > 0)
				//Command.Move (0, hydraControllerLeft.JoystickY);
		}

		if (interaction) {
			//Command.MoveCamera (hydraControllerRight.JoystickX, hydraControllerRight.JoystickY);

			if (hydraControllerLeft.GetButton (SixenseButtons.TRIGGER) && !fix) {
				//Command.SetDirections (leftPointer.transform.forward, leftHand.transform.position);
				//Command.Select ();
				fix = true;
			} else if (!hydraControllerLeft.GetButton (SixenseButtons.TRIGGER))
				fix = false;

			if (hydraControllerRight.GetButtonDown (SixenseButtons.TRIGGER) && !fix) {
//				Debug.Log ("pointer = " + rightPointer.transform.forward);
//				Debug.Log ("camera = " + Camera.main.transform.forward);
				//Command.SetDirections (rightPointer.transform.forward, 
					rightHand.transform.position);
				//Command.Select ();
				fix = true;
			} else if (!hydraControllerRight.GetButtonDown (SixenseButtons.TRIGGER))
				fix = false;

			if (hydraControllerLeft.GetButton (SixenseButtons.BUMPER) ||
			   hydraControllerRight.GetButton (SixenseButtons.BUMPER))
				//Command.Release ();

			if (hydraControllerRight.GetButtonDown (SixenseButtons.ONE)) {
				//MessagesManager.instance.PlayNextInstruction ();
			}

			if (hydraControllerRight.GetButtonDown (SixenseButtons.TWO)) {
				//MessagesManager.instance.RepeatInstruction ();
			}
		}
	}

//	public void HeadTrackingInteraction(){
//
//		//rotate crosshair
//		if (initialPosFixed) {
//			Vector3 angles = hydraControllerRight.Rotation.eulerAngles;
//			Vector3 diff = new Vector3();
//
//			if (Mathf.Abs (angles.y - lastAngles.y) > 0.5f) {
//				diff.y = angles.y - lastAngles.y;
//				lastAngles.y = angles.y;
//				diff.y *= 1.8f;
//			} 
//			if (Mathf.Abs (angles.x - lastAngles.x) > 0.5f) {
//				diff.x = angles.x - lastAngles.x;
//				lastAngles.x = angles.x;
//			}
//			Rotate (diff);
//		}
//
//		if(hydraControllerLeft.GetButtonDown(SixenseButtons.BUMPER)){
//			initialPosFixed = true;
//			lastAngles = hydraControllerRight.Rotation.eulerAngles;
//			if(experimentTime)
//				training = true;
//		}
//
//		if(hydraControllerLeft.GetButtonDown(SixenseButtons.ONE)){
//			if (training) {
//				CrosshairManager.instance.RestartGame ();
//				MessagesController.PlayFirstInstruction ();
//				training = false;
//				experimentTime = true;
//			} 
//			lastAngles = hydraControllerRight.Rotation.eulerAngles;
//		}
//
//
//		if (hydraControllerLeft.GetButtonDown (SixenseButtons.BUMPER) && initialPosFixed) {
//			Command.Select ();
//		}
//
//		//messages controller
//		if (hydraControllerLeft.GetButtonDown (SixenseButtons.TWO)) {
//			MessagesController.PlayNextInstruction ();
//		}
//
//		if (hydraControllerLeft.GetButtonDown (SixenseButtons.FOUR)) {
//			MessagesController.RepeatInstruction ();
//		}
//
//		if (initialPosFixed) {
//			Command.Move (hydraControllerLeft.JoystickX, hydraControllerLeft.JoystickY);
//		}
//	}
//		
//	public void Rotate(Vector3 angles){
//		CrosshairManager.instance.AddCrosshairAngles (angles);
//		CrosshairManager.instance.MoveCrosshair ();
//	}
}*/
