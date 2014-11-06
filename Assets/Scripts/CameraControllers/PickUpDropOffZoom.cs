using UnityEngine;
using System.Collections;

public class PickUpDropOffZoom : MonoBehaviour {

	public GameObject parametersButton;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.pickUpLocation && parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.dropOffLocation) return;

        Camera.main.transform.position = this.transform.position;

    }
}
