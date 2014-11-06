using UnityEngine;
using System.Collections;

public class toCenterOfFrontface : MonoBehaviour {
	private GameObject factoryBuilder;

	// Use this for initialization
	void Start () {
		factoryBuilder = GameObject.Find ("FactoryBuilderObj");
	}
	
	// Update is called once per frame
	void Update () {
		float colHeight = factoryBuilder.GetComponent<FactoryBuilder> ().AveragePalsHigh;
		if (Time.time > 1) {
			if (this.transform.position.y != colHeight) {
				this.transform.position = new Vector3 (this.transform.position.x, colHeight/2, this.transform.position.z);
			}
		}
	}
}
