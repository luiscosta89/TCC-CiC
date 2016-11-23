using UnityEngine;
using System.Collections;

public class Remove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	//Pesquisar se eh melhor atribuir um botao para cada atomo ou fazer um menu de selecao



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {

			if(name=="Carbon" || name=="Oxygen" || name=="Nitrogen" || name=="Phosphorus")
			{
				MeshRenderer rend = GetComponent<MeshRenderer> ();
				rend.enabled = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.Z)) {

			if(name=="Carbon")
			{
				MeshRenderer rend = GetComponent<MeshRenderer> ();
				rend.enabled = true;
			}
		}
	
	}
}
