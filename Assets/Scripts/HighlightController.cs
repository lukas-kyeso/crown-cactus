using UnityEngine;
using System.Collections;

public enum highlightSection { NumberOfLevels, NumberOfBaysDownAisle, NumberOfAisles, VerticalBeamSpacing, BeamLength, NumberOfPalletsPerBeam, BottomBeamHeight, InboundOutbound, PickUp, DropOff, NumberOfPAndDLevels, NumberOfPAndDBeamsAcross, none }

public class HighlightController {

    FactoryBuilder factoryBuilder;
    ParametersButton parametersButton;
    int NumPalletsHigh;
    int NumPalletsWide;


    // highlight panels
    private GameObject[] highlightPlanes;
    int highlightPlanesCount = 0;

    // Use this for initialization
    void Start() {

    }

    public HighlightController() {
        factoryBuilder = GameObject.Find("FactoryBuilderObj").GetComponent<FactoryBuilder>();
        parametersButton = GameObject.Find("ParametersButton").GetComponent<ParametersButton>();
    }
    // Update is called once per frame
    void Update() {

    }

    public void SetHighlightArea(highlightSection sectionToHighlight) {
        NumPalletsHigh = factoryBuilder.columns[0].NumPalletsHigh;
        NumPalletsWide = factoryBuilder.columns[0].NumPalletsWide;
        float palletWidth = factoryBuilder.PalletWidth;


        GameObject.Find("BottomLine").GetComponent<MeshRenderer>().material = (Material)Resources.Load("FloorOutlineBlackMaterial");
        factoryBuilder.toggleAisleNumbers(sectionToHighlight == highlightSection.NumberOfAisles);

        // clear height and width outlines
        if (highlightPlanes != null) {
            for (int i = 0; i < highlightPlanes.Length; i++) {
                GameObject.Destroy(highlightPlanes[i]);
            }
            highlightPlanes = null;
        }

        // clear aisle outlines
        foreach (Column c in factoryBuilder.columns) {
            if (c == null)
                break;

            c.toggleAisleNumberAndCarpet(false);

        }

        if (sectionToHighlight == highlightSection.VerticalBeamSpacing) {
            highlightPlanes = new GameObject[NumPalletsHigh + 2];
            GameObject topPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/VerticalBeamSpacingHolder"));
            GameObject bottomPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/VerticalBeamSpacingHolder"));
            GameObject arrowPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/ArrowHighlightHolder"));

            // grab correct pallet
            GameObject middleOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, NumPalletsHigh / 2);
            float posOfDisabledFrameX = middleOfLastColumnPallet.transform.FindChild("frameFar").transform.position.z;

            // grab pallet above that one  - used to know where to place topPlane 
            //  GameObject middleOfLastColumnPalletPlusOneY = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, NumPalletsHigh / 2 );

            // for 3
            GameObject middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 + 1, NumPalletsHigh / 2);
            float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
            float posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;


            float worldYPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.y;
            float worldXPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.z;
            float worldZPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.x;


            float worldYPosOfMiddlePlusOneYPallet = worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f;

            // checks whether number of bays is even of odd, so we can offset highlighting to correct pos
            bool even = (parametersButton.NumPalletsWide / parametersButton.PalletsBetweenFrames) % 2 == 0 ? true : false;

            // used to know much to offset highlighting
            float palletsPerBay = parametersButton.PalletsBetweenFrames;


            topPlane.transform.parent = middleOfLastColumnPallet.transform;
            bottomPlane.transform.parent = middleOfLastColumnPallet.transform;
            arrowPlane.transform.parent = middleOfLastColumnPallet.transform;


            topPlane.transform.localScale = new Vector3(0.1f, .10f, parametersButton.BeamLength / 2000f);
            bottomPlane.transform.localScale = new Vector3(0.1f, .10f, parametersButton.BeamLength / 2000f);
            arrowPlane.transform.localScale = new Vector3(0.1f, parametersButton.VerticalBeamSpacing / 1000f, 1);

