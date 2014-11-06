using UnityEngine;
using System.Collections;

public class PalletPlus : MonoBehaviour
{


    public Vector2 beginPosition = Vector2.zero,
            movePosition = Vector2.zero;
    public float movedDist = 0.0f,
            DistanceX = 0,
            DistanceY = 0;
    private Vector3 screenPoint,
            offset;
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
     //   Camera.main.GetComponent<MoveCamera>().setDraggingPallet(true);
         if (gameObject.transform.name == "PlusCloseEnd")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsEndOfSelectedCol(ColumnEnd.Low);
            else
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsEndOfAllCols(ColumnEnd.Low);

        }
        if (gameObject.transform.name == "PlusFarEnd")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsEndOfSelectedCol(ColumnEnd.High);
            else
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsEndOfAllCols(ColumnEnd.High);
        }


        if (gameObject.transform.name == "PlusTop")
        {
            if (!allSelected)
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsTopOfSelectedCol();
            else
                factorybuilder.GetComponent<FactoryBuilder>().addPalletsTopAllCols();
        }
        if (gameObject.transform.name == "PlusRight")
        {
            factorybuilder.GetComponent<FactoryBuilder>().increaseNumCols(ColumnEnd.High);
         }
        if (gameObject.transform.name == "PlusLeft")
        {
            factorybuilder.GetComponent<FactoryBuilder>().increaseNumCols(ColumnEnd.Low);
        }
    
    }

    void OnMouseUp()
    {
   //     Camera.main.GetComponent<MoveCamera>().setDraggingPallet(false);
    }
 
    void OnMouseDrag()
    {

    }
}
