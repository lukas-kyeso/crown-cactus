using UnityEngine;
using System.Collections;

public class LevelsZoom : MonoBehaviour
{
    public GameObject top;
    public GameObject bottom;
    public GameObject parametersButton;



    private bool hasBeenMovedFromInitial = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // should we even be on aisle loc? if not then get out
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.levelsLocation) return;

        Camera.main.transform.rotation = this.transform.rotation;
        Camera.main.transform.Rotate(new Vector3(0, 90, 0)); // problem?

        if (!hasBeenMovedFromInitial)
        {

            Camera.main.transform.position = this.transform.position;
            hasBeenMovedFromInitial = true;

        }
        Vector3 topVectorScreen = Camera.main.WorldToScreenPoint(top.transform.position);
        Vector3 bottomVectorScreen = Camera.main.WorldToScreenPoint(bottom.transform.position);

     
        Camera.main.transform.position = this.transform.position;

    
        // zoom out
        if (topVectorScreen.y > Screen.height - 100)
        {
            Camera.main.transform.Translate(new Vector3(0, 0, -1));
            this.transform.position = Camera.main.transform.position;
        }
        // zoom in
       // else if (bottomVectorScreen.y > Screen.height / 4 )
            
        else if (topVectorScreen.y < Screen.height/5)
        {
            Camera.main.transform.Translate(new Vector3(0, 0, 1));
            this.transform.position = Camera.main.transform.position;
        }

       
    }
}
