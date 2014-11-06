using UnityEngine;
using System.Collections;

// platform is the pallet row and trucks shown in pick and drop off views

public class PlatformController : MonoBehaviour {

    ParametersButton parametersButton;
	// Use this for initialization
	void Start () {
        parametersButton = GameObject.Find("ParametersButton").GetComponent<ParametersButton>();
	}
	
	// Update is called once per frame
	void Update () {

  
        Renderer[] rs = GetComponentsInChildren<Renderer>();

        if (parametersButton.currentCameraLocation == Location.pickUpLocation || parametersButton.currentCameraLocation == Location.dropOffLocation)
        {
            foreach (Renderer r in rs)
            {
                if (r.transform.name != "pickUpDropOffHighlight")
                {
                    r.enabled = true;
                }
            } 
        }
        else
        {
            foreach (Renderer r in rs)
                {
                    r.enabled = false;
                }
        }
	}
}
