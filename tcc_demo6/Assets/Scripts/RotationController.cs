using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;


public class RotationController : MonoBehaviour {

	public static RotationController control;

	public HandleConnections handleConnections = new HandleConnections();
	public volatile BodyRotator rotator;
	public bool RUNNING = false;
	public TcpListener tcpListener;
	private Thread tcpServerRunThread;
	public volatile Transforms t = new Transforms();
	public Transforms objActualTranform = new Transforms();

	private bool first = true;
	private Quaternion lastRotation = new Quaternion ();

	private GameObject player;

	void Awake () {
		player = GameObject.Find ("FPSController");

		rotator = this.GetComponent<BodyRotator> ();
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
			tcpListener = new TcpListener (IPAddress.Any, 8002);
			tcpListener.Start ();
			RUNNING = true;
			tcpServerRunThread = new Thread (new ThreadStart (TcpServerRun));
			tcpServerRunThread.Start ();

		} else if (control != this) {
			Destroy (gameObject);
		}

	}

	public void TcpServerRun(){
		while(RUNNING) {
			try{
				TcpClient c = tcpListener.AcceptTcpClient();
				new Thread(new ThreadStart(()=> DeviceListener(c))).Start();
			}
			catch(Exception ex){
				print("Error Listener thread");
				print (ex.Message);
			}

		}
	}

	void DeviceListener (TcpClient clientDevice){
		Matrix4x4 clientDeviceMatrix;
		NetworkStream stream = clientDevice.GetStream ();

		while (clientDevice.Connected && RUNNING) {
			int pos = 0;
			byte[] bytes = new byte[261];
			bool abort = false;
			while (pos != 261 && !abort) {
				if (!clientDevice.Connected || !RUNNING)
					abort = true;
				int l = stream.Read (bytes, pos, bytes.Length - pos);
				if (l == 0)
					abort = true;
				pos += l;
			}

			if (abort)
				break;

			clientDeviceMatrix = Utils.ConvertToMatrix (bytes, 192);

			t.mutex.WaitOne ();
			Matrix4x4 rot = clientDeviceMatrix;
			Matrix4x4 tr = rotator.matrix.inverse * rot * rotator.matrix;

			//Camera Rotation

			if (!Utils.isNaN (tr.GetRotation ())) {
				if (first) {
					lastRotation = tr.GetRotation ();
					first = false;
				} else {
					//Debug.Log (clientDeviceMatrix.GetRotation ());
					t.rotateMatrix = tr * t.rotateMatrix;
					//Debug.Log("diff = " + Quaternion.Inverse(tr.GetRotation ()) * lastRotation);
					//Debug.Log ("cur = " + tr.GetRotation());
					//lastRotation = Quaternion.Lerp (lastRotation, tr.GetRotation (), 0.5f);
					//Debug.Log ("last = " + lastRotation);
					rotator.rotation = (Quaternion.Inverse(lastRotation) * tr.GetRotation());
				}//rotator.rotation = tr.GetRotation ();



			}
			t.mutex.ReleaseMutex ();

		}
		stream.Close ();
		clientDevice.Close ();

	}

	public void OnApplicationQuit() { // when application quit
		RUNNING = false;

		tcpServerRunThread.Abort ();  // Shutdown server
		tcpListener.Stop();

	}
		
}
