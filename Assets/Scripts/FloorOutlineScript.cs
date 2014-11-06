using UnityEngine;
using System.Collections;

public class FloorOutlineScript : MonoBehaviour {

	private GameObject factoryBuilder;
	private GameObject parameters;

	// Use this for initialization
	void Start () {
		factoryBuilder = GameObject.Find("FactoryBuilderObj");
		parameters = GameObject.Find("ParametersButton");
	}


	// Update is called once per frame
	void LateUpdate () {
		// pos
		float avgColXpos = factoryBuilder.GetComponent<FactoryBuilder>().getAvgXpos();
		float avgColZpos = factoryBuilder.GetComponent<FactoryBuilder>().getAvgZpos();
		this.transform.position = new Vector3(avgColXpos, 0, avgColZpos);

		// scale
		Vector3 shrink = new Vector3(0.00f, 0.00f, 0.00f);  //shrinks each pane by a uniform amount
		float numColumns = parameters.GetComponent<ParametersButton>().NumAisles;
		float lowestX = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos(0);
		float highestX = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos((int)numColumns - 1);
		float lowestZ = factoryBuilder.GetComponent<FactoryBuilder>().getLowestFrameZpos();
		float highestZ = factoryBuilder.GetComponent<FactoryBuilder>().getHighestFrameZpos();
		transform.localScale = new Vector3((highestX - lowestX) / 10, 1, (highestZ - lowestZ) / 10) - shrink;
	}
}
