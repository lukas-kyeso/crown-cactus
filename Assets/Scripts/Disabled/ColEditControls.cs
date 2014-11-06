using UnityEngine;
using System.Collections;

public class ColEditControls : MonoBehaviour {
    private GameObject factoryBuilder;
    private GameObject parameters;
    private GameObject topEditFace;
    private GameObject leftEditFace;
    private GameObject rightEditFace;
    private GameObject farEndEditFace;
    private GameObject closeEndEditFace;
    private GameObject dragTopEditFace;
    private GameObject dragLeftEditFace;
    private GameObject dragRightEditFace;
    private GameObject dragFarEndEditFace;
    private GameObject dragCloseEndEditFace;
    private GameObject plusTopEditFace;
    private GameObject plusLeftEditFace;
    private GameObject plusRightEditFace;
    private GameObject plusFarEndEditFace;
    private GameObject plusCloseEndEditFace;
    private GameObject minusTopEditFace;
    private GameObject minusLeftEditFace;
    private GameObject minusRightEditFace;
    private GameObject minusFarEndEditFace;
    private GameObject minusCloseEndEditFace;

    private int curCol = -1;
    private bool isEditingAllCols = false;

    // Use this for initialization
    void Start() {
        topEditFace = (GameObject)Instantiate(Resources.Load("SelectionPlane"));
        leftEditFace = (GameObject)Instantiate(Resources.Load("SelectionPlane"));
        rightEditFace = (GameObject)Instantiate(Resources.Load("SelectionPlane"));
        farEndEditFace = (GameObject)Instantiate(Resources.Load("SelectionPlane"));
        closeEndEditFace = (GameObject)Instantiate(Resources.Load("SelectionPlane"));
        dragTopEditFace = (GameObject)Instantiate(Resources.Load("Drag"));
        dragLeftEditFace = (GameObject)Instantiate(Resources.Load("Drag"));
        dragRightEditFace = (GameObject)Instantiate(Resources.Load("Drag"));
        dragFarEndEditFace = (GameObject)Instantiate(Resources.Load("Drag"));
        dragCloseEndEditFace = (GameObject)Instantiate(Resources.Load("Drag"));
        plusTopEditFace = (GameObject)Instantiate(Resources.Load("Plus"));
        plusLeftEditFace = (GameObject)Instantiate(Resources.Load("Plus"));
        plusRightEditFace = (GameObject)Instantiate(Resources.Load("Plus"));
        plusFarEndEditFace = (GameObject)Instantiate(Resources.Load("Plus"));
        plusCloseEndEditFace = (GameObject)Instantiate(Resources.Load("Plus"));
        minusTopEditFace = (GameObject)Instantiate(Resources.Load("Minus"));
        minusLeftEditFace = (GameObject)Instantiate(Resources.Load("Minus"));
        minusRightEditFace = (GameObject)Instantiate(Resources.Load("Minus"));
        minusFarEndEditFace = (GameObject)Instantiate(Resources.Load("Minus"));
        minusCloseEndEditFace = (GameObject)Instantiate(Resources.Load("Minus"));
        topEditFace.name = "colEditTop";
        leftEditFace.name = "colEditLeft";
        rightEditFace.name = "colEditRight";
        farEndEditFace.name = "colEditFar";
        closeEndEditFace.name = "colEditClose";
        dragTopEditFace.name = "DragTop";   // DO NOT CHANGE ANY OF THESE NAMES
        dragLeftEditFace.name = "DragLeft";
        dragRightEditFace.name = "DragRight";
        dragFarEndEditFace.name = "DragFarEnd";
        dragCloseEndEditFace.name = "DragCloseEnd";
        plusTopEditFace.name = "PlusTop";
        plusLeftEditFace.name = "PlusLeft";
        plusRightEditFace.name = "PlusRight";
        plusFarEndEditFace.name = "PlusFarEnd";
        plusCloseEndEditFace.name = "PlusCloseEnd";
        minusTopEditFace.name = "MinusTop";
        minusLeftEditFace.name = "MinusLeft";
        minusRightEditFace.name = "MinusRight";
        minusFarEndEditFace.name = "MinusFarEnd";
        minusCloseEndEditFace.name = "MinusCloseEnd";
        topEditFace.SetActive(false);
        leftEditFace.SetActive(false);
        rightEditFace.SetActive(false);
        farEndEditFace.SetActive(false);
        closeEndEditFace.SetActive(false);
        dragTopEditFace.SetActive(false);
        dragLeftEditFace.SetActive(false);
        dragRightEditFace.SetActive(false);
        dragFarEndEditFace.SetActive(false);
        dragCloseEndEditFace.SetActive(false);
        plusTopEditFace.SetActive(false);
        plusLeftEditFace.SetActive(false);
        plusRightEditFace.SetActive(false);
        plusFarEndEditFace.SetActive(false);
        plusCloseEndEditFace.SetActive(false);
        minusTopEditFace.SetActive(false);
        minusLeftEditFace.SetActive(false);
        minusRightEditFace.SetActive(false);
        minusFarEndEditFace.SetActive(false);
        minusCloseEndEditFace.SetActive(false);
        topEditFace.transform.parent = gameObject.transform;
        leftEditFace.transform.parent = gameObject.transform;
        rightEditFace.transform.parent = gameObject.transform;
        farEndEditFace.transform.parent = gameObject.transform;
        closeEndEditFace.transform.parent = gameObject.transform;
        dragTopEditFace.transform.parent = gameObject.transform;
        dragLeftEditFace.transform.parent = gameObject.transform;
        dragRightEditFace.transform.parent = gameObject.transform;
        dragFarEndEditFace.transform.parent = gameObject.transform;
        dragCloseEndEditFace.transform.parent = gameObject.transform;
        plusTopEditFace.transform.parent = gameObject.transform;
        plusLeftEditFace.transform.parent = gameObject.transform;
        plusRightEditFace.transform.parent = gameObject.transform;
        plusFarEndEditFace.transform.parent = gameObject.transform;
        plusCloseEndEditFace.transform.parent = gameObject.transform;
        minusTopEditFace.transform.parent = gameObject.transform;
        minusLeftEditFace.transform.parent = gameObject.transform;
        minusRightEditFace.transform.parent = gameObject.transform;
        minusFarEndEditFace.transform.parent = gameObject.transform;
        minusCloseEndEditFace.transform.parent = gameObject.transform;
       

        factoryBuilder = GameObject.Find("FactoryBuilderObj");
        parameters = GameObject.Find("ParametersButton");
    }

