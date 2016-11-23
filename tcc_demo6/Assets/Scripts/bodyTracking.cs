using UnityEngine;
using System.Collections;

// Activate head tracking using the gyroscope
public class bodyTracking : MonoBehaviour {
	public Transform target;
	//public GameObject cube; // First Person Controller parent node
	//public GameObject head; // First Person Controller camera
	//private GameObject body;

	// The initials orientation
	private int initialOrientationX;
	private int initialOrientationY;
	private int initialOrientationZ;

	// Use this for initialization
	void Start () {
		// Activate the gyroscope
		Input.gyro.enabled = true;

		//body = GameObject.Find ("FPSController");

		// Save the firsts values
		//initialOrientationX = (int)Input.gyro.rotationRateUnbiased.x;
		initialOrientationY = (int)Input.gyro.rotationRateUnbiased.y;
		//initialOrientationZ = (int)-Input.gyro.rotationRateUnbiased.z;
	}

	// Update is called once per frame
	void Update () {

		// Rotate the player and head using the gyroscope rotation rate
		target.transform.Rotate (0, initialOrientationY -Input.gyro.rotationRateUnbiased.y, 0);
		//head.transform.Rotate (initialOrientationX -Input.gyro.rotationRateUnbiased.x, 0, initialOrientationZ + Input.gyro.rotationRateUnbiased.z);

	}
}