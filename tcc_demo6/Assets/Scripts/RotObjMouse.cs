using UnityEngine;
using System.Collections;

public class RotObjMouse : MonoBehaviour {

	// Use this for initialization

	public float horizontalSpeed = 2.0f;
	public float verticalSpeed = 2.0f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (1)) {

			float h = horizontalSpeed * Input.GetAxis ("Mouse X");
			float v = verticalSpeed * Input.GetAxis ("Mouse Y");

			gameObject.transform.Rotate (v, h, 0);
		}
	
	}
}
