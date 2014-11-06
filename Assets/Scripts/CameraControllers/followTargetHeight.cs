using UnityEngine;
using System.Collections;

public class followTargetHeight : MonoBehaviour {

	public GameObject parameters;
	public Transform target;

	private float currentNoOfColumns, newNoOfColumns, temp, diff;

	private bool move = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (this.transform.name.Equals ("NumberofLevels")) {

		} else if (this.transform.name.Equals ("NumberofBaysDownAisleCamera")) {

		} else if (this.transform.name.Equals ("NumberofAisles")) {

		}

		float diff = target.transform.position.y - this.transform.position.y;
		if(move == false){
			if(Time.time > 1){
				move = true;
			}
		}
		if(move){
			if (diff < -1) {
				if (this.transform.position.y > target.transform.position.y) {
					this.transform.Translate (new Vector3 (0f, -1f, 0f));
				}
			}

			if (diff > 1) {
				if (this.transform.position.y < target.transform.position.y) {
					this.transform.Translate (new Vector3 (0f, 1f, 0f));
				}
			}
		}

	}
}
