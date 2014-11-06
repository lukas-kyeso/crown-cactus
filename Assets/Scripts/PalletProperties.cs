using UnityEngine;
using System.Collections;

public class PalletProperties : MonoBehaviour {
	private int colNum = 0;
	private ColumnSide side = ColumnSide.Unassigned;
	private float xPos;
	private Vector3 targetScale = Vector3.zero;
	private float fadeVal = 0.0f;
	private float timeLastChangedColor = 0.0f;
	private float timeBetweenColorChanges = 0.1f;
	private float colorInc = 0.1f;

	private ParametersButton parametersButton;

	
	public int ColNum {
		get {
			return colNum;
		}
		set {
			colNum = value;
		}
	}
	
	public ColumnSide Side {
		get {
			return side;
		}
		set {
			side = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.zero;
		parametersButton = GameObject.Find ("ParametersButton").GetComponent<ParametersButton> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 6);
        }
        else transform.localScale = targetScale;
		
//		foreach (Transform t in transform) {
//			//change color
//			if(fadeVal < 1 && Time.time > timeLastChangedColor + timeBetweenColorChanges){
//				fadeVal += colorInc;
//				
//				//Color c = t.gameObject.renderer.material.color;
//				//c.a = fadeVal;
//				//t.gameObject.renderer.material.color = c;
//				timeLastChangedColor = Time.time;
//			}
//		}	
	
		transform.FindChild("polySurface206").transform.localScale = new Vector3 (1f, parametersButton.VerticalBeamSpacing / 940f, parametersButton.BeamLength / parametersButton.PalletsBetweenFrames / 1100f);
	}
	
	public float XPos {
		get { return xPos; }
		set { xPos = value; }
	}
	
	public Vector3 TargetScale {
		get { return targetScale; }
		set { targetScale = value; }
	}
}
