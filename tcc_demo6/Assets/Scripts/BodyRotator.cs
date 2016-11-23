using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class BodyRotator : MonoBehaviour {
	public Quaternion rotation;
	public Matrix4x4 matrix;
	private Quaternion lastRotation;

	public GameObject body;
	public GameObject hands;

	private GameObject player;

	void Start(){

		player = GameObject.Find ("FPSController");
		matrix = player.transform.localToWorldMatrix;
		rotation = new Quaternion ();
		lastRotation = new Quaternion ();

		//Command.rotator = this;
	}

	void Update(){
		matrix = player.transform.localToWorldMatrix;

		if (!Utils.isNaN (rotation)) {
			rotation.x = 0;
			rotation.z = 0;
			body.transform.rotation = rotation;
//			hands.transform.rotation = Quaternion.LookRotation(body.transform.forward);
//			Vector3 position = Camera.main.transform.position;
//			position.y -= 0.1f;
//			body.transform.position = position;

//			body.transform.Rotate (new Vector3 (0, angle * speed, 0));
//			Vector3 position = Camera.main.transform.position + (transform.forward * 0.1f);
//			position.y -= 0.1f;
//			transform.position = position;
		}
	}
		
}
