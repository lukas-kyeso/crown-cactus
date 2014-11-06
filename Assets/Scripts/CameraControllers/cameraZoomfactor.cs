using UnityEngine;
using System.Collections;

public class cameraZoomfactor : MonoBehaviour {
	private GameObject factoryBuilder;

	// Use this for initialization
	void Start () {
		factoryBuilder = GameObject.Find ("FactoryBuilderObj");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float colHeight = factoryBuilder.GetComponent<FactoryBuilder> ().AveragePalsHigh;
		//float colLevels = factoryBuilder.GetComponent<FactoryBuilder> ().getNumPalletsHighOnCol (0);
		if (Time.time > 1) {
			if (this.transform.position.y != colHeight) {
				this.transform.position = new Vector3 (this.transform.position.x, colHeight, this.transform.position.z);
			}
		}
	}
}
