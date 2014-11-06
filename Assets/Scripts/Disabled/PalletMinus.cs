using UnityEngine;
using System.Collections;

public class PalletMinus : MonoBehaviour
{


    public Vector2 beginPosition = Vector2.zero,
            movePosition = Vector2.zero;
    public float movedDist = 0.0f,
            DistanceX = 0,
            DistanceY = 0;
    private Vector3 screenPoint, offset;
    private GameObject factorybuilder;
    private float distanceToPlus, distanceToMinus;
    private float oldDistanceToPlus, oldDistanceToMinus;

    // Use this for initialization
    void Start()
    {

        factorybuilder = GameObject.Find("FactoryBuilderObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        bool allSelected = factorybuilder.GetComponent<FactoryBuilder>().getAllColsSelected();
   //     Camera.main.GetComponent<MoveCamera>().setDraggingPallet(true);
         if (gameObject.transform.name == "MinusCloseEnd")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.Low);
            else
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.Low);

        }
        if (gameObject.transform.name == "MinusFarEnd")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.High);
            else
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.High);
        }


        if (gameObject.transform.name == "MinusTop")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsTopOfSelectedCol();
            else
                factorybuilder.GetComponent<FactoryBuilder>().deletePalletsTopAllCols();
        }
        if (gameObject.transform.name == "MinusRight")
        {
        
            factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.High);
         }
        if (gameObject.transform.name == "MinusLeft")
        {
        
            factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.Low);
        }
    
    }

    void OnMouseUp()
    {
    //  Camera.main.GetComponent<MoveCamera>().setDraggingPallet(false);
    }
 
    void OnMouseDrag()
    {

    }
}