            if (palletsPerBay == 1) {
                topPlane.transform.position = new Vector3(0, worldYPosOfMiddlePlusOneYPallet, worldXPosOfMiddlePallet);
                bottomPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet, worldXPosOfMiddlePallet);
                arrowPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, worldXPosOfMiddlePallet);
            }
            else if (palletsPerBay == 2) {
                if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                    posOfDisabledFrameX = posOfDisabledFramePlusOneX;
                }

                topPlane.transform.position = new Vector3(0, worldYPosOfMiddlePlusOneYPallet, posOfDisabledFrameX);
                bottomPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet, posOfDisabledFrameX);
                arrowPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, posOfDisabledFrameX);
            }
            else if (palletsPerBay == 3) {
                if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                    xPosOfMiddleOfLastColumnPalletPlusOneX = worldXPosOfMiddlePallet;
                }
                topPlane.transform.position = new Vector3(0, worldYPosOfMiddlePlusOneYPallet, xPosOfMiddleOfLastColumnPalletPlusOneX);
                bottomPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet, xPosOfMiddleOfLastColumnPalletPlusOneX);
                arrowPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
            }
            else if (palletsPerBay == 4) {
                topPlane.transform.position = new Vector3(0, worldYPosOfMiddlePlusOneYPallet, posOfDisabledFramePlusOneX);
                bottomPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet, posOfDisabledFramePlusOneX);
                arrowPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, posOfDisabledFramePlusOneX);
            }

            highlightPlanes[0] = topPlane;
            highlightPlanes[1] = bottomPlane;
            highlightPlanes[2] = arrowPlane;

        }

        if (sectionToHighlight == highlightSection.BeamLength) {
            highlightPlanes = new GameObject[2];
            GameObject bottomPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/VerticalBeamSpacingHolder"));
            GameObject arrowPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/ArrowHighlightHolder"));

            // grab correct pallet - works for 2
            GameObject middleOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, NumPalletsHigh / 2);
            float middleOfLastColumnPalletY = middleOfLastColumnPallet.transform.position.y;
            float middleOfLastColumnPalletX = middleOfLastColumnPallet.transform.position.z;

            float posOfDisabledFrameX = middleOfLastColumnPallet.transform.FindChild("frameFar").transform.position.z;
            float worldZPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.x;
            // for 3
            GameObject middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 + 1, NumPalletsHigh / 2);
            float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
            float posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;



            // used to know much to offset highlighting
            float palletsPerBay = parametersButton.PalletsBetweenFrames;

            bottomPlane.transform.parent = middleOfLastColumnPallet.transform;
            arrowPlane.transform.parent = middleOfLastColumnPallet.transform;

            bottomPlane.transform.localScale = new Vector3(0, .1f, parametersButton.BeamLength / 2000f);
            arrowPlane.transform.localScale = new Vector3(0, parametersButton.BeamLength / 1000f, .5f);
            arrowPlane.transform.Rotate(90, 0, 0);
            if (palletsPerBay == 1) {
                bottomPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY, middleOfLastColumnPalletX);
                arrowPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY + .2f, middleOfLastColumnPalletX);
            }
            else if (palletsPerBay == 2) {
                if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                    posOfDisabledFrameX = posOfDisabledFramePlusOneX;
                }
                bottomPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY, posOfDisabledFrameX);
                arrowPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY + .2f, posOfDisabledFrameX);
            }
            else if (palletsPerBay == 3) {
                if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                    xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletX;
                }
                bottomPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY, xPosOfMiddleOfLastColumnPalletPlusOneX);
                arrowPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY + .2f, xPosOfMiddleOfLastColumnPalletPlusOneX);

            }
            else if (palletsPerBay == 4) {
                bottomPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY, posOfDisabledFramePlusOneX);
                arrowPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY + .2f, posOfDisabledFramePlusOneX);
            }

            highlightPlanes[0] = bottomPlane;
            highlightPlanes[1] = arrowPlane;

        }
        else if (sectionToHighlight == highlightSection.NumberOfLevels) {
            highlightPlanes = new GameObject[NumPalletsHigh];
            for (int i = 0; i <= NumPalletsHigh - 1; i++) {
                highlightColumnHeight(i);
            }
        }
        else if (sectionToHighlight == highlightSection.NumberOfAisles) {
            foreach (Column c in factoryBuilder.columns) {
                if (c == null)
                    break;
                c.toggleAisleNumberAndCarpet(true);
            }
        }
        else if (sectionToHighlight == highlightSection.NumberOfBaysDownAisle) {
            highlightPlanes = new GameObject[NumPalletsWide];
            for (int i = 0; i < NumPalletsWide - 1; i = i + parametersButton.PalletsBetweenFrames) {
                highlightColumnLength(i);

            }
            if (parametersButton.PalletsBetweenFrames == 1) {
                highlightPalletsPerBeam(NumPalletsWide - 1);
            }


        }
        else if (sectionToHighlight == highlightSection.NumberOfPalletsPerBeam) {
            highlightPlanes = new GameObject[NumPalletsWide];
            highlightPalletsPerBeam(NumPalletsWide / 2);
        }
        else if (sectionToHighlight == highlightSection.BottomBeamHeight) {
            highlightPlanes = new GameObject[NumPalletsWide * 20];
            GameObject.Find("BottomLine").GetComponent<MeshRenderer>().material = (Material)Resources.Load("HighlightLineMaterial");
            GameObject arrowPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/ArrowHalfHighlightHolder"));

            // draw arrow
            // grab correct pallet - works for 2
            GameObject bottomOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, 0);
            float bottomeOfLastColumnPalletY = bottomOfLastColumnPallet.transform.position.y;
            float worldZPosOfBottomPallet = bottomOfLastColumnPallet.transform.position.x;

            highlightPlanes[NumPalletsWide * 20 - 1] = arrowPlane;
            arrowPlane.transform.position = new Vector3(0, bottomOfLastColumnPallet.transform.position.y, GameObject.Find("BottomLine").transform.position.z);

            for (int i = 0; i < NumPalletsWide; i = i + parametersButton.PalletsBetweenFrames) {
                GameObject bottomPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/VerticalBeamSpacingHolder"));


                // grab correct pallet - works for 2
                GameObject middleOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, i, 0);
                float middleOfLastColumnPalletY = middleOfLastColumnPallet.transform.position.y;
                float posOfDisabledFrameX = middleOfLastColumnPallet.transform.FindChild("frameFar").transform.position.z;
                float worldZPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.x;
                // for 3
                GameObject middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 + 1, NumPalletsHigh / 2);
                float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
                float posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;


                // used to know much to offset highlighting
                float palletsPerBay = parametersButton.PalletsBetweenFrames;

                bottomPlane.transform.parent = middleOfLastColumnPallet.transform;

                bottomPlane.transform.localScale = new Vector3(0.1f, .1f, parametersButton.BeamLength / 2000f * parametersButton.NumberOfBaysDownAisle);

                bottomPlane.transform.position = new Vector3(0, middleOfLastColumnPalletY, GameObject.Find("BottomLine").transform.position.z);
                highlightPlanes[i] = bottomPlane;


            }

        }
        else if (sectionToHighlight == highlightSection.InboundOutbound) {
            if (parametersButton.GetComponent<ParametersButton>().IOStationToString == "Sam...") {
                highlightPlanes = new GameObject[(NumPalletsHigh / 2 + 1) + 2];
                for (int i = 0; i < NumPalletsHigh / 2 + 1; i++) {
                    highlightBayAt(0, i, sectionToHighlight);
                }
                highlightPlanesCount = 0;
            }
            else if (parametersButton.GetComponent<ParametersButton>().IOStationToString == "Opp...") {

                highlightPlanes = new GameObject[2 * (NumPalletsHigh / 2 + 1) + 2];
                for (int i = 0; i < NumPalletsHigh / 2 + 1; i++) {
                    highlightBayAt(NumPalletsWide - (parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames - 1), i, sectionToHighlight);
                    highlightBayAt(0, i, sectionToHighlight);
                }
                highlightPlanesCount = 0;

            }

        }
        else if (sectionToHighlight == highlightSection.PickUp) {
            if (parametersButton.GetComponent<ParametersButton>().PickUpLocationString == "P&D") {
                highlightPlanes = new GameObject[(NumPalletsHigh / 2 + 1) + 2];
                for (int i = 0; i < NumPalletsHigh / 2 + 1; i++) {
                    highlightBayAt(0, i, sectionToHighlight);
                }
                highlightPlanesCount = 0;

                Renderer[] rs = GameObject.Find("Platform").GetComponentsInChildren<Renderer>();
                {
                    foreach (Renderer r in rs) {
                        if (r.transform.name == "pickUpDropOffHighlight") {
                            r.enabled = false;
                        }
                    }
                }
            }
            else if (parametersButton.GetComponent<ParametersButton>().PickUpLocationString == "DOCK") {

                Renderer[] rs = GameObject.Find("Platform").GetComponentsInChildren<Renderer>();
                {
                    foreach (Renderer r in rs) {
                        if (r.transform.name == "pickUpDropOffHighlight") {
                            r.enabled = true;
                        }
                    }
                }


            }
        }
        else if (sectionToHighlight == highlightSection.DropOff) {
            if (parametersButton.GetComponent<ParametersButton>().DropOffLocationString == "P&D") {
                highlightPlanes = new GameObject[(NumPalletsHigh / 2 + 1) + 2];
                for (int i = 0; i < NumPalletsHigh / 2 + 1; i++) {
                    highlightBayAt(0, i, sectionToHighlight);
                }
                highlightPlanesCount = 0;

                Renderer[] rs = GameObject.Find("Platform").GetComponentsInChildren<Renderer>();
                {
                    foreach (Renderer r in rs) {
                        if (r.transform.name == "pickUpDropOffHighlight") {
                            r.enabled = false;
                        }
                    }
                }
            }
            else if (parametersButton.GetComponent<ParametersButton>().DropOffLocationString == "DOCK") {

                Renderer[] rs = GameObject.Find("Platform").GetComponentsInChildren<Renderer>();
                {
                    foreach (Renderer r in rs) {
                        if (r.transform.name == "pickUpDropOffHighlight") {
                            r.enabled = true;
                        }
                    }
                }


            }
        }

        else if (sectionToHighlight == highlightSection.NumberOfPAndDLevels) {
            highlightPlanes = new GameObject[parametersButton.GetComponent<ParametersButton>().PAndDLevels];
            for (int i = 0; i < parametersButton.GetComponent<ParametersButton>().PAndDLevels; i++) {
                float x = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, 0, i).transform.position.z;
                float y = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, 0, i).transform.position.y;

                GameObject highlightPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/NumberOfColumnsHighlightHolder"));
                highlightPlane.transform.position = new Vector3(0, y + parametersButton.GetComponent<ParametersButton>().VerticalBeamSpacing / 2000f, x);
                highlightPlane.transform.localScale = new Vector3(1, parametersButton.GetComponent<ParametersButton>().VerticalBeamSpacing / 1100f, parametersButton.GetComponent<ParametersButton>().BeamLength / 2000f / parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames);
                highlightPlanes[i] = highlightPlane;

            }
        }
        else if (sectionToHighlight == highlightSection.NumberOfPAndDBeamsAcross) {
            highlightPlanes = new GameObject[NumPalletsWide];
            for (int i = 0; i < parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames * parametersButton.GetComponent<ParametersButton>().PAndDBeamsAcross; i = i + parametersButton.PalletsBetweenFrames) {
                highlightColumnLength(i);

            }
            if (parametersButton.PalletsBetweenFrames == 1) {
                highlightPalletsPerBeam(NumPalletsWide - 1);
            }
        }
        else if (sectionToHighlight == highlightSection.none) {
            // clear height and width outlines
            if (highlightPlanes != null) {
                for (int i = 0; i < highlightPlanes.Length; i++) {
                    GameObject.Destroy(highlightPlanes[i]);
                }

            }

            // clear aisle outlines
            foreach (Column c in factoryBuilder.columns) {
                if (c == null)
                    break;

                c.toggleAisleNumberAndCarpet(false);

            }
        }



    }

    private void highlightBayAt(int x, int y, highlightSection sectionToHighlight) {

        if (parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames == 1 && x == NumPalletsWide - (parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames - 1))
            x -= 1;
        NumPalletsHigh = factoryBuilder.columns[0].NumPalletsHigh;
        NumPalletsWide = factoryBuilder.columns[0].NumPalletsWide;

        GameObject newPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/NumberOfColumnsHighlightHolder"));
        // grab correct pallet


        GameObject chosenPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y);



        // grab pallet above that one  - used to know where to place topPlane
        GameObject middleOfLastColumnPalletPlusOneY = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y + 1);


        GameObject middleOfLastColumnPalletPlusOneX;
        GameObject disabledFramePlusOneX;
        GameObject disabledFramePlusTwoX;

        if (x == 0) {
            middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x + 1, y);
            disabledFramePlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x + 1, y).transform.FindChild("frameFar").gameObject;
            disabledFramePlusTwoX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x + 2, y).transform.FindChild("frameFar").gameObject;
        }
        else {
            middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y);
            disabledFramePlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y);
            disabledFramePlusTwoX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y);
        }

        GameObject middleOfLastColumnPalletMinusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x, y);
        ;
        if (x == NumPalletsWide - 1) {
            chosenPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, x - 1, y);
        }



        float posOfDisabledFrameX = chosenPallet.transform.FindChild("frameFar").transform.position.z;

        // for 3


        float xPosOfMiddleOfLastColumnPalletMinusOneX = middleOfLastColumnPalletMinusOneX.transform.position.z;
        float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
        float posOfDisabledFramePlusOneX = chosenPallet.transform.FindChild("frameFar").transform.position.z;

        float posOfDisabledFramePlusTwoX = disabledFramePlusOneX.transform.position.z;
        float posOfDisabledFramePlusThreeX = disabledFramePlusTwoX.transform.position.z;


        float worldYPosOfMiddlePallet = chosenPallet.transform.position.y;
        float worldXPosOfMiddlePallet = chosenPallet.transform.position.z;
        float worldZPosOfMiddlePallet = chosenPallet.transform.position.x;


        worldZPosOfMiddlePallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, 0).transform.position.x;


        // no pallet above then just use normal one
        if (middleOfLastColumnPalletPlusOneY == null)
            middleOfLastColumnPalletPlusOneY = chosenPallet;
        float worldYPosOfMiddlePlusOneYPallet = middleOfLastColumnPalletPlusOneY.transform.position.y;


        // used to know much to offset highlighting
        float palletsPerBay = parametersButton.PalletsBetweenFrames;




        if (palletsPerBay == 1) {
            if (x == 0) {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, worldXPosOfMiddlePallet);
            }
            else {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
            }

        }
        else if (palletsPerBay == 2) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusOneX);
            }
            else {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFrameX);
            }


        }
        else if (palletsPerBay == 3) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                if (x == 0) {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
                }
                else {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, worldXPosOfMiddlePallet);
                }

            }
            else {
                if (x == 0) {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
                }
                else {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
                }

            }
        }
        else if (palletsPerBay == 4) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                if (x == 0) {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusTwoX);
                }
                else {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusOneX);
                }

            }
            else {
                if (x == 0) {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusTwoX);
                }
                else {
                    newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusOneX);
                }

            }

        }

        highlightPlanes[highlightPlanesCount++] = newPlane;

        newPlane.transform.parent = chosenPallet.transform;
        newPlane.transform.localScale = new Vector3(1f, parametersButton.VerticalBeamSpacing / 1100f, parametersButton.BeamLength / 2000f);


        if (y == 0 && sectionToHighlight == highlightSection.InboundOutbound) {
            if (parametersButton.GetComponent<ParametersButton>().IOStationToString == "Sam...") {
                GameObject arrowPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/ArrowHighlightHolder"));
                arrowPlane.transform.position = new Vector3(0, -.2f, newPlane.transform.position.z);
                arrowPlane.transform.localScale = new Vector3(1, parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames, 1);
                arrowPlane.transform.Rotate(new Vector3(90, 0, 0));
                highlightPlanes[highlightPlanesCount++] = arrowPlane;
            }
            else if (parametersButton.GetComponent<ParametersButton>().IOStationToString == "Opp...") {
                GameObject arrowPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/ArrowRightHighlightHolder"));
                arrowPlane.transform.position = new Vector3(0, -.2f, newPlane.transform.position.z);
                arrowPlane.transform.localScale = new Vector3(1, parametersButton.GetComponent<ParametersButton>().PalletsBetweenFrames, 1);
                arrowPlane.transform.Rotate(new Vector3(-90, 0, 0));
                highlightPlanes[highlightPlanesCount++] = arrowPlane;
            }
        }


    }

    private void highlightPalletsPerBeam(int i) {

        NumPalletsHigh = factoryBuilder.columns[0].NumPalletsHigh;
        NumPalletsWide = factoryBuilder.columns[0].NumPalletsWide;

        GameObject newPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/NumberOfColumnsHighlightHolder"));


        // grab correct pallet
        GameObject middleOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, i, NumPalletsHigh / 2);

        float posOfDisabledFrameX = middleOfLastColumnPallet.transform.FindChild("frameFar").transform.position.z;

        // grab pallet above that one  - used to know where to place topPlane
        GameObject middleOfLastColumnPalletPlusOneY = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, NumPalletsHigh / 2 + 1);

        // for 3
        GameObject middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 + 1, NumPalletsHigh / 2);

        float posOfDisabledFramePlusOneX = 0;

        // for 4, increments by 2
        GameObject middleOfLastColumnPalletPlusTwoX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 - 1, NumPalletsHigh / 2);
        float posOfDisabledFramePlusTwoX = middleOfLastColumnPalletPlusTwoX.transform.FindChild("frameFar").transform.position.z;

        float worldYPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.y;
        float worldXPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.z;
        float worldZPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.x;



        // no pallet above then just use normal one
        if (middleOfLastColumnPalletPlusOneY == null)
            middleOfLastColumnPalletPlusOneY = middleOfLastColumnPallet;
        float worldYPosOfMiddlePlusOneYPallet = middleOfLastColumnPalletPlusOneY.transform.position.y;


        // used to know much to offset highlighting
        float palletsPerBay = parametersButton.PalletsBetweenFrames;

        float xPosOfMiddleOfLastColumnPalletPlusOneX = 0;
        if (palletsPerBay == 3) xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
        if (palletsPerBay == 2 || palletsPerBay == 4) posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;

        if (palletsPerBay == 1) {
            if (parametersButton.NumPalletsHigh == 2 || parametersButton.NumPalletsHigh == 1) {
                worldYPosOfMiddlePlusOneYPallet += parametersButton.VerticalBeamSpacing / 1000f;
            }
            newPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, worldXPosOfMiddlePallet);
        }
        else if (palletsPerBay == 2) {

            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                posOfDisabledFrameX = posOfDisabledFramePlusOneX;
            }
            if (parametersButton.NumPalletsHigh == 2 || parametersButton.NumPalletsHigh == 1) {
                worldYPosOfMiddlePlusOneYPallet += parametersButton.VerticalBeamSpacing / 1000f;
            }
            newPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, posOfDisabledFrameX);

        }
        else if (palletsPerBay == 3) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                xPosOfMiddleOfLastColumnPalletPlusOneX = worldXPosOfMiddlePallet;
            }
            if (parametersButton.NumPalletsHigh == 2 || parametersButton.NumPalletsHigh == 1) {
                worldYPosOfMiddlePlusOneYPallet += parametersButton.VerticalBeamSpacing / 1000f;
            }
            newPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
        }
        else if (palletsPerBay == 4) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                posOfDisabledFramePlusOneX = posOfDisabledFramePlusTwoX;
            }
            if (parametersButton.NumPalletsHigh == 2 || parametersButton.NumPalletsHigh == 1) {
                worldYPosOfMiddlePlusOneYPallet += parametersButton.VerticalBeamSpacing / 1000f;
            }
            newPlane.transform.position = new Vector3(0, (worldYPosOfMiddlePallet + worldYPosOfMiddlePlusOneYPallet) / 2, posOfDisabledFramePlusOneX);
        }

        highlightPlanes[i] = newPlane;

        newPlane.transform.parent = middleOfLastColumnPallet.transform;
        newPlane.transform.localScale = new Vector3(1f, parametersButton.VerticalBeamSpacing / 1100f, parametersButton.BeamLength / 2000f);
        //  Debug.Log(newPlane.transform.position);

    }

    private void highlightColumnHeight(int i) {
        NumPalletsHigh = factoryBuilder.columns[0].NumPalletsHigh;
        NumPalletsWide = factoryBuilder.columns[0].NumPalletsWide;

        GameObject newPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/NumberOfColumnsHighlightHolder"));
        // grab correct pallet
        GameObject middleOfLastColumnPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, i);



        // grab pallet above that one  - used to know where to place topPlane
        GameObject middleOfLastColumnPalletPlusOneY = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, i + 1);


        GameObject middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 + 1, i);

        GameObject middleOfLastColumnPalletMinusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2 - 1, i);


        GameObject g = (GameObject)middleOfLastColumnPallet.transform.FindChild("frameFar").gameObject;
        float posOfDisabledFrameX = g != null ? g.transform.position.z : 0;

        // for 3


        float xPosOfMiddleOfLastColumnPalletMinusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
        float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
        float posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;


        float worldYPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.y;
        float worldXPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.z;
        float worldZPosOfMiddlePallet = middleOfLastColumnPallet.transform.position.x;


        worldZPosOfMiddlePallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, 0).transform.position.x;


        // no pallet above then just use normal one
        if (middleOfLastColumnPalletPlusOneY == null)
            middleOfLastColumnPalletPlusOneY = middleOfLastColumnPallet;
        float worldYPosOfMiddlePlusOneYPallet = middleOfLastColumnPalletPlusOneY.transform.position.y;


        // used to know much to offset highlighting
        float palletsPerBay = parametersButton.PalletsBetweenFrames;




        if (palletsPerBay == 1) {
            newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, worldXPosOfMiddlePallet);
        }
        else if (palletsPerBay == 2) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusOneX);
            }
            else {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFrameX);
            }


        }
        else if (palletsPerBay == 3) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, worldXPosOfMiddlePallet);
            }
            else {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletPlusOneX);
            }
        }
        else if (palletsPerBay == 4) {
            if (parametersButton.NumberOfBaysDownAisle % 2 == 1) {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, xPosOfMiddleOfLastColumnPalletMinusOneX);
            }
            else {
                newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + parametersButton.VerticalBeamSpacing / 1000f / 2, posOfDisabledFramePlusOneX);
            }

        }

        highlightPlanes[i] = newPlane;

        newPlane.transform.parent = middleOfLastColumnPallet.transform;
        newPlane.transform.localScale = new Vector3(1f, parametersButton.VerticalBeamSpacing / 1100f, parametersButton.BeamLength / 2000f);
        //  Debug.Log(newPlane.transform.position);
    }

    private void highlightColumnLength(int i) {
        NumPalletsHigh = factoryBuilder.columns[0].NumPalletsHigh;
        NumPalletsWide = factoryBuilder.columns[0].NumPalletsWide;

        GameObject newPlane = (GameObject)GameObject.Instantiate(Resources.Load("Highlighting/NumberOfColumnsHighlightHolder"));
        // grab correct pallet
        GameObject currentPallet = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, i, NumPalletsHigh / 2);



        // grab pallet above that one  - used to know where to place topPlane
        GameObject middleOfLastColumnPalletPlusOneY = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, NumPalletsWide / 2, NumPalletsHigh / 2 + 1);



        // for 3

        GameObject middleOfLastColumnPalletPlusOneX;


        middleOfLastColumnPalletPlusOneX = factoryBuilder.getPalletAtPosition(0, ColumnSide.Left, i + 1, NumPalletsHigh / 2);


        float xPosOfMiddleOfLastColumnPalletPlusOneX = middleOfLastColumnPalletPlusOneX.transform.position.z;
        float posOfDisabledFramePlusOneX = middleOfLastColumnPalletPlusOneX.transform.FindChild("frameFar").transform.position.z;


        float worldYPosOfMiddlePallet = currentPallet.transform.position.y;
        float worldXPosOfMiddlePallet = currentPallet.transform.position.z;
        float worldZPosOfMiddlePallet = currentPallet.transform.position.x;


        // no pallet above then just use normal one
        if (middleOfLastColumnPalletPlusOneY == null)
            middleOfLastColumnPalletPlusOneY = currentPallet;
        float worldYPosOfMiddlePlusOneYPallet = middleOfLastColumnPalletPlusOneY.transform.position.y;


        // used to know much to offset highlighting
        float palletsPerBay = parametersButton.PalletsBetweenFrames;

        float posOfDisabledFrameX = 0;
        if (palletsPerBay == 2) posOfDisabledFrameX = currentPallet.transform.FindChild("frameFar").transform.position.z;

        float halfPalletHeight = parametersButton.VerticalBeamSpacing / 2000f;

        if (palletsPerBay == 1) {
            newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + halfPalletHeight, worldXPosOfMiddlePallet);
        }
        else if (palletsPerBay == 2) {


            newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + halfPalletHeight, posOfDisabledFrameX);

        }
        else if (palletsPerBay == 3) {



            newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + halfPalletHeight, xPosOfMiddleOfLastColumnPalletPlusOneX);


        }
        else if (palletsPerBay == 4) {
            newPlane.transform.position = new Vector3(0, worldYPosOfMiddlePallet + halfPalletHeight, posOfDisabledFramePlusOneX);
        }


        highlightPlanes[i] = newPlane;

        newPlane.transform.parent = currentPallet.transform;
        newPlane.transform.localScale = new Vector3(1f, parametersButton.VerticalBeamSpacing / 1100f, parametersButton.BeamLength / 2000f);
        //  Debug.Log(newPlane.transform.position);
    }
}
