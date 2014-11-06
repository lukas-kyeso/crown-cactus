using UnityEngine;
using System.Collections;

public class BottomBeamHeightFixedDistance : MonoBehaviour
{
    // public GameObject VerticalBeamSpacing_Length_PalletsPerBeamTarget;
    public GameObject factoryBuilder, parametersButton;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // should we even be on aisle loc? if not then get out
        if (parametersButton.GetComponent<ParametersButton>().currentCameraLocation != Location.bottomBeamHeightLocation) return;


        Camera.main.transform.position = this.transform.position;
       // Camera.main.transform.localEulerAngles = new Vector3(0, 110, 0);
        float distance = 10;
        float columnX = factoryBuilder.GetComponent<FactoryBuilder>().getFirstColumnX();
        float middlePalletZ = factoryBuilder.GetComponent<FactoryBuilder>().getMiddlePalletOfLastColumnZ() + 5;
        float middlePalletY = factoryBuilder.GetComponent<FactoryBuilder>().getMiddlePalletOfLastColumnY();
        Vector3 closestPallet = factoryBuilder.GetComponent<FactoryBuilder>().getFurthestPalXVector();
        this.transform.position = new Vector3(columnX - distance, middlePalletY, middlePalletZ);
    }
}