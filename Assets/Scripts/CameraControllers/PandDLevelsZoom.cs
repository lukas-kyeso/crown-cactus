using UnityEngine;
using System.Collections;

public class PandDLevelsZoom : MonoBehaviour
{

    public GameObject parametersButton;

    public GameObject farLeft, farRight;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.pAndDLevelsLocation && parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.pAndDWidthLocation) return;

        Camera.main.transform.position = this.transform.position;

        Vector3 screenLeftX = Camera.main.WorldToScreenPoint(farLeft.transform.position);
        Vector3 screenRightX = Camera.main.WorldToScreenPoint(farRight.transform.position);

        if (screenLeftX.x > 200 && screenRightX.x < Screen.width - 200)
        {
            Camera.main.transform.position += transform.forward * 1;
            this.transform.position = Camera.main.transform.position;
        }

        else if (screenLeftX.x < 100 || screenRightX.z > Screen.width + 100)
        {
            Camera.main.transform.position += -transform.forward * 1;
            this.transform.position = Camera.main.transform.position;
        }
    }
}