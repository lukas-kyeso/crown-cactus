using UnityEngine;
using System.Collections;

public class BaysZoom : MonoBehaviour {
    public GameObject farLeft;
    public GameObject farRight;
    public GameObject parametersButton;



    private bool hasBeenMovedFromInitial = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // should we even be on bays loc? if not then get out
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.bayLocation) return;

        Camera.main.transform.rotation = this.transform.rotation;
        Camera.main.transform.Rotate(new Vector3(0, 90, 0));
        Camera.main.transform.position = this.transform.position;

        Vector3 screenLeftX = Camera.main.WorldToScreenPoint(farLeft.transform.position);
        Vector3 screenRightX = Camera.main.WorldToScreenPoint(farRight.transform.position);

        if (screenLeftX.x < 200 || screenLeftX.x > Screen.width)
        {
            Camera.main.transform.Translate(new Vector3(0, 0, -1));
            this.transform.position = Camera.main.transform.position;
        }

        else if (screenLeftX.x > Screen.width / 4 && screenRightX.x < Screen.width - Screen.width / 4)
        {
            Camera.main.transform.Translate(new Vector3(0, 0, 1));
            this.transform.position = Camera.main.transform.position;
        }
	}
}
