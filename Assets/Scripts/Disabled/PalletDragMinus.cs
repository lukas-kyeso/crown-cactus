using UnityEngine;
using System.Collections;

public class PalletDragMinus : MonoBehaviour
{


    public Vector2 beginPosition = Vector2.zero,
            movePosition = Vector2.zero;
    public float movedDist = 0.0f,
            DistanceX = 0,
            DistanceY = 0;
    private Vector3 screenPoint,
            scanPos = Vector3.zero;
    private GameObject factorybuilder;
    public bool dragging = false;
    private Camera mainCam;
    private Vector3 pressedPos;
    private float timeOfLastEvent; 
    // Use this for initialization
    void Start()
    {
        timeOfLastEvent = Time.time;
        mainCam = GameObject.Find("Main Camera").camera;
 
        factorybuilder = GameObject.Find("FactoryBuilderObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
      //  dragging = true;
        mainCam.GetComponent<MoveCamera>().setDraggingPallet(true);
   
        screenPoint = mainCam.WorldToScreenPoint(scanPos);
        pressedPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        timeOfLastEvent = Time.time;
    }

    void OnMouseUp()
    {
        // needed so we know when we can deselect everything
        factorybuilder.GetComponent<FactoryBuilder>().dragging = false;
     //   dragging = false;
        mainCam.GetComponent<MoveCamera>().setDraggingPallet(false);
    }
    // BE WARY YE WHO ENTER - lukas
    void OnMouseDrag()
    {
        if (timeOfLastEvent + .05 > Time.time)
        {
            return;
        }
        dragging = true;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        if (Vector3.Distance(curScreenPoint, pressedPos) < 30)
        {
            return;
        }
       






        bool allSelected = factorybuilder.GetComponent<FactoryBuilder>().getAllColsSelected();

      
        

        // front 
        if (gameObject.transform.parent.name == "DragCloseEnd")
        {
          
            Vector3 closestVector = factorybuilder.GetComponent<FactoryBuilder>().getClosestPalZVector();

            //drag away

            if (mainCam.transform.localEulerAngles.y > 230 && mainCam.transform.localEulerAngles.y < 320)
            {
              
                //drag inwards
                if (mainCam.WorldToScreenPoint(closestVector).x + 1 < curScreenPoint.x)
                {
                    
                    if (!allSelected){
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.Low);
                        timeOfLastEvent = Time.time;
                    }

                    else
                    {
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.Low);
                        timeOfLastEvent = Time.time;
                    }
                       

                }
            }
            else if (mainCam.transform.localEulerAngles.y > 0 && mainCam.transform.localEulerAngles.y < 170)
            {
              
                //drag inwards
                if (mainCam.WorldToScreenPoint(closestVector).x - 1 > curScreenPoint.x)
                {

                    if (!allSelected){
                        timeOfLastEvent = Time.time;
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.Low);
                    }
                       
                    else
                    {
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.Low);
                        timeOfLastEvent = Time.time;
                    }
                        
                }
            }

        }
        //back
        if (gameObject.transform.parent.name == "DragFarEnd")
        {
         
            Vector3 furthestVector = factorybuilder.GetComponent<FactoryBuilder>().getFurthestPalZVector();
            //drag away
            if (mainCam.transform.localEulerAngles.y > 180 && mainCam.transform.localEulerAngles.y < 360)
            {
               
                //drag inwards
                if (mainCam.WorldToScreenPoint(furthestVector).x - 50 > curScreenPoint.x)
                {
                    if (!allSelected){
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.High);
                         timeOfLastEvent = Time.time;
                    }
                        
                    else
                    {
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.High);
                        timeOfLastEvent = Time.time;
                    }
                        
                }

            }
            else
            {


               
                //drag inwards
                if (mainCam.WorldToScreenPoint(furthestVector).x + 50 < curScreenPoint.x)
                {
                    if (!allSelected){
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfSelectedCol(ColumnEnd.High);
                        timeOfLastEvent = Time.time;
                    }

                    else
                    {
                        factorybuilder.GetComponent<FactoryBuilder>().deletePalletsEndOfAllCols(ColumnEnd.High);
                        timeOfLastEvent = Time.time;
                    }
                        
                }
            }
        }


        if (gameObject.transform.parent.name == "DragTop")
        {

            Vector3 highestVector = factorybuilder.GetComponent<FactoryBuilder>().getAnyHighestPlatVector();
           
            //drag inwards
            if (mainCam.WorldToScreenPoint(highestVector).y + 50 > curScreenPoint.y)
            {

                if (!allSelected){
                    factorybuilder.GetComponent<FactoryBuilder>().deletePalletsTopOfSelectedCol();
                    timeOfLastEvent = Time.time;
                }

                else
                {
                    factorybuilder.GetComponent<FactoryBuilder>().deletePalletsTopAllCols();
                    timeOfLastEvent = Time.time;
                }
                    
            }
        }
        if (gameObject.transform.parent.name == "DragRight")
        {
            Vector3 furthestVector = factorybuilder.GetComponent<FactoryBuilder>().getFurthestPalXVector();
            if (mainCam.transform.localEulerAngles.y < 60 || mainCam.transform.localEulerAngles.y > 240)
            {

                //drag away
                if (mainCam.WorldToScreenPoint(furthestVector).x - 50 > curScreenPoint.x)
                {
                    factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.Low);
                    timeOfLastEvent = Time.time;
                    
                }


            }
            else if (mainCam.transform.localEulerAngles.y > 120 && mainCam.transform.localEulerAngles.y < 240)
            {

                //drag away
                if (mainCam.WorldToScreenPoint(furthestVector).x + 50 < curScreenPoint.x)
                {
                    factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.Low);
                    timeOfLastEvent = Time.time;
                }


            }

    
               
        }
        if (gameObject.transform.parent.name == "DragLeft")
        {
           
         
            Vector3 closestVector = factorybuilder.GetComponent<FactoryBuilder>().getClosestPalXVector();
            if (mainCam.transform.localEulerAngles.y > 300 || mainCam.transform.localEulerAngles.y < 60)
            {
       
                //drag away
                if (mainCam.WorldToScreenPoint(closestVector).x + 50 < curScreenPoint.x)
                {
                    factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.Low);
                    timeOfLastEvent = Time.time;
                }


            }
            else if (mainCam.transform.localEulerAngles.y > 75 && mainCam.transform.localEulerAngles.y < 240)
            {
              
                //drag away
                if (mainCam.WorldToScreenPoint(closestVector).x - 50 > curScreenPoint.x)
                {
                    factorybuilder.GetComponent<FactoryBuilder>().decreaseNumCols(ColumnEnd.Low);
                    timeOfLastEvent = Time.time;
                }


            }

        }
    }
}
