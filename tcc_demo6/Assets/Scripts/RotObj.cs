using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class RotObj : MonoBehaviour 
{
	//Get the input vector from Hydra
	SixenseInput.Controller leftController = SixenseInput.GetController(SixenseHands.LEFT);
	SixenseInput.Controller rightController = SixenseInput.GetController(SixenseHands.RIGHT);

	private float x;
	private float y;

	private float rotAngle;

	private float rotationX = 0;
	private float rotationY = 0;

	private float sensitivityX = 150;
	private float sensitivitiY = 150;

	private float minimumY = -60;
	private float maximumY = 60;

	private GameObject player;

	void Start() {
		player = GameObject.Find ("FPSController");
	}

	void Update() {

		if (leftController == null || rightController == null) {
			leftController = SixenseInput.GetController (SixenseHands.LEFT);
			rightController = SixenseInput.GetController (SixenseHands.RIGHT);
		}

		if(rightController != null && rightController.Enabled) {


			//Rotate scene
			/*if (rightController.GetButtonDown (SixenseButtons.JOYSTICK)) {

				rotationX = player.transform.localEulerAngles.y + rightController.JoystickX * sensitivityX * Time.deltaTime; 

				float joystickY = rightController.JoystickY;

				if (joystickY > 0.05f || joystickY < 0.0f) {
					rotationY += rightController.JoystickY * sensitivitiY * Time.deltaTime;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				}

				player.transform.localEulerAngles = new Vector3 (0, rotationX, 0);

				Camera.current.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
			} */

			//if(leftController.GetButtonDown (SixenseButtons.JOYSTICK)) {
				float x = rightController.JoystickX;
				float y = rightController.JoystickY;

				float R_analog_threshold = 0.10f;

				if (Mathf.Abs (x) < R_analog_threshold) {
					x = 0.0f;
				}

				if (Mathf.Abs (y) < R_analog_threshold) {
					y = 0.0f;
				}

				if (x != 0.0f || y != 0.0f) {
					rotAngle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;

					transform.Rotate (new Vector3 (0, x, y));
				}
			//}

			
		}

	}
}
	
	
	
