using UnityEngine;
using System.Collections;

public class LeftRight : TouchLogicV2
{
	public float zoomSpeed = 5.0f;
	private float moveSpeed = 20.0f;
	
	public Vector2 beginPosition = Vector2.zero,
	movePosition = Vector2.zero;
	
	public float movedDist = 0.0f,
	DistanceX = 0,
	DistanceY = 0;
	
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
		//you can do some logic before you check for the touches on screen	
		if(!disableTouches)//(optional) dynamically change whether or not to check for touches
			base.Update();//must have this somewhere or TouchLogicV2's Update will be totally overwritten by this class's Update
	}
	
	public override void OnTouchMoved()
	{
		if (Input.touches.Length ==1)
			Move();
	}
	public override void OnTouchStayed()
	{

	}
	public override void OnTouchBegan(){
//		beginPosition = Input.mousePosition;
	}
	
	//find distance between the 2 touches 1 frame before & current frame
	//if the delta distance increased, zoom in, if delta distance decreased, zoom out
	void Move()
	{
		//Caches touch positions for each finger
		switch(TouchLogicV2.currTouch)
		{
		case 0://first touch
			currTouch1 = Input.GetTouch(0).position;
			lastTouch1 = currTouch1 - Input.GetTouch(0).deltaPosition;
			break;
		}
		//finds the distance between your moved touches
		//we dont want to find the distance between 1 finger and nothing
		if(TouchLogicV2.currTouch < 1)
		{

			beginPosition = Input.mousePosition;
//			movePosition = Input.mousePosition;
			
			DistanceX = (beginPosition.x - movePosition.x);
			DistanceY = (beginPosition.y - movePosition.y);
			
			movedDist = moveDist;
			
			//Calculate the move factor
			moveFactor = Mathf.Clamp(moveDist,-30.0f,30.0f);
			
			moveDist = Vector2.Distance(currTouch1, lastTouch1);
			
			if (DistanceX > 0){
				Camera.main.transform.Translate((Vector3.left * moveFactor * moveSpeed * Time.deltaTime*1));
				Debug.Log("LEFT");
			}
			if (DistanceX < 0){
				Camera.main.transform.Translate((Vector3.right * moveFactor * moveSpeed * Time.deltaTime*1));
				Debug.Log("RIGHT");
			}
			movePosition = beginPosition;
		}
		else
		{
			moveDist = 0.0f;
		}
	}
}