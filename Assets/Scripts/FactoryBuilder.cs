
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;


//holds all information about pallets and has the functions to edit them.
public class FactoryBuilder : MonoBehaviour {
    public Column[] columns;
    private GameObject uiEditControl;
    public GameObject parameters;
    private GameObject aisleWhiteCarpet;
    public LayerMask uiEditMask;
    public Camera UIcam;
    public Camera gimbalCam;
    private GameObject camTarget;
    private bool placedInitialPallets = false;

    private int totalNumberOFPallets = 0;
    //settings
    private float numPalletsWide = 0;
    private float numPalletsHigh = 0;
    private float palletWidth = 0;


    private float palletDepth = 0;
    private float palletHeight = 0;
    private float adjColSpacing = 0;

    //values to use when making new columns, based on an average of whats already there
    private int averagePalsWide = 0;
    private int averagePalsHigh = 0;
    private float averagePalStartZ = 0;

    //either one column can be selected, or they all can.
    private int selectedCol = -1;   //-1 for none
    private bool allSelected = false;
    private bool oneSelected = false;

    private float timeSelectedCol = 0;
    private float doubleTapTime = 1f;

    public bool dragging = false;
    public GameObject currentDragButton;
    public GameObject cameraZoomfactorOnView;

    //fps
    private float frameRateTooLow = 15;
    private float frameRateSafe = 25;
    private float timeLastCheckedFrameRate = 0;
    private float timeBetweenFrameRateChecks = 2.5f;
    private float frameRateCamDistThreshold = 50;
    private bool[] columnPerfStates = new bool[200];
    private float frameRateAtLastChange = 0;
    private float perfDiffSinceLastChange = 0;
    private float numFpsRecords = 60;
    private float[] lastFewFpsReadings = new float[60];
    private int nextfpsidx = 0;
    private float performanceStartTime = 3f;    //lets the fps have time to become constant after loading 

    //// highlight panels
    //private GameObject[] highlightPlanes;
    // Use this for initialization
    void Start() {
        uiEditControl = (GameObject)Instantiate(Resources.Load("UIEditControl"));
        camTarget = GameObject.Find("CameraTarget");
        aisleWhiteCarpet = (GameObject)Resources.Load("AisleWhiteCarpet");

        for (int i = 0; i < lastFewFpsReadings.Length - 1; i++)
            lastFewFpsReadings[i] = frameRateTooLow + 1;
    }


    // Update is called once per frame
    void Update() {
        //   if(currentDragButton != null)        print(currentDragButton.name);
        checkForInput();
        if (placedInitialPallets) {
            updateColumns();
            updateForPerformance();
        }
        camTarget.transform.position = getFactoryCenter();
    }

