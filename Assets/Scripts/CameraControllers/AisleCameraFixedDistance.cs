using UnityEngine;
using System.Collections;



public class AisleCameraFixedDistance : MonoBehaviour {
    public GameObject factoryBuilder, parametersButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.aisleLocation) return;
        if (factoryBuilder.GetComponent<FactoryBuilder>().getLastColumnX() == 0) return;

        float distance = 12;
        float columnX = factoryBuilder.GetComponent<FactoryBuilder>().getLastColumnX();
        Vector3 closestPallet = factoryBuilder.GetComponent<FactoryBuilder>().getFurthestPalXVector();
        this.transform.position = new Vector3(distance + columnX, 5, closestPallet.z - 9 );
	}
}
