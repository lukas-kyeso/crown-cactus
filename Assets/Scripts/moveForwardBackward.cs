using UnityEngine;
using System.Collections;

public class moveForwardBackward : MonoBehaviour {
	private GameObject factoryBuilder;
	private GameObject cameraTarget;
	private float temp;
	// Use this for initialization
	void Start () {
		factoryBuilder = GameObject.Find ("FactoryBuilderObj");
		cameraTarget = GameObject.Find("CameraTarget");
	}
	
	// Update is called once per frame
	void Update () {
		float levelOfpallets = factoryBuilder.GetComponent<FactoryBuilder> ().AveragePalsHigh;
		float distance = Vector3.Distance (this.transform.position, cameraTarget.transform.position);
		if (this.transform.name.Equals ("TOP")) {
			temp = levelOfpallets * 35;
		}else{
			temp = levelOfpallets * 5;
		}
		if(Time.time >1){
			if (distance/2 <= temp) {
				this.transform.Translate(Vector3.back * Time.deltaTime*10);
			}
			if (distance > temp) {
				this.transform.Translate(Vector3.forward * Time.deltaTime*10);
			}
		}
	}
}