    public void toggleAisleNumbers(bool toggle) {
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            columns[i].toggleAisleNumberAndCarpet(toggle);
        }
    }
    void LateUpdate() {
        fetchSettings();
        if (!placedInitialPallets) {
            averagePalsWide = (int)numPalletsWide;
            averagePalsHigh = (int)numPalletsHigh;
            placeInitialPallets();
            placedInitialPallets = true;
        }
    }


    void OnGUI() {

    }


    private void checkForInput() {
        if (!parameters.GetComponent<ParametersButton>().EnableTouchControls) return;
        //mouse clicks
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0) {

            // quit if dragging already
            if (dragging) {
                return;
            }

            // check if button hit
            RaycastHit hitButton;
            Ray buttonRay = UIcam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(buttonRay, out hitButton, Mathf.Infinity, uiEditMask)) {

                if (hitButton.transform.name.StartsWith("Plus") || hitButton.transform.name.StartsWith("Minus") || hitButton.transform.name.StartsWith("Drag")) {   //deselect column 	 	
                    return;
                }
            }


            // do if only one column already selected
            if (oneSelected) {
                // Getting the closest pallet that the raycast hit
                Ray raymain = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(raymain, Mathf.Infinity);

                int i = 0;
                int indexOfClosestPallet = 99999;
                float distanceOfClosestPallet = Mathf.Infinity;
                while (i < hits.Length) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.name == "Pallet(Clone)") {
                        //   print("yep pallet");
                        if (hit.distance < distanceOfClosestPallet) {
                            indexOfClosestPallet = i;
                            distanceOfClosestPallet = hit.distance;
                        }
                    }
                    i++;
                }

                // havnt clicked on a pallet (must have clicked floor or sky) - deselect everything and quit
                if (distanceOfClosestPallet == Mathf.Infinity) {

                    //   selectedCol = -1;
                    uiEditControl.GetComponent<ColEditControls>().show(false);
                    uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(false);
                    allSelected = false;
                    oneSelected = false;
                    return;
                }

                // select the new column
                putColumnInEditMode(hits[indexOfClosestPallet].transform.gameObject.GetComponent<PalletProperties>().ColNum);
                uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(false);

                timeSelectedCol = Time.time;
                return;

            }

            // if we have already selected all columns
            if (allSelected) {
                // Finding the closest pallet
                Ray raymain = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(raymain, Mathf.Infinity);

                int i = 0;
                int indexOfClosestPallet = 99999;
                float distanceOfClosestPallet = Mathf.Infinity;
                while (i < hits.Length) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.name == "Pallet(Clone)") {
                        //   print("yep pallet");
                        if (hit.distance < distanceOfClosestPallet) {
                            indexOfClosestPallet = i;
                            distanceOfClosestPallet = hit.distance;
                        }
                    }
                    i++;
                }

                // havnt clicked on a pallet - deselect everything and quit
                if (distanceOfClosestPallet == Mathf.Infinity) {
                    //   selectedCol = -1;
                    uiEditControl.GetComponent<ColEditControls>().show(false);
                    uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(false);
                    allSelected = false;
                    return;
                }


                if (Time.time - timeSelectedCol < doubleTapTime) {
                    putColumnInEditMode(hits[indexOfClosestPallet].transform.gameObject.GetComponent<PalletProperties>().ColNum);
                    uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(false);
                    allSelected = false;
                    oneSelected = true;
                    timeSelectedCol = Time.time;
                }
                timeSelectedCol = Time.time;
                return;
            }

           // nothing has been selected
            else {
                RaycastHit hitf;
                Ray ray = UIcam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitf, Mathf.Infinity)) {

                    //check we clicked a pallet, doesnt matter which
                    if (hitf.transform.name == "Pallet(Clone)") {
                        // select all pallets
                        putColumnInEditMode(hitf.transform.gameObject.GetComponent<PalletProperties>().ColNum);
                        uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(true);
                        allSelected = true;
                    }
                    else // nothing selected
                    {
                        uiEditControl.GetComponent<ColEditControls>().show(false);
                        uiEditControl.GetComponent<ColEditControls>().setIsAllEditor(false);
                        allSelected = false;
                    }
                }
            }
            timeSelectedCol = Time.time;
        }
    }


    private void updateColumns() {
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            Column c = columns[i];
            if (c != null) c.Update();
        }
    }


    private void fetchSettings() {
        numPalletsWide = parameters.GetComponent<ParametersButton>().NumPalletsWide;
        numPalletsHigh = parameters.GetComponent<ParametersButton>().NumPalletsHigh;
        palletWidth = parameters.GetComponent<ParametersButton>().PalletISO.getWidth();
        palletDepth = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        palletHeight = parameters.GetComponent<ParametersButton>().PalletHeight;
        adjColSpacing = parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
    }


    private void destroyExistingPallets() {
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            Column c = columns[i];
            foreach (GameObject p in c.Left)
                Destroy(p);
            foreach (GameObject p in c.Right)
                Destroy(p);

            columns[i] = null;
        }
    }


    private void placeInitialPallets() {
        columns = new Column[100];
        for (int col = 0; col < parameters.GetComponent<ParametersButton>().NumAisles; col++)
            columns[col] = new Column(col, averagePalsWide, averagePalsHigh, averagePalStartZ);
    }

    private void updateForPerformance() {
        if (Time.time > performanceStartTime) {
            //set average frame rate
            lastFewFpsReadings[nextfpsidx] = 1 / Time.smoothDeltaTime;
            float avgFps = 0;
            foreach (float f in lastFewFpsReadings) avgFps += f;
            avgFps /= lastFewFpsReadings.Length;
            nextfpsidx = nextfpsidx < numFpsRecords - 1 ? nextfpsidx + 1 : 0;

            //check performance change since last col swap
            perfDiffSinceLastChange = avgFps - frameRateAtLastChange;
            //Debug.Log(" FPS" + 1 / Time.smoothDeltaTime + " AFPS:" + avgFps + "  AtLastChange:" + frameRateAtLastChange + "  Diff:" + perfDiffSinceLastChange);

            //for each column, swap it out if it's inside the performance range
            for (int i = 1; i < columns.Length - 1; i++) {
                Column c = columns[i];
                if (c != null) {
                    Vector3 colPos = c.getColPos();
                    bool on = Vector3.Distance(Camera.main.transform.position, colPos) > frameRateCamDistThreshold;
                    c.setPerformanceViewOn(true);
                    columnPerfStates[i] = on;
                    frameRateAtLastChange = avgFps;
                }
            }

            //if its time to check for performance again...
            if (Time.time > timeLastCheckedFrameRate + timeBetweenFrameRateChecks) {
                //if average fps is in the safe zone...
                if (avgFps > frameRateSafe) {
                    //get distance of furthest col from the camera
                    float maxColDist = 0;
                    foreach (Column col in columns)
                        if (col != null)
                            if (maxColDist < Vector3.Distance(Camera.main.transform.position, col.getColPos()))
                                maxColDist = Vector3.Distance(Camera.main.transform.position, col.getColPos());

                    //  Debug.Log("Increasing distance cols can be seen from: " + frameRateCamDistThreshold + " " + maxColDist + " " + avgFps);

                    if (frameRateCamDistThreshold < maxColDist) {
                        frameRateCamDistThreshold += 5f;
                    }
                }
                //if average fps is in the danger zone...
                if (avgFps < frameRateTooLow) {
                    //     Debug.Log("Decreasing distance cols can be seen from: " + frameRateCamDistThreshold + " " + avgFps);
                    if (frameRateCamDistThreshold > 0) {
                        frameRateCamDistThreshold -= 5f;
                    }
                }
                timeLastCheckedFrameRate = Time.time;
            }
        }
    }


    public void increaseNumCols(ColumnEnd end) {
        fetchSettings();

        int numColumns = (int)parameters.GetComponent<ParametersButton>().NumAisles;
        //make room if necessary
        if (numColumns >= columns.Length) {
            Column[] bigger = new Column[columns.Length + 50];
            for (int i = 0; i < numColumns; i++)
                bigger[i] = columns[i];
            columns = bigger;
        }

        columns[(int)numColumns] = new Column(numColumns, averagePalsWide, averagePalsHigh, averagePalStartZ);
        parameters.GetComponent<ParametersButton>().NumAisles++;

        //addAisleCarpet(numColumns);
    }

    public void addAisleCarpet(int columnIndex) {
        float columnWidth = 2.2f;
        float aisleWidth = 1.95f;
        GameObject newAisleAtBase = (GameObject)Instantiate(aisleWhiteCarpet, new Vector3(2 + ((columnIndex + 1) * columnWidth) + ((columnIndex + 1) * aisleWidth) - (columnWidth * 2 + aisleWidth * 2), 0, averagePalsWide / 2), transform.rotation);
        newAisleAtBase.transform.localScale = new Vector3(.21f, 1f, 1.8f);

    }
    public void placeInitialAisleCarpets(int columnIndex) {
        float columnWidth = 2.2f;
        float aisleWidth = 1.95f;
        if (columnIndex == 0) {
            GameObject newAisleAtBase = (GameObject)Instantiate(aisleWhiteCarpet, new Vector3(2, 0, averagePalsWide / 2), transform.rotation);
            newAisleAtBase.transform.localScale = new Vector3(.2f, 1f, 1.8f);
        }
        if (columnIndex >= parameters.GetComponent<ParametersButton>().NumAisles - 2) {
            return;
        }

        GameObject newAisle = (GameObject)Instantiate(aisleWhiteCarpet, new Vector3(2 + ((columnIndex + 1) * columnWidth) + ((columnIndex + 1) * aisleWidth), 0, averagePalsWide / 2), transform.rotation);
        //  aisleWhiteCarpet.transform.position = new Vector3(columnIndex * columnWidth, 0, 0);
        newAisle.transform.localScale = new Vector3(.21f, 1f, 1.8f);
    }
    public void refreshNumberOfColumns(int newNumber) {

        if (newNumber < parameters.GetComponent<ParametersButton>().NumAisles) {
            while (newNumber < parameters.GetComponent<ParametersButton>().NumAisles) {
                decreaseNumCols(ColumnEnd.High);

            }
        }

        else if (newNumber > parameters.GetComponent<ParametersButton>().NumAisles) {
            while (newNumber > parameters.GetComponent<ParametersButton>().NumAisles) {
                increaseNumCols(ColumnEnd.High);

            }
        }
    }

    public void refreshNumberOfPalletsDownAisle(int newColumnLength) {

        if (newColumnLength < averagePalsWide) {
            while (newColumnLength < averagePalsWide) {
                deletePalletsEndOfAllCols(ColumnEnd.High);

            }
        }

        else if (newColumnLength > averagePalsWide) {
            while (newColumnLength > averagePalsWide) {
                addPalletsEndOfAllCols(ColumnEnd.High);

            }
        }

        // parameters.GetComponent<ParametersButton>().highlightController.SetHighlightArea(highlightSection.columnLength);
    }

    public void refreshNumberOfLevels(int newNumberOfLevels) {
        if (newNumberOfLevels < averagePalsHigh) {
            while (newNumberOfLevels < averagePalsHigh) {
                deletePalletsTopAllCols();

            }
        }

        else if (newNumberOfLevels > averagePalsHigh) {
            while (newNumberOfLevels > averagePalsHigh) {
                addPalletsTopAllCols();

            }
        }
    }

    public void refreshNumberOfPalletsPerBay(int newNumberOfPalletsPerBay) {

    }



    public void decreaseNumCols(ColumnEnd end) {
        if (parameters.GetComponent<ParametersButton>().NumAisles > 2) {
            parameters.GetComponent<ParametersButton>().NumAisles--;
            columns[(int)parameters.GetComponent<ParametersButton>().NumAisles].DestroySelf();
            columns[(int)parameters.GetComponent<ParametersButton>().NumAisles].destroyAll();
            columns[(int)parameters.GetComponent<ParametersButton>().NumAisles] = null;
            uiEditControl.GetComponent<ColEditControls>().setCol(0);
        }
        refreshBottomBar();
    }


    public void addPalletsEndOfAllCols(ColumnEnd end) {
        bool success = false;

        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++)
            if (columns[i].canAddPalletsOnEnd()) {
                columns[i].addPalletsOnEnd(end);
                success = true;

                //columns[i].refreshAllPalletAndFrameTransforms();
            }

        //modify average values
        if (success) {
            if (end == ColumnEnd.Low)
                averagePalStartZ -= palletWidth;
            averagePalsWide++;
            parameters.GetComponent<ParametersButton>().NumPalletsWide++;

        }
    }

    public void deletePalletsEndOfAllCols(ColumnEnd end) {
        bool success = false;


        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++)
            if (columns[i].canDeletePalletsFromEnd()) {
                columns[i].deletePalletsOnEnd(end);
                success = true;

            }

        //modify average values
        if (success) {
            if (end == ColumnEnd.Low)
                averagePalStartZ += palletWidth;

            if (averagePalsWide > 0)
                averagePalsWide--;
            parameters.GetComponent<ParametersButton>().NumPalletsWide--;
            uiEditControl.GetComponent<ColEditControls>().setCol(0);
        }
    }

    public void setBottomBarOn(bool on) {
        foreach (Column c in columns)
            if (c != null)
                c.setBottomBarOn(on);
    }

    public void refreshBottomBar() {
        bool bottomBarOn = parameters.GetComponent<ParametersButton>().FirstLevelBeamPresent;
        foreach (Column c in columns) {
            if (c != null) {
                c.setBottomBarOn(bottomBarOn);
                c.refreshBottomBarHeight();
                c.refreshBottomBarLegs();
            }
        }
    }

    public void refreshAllPalletAndFrameTransforms() {
        foreach (Column c in columns) {
            if (c != null) {
                c.refreshAllPalletAndFrameTransforms();
            }
        }
    }

    public GameObject getPalletAtPosition(int column, ColumnSide side, int x, int y) {
        return columns[column].getPallet(side, x, y);
    }
    private void updateScale(GameObject g) {
        g.transform.localScale = new Vector3(palletDepth, palletHeight, palletWidth);
    }

    public void addPalletsEndOfSelectedCol(ColumnEnd end) {
        if (columns[selectedCol].canAddPalletsOnEnd()) {
            columns[selectedCol].addPalletsOnEnd(end);
        }
    }

    public void deletePalletsEndOfSelectedCol(ColumnEnd end) {
        if (columns[selectedCol].canDeletePalletsFromEnd()) {
            columns[selectedCol].deletePalletsOnEnd(end);
        }
    }

    public void addPalletsTopAllCols() {
        bool success = false;

        fetchSettings();
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++)
            if (columns[i].canAddPalletsOnTop()) {
                columns[i].addPalletsOnTop();
                success = true;
            }

        if (success) {
            averagePalsHigh++;
            parameters.GetComponent<ParametersButton>().NumPalletsHigh++;
        }

    }

    public float getMiddlePalletOfLastColumnZ() {
        int totalNumberOfAisles = (int)parameters.GetComponent<ParametersButton>().NumAisles - 1;
        return columns[totalNumberOfAisles].getPallet(ColumnSide.Left, columns[totalNumberOfAisles].NumPalletsWide / 2, columns[totalNumberOfAisles].NumPalletsHigh / 2).transform.position.z;
    }
    public float getMiddlePalletOfLastColumnY() {
        int totalNumberOfAisles = (int)parameters.GetComponent<ParametersButton>().NumAisles - 1;
        return columns[totalNumberOfAisles].getPallet(ColumnSide.Left, columns[totalNumberOfAisles].NumPalletsWide / 2, columns[totalNumberOfAisles].NumPalletsHigh / 2).transform.position.y;
    }
    public void deletePalletsTopAllCols() {
        bool success = false;

        fetchSettings();
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++)
            if (columns[i].canDeletePalletsFromTop()) {
                columns[i].deletePalletsOnTop();
                success = true;
            }

        if (success) {
            averagePalsHigh--;
            parameters.GetComponent<ParametersButton>().NumPalletsHigh--;
        }

    }

    public void addPalletsTopOfSelectedCol() {
        if (columns[selectedCol].canAddPalletsOnTop()) {
            columns[selectedCol].addPalletsOnTop();
        }
    }

    public void deletePalletsTopOfSelectedCol() {
        if (columns[selectedCol].canDeletePalletsFromTop())
            columns[selectedCol].deletePalletsOnTop();
    }

    //removed due to performance issues
    private void setObjGhosted(GameObject g, bool b) {
        Color pc = g.renderer.material.color;
        pc.a = b ? 0.5f : 1.0f;
        g.renderer.material.color = pc;
    }


    public void putColumnInEditMode(int i) {
        selectedCol = i;
        uiEditControl.GetComponent<ColEditControls>().updateTransformOnCol(i);
        uiEditControl.GetComponent<ColEditControls>().show(true);
    }

    public float getColHeight(int i) {
        return columns[i].NumPalletsHigh * parameters.GetComponent<ParametersButton>().VerticalBeamSpacing * 0.001042f;
    }

    public int getNumPalletsHighOnCol(int i) {
        return columns[i].NumPalletsHigh;
    }


    public float getHighestY() {
        if (placedInitialPallets) {
            float highestY = 0;
            for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
                Column a = columns[i];
                if (highestY < a.NumPalletsHigh * palletHeight)
                    highestY = a.NumPalletsHigh * palletHeight;
            }
            return highestY;
        }
        return -1;
    }

    public float getColLength(int i) {
        float colFarZ = getColFarZ(i);
        float colCloseZ = getColCloseZ(i);
        return (colFarZ - colCloseZ);
    }

    public float getColWidth() {
        return parameters.GetComponent<ParametersButton>().PalletISO.getDepth() * 2 + parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
    }

    public float getAvgXpos() {
        if (placedInitialPallets) {
            int numCols = (int)parameters.GetComponent<ParametersButton>().NumAisles;
            return columns[numCols - 1].getColXPos() / 2;
        }
        return -1;
    }

    public float getAvgZpos() {
        float highestZ = 0;
        float lowestZ = float.MaxValue;
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            float cz = getColCloseZ(i);
            float fz = getColFarZ(i);
            lowestZ = (cz < lowestZ) ? cz : lowestZ;
            highestZ = (fz > highestZ) ? fz : highestZ;
        }
        return (highestZ + lowestZ) / 2;
    }

    public float getHighestPalZpos() {
        float highestZ = 0;
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            float fz = getColFarZ(i);
            highestZ = (fz > highestZ) ? fz : highestZ;
        }
        return highestZ;
    }

    public float getHighestFrameZpos() {
        float highestZ = 0;
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            float fz = getColFarFrameZ(i);
            highestZ = (fz > highestZ) ? fz : highestZ;
        }
        return highestZ;
    }

    public float getLowestPalZpos() {
        float lowestZ = float.MaxValue;
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            float cz = getColCloseZ(i);
            lowestZ = (cz < lowestZ) ? cz : lowestZ;
        }
        return lowestZ;
    }

    public float getLowestFrameZpos() {
        float lowestZ = float.MaxValue;
        for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
            float cz = getColCloseFrameZ(i);
            lowestZ = (cz < lowestZ) ? cz : lowestZ;
        }
        return lowestZ;
    }

    public float getColCloseZ(int i) {
        if (placedInitialPallets) {
            Column c = columns[i];
            GameObject p = (GameObject)c.getPallet(ColumnSide.Left, 0, 0);
            return p.transform.position.z;
        }
        return -1;
    }

    public float getColFarZ(int i) {
        if (placedInitialPallets) {
            GameObject p = (GameObject)columns[i].getPallet(ColumnSide.Left, columns[i].NumPalletsWide - 1, 0);
            return p.transform.position.z;
        }
        return -1;
    }

    public float getColFarFrameZ(int i) {
        float furthestZ = 0;
        if (placedInitialPallets) {
            GameObject p = (GameObject)columns[i].getPallet(ColumnSide.Left, columns[i].NumPalletsWide - 1, 0);
            //get pos of furthest frame Z
            foreach (Transform t in p.transform) {
                if (t.gameObject.name == "frameFar")
                    furthestZ = t.position.z;
            }
            return furthestZ;
        }
        return -1;
    }

    public float getColCloseFrameZ(int i) {
        float closestZ = float.MaxValue;
        if (placedInitialPallets) {
            GameObject p = (GameObject)columns[i].getPallet(ColumnSide.Left, 0, 0);
            //get pos of furthest frame Z
            foreach (Transform t in p.transform) {
                if (t.gameObject.name == "frameClose")
                    closestZ = t.position.z;
            }
            return closestZ;
        }
        return -1;
    }

    public Vector3 getClosestPalZVector() {
        //just get an arbitary pallet at x pos 0
        //Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
        int col = selectedCol;
        if (allSelected)
            col = (int)parameters.GetComponent<ParametersButton>().NumAisles / 2;
        GameObject p = (GameObject)columns[col].getPallet(ColumnSide.Left, 0, 0);
        return p.transform.position;
    }

    public Vector3 getClosestPalXVector() {
        //just get an arbitary pallet at x pos 0
        GameObject p = (GameObject)columns[0].getPallet(ColumnSide.Left, 0, 0);
        return p.transform.position;
    }

    public Vector3 getFurthestPalZVector() {
        int selectColInt = selectedCol;
        if (allSelected) selectColInt = 0;
        //just get an arbitary pallet at x pos columns[selectedCol].NumPalletsWide-1
        GameObject p = (GameObject)columns[selectColInt].getPallet(ColumnSide.Left, columns[selectColInt].NumPalletsWide - 1, 0);
        return p.transform.position;
    }

    public Vector3 getFurthestPalXVector() {
        //just get an arbitary pallet at x pos columns[selectedCol].NumPalletsWide-1

        GameObject p = (GameObject)columns[(int)parameters.GetComponent<ParametersButton>().NumAisles - 1].getPallet(ColumnSide.Left, 0, 0);
        return p.transform.position;
    }

    public Vector3 getAnyHighestPlatVector() {
        Vector3 anyHighestPlat = new Vector3(columns[selectedCol].getColXPos() + (palletDepth / 2 + (adjColSpacing / 2)), columns[selectedCol].NumPalletsHigh * palletHeight, getColCloseZ(selectedCol));
        return anyHighestPlat;
    }

    public bool getAllColsSelected() {
        return allSelected;
    }

    public float getColXPos(int i) {
        if (placedInitialPallets) {
            return columns[i].getColXPos();
        }
        else {
            return -1;
        }

    }

    public int getAveragePalsWide() {
        return averagePalsWide;
    }

    public bool canUseCol(int i) {
        return columns[i] != null && !columns[i].IsDying();
    }

    private Vector3 getFactoryCenter() {
        return new Vector3(getAvgXpos(), getHighestY() / 2, getAvgZpos());
    }

    public float getFirstColumnX() {
        if (columns == null) return 0;
        return columns[0].getColXPos();
    }
    public float getLastColumnX() {
        if (columns == null) return 0;
        return columns[(int)parameters.GetComponent<ParametersButton>().NumAisles - 1].getColXPos();
    }
    public int getTotalNumberOfPallets() {
        return totalNumberOFPallets;
    }
    public void calculateTotalNumberOfPallets() {
        int n = 0;
        //        if (columns != null) {
        //            for (int i = 0; i < parameters.GetComponent<ParametersButton>().NumAisles; i++) {
        //                n += columns[i].NumPalletsHigh * columns[i].NumPalletsWide;
        //            }
        //			n+=columns [0].NumPalletsHigh * columns [0].NumPalletsWide;
        //        }
        n = ((int)parameters.GetComponent<ParametersButton>().NumAisles - 1) * (columns[0].NumPalletsHigh * columns[0].NumPalletsWide * 2);

        totalNumberOFPallets = n;
    }

    public int AveragePalsHigh {
        get { return averagePalsHigh; }
    }

    public void setPalletsPerBayInColumn() {

        for (int col = 0; col < parameters.GetComponent<ParametersButton>().NumAisles; col++)
            columns[col].refreshAllPalletAndFrameTransforms();
    }
    public float PalletWidth {
        get { return palletWidth; }
    }
}



public enum ColumnEnd {
    Low, High //Low = lower z, High = higher z value.
}

public enum ColumnSide {
    Left, Right, Unassigned //Left = lower x, Right = higher x value.
}



