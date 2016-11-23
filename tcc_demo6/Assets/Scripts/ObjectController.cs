using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour {

	public static ObjectController instance {
		get { return FindObjectOfType<ObjectController>(); }
	}

	//public List<SelectionInfo> selectionInfos;
	//public List<ReleaseInfo> releaseInfos;

	private GameObject selectedObject; //current object selected
	private Transform originalParent; //original parent of the selected object
	private Vector3 originalPosition;
	private Vector3 originalRotation;

	private bool rope = false;

	public List<Texture> textures;
	private GameObject helmet, taskCard, lp, lv;

	private Vector3 xyz;

	private bool luvaPesada = false, luvaVaqueta = false, cinto = false;
	//private RopeBuilder ropeBuilder;
	private List<string> connectedCones;
	private List<string> connectedRopes;

	private Transform newParent;

	public bool training = false;

	// Use this for initialization
	void Start () {
		//selectionInfos = new List<SelectionInfo> ();
		//releaseInfos = new List<ReleaseInfo> ();

		//ropeBuilder = transform.GetComponent<RopeBuilder> ();

		if (!training) {
			helmet = GameObject.Find ("RightHelmet");
			helmet.SetActive (false);

			taskCard = GameObject.Find ("RightTaskCard");
			taskCard.SetActive (false);

			lv = GameObject.Find ("LuvaVaqueta");
			lp = GameObject.Find ("LuvaPesada");
		}
		xyz = new Vector3(0,0,0);
		connectedCones = new List<string> ();
		connectedRopes = new List<string> ();

		//newParent = GameObject.Find ("Body").transform;
		//newParent = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void MoveObject(){

	}

	public void Select(Vector3 forward, Vector3 position){

		Ray ray = new Ray (position, forward);

		RaycastHit hitinfo = new RaycastHit ();
		Physics.Raycast (ray, out hitinfo);

		//Debug.Log ("hand dir = " + forward);
		//Debug.Log ("camera dir = " + Camera.main.transform.forward);

		if (hitinfo.collider != null) {
			GameObject hitobj = hitinfo.collider.gameObject;
			if (hitobj.tag == "Selectable") {

				float distance = Vector3.Distance (position, hitobj.transform.position);
				float maxDistance = 1.5f;

				//Debug.Log (hitobj.transform.position - position);
				//Debug.Log ("distance = " + distance);

				bool valid = false;
				string objName = hitobj.name;
				if (objName.Substring (0, 4) == "rope") {
					string name = connectedRopes.Find (p => (p == objName));
					if (name == null)
						valid = true;
				}
				else if (objName == "LuvaVaqueta" && distance > 6f)
					valid = true;
				else if (hitobj.transform.parent.name == "Signposts" && distance > 9.7f)
					valid = true;
				else if (objName.Substring (0, 4) == "Cone") { 
					if (distance < maxDistance && distance > 0.8f)
						valid = true;
				}
				else if (objName.Substring (0, 4)  != "Cone" && distance < maxDistance)
					valid = true;
				
				
				if (valid) {
					//Debug.Log (selectedObject);
					if (!selectedObject && connectedCones.Find (p => (p == objName)) == null) {
						selectedObject = hitobj;
						Debug.Log ("selectedObject = " + selectedObject.name);

						originalParent = selectedObject.transform.parent;
						originalPosition = selectedObject.transform.localPosition;
						originalRotation = selectedObject.transform.eulerAngles;

						if (selectedObject.transform.parent.name == "Eletrician") {
							//UpdatePlayerClothes ();
						} else {
							string parent = selectedObject.transform.parent.name;
							if (parent == "Signposts") {
								xyz = new Vector3 (-0.05f, 0.2f, -0.07f);
							} else if (parent == "Ropes") {
								//ropeBuilder.rope = selectedObject;
								connectedRopes.Add (selectedObject.name);
								selectedObject.GetComponent<BoxCollider> ().enabled = false;
								rope = true;
								xyz = new Vector3 (-1.0f, 0.4f, -0.7f);
							} else if (parent == "Cones") {
								xyz = new Vector3 (-0.8f, 0.4f, -0.6f);
							} else {
								xyz = new Vector3 (-0.9f, 0.6f, -0.7f);
							}

							selectedObject.transform.eulerAngles = new Vector3 (0, 0, 0);

							Vector3 pos = selectedObject.transform.position;
							pos.x += forward.x * (distance * xyz.x);
							pos.z += forward.z * (distance * xyz.z);
							pos.y += xyz.y;
							selectedObject.transform.position = pos;
							selectedObject.transform.parent = newParent;

							//if(!rope)	
								//selectedObject.transform.eulerAngles = originalRotation;

							//SelectionInfo info = selectedObject.AddComponent<SelectionInfo>();
							//selectionInfos.Add(info);
						}

					} else if (rope) {
						if (hitobj.transform.parent.name == "Cones") {
							/*if (!ropeBuilder.origin) {
								ropeBuilder.origin = hitobj;
								connectedCones.Add (hitobj.name);
								ObjectFader.FadeColor(hitobj, 1.5f * Color.white + 3f * Color.red, 0.25f);
							} else if (!ropeBuilder.target && ropeBuilder.origin.name != hitobj.name) {
								ropeBuilder.target = hitobj;
								ObjectFader.FadeColor(hitobj, 1.5f * Color.white + 3f * Color.red, 0.25f);
								ropeBuilder.BuildRope ();
								connectedCones.Add (hitobj.name);
								rope = false;
								selectedObject = null;
							}*/
						}
					}
				}
			}
		}
	}

	/*void UpdatePlayerClothes(){
		SkinnedMeshRenderer eletrician = GameObject.Find ("Eletricista2").GetComponent<SkinnedMeshRenderer>();
		Material[] mats;

		/if (selectedObject.name == "TaskCard") {
			taskCard.SetActive (true);
			selectedObject.SetActive (false);
			selectedObject = null;
		} 

		if (selectedObject.name == "Helmet") {
			helmet.SetActive (true);
			selectedObject.SetActive (false);
			selectedObject = null;
		} 

		if (selectedObject.name == "LuvaPesada") {
			if (cinto) {
				if (luvaVaqueta) {
					lv.SetActive (true);
				}
				eletrician.material.mainTexture = textures [2];
			} else {
				eletrician.material.mainTexture = textures [3];
				lv.SetActive (true);
			}
			luvaPesada = true;
		} else if (selectedObject.name == "LuvaVaqueta") {
			if (cinto) {
				if (luvaPesada) {
					lp.SetActive (true);
				}
				eletrician.material.mainTexture = textures [0];
			} else {
				eletrician.material.mainTexture = textures [4];
				lp.SetActive (true);
			}
			luvaVaqueta = true;
		} else if (selectedObject.name == "SeatBelt") {
			if (luvaPesada) {
				eletrician.material.mainTexture = textures [2];
			} else if (luvaVaqueta) {
				eletrician.material.mainTexture = textures [0];
			} else {
				eletrician.material.mainTexture = textures [1];
			}
			cinto = true;
		} 

		selectedObject.SetActive (false);
		selectedObject = null;
	}*/

	public void Release(){
		if (selectedObject) {	
			selectedObject.transform.parent = originalParent;
			Vector3 pos = selectedObject.transform.position;
			pos.y -= xyz.y;
			selectedObject.transform.position = pos;

			//ReleaseInfo info = selectedObject.AddComponent<ReleaseInfo>();
			//releaseInfos.Add(info);

			if (rope) {
				//RopeBuilder.instance.Clear ();
				rope = false;
			}

			selectedObject = null;
		}
	}

}
