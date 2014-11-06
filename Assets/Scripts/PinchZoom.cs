 using UnityEngine;
using System.Collections;

public class PinchZoom : TouchLogicV2
{
	private float zoomSpeed = 3.0f;
	
	public GameObject positionScoutMarker;
	
	//buckets for caching our touch positions
	private Vector2 currTouch1 = Vector2.zero,
	lastTouch1 = Vector2.zero,
	currTouch2 = Vector2.zero,
	lastTouch2 = Vector2.zero;


	private float disCenToCam = 0.0f;
	//used for holding our distances and calculating our zoomFactor
	private float currDist = 0.0f,
	lastDist = 0.0f,
	zoomFactor = 0.0f,
	scaleFactor = 0.0f;
	
	public bool disableTouches;

    private GameObject factoryBuilder;

    public void Start()
    {
        factoryBuilder = GameObject.Find("FactoryBuilderObj");
    }
	public override void Update()//(optional) only use Update if you need to
	{

		// auto zoom back if too close to target
        disCenToCam = Vector3.Distance(Camera.main.transform.position, positionScoutMarker.transform.position);
        if (disCenToCam < 10)
            Camera.main.transform.Translate(Vector3.back * Time.deltaTime * 5);

        //disableTouches = factoryBuilder.GetComponent<FactoryBuilder>().dragging;
		//you can do some logic before you check for the touches on screen	
		if(!disableTouches)//(optional) dynamically change whether or not to check for touches
			base.Update();//must have this somewhere or TouchLogicV2's Update will be totally overwritten by this class's Update
	}
	
	public override void OnTouchMovedAnywhere()
	{
		if(GUIUtility.hotControl == 0)	Zoom();
	}
	public override void OnTouchStayedAnywhere()
	{
//		Zoom();
	}
	
	//find distance between the 2 touches 1 frame before & current frame
	//if the delta distance increased, zoom in, if delta distance decreased, zoom out
	void Zoom()
	{
		//Caches touch positions for each finger
		switch(TouchLogicV2.currTouch)
		{
		case 0://first touch
			currTouch1 = Input.GetTouch(0).position;
			lastTouch1 = currTouch1 - Input.GetTouch(0).deltaPosition;
			break;
		case 1://second touch
			currTouch2 = Input.GetTouch(1).position;
			lastTouch2 = currTouch2 - Input.GetTouch(1).deltaPosition;
			break;
		}
		
		//finds the distance between your moved touches
		//we dont want to find the distance between 1 finger and nothing
		if(TouchLogicV2.currTouch >= 1)
		{
			currDist = Vector2.Distance(currTouch1, currTouch2);
			lastDist = Vector2.Distance(lastTouch1, lastTouch2);
		}
		else
		{
			currDist = 0.0f;
			lastDist = 0.0f;
		}
		
		//Calculate the zoom magnitude
		zoomFactor = Mathf.Clamp(lastDist - currDist,-30.0f,30.0f);
		scaleFactor = Mathf.Clamp (lastDist = currDist, -1.0f, 1.0f);


		// ortho zoom limit
		if (Camera.main.isOrthoGraphic) {
			if(Camera.main.orthographicSize < 1.6f && zoomFactor < 0){
				// do nothing because we are too close and moving even closer
			}
			else{
				Camera.main.orthographicSize = Camera.main.orthographicSize + Time.deltaTime * zoomFactor * zoomSpeed;
			}
		}

		// persp zoom limit
		else{
			disCenToCam = Vector3.Distance (Camera.main.transform.position, positionScoutMarker.transform.position);
			if(disCenToCam < 10 && zoomFactor < 0){
				// do nothing because we are too close and moving even closer
			}
			else{
				Camera.main.transform.Translate (Vector3.back * zoomFactor * zoomSpeed * Time.deltaTime);
			}
		}
	}
}