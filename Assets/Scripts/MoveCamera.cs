using UnityEngine;
using System.Collections;

public class MoveCamera : TouchLogicV2
{
	// pallet dragging switch - for knowing when to disable moving because we don't want both same time
	private bool draggingPallet = false;
	
	private float moveSpeed = 1.3f;
	
	private Vector2 beginPosition = Vector2.zero,
	movePosition = Vector2.zero;
	
	private float DistanceX = 0,
	DistanceY = 0;
	
	public GameObject centre;
	private float distance = 0.0f;
	private float angle;
	private bool oneTouchMove;
	
	public void setDraggingPallet(bool setDraggingPallet){
		draggingPallet = setDraggingPallet;
	}
	
	public bool getDraggingPallet(){
		return draggingPallet;
	}
	
	//buckets for caching our touch positions
	private Vector2 currTouch1 = Vector2.zero,
	lastTouch1 = Vector2.zero,
	currTouch2 = Vector2.zero,
	lastTouch2 = Vector2.zero;
	
	//used for holding our distances and calculating our zoomFactor
	private float currDist = 0.0f,
	moveDist = 0.0f,
	lastDist = 0.0f,
	moveFactor = 0.0f,
	zoomFactor = 0.0f;
	
	public bool disableTouches;
	
	public override void Update()//(optional) only use Update if you need to
	{
		if(draggingPallet) return;
		//you can do some logic before you check for the touches on screen	
		if(!disableTouches)//(optional) dynamically change whether or not to check for touches
			base.Update();//must have this somewhere or TouchLogicV2's Update will be totally overwritten by this class's Update
	}
	
	public override void OnTouchMovedAnywhere()
	{
		//		if(draggingPallet) return;
		if (Input.touches.Length == 1 && GUIUtility.hotControl == 0 ) 
			Move();
	}
	public override void OnTouchStayed()
	{
		
	}
	
	public override void OnTouchBeganAnywhere(){
		beginPosition = Input.mousePosition;
		movePosition = beginPosition;
	}
	
	//find distance between the 2 touches 1 frame before & current frame
	//if the delta distance increased, zoom in, if delta distance decreased, zoom out
	void Move()
	{
		if(draggingPallet) return;
		//Caches touch positions for each finger
		
		//finds the distance between your moved touches
		//we dont want to find the distance between 1 finger and nothing
		if (TouchLogicV2.currTouch < 1) {
			GameObject.Find ("ParametersButton").GetComponent<ParametersButton> ().currentCameraLocation = Location.none;
			GameObject.Find ("ParametersButton").GetComponent<ParametersButton> ().highlightController.SetHighlightArea (highlightSection.none);
			beginPosition = Input.mousePosition;
			
			DistanceX = (beginPosition.x - movePosition.x);
			DistanceY = (beginPosition.y - movePosition.y);
			
			angle = Vector3.Angle (centre.transform.position - this.transform.position, Vector3.up);
			
			//Calculate the move factor
			moveFactor = Mathf.Clamp (moveDist, -30.0f, 30.0f);
			
			moveDist = Vector2.Distance (currTouch1, lastTouch1);
			
			//			if (Vector3.Angle (centre.transform.position - this.transform.position, Vector3.up) < 85) {
			//				if (DistanceY < 0)
			//						Camera.main.transform.RotateAround (centre.transform.position, transform.right, -DistanceY * Time.deltaTime * 10f);
			//			} else {
			//				if (Vector3.Angle (centre.transform.position - this.transform.position, Vector3.up) > 85) {
			//				//	if(DistanceY + transform.localRotation.x > 0){
			//						Camera.main.transform.RotateAround (centre.transform.position, transform.right, -DistanceY * Time.deltaTime * 10f);
			//				//	}	
			//				}
			//				Camera.main.transform.RotateAround (centre.transform.position, new Vector3 (0.0f, 0.1f, 0.0f), DistanceX * Time.deltaTime * 10f);
			//			}
			angle = Vector3.Angle (centre.transform.position - this.transform.position, Vector3.up);
			if(angle > 95){
				if(DistanceY > 0){
					float tempY = DistanceY * 2;
					float tempY2 = (tempY * -1);
					Camera.main.transform.RotateAround (centre.transform.position, transform.right, tempY2 * Time.deltaTime * 1f);
				}
			}
			if(angle < 185){
				if(DistanceY < 0){
					float tempY = DistanceY * 2;
					float tempY2 = (tempY * -1);
					//					while(Camera.main.transform.rotation.x > 1)
					Camera.main.transform.RotateAround (centre.transform.position, transform.right, tempY2 * Time.deltaTime * 1f);
				}
			}
			
			//			Camera.main.transform.RotateAround (centre.transform.position, transform.right, -DistanceY * Time.deltaTime * 10f);
			Camera.main.transform.RotateAround (centre.transform.position, new Vector3 (0.0f, 0.1f, 0.0f), DistanceX * Time.deltaTime * 8f);
			
			movePosition = beginPosition;
		}
		else
		{
			//oneTouchMove = false;
			moveDist = 0.0f;
		}
	}
}