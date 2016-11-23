using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class PointCloudManager : MonoBehaviour {

	// File
	public string dataPath;
	private string fileoff;
	private string filepdb;
	public Material matVertex;

	private string firstLine;
	private string secondLine;

	//Number of atoms
	private int carbon = 0;
	private int oxygen = 0;
	private int hydrogen = 0;
	private int nitrogen = 0;
	private int sulfur = 0;
	private int phosphorus = 0;
	private int iron = 0;

	// GUI
	private float progress = 0;
	private string guiText;
	private bool loaded = false;

	// PointCloud
	private GameObject pointCloud;
	private GameObject cloud;

	public float scale = 1;
	private bool invertYZ = false;
	private bool forceReload = true;

	public int numPoints;
	public int numPointGroups;
	private int limitPoints = 65000;

	private Vector3[] points;
	private GameObject[] spheres;
	private Color[] colors;
	private Vector3 minValue;

	private GameObject player;

	void Start () {
		// Get Filename
		fileoff = Path.GetFileName (dataPath);
		filepdb = Path.GetFileName (dataPath);

		loadScene ();
	}

	void loadScene(){
		// Check if the PointCloud was loaded previously
		if (forceReload) {
			loadPointCloud ();
		}
		else
			// Load stored PointCloud
			loadPointCloud();
	}

	void loadPointCloud(){
		// Check what file exists
		if (File.Exists (Application.dataPath + dataPath + ".off")) {
			// load off
			StartCoroutine ("loadOFF", dataPath + ".off");
		}		
		else 
			Debug.Log ("File '" + dataPath + "' could not be found");
		}

	void loadPDBFile(){
		// Check what file exists
		if (File.Exists (Application.dataPath + dataPath + ".pdb")) {
			// load off
			readPDBFile(dataPath + ".pdb");
		}		
		else 
			Debug.Log ("File '" + dataPath + "' could not be found");
	}

	// Start Coroutine of reading the points from the OFF file and creating the meshes
	IEnumerator loadOFF(string dPath){
			// Read file
			StreamReader sr = new StreamReader (Application.dataPath + dPath);
			sr.ReadLine (); // OFF
			string[] buffer = sr.ReadLine ().Split (); // nPoints, nFaces

			numPoints = int.Parse (buffer [0]);
			points = new Vector3[numPoints];
			colors = new Color[numPoints];
			minValue = new Vector3 ();
			spheres = new GameObject[numPoints];

			for (int i = 0; i < numPoints; i++) {
				buffer = sr.ReadLine ().Split ();

			// Relocate Points near the origin
			calculateMin(points[i]);

				if (!invertYZ) {
					points [i] = new Vector3 (float.Parse (buffer [0]) * scale, float.Parse (buffer [1]) * scale, float.Parse (buffer [2]) * scale);
					// Creates a sphere on the point position
					spheres [i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					spheres [i].AddComponent <Scale>();
					spheres [i].tag = "Unselected";
					spheres [i].transform.position = points [i];		

				} else {
					points [i] = new Vector3 (float.Parse (buffer [0]) * scale, float.Parse (buffer [2]) * scale, float.Parse (buffer [1]) * scale);
					// Creates a sphere on the point position
					spheres [i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					spheres [i].AddComponent <Scale>();
					spheres [i].tag = "Unselected";
					spheres [i].transform.position = points [i];					
				}
				
				//Assign colors to spheres
				if (buffer.Length >= 5) {
					colors [i] = new Color (int.Parse (buffer [3]) / 255.0f, int.Parse (buffer [4]) / 255.0f, int.Parse (buffer [5]) / 255.0f);
					spheres [i].GetComponent<Renderer> ().material.color = colors [i];
				} else
					colors [i] = Color.cyan;
				spheres [i].GetComponent<Renderer> ().material.color = colors [i];

				// GUI
				progress = i * 1.0f / (numPoints - 1) * 1.0f;
				if (i % Mathf.FloorToInt (numPoints / 20) == 0) {
					guiText = i.ToString () + " out of " + numPoints.ToString () + " loaded";
					yield return null;
				}
			}

			// Instantiate Point Groups
			numPointGroups = Mathf.CeilToInt (numPoints * 1.0f / limitPoints * 1.0f);

			pointCloud = new GameObject (fileoff);

			for (int i = 0; i < numPointGroups - 1; i++) {
				InstantiateMesh (i, limitPoints);
				if (i % 10 == 0) {
					guiText = i.ToString () + " out of " + numPointGroups.ToString () + " PointGroups loaded";
					yield return null;
				}
			}
			InstantiateMesh (numPointGroups - 1, numPoints - (numPointGroups - 1) * limitPoints);
						

		loadPDBFile ();

		player = GameObject.Find ("FPSController");
		pointCloud.transform.localPosition = player.transform.localPosition;
	}

	void readPDBFile (string dPath){	

		int tempPoints = numPoints;

		using (StreamReader reader = new StreamReader (Application.dataPath + dPath))
		{
			bool hasWord = false;
			bool hasTitle = false;
			int i = 0;
			int index;
			string line = null;

			//Read file and find first TITLE
			while (!reader.EndOfStream) {
				line = reader.ReadLine ();

				if (line.StartsWith ("TITLE")) {
					hasTitle = true;
					line = line.TrimEnd ();
					break;
				}
			}

			if (hasTitle) {
				index = line.IndexOf("TITLE");

				firstLine = (index < 0)
					? line
					: line.Remove(index, "TITLE".Length);

				line = reader.ReadLine ();

				index = line.IndexOf("TITLE");

				secondLine = (index < 0)
					? line
					: line.Remove(index, "TITLE".Length);

				firstLine = firstLine.TrimStart ();
				secondLine = secondLine.TrimStart ();
			}

			Debug.Log (firstLine);
			Debug.Log (secondLine);

			//Read file and find first ATOM
			while (!reader.EndOfStream) {
				line = reader.ReadLine ();

				if (line.StartsWith ("ATOM")) {
					hasWord = true;
					line = line.TrimEnd ();
					Debug.Log ("ATOM line");
					break;
				}
			}

			if(hasWord)					
			{
				//Adjust sizes of each atom by name
				while(i < tempPoints)				
				{
					if (line.StartsWith ("ATOM") && line.EndsWith ("O")) {
						spheres [i].name = "Oxygen";
						spheres [i].transform.localScale += new Vector3 (1.81f, 1.81f, 1.81f);
						oxygen++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("C")) {
						spheres [i].name = "Carbon";
						spheres [i].transform.localScale += new Vector3 (2.53f, 2.53f, 2.53f);
						carbon++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("N")) {
						spheres [i].name = "Nitrogen";
						spheres [i].transform.localScale += new Vector3 (2.11f, 2.11f, 2.11f);
						nitrogen++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("H")) {
						spheres [i].name = "Hydrogen";
						spheres [i].transform.localScale += new Vector3 (2.0f, 2.0f, 2.0f);
						hydrogen++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("P")) {
						spheres [i].name = "Phosphorus";
						spheres [i].transform.localScale += new Vector3 (3.7f, 3.7f, 3.7f);
						phosphorus++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("FE")) {
						spheres [i].name = "Iron";
						spheres [i].transform.localScale += new Vector3 (5.89f, 5.89f, 5.89f);
						iron++;
						i++;
					} else if (line.StartsWith ("ATOM") && line.EndsWith ("S")) {
						spheres [i].name = "Sulfur";
						spheres [i].transform.localScale += new Vector3 (3.32f, 3.32f, 3.32f);
						sulfur++;
						i++;
					}

					line = reader.ReadLine ();
					line = line.TrimEnd ();
					}
			}
		}
	}

	void InstantiateMesh(int meshInd, int nPoints){
		// Create Mesh
		GameObject pointGroup = new GameObject (fileoff + meshInd);
		pointGroup.AddComponent<MeshFilter> ();
		pointGroup.AddComponent<MeshRenderer> ();
		pointGroup.GetComponent<Renderer>().material = matVertex;

		pointGroup.GetComponent<MeshFilter> ().mesh = CreateMesh (meshInd, nPoints, limitPoints);
		pointGroup.transform.parent = pointCloud.transform;

		pointCloud.AddComponent<RotObj> ();
		pointCloud.AddComponent<RotObjMouse> ();

		//Make all spheres child of the main GameObject
		for (int i = 0; i < numPoints; i++) {
			spheres [i].transform.parent = pointCloud.transform;
		}

		//Adds moving scrit after set parent
		for (int i = 0; i < numPoints; i++) {
			spheres [i].AddComponent <Move>();
		}

		//Adds mouse draggins script to each sphere
		//for (int i = 0; i < numPoints; i++) {
			//spheres [i].AddComponent <mouseDragObject> ();
		//}
	}

	Mesh CreateMesh(int id, int nPoints, int limitPoints){

		Mesh mesh = new Mesh ();

		Vector3[] myPoints = new Vector3[nPoints]; 
		int[] indecies = new int[nPoints];
		Color[] myColors = new Color[nPoints];

		for(int i=0;i<nPoints;++i) {
			myPoints[i] = points[id*limitPoints + i] - minValue;
			indecies[i] = i;
			myColors[i] = colors[id*limitPoints + i];
		}

		mesh.vertices = myPoints;
		mesh.colors = myColors;
		mesh.SetIndices(indecies, MeshTopology.Points,0);
		mesh.uv = new Vector2[nPoints];
		mesh.normals = new Vector3[nPoints];

		return mesh;
	}

	void calculateMin(Vector3 point){
		if (minValue.magnitude == 0)
			minValue = point;
		if (point.x < minValue.x)
			minValue.x = point.x;
		if (point.y < minValue.y)
			minValue.y = point.y;
		if (point.z < minValue.z)
			minValue.z = point.z;
	}
}


