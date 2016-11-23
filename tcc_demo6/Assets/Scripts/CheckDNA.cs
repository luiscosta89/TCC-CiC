using UnityEngine;
using System.Collections;

public class CheckDNA : MonoBehaviour {

	private bool error = true;

	private GameObject final;

	// Use this for initialization
	void Start () {
		final = GameObject.Find ("Result");	
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject atom in GameObject.FindGameObjectsWithTag("Selected")) {
			if (atom == null) {
				error = true;
			} else if (GameObject.FindGameObjectsWithTag("Selected").Length == 16 && atom.name == "Phosphorus") {
				error = false;
			}

			if (error == false) {
				Debug.Log ("Passou no teste!");
				final.tag = "Completed";
			}
	
	}
	}
}