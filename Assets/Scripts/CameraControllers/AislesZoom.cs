using UnityEngine;
using System.Collections;

public class AislesZoom : MonoBehaviour {
    public GameObject farLeft;
    public GameObject farRight;
    public GameObject parametersButton;



    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // should we even be on aisle loc? if not then get out
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.aisleLocation) return;


		Camera.main.transform.position = this.transform.position;

	}
}
