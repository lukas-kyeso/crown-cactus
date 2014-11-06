using UnityEngine;
using System.Collections;

public class IsoCameraLocation : MonoBehaviour {
	private GameObject factoryBuilder;
	// Use this for initialization
	void Start () {
		factoryBuilder = GameObject.Find ("FactoryBuilderObj");
	}
	
	// Update is called once per frame
	void Update () {
		// End X position of last columns each columns is 4.15 width X
		float numberOfColumns = factoryBuilder.GetComponent<FactoryBuilder> ().getLastColumnX();
		// level of pallet each is 1.1 height Y
		float levelOfpallets = factoryBuilder.GetComponent<FactoryBuilder> ().AveragePalsHigh;

//		print (numberOfColumns+","+levelOfpallets*1.1);

		this.transform.position = new Vector3 ((numberOfColumns+32.55f), ((levelOfpallets * 1.1f)+9.5f), -30f);
	}
}
