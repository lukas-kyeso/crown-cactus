using UnityEngine;
using System.Collections;

public class JSONReciever : MonoBehaviour {

	string jsonString = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void ParseJSON(string string_parameter){
		jsonString = string_parameter;
	}

	void OnGUI(){
		GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 100, 100), jsonString);
	}
}