    // Update is called once per frame
    void Update() {
        if (curCol >= 0)
            refresh();  //maybe put this on a timer?
    }


    public void updateTransformOnCol(int i) {       
        ///* 
        float numColumns = parameters.GetComponent<ParametersButton>().NumAisles;
        float colXpos = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos(i);
        float colHeight = factoryBuilder.GetComponent<FactoryBuilder>().getColHeight(i);
        float colLength = factoryBuilder.GetComponent<FactoryBuilder>().getColLength(i);
        float farPalletZ = factoryBuilder.GetComponent<FactoryBuilder>().getColFarZ(i);
        float closePalletZ = factoryBuilder.GetComponent<FactoryBuilder>().getColCloseZ(i);
        float palletWidth = parameters.GetComponent<ParametersButton>().PalletISO.getWidth();
        float palletDepth = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        float palletHeight = parameters.GetComponent<ParametersButton>().PalletHeight;
        float adjColSpacing = parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
        int numPalletsHigh = factoryBuilder.GetComponent<FactoryBuilder>().getNumPalletsHighOnCol(i);

        float avgColXpos = factoryBuilder.GetComponent<FactoryBuilder>().getAvgXpos();
        float highestY = factoryBuilder.GetComponent<FactoryBuilder>().getHighestY() + palletHeight / 2;
        float avgColZpos = factoryBuilder.GetComponent<FactoryBuilder>().getAvgZpos();
        float lowestX = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos(0);
        float highestX = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos((int)numColumns - 1);
        float lowestZ = factoryBuilder.GetComponent<FactoryBuilder>().getLowestPalZpos() - palletDepth / 2;
        float highestZ = factoryBuilder.GetComponent<FactoryBuilder>().getHighestPalZpos() + palletDepth / 2;

        //position
        if (isEditingAllCols) {
            topEditFace.transform.position = new Vector3(avgColXpos, highestY, avgColZpos);
            leftEditFace.transform.position = new Vector3(lowestX, highestY / 2, avgColZpos);
            rightEditFace.transform.position = new Vector3(highestX, highestY / 2, avgColZpos);
            farEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2, highestZ);
            closeEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2, lowestZ);

            dragTopEditFace.transform.position = new Vector3(highestX, highestY, avgColZpos);
            dragLeftEditFace.transform.position = new Vector3(lowestX, highestY / 2, lowestZ);
            dragRightEditFace.transform.position = new Vector3(highestX, highestY / 2, highestZ);
            dragFarEndEditFace.transform.position = new Vector3(lowestX, highestY / 2, highestZ);
            dragCloseEndEditFace.transform.position = new Vector3(highestX, highestY / 2, lowestZ);

