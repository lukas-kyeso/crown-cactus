using UnityEngine;
using System.Collections;


public class CameraRotationController : MonoBehaviour {

    public Transform NumberOfLevelsTarget;
	public Transform NumberOfAislesTarget;
    public Transform BottomBeamHeightTarget;
    public Transform MiddleOfFirstColumnTarget;
	public Transform InboundOutboundTarget;
    public Transform PickUpDropOffTarget;
    public Transform PandDLevelsTarget;
    public Transform PandDWidthTarget;

	// Two finger zoom 

	public Camera selectedCamera;
	public Camera UICamera;


    public GameObject flrCenter;

	private float distance = 30;


	private Vector3 ydistance;


	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
    void LateUpdate()
	{
      
			if(GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.aisleLocation){
			this.transform.LookAt(NumberOfAislesTarget.position  + -Camera.main.transform.right * 4);
		}
//		else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.bayLocation)
//            {
//			print ("in here");
//				// do nothing
//            }
            else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.levelsLocation)
            {
			this.transform.LookAt(NumberOfLevelsTarget.position  + -Camera.main.transform.right * 4);
		}
		else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.verticalBeamSpacingLocation)
            {
             //   this.transform.LookAt(MiddleOfFirstColumnTarget);
            }
            else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.bottomBeamHeightLocation)
            {
			this.transform.LookAt(BottomBeamHeightTarget.position );
		}
		else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.sideLocation)
            {
                
            }
			else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.inboundOutboundStationLocation)
			{
			this.transform.LookAt(InboundOutboundTarget.position);
			}
            else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.pickUpLocation || GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.dropOffLocation)
            {
			this.transform.LookAt(PickUpDropOffTarget.position);
		}
		else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.pAndDLevelsLocation)
            {
			this.transform.LookAt(PandDLevelsTarget.position);
		}
		else if (GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation == Location.pAndDWidthLocation)
            {
			this.transform.LookAt(PandDWidthTarget.position);
		}
		else{
				this.transform.LookAt (flrCenter.transform.position + -Camera.main.transform.right * 8);
			}
   

        mouseZoom();
        updatePosition();

	}

	void Update()
	{
	
	}

	private void mouseZoom()
	{
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if(Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				selectedCamera.transform.Translate(0,0,1);
				distance = Vector3.Distance(selectedCamera.transform.position,flrCenter.transform.position);
			}
			if(Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				selectedCamera.transform.Translate(0,0,-1);
				distance = Vector3.Distance(selectedCamera.transform.position,flrCenter.transform.position);
			}
		}
    }

	private void updatePosition()
	{
		if (Input.touchCount == 2) return;


		// keyboard controls
		if (Input.GetKey (KeyCode.A))
		{
            GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation = Location.none;
			this.transform.Translate (Vector3.left * (Time.deltaTime*10));
		}
		
		if (Input.GetKey (KeyCode.D))
		{
            GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation = Location.none;
			this.transform.Translate (Vector3.right * (Time.deltaTime*10));
		}
		
		if (Input.GetKey (KeyCode.W))
		{
            GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation = Location.none;
			this.transform.Translate (Vector3.up * (Time.deltaTime*10));
		}
		
		if (Input.GetKey (KeyCode.S))
		{
            GameObject.Find("ParametersButton").GetComponent<ParametersButton>().currentCameraLocation = Location.none;
			this.transform.Translate (Vector3.down * (Time.deltaTime*10));
		}

		
    }
}
