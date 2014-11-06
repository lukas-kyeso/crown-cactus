using UnityEngine;
using System.Collections;

public enum gimbalOrientations{north, east, south, west, top};

public class GimbalPointerScript : MonoBehaviour {

	public GameObject mainCamera;
	public gimbalOrientations orientation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
//		mainCamera.GetComponent<CameraRotationController>().jumpToPosition(orientation);
	}
}