            plusTopEditFace.transform.position = new Vector3(avgColXpos, highestY, avgColZpos + 1.2f);
            plusLeftEditFace.transform.position = new Vector3(lowestX, highestY / 2 + 1.2f, avgColZpos);
            plusRightEditFace.transform.position = new Vector3(highestX, highestY / 2 + 1.2f, avgColZpos);
            plusFarEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2 + 1.2f, highestZ);
            plusCloseEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2 + 1.2f, lowestZ);

            minusTopEditFace.transform.position = new Vector3(avgColXpos, highestY, avgColZpos - 1.2f);
            minusLeftEditFace.transform.position = new Vector3(lowestX, highestY / 2 - 1.2f, avgColZpos);
            minusRightEditFace.transform.position = new Vector3(highestX, highestY / 2 - 1.2f, avgColZpos);
            minusFarEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2 - 1.2f, highestZ);
            minusCloseEndEditFace.transform.position = new Vector3(avgColXpos, highestY / 2 - 1.2f, lowestZ);
        }
        else {
            float endFacePosXAdj = 0;
            if (i == 0) { //only cover left side
                colXpos = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos(i) + (palletDepth/2 + adjColSpacing / 2);
                endFacePosXAdj = (palletDepth/2 + adjColSpacing / 2);
            }
            else if (i == numColumns - 1){  //only cover right side
                colXpos = factoryBuilder.GetComponent<FactoryBuilder>().getColXPos(i) - (palletDepth/2 + adjColSpacing / 2);
                endFacePosXAdj = (palletDepth/2 + adjColSpacing / 2);    
            }
            else 
                endFacePosXAdj = (palletDepth + adjColSpacing / 2);
            
            topEditFace.transform.position = new Vector3(colXpos, colHeight, farPalletZ - colLength / 2);
            leftEditFace.transform.position = new Vector3(colXpos - endFacePosXAdj, colHeight / 2, farPalletZ - colLength / 2);
          
            rightEditFace.transform.position = new Vector3(colXpos + endFacePosXAdj, colHeight / 2, farPalletZ - colLength / 2);
            farEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2, farPalletZ + (palletWidth / 2));
            closeEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2, closePalletZ - (palletWidth / 2));

            dragTopEditFace.transform.position = new Vector3(colXpos + 1, colHeight, farPalletZ - colLength / 2);
            dragLeftEditFace.transform.position = leftEditFace.transform.position;
            dragRightEditFace.transform.position = rightEditFace.transform.position;
            dragFarEndEditFace.transform.position = new Vector3(colXpos - 1f, colHeight / 2, farPalletZ + (palletWidth / 2));
            dragCloseEndEditFace.transform.position = new Vector3(colXpos + 1f, colHeight / 2, closePalletZ - (palletWidth / 2));

            plusTopEditFace.transform.position = new Vector3(colXpos, colHeight, avgColZpos + 1.2f);
            plusFarEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2 + 1.2f, farPalletZ + (palletWidth / 2));
            plusCloseEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2 + 1.2f, closePalletZ - (palletWidth / 2));

            minusTopEditFace.transform.position = new Vector3(colXpos, colHeight, avgColZpos - 1.2f);
            minusFarEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2 - 1.2f, farPalletZ + (palletWidth / 2));
            minusCloseEndEditFace.transform.position = new Vector3(colXpos, colHeight / 2 - 1.2f, closePalletZ - (palletWidth / 2));
        
        }
        


        //scale
        Vector3 shrink = new Vector3(0.00f, 0.00f, 0.00f);  //shrinks each pane by a uniform amount
        if (isEditingAllCols) {
            topEditFace.transform.localScale = new Vector3((highestX - lowestX) / 10, 1, (highestZ - lowestZ) / 10) - shrink;
            leftEditFace.transform.localScale = new Vector3(highestY / 10, 1, (highestZ - lowestZ) / 10) - shrink;
            rightEditFace.transform.localScale = new Vector3(highestY / 10, 1, (highestZ - lowestZ) / 10) - shrink;
            farEndEditFace.transform.localScale = new Vector3((highestX - lowestX) / 10, 1, highestY / 10) - shrink;
            closeEndEditFace.transform.localScale = new Vector3((highestX - lowestX) / 10, 1, highestY / 10) - shrink;

            dragLeftEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);
            dragRightEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);

            plusLeftEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            plusRightEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            minusLeftEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            minusRightEditFace.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
 
        }
        else {
            float endFaceScaleX = 0;
            if (i == 0) 
                endFaceScaleX = (palletWidth + adjColSpacing) * 0.1f;            
            else if (i == numColumns - 1)
                endFaceScaleX = (palletWidth + adjColSpacing) * 0.1f;            
            else 
                endFaceScaleX = (palletWidth * 2 + adjColSpacing) * 0.1f;

            topEditFace.transform.localScale = new Vector3(endFaceScaleX, 1, colLength * 0.1f + 0.1f) - shrink;
            farEndEditFace.transform.localScale = new Vector3(endFaceScaleX, 1, numPalletsHigh * palletHeight * 0.1f) - shrink;
            closeEndEditFace.transform.localScale = new Vector3(endFaceScaleX, 1, numPalletsHigh * palletHeight * 0.1f) - shrink;
            leftEditFace.transform.localScale = new Vector3((palletHeight * numPalletsHigh) * 0.1f, 1, colLength * 0.1f + 0.1f) - shrink;
            rightEditFace.transform.localScale = new Vector3((palletHeight * numPalletsHigh) * 0.1f, 1, colLength * 0.1f + 0.1f) - shrink;

            dragLeftEditFace.transform.localScale = new Vector3(0, 0, 0);
            dragRightEditFace.transform.localScale = new Vector3(0, 0, 0);

            plusLeftEditFace.transform.localScale = new Vector3(0, 0, 0);
            plusRightEditFace.transform.localScale = new Vector3(0, 0, 0);

            minusLeftEditFace.transform.localScale = new Vector3(0, 0, 0);
            minusRightEditFace.transform.localScale = new Vector3(0, 0, 0);
     
        }


        //rotation
        Quaternion leftq = leftEditFace.transform.rotation;
        Quaternion rightq = rightEditFace.transform.rotation;
        Quaternion farq = farEndEditFace.transform.rotation;
        Quaternion closeq = closeEndEditFace.transform.rotation;
        leftq.eulerAngles = new Vector3(0, 0, 90);
        rightq.eulerAngles = new Vector3(0, 0, 270);
        farq.eulerAngles = new Vector3(90, 0, 0);
        closeq.eulerAngles = new Vector3(270, 0, 0);
        leftEditFace.transform.rotation = leftq;
        rightEditFace.transform.rotation = rightq;
        closeEndEditFace.transform.rotation = closeq;
        farEndEditFace.transform.rotation = farq;
        dragTopEditFace.transform.rotation = topEditFace.transform.rotation;
        dragLeftEditFace.transform.localEulerAngles = new Vector3(270, 90, 0);
        dragRightEditFace.transform.localEulerAngles = new Vector3(270, 270, 0);
        dragFarEndEditFace.transform.localEulerAngles = new Vector3(270, 180, 0);
        dragCloseEndEditFace.transform.rotation = closeEndEditFace.transform.rotation;

        plusTopEditFace.transform.rotation = topEditFace.transform.rotation;
        plusLeftEditFace.transform.localEulerAngles = new Vector3(270, 90, 0);
        plusRightEditFace.transform.localEulerAngles = new Vector3(270, 270, 0);
        plusFarEndEditFace.transform.localEulerAngles = new Vector3(270, 180, 0);
        plusCloseEndEditFace.transform.rotation = closeEndEditFace.transform.rotation;

        minusTopEditFace.transform.rotation = topEditFace.transform.rotation;
        minusLeftEditFace.transform.localEulerAngles = new Vector3(270, 90, 0);
        minusRightEditFace.transform.localEulerAngles = new Vector3(270, 270, 0);
        minusFarEndEditFace.transform.localEulerAngles = new Vector3(270, 180, 0);
        minusCloseEndEditFace.transform.rotation = closeEndEditFace.transform.rotation;
        curCol = i;
        //*/
    }

    public void refresh() {
        updateTransformOnCol(curCol);
    }

    public void show(bool b) {
        topEditFace.SetActive(b);
        farEndEditFace.SetActive(b);
        closeEndEditFace.SetActive(b);
        dragTopEditFace.SetActive(b);
        dragFarEndEditFace.SetActive(b);
        dragCloseEndEditFace.SetActive(b);
        leftEditFace.SetActive(b);
        rightEditFace.SetActive(b);
        dragLeftEditFace.SetActive(b);
        dragRightEditFace.SetActive(b);

        plusTopEditFace.SetActive(b);
        plusLeftEditFace.SetActive(b);
        plusRightEditFace.SetActive(b);
        plusFarEndEditFace.SetActive(b);
        plusCloseEndEditFace.SetActive(b);

        minusTopEditFace.SetActive(b);
        minusLeftEditFace.SetActive(b);
        minusRightEditFace.SetActive(b);
        minusFarEndEditFace.SetActive(b);
        minusCloseEndEditFace.SetActive(b);
    }

    public void setCol(int i) {
        curCol = i;
    }

    public void setIsAllEditor(bool b) {
        isEditingAllCols = b;
    }
}

public enum UIFaceType {
    Top, Side, Unassigned
}

