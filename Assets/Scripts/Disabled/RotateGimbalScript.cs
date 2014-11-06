using UnityEngine;
using System.Collections;

public class RotateGimbalScript : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Inverse(target.transform.rotation);
	}
}
