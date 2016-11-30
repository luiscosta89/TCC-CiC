using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class GenerateLogFile : MonoBehaviour {

	//Hydra
	SixenseInput.Controller leftController;
	SixenseInput.Controller rightController;

	//Player
	private GameObject player;

	//Hand
	private GameObject hand;

	//Errors
	Select unselectErrors;

	//Camera
	private GameObject cam;

	//Player original position
	private Vector3 lastPos;
	private float totalDistance = 0;

	//Camera original position
	private Vector3 camLastPos;
	private float totalCamDistance;

	//Hand position
	private Vector3 lastHandPos;
	private float totalHandDistance = 0;

	//Counters - Left
	private int twoLeft = 0;
	private int threeLeft = 0;
	private int fourLeft = 0;
	private int bumperLeft = 0;
	private int triggerLeft = 0;

	//Counters - Right
	private int oneRight = 0;
	private int fourRight = 0;
	private int threeRight = 0;
	private int bumperRight = 0;
	private int triggerRight = 0;

	//Result state
	private GameObject final;
	private bool passed = false;

	private int newErrors = 0;

	//Filenames
	private string filenameDNA = "TestLogDNA.txt";
	private string filenameGFP = "TestLogGFP.txt";
	private string filenameHEMO = "TestLogHEMO.txt";
	private string currentFile;
	private Scene currentScene;
	private string activeScene;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("FPSController");
		lastPos = player.transform.position;

		cam = GameObject.Find ("FirstPersonCharacter");
		camLastPos = cam.transform.transform.position;

		currentScene = SceneManager.GetActiveScene ();
		activeScene = currentScene.name;

		final = GameObject.Find ("Result");

		hand = GameObject.Find ("Hand - Left");

		unselectErrors = player.GetComponent<Select> ();
	}
	
	// Update is called once per frame
	void Update () {

		//get the controller for both hands
		if (leftController == null || rightController == null) {
			leftController = SixenseInput.GetController (SixenseHands.LEFT);
			rightController = SixenseInput.GetController (SixenseHands.RIGHT);
		}

		if ((leftController != null && leftController.Enabled) && (rightController != null && rightController.Enabled)) {

			if (leftController.GetButtonDown (SixenseButtons.TWO)) {
				twoLeft++;
			}
			if (leftController.GetButtonDown (SixenseButtons.THREE)) {
				threeLeft++;
			}
			if (leftController.GetButtonDown (SixenseButtons.FOUR)) {
				fourLeft++;
			}
			if (leftController.GetButtonDown (SixenseButtons.BUMPER)) {
				bumperLeft++;
			}
			if (leftController.GetButtonDown (SixenseButtons.TRIGGER)) {
				triggerLeft++;
			}


			if (rightController.GetButtonDown (SixenseButtons.ONE)) {
				oneRight++;
			}
			if (rightController.GetButtonDown (SixenseButtons.FOUR)) {
				fourRight++;
			}
			if (rightController.GetButtonDown (SixenseButtons.THREE)) {
				threeRight++;
			}
			if (rightController.GetButtonDown (SixenseButtons.BUMPER)) {
				bumperRight++;
			}
			if (rightController.GetButtonDown (SixenseButtons.TRIGGER)) {
				triggerRight++;
			}

		}

		if (final.tag == "Completed") {
			passed = true;
		}

			//Player position
			totalDistance += Vector3.Distance (player.transform.position, lastPos);
			lastPos = player.transform.position;

			//Hand Position
			totalHandDistance += Vector3.Distance (player.transform.position, lastHandPos);
			lastHandPos = hand.transform.position;

			//Camera postion
			totalCamDistance += Vector3.Angle(cam.transform.localEulerAngles,camLastPos);
			camLastPos = cam.transform.localEulerAngles;

					
		if (Input.GetKeyDown (KeyCode.T) && activeScene != "tutorial") {
				
			//Add errors before recording data;
			unselectErrors.numError += threeLeft + fourRight;
		
			WriteFile ();
			UnityEditor.EditorApplication.isPlaying = false;
			}
	
		}

	void WriteFile(){

		switch(activeScene) {
		case "dna":
			currentFile = filenameDNA;
			if (twoLeft + oneRight != 16) {
				newErrors = (twoLeft + oneRight) - unselectErrors.numError;
				unselectErrors.numError += newErrors;
			}
			break;
		case "green":
			currentFile = filenameGFP;
			if (twoLeft + oneRight != 7) {
				newErrors = (twoLeft + oneRight) - unselectErrors.numError;
				unselectErrors.numError += newErrors + triggerLeft + triggerRight;
			}
			break;
		case "hemo":
			currentFile = filenameHEMO;
			unselectErrors.numError += bumperLeft + bumperRight;
			break;
		}

		if (!File.Exists (currentFile)) {

			using (StreamWriter writer = File.CreateText (currentFile)) {

				writer.WriteLine ("This is my log file: " + DateTime.Now);
				writer.WriteLine ("Total distance walked: " + totalDistance);
				writer.WriteLine ("Total angle rotated camera: " + totalCamDistance);
				writer.WriteLine ("Total angle translated camera: " + totalHandDistance*1.5f);				
				writer.WriteLine ("Total time: " + Time.realtimeSinceStartup);

				writer.WriteLine ("# Pressed 2 - Left: " + twoLeft);
				writer.WriteLine ("# Pressed 3 - Left: " + threeLeft);
				writer.WriteLine ("# Pressed 4 - Left: " + fourLeft);

				writer.WriteLine ("# Pressed Bumper - Left: " + bumperLeft);
				writer.WriteLine ("# Pressed Trigger - Left: " + triggerLeft);

				writer.WriteLine ("# Pressed 1 - Right: " + oneRight);
				writer.WriteLine ("# Pressed 4 - Right: " + fourRight);
				writer.WriteLine ("# Pressed 3 - Right: " + threeRight);

				writer.WriteLine ("# Pressed Bumper - Right: " + bumperRight);
				writer.WriteLine ("# Pressed Trigger - Right: " + triggerRight);

				writer.WriteLine ("Number of errors: " + unselectErrors.numError);

				writer.WriteLine ("Passed? " + passed);

				writer.WriteLine ("");

				writer.Close ();
			}

		} else {
			using (StreamWriter writer = File.AppendText (currentFile)) {

				writer.WriteLine ("This is my log file: " + DateTime.Now);
				writer.WriteLine ("Total distance: " + totalDistance);
				writer.WriteLine ("Total angle rotated camera: " + totalCamDistance);
				writer.WriteLine ("Total angle translated camera: " + totalHandDistance*1.5f);				
				writer.WriteLine ("Total time: " + Time.realtimeSinceStartup);

				writer.WriteLine ("# Pressed 2 - Left: " + twoLeft);
				writer.WriteLine ("# Pressed 3 - Left: " + threeLeft);
				writer.WriteLine ("# Pressed 4 - Left: " + fourLeft);

				writer.WriteLine ("# Pressed Bumper - Left: " + bumperLeft);
				writer.WriteLine ("# Pressed Trigger - Left: " + triggerLeft);

				writer.WriteLine ("# Pressed 1 - Right: " + oneRight);
				writer.WriteLine ("# Pressed 4 - Right: " + fourRight);
				writer.WriteLine ("# Pressed 3 - Right: " + threeRight);

				writer.WriteLine ("# Pressed Bumper - Right: " + bumperRight);
				writer.WriteLine ("# Pressed Trigger - Right: " + triggerRight);

				writer.WriteLine ("Number of errors: " + unselectErrors.numError);

				writer.WriteLine ("Passed? " + passed);

				writer.WriteLine ("");

				writer.Close ();			
			}

		}

	}
				
}
