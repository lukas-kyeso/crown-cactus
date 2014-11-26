using UnityEngine;
using System.Collections;

public class Column {
    private GameObject parameters;
    private GameObject factoryBuilder;
    private GameObject aisleWhiteCarpet;
    private GameObject aisleNumberPlane;
    private Texture aisleNormalTexture;
    private Texture aisleOrangeTexture;
    private int colNum = -1;
    private int numPalletsWide = 1;
    private int numPalletsHigh = 1;
    private float baseZ = 0;     //z value of the pallet with the lowest z value in the column
    private GameObject[,] left;
    private GameObject[,] right;
    private int maxColWidth = 50;
    private int maxColHeight = 50;
    private bool isDying = false;
    private bool hasSetInitTransform = false;
    private GameObject columnObject;
    private GameObject newAisleAtBase;
    private GameObject numberPlane;
    private GameObject colLowPoly;
    private float startHeight = 0;
    private bool bottomBeamOn = false;
    //these only appear when bottom bar is on, to fill in the space of lifting the column
    private GameObject extraLFrameFar;
    private GameObject extraRFrameFar;
    private GameObject extraLFrameClose;
    private GameObject extraRFrameClose;
    private float vertBeamSpacing;
    //private Shader shader;

    public Column(int idx, int w, int h, float z) {
        colNum = idx;
        numPalletsWide = w;
        numPalletsHigh = h;
        baseZ = z;
        aisleNormalTexture = Resources.Load<Texture>("Aisle/aisleNormalTexture");
        aisleOrangeTexture = Resources.Load<Texture>("Aisle/aisleOrangeTexture");
        parameters = GameObject.Find("ParametersButton");
        factoryBuilder = GameObject.Find("FactoryBuilderObj");
        aisleWhiteCarpet = (GameObject)Resources.Load("AisleWhiteCarpet");
        aisleNumberPlane = (GameObject)Resources.Load("WhiteCarpetNumber");
        colLowPoly = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load("ColumnLowPoly"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        colLowPoly.SetActive(false);
        columnObject = new GameObject();
        columnObject.transform.name = "Column";
        createInitialPallets(columnObject);
        createAisle(columnObject);

        //bottom bar setup
        startHeight = columnObject.transform.position.y;
        createExtraFrames();
        setBottomBarOn(parameters.GetComponent<ParametersButton>().FirstLevelBeamPresent);

		if (colNum != 0)
						setPerformanceViewOn (true);
    }

    public void Update() {
        // draw carpet
        if (numberPlane != null) {
            Vector3 numberPos = new Vector3(colXWithDeviation(ColumnSide.Left) - 1.5f, .26f, -2);
            numberPlane.transform.position = numberPos;
        }


        if (colNum == 0) {
            //bookend, delete left column if it exists
            foreach (GameObject p in left)
                GameObject.Destroy(p);
        }
        else if (colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1) {
            //bookend, delete right column if it exists
            foreach (GameObject p in right)
                GameObject.Destroy(p);
        }
        else {
            //check that both arrays have at least one pallet, if not, then match the dimensions specified in its variables
            if (right[0, 0] == null) {  //left side
                for (int i = 0; i < numPalletsWide; i++) {
                    for (int y = 0; y < numPalletsHigh; y++) {
                        GameObject rightPallet = createPallet(ColumnSide.Right, i, y);
                        right[i, y] = (rightPallet);
                    }
                }
            }
            if (left[0, 0] == null) {   //right side
                for (int i = 0; i < numPalletsWide; i++) {
                    for (int y = 0; y < numPalletsHigh; y++) {
                        GameObject leftPallet = createPallet(ColumnSide.Left, i, y);
                        left[i, y] = (leftPallet);
                    }
                }
            }
        }

        //refreshAllPalletAndFrameTransforms ();
        setBottomBarOn(bottomBeamOn);   //if you can figure out how to call this when aisles are created/destroyed, move this.
        updatePerformanceBoxTransform();
    }

    public void toggleAisleNumberAndCarpet(bool toggle) {
        if (numberPlane != null) {
            //GameObject.Destroy(numberPlane);
            numberPlane.GetComponent<MeshRenderer>().enabled = toggle;
            if (toggle) {
                newAisleAtBase.renderer.material.mainTexture = aisleOrangeTexture;
            }
            else {
                newAisleAtBase.renderer.material.mainTexture = aisleNormalTexture;
            }
        }

    }
    //sets the positiona and size of the low poly column replacement, activates/deactivates pallets as appropriate
    public void setPerformanceViewOn(bool on) {
        if (on) {
            //envelop column in colLowPoly
            colLowPoly.SetActive(true);

            //deactive all pallets
            foreach (GameObject p in left) { if (p) p.SetActive(false); }
            foreach (GameObject p in right) { if (p) p.SetActive(false); }
        }
        else {
            //active all pallets
            foreach (GameObject p in left) { if (p) p.SetActive(true); }
            foreach (GameObject p in right) { if (p) p.SetActive(true); }
            colLowPoly.SetActive(false);
        }
    }

    private void updatePerformanceBoxTransform() {
        //position
        float xPos = -1;
        if (colNum == 0)
            xPos = getColXPos() + parameters.GetComponent<ParametersButton>().PalletISO.getWidth() / 2 + parameters.GetComponent<ParametersButton>().AdjColumnSpacing / 2;
        else if (colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1)
            xPos = getColXPos() - parameters.GetComponent<ParametersButton>().PalletISO.getWidth() / 2 + parameters.GetComponent<ParametersButton>().AdjColumnSpacing / 2;
        else
            xPos = getColXPos();

        float yPos = bottomBeamOn ? parameters.GetComponent<ParametersButton>().BottomBeamHeight * 0.001f : 0;
        float zPos = factoryBuilder.GetComponent<FactoryBuilder>().getAvgZpos();

        colLowPoly.transform.position = new Vector3(xPos, yPos, zPos);


        float beamLength = parameters.GetComponent<ParametersButton>().BeamLength * 0.001f;
        float palsBetweenFrames = parameters.GetComponent<ParametersButton>().PalletsBetweenFrames;
        float palletZLength = beamLength / palsBetweenFrames;

        //float palletZLength = parameters.GetComponent<ParametersButton>().NumPalletsWide;

        //scale
        float scX = (colNum == 0 || colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1) ? factoryBuilder.GetComponent<FactoryBuilder>().getColWidth() / 2 : factoryBuilder.GetComponent<FactoryBuilder>().getColWidth();
        float scY = factoryBuilder.GetComponent<FactoryBuilder>().getColHeight(colNum);
        //float scZ = factoryBuilder.GetComponent<FactoryBuilder>().getColLength(colNum) + parameters.GetComponent<ParametersButton>().PalletISO.getWidth()*2;
        //float scZ = factoryBuilder.GetComponent<FactoryBuilder>().getColLength(colNum) + parameters.GetComponent<ParametersButton>().PalletISO.getWidth() * parameters.GetComponent<ParametersButton>().PalletsBetweenFrames;
        float scZ = parameters.GetComponent<ParametersButton>().BeamLength * 0.001f * parameters.GetComponent<ParametersButton>().NumberOfBaysDownAisle;
       
        //float scZ = palletZLength * (float)parameters.GetComponent<ParametersButton>().NumPalletsWide + (palletZLength / 2f);
     //   float scZ = (palletZLength) * (beamLength / 1.9f);

      //  float scZ = beamLength * ( (float)parameters.GetComponent<ParametersButton>().NumberOfBaysDownAisle);

        float dz = 0;
        if (left[0, 0] != null) dz = left[numPalletsWide - 1, 0].transform.position.z - left[0, 0].transform.position.z;
        else dz = right[numPalletsWide - 1, 0].transform.position.z - right[0, 0].transform.position.z;
        //float z = (dz / 1);
      //  float scZ = dz;// +z;
        scZ = scZ * 1.072f;
        //float scZ = Mathf.Abs(GameObject.Find("LeftLine").transform.position.z - GameObject.Find("RightLine").transform.position.z) * 1.03f;
        //float scZ = factoryBuilder.GetComponent<FactoryBuilder>().getColLength(colNum) + (palletZLength*1.5f);
        //float scZ = parameters.GetComponent<ParametersButton>().NumPalletsWide * palletZLength;
        colLowPoly.transform.localScale = new Vector3(scX, scY, scZ);



        int numPalsHigh = (int)parameters.GetComponent<ParametersButton>().NumPalletsHigh;
        int numPalsWide = (int)parameters.GetComponent<ParametersButton>().NumPalletsWide;
        colLowPoly.transform.Find("X").renderer.material.mainTextureScale = new Vector2(numPalsWide, numPalsHigh);
        colLowPoly.transform.Find("Z+").renderer.material.mainTextureScale = new Vector2((colNum == 0 || colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1) ? 1 : 2, numPalsHigh);
        colLowPoly.transform.Find("Z-").renderer.material.mainTextureScale = new Vector2((colNum == 0 || colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1) ? 1 : 2, numPalsHigh);
        colLowPoly.transform.Find("Top").renderer.material.mainTextureScale = new Vector2((colNum == 0 || colNum == parameters.GetComponent<ParametersButton>().NumAisles - 1) ? 1 : 2, numPalsWide);
    }

    public void setBottomBarOn(bool b) {
        bottomBeamOn = b;
        if (b) addBottomBar();
        else removeBottomBar();
    }

    public void addBottomBar() {
        bottomBeamOn = true;
        refreshBottomBarHeight();

        for (int i = 0; i < numPalletsWide; i++) {
            GameObject rightPal = right[i, 0];
            GameObject leftPal = left[i, 0];

            setBottomBarOnPalletActive(leftPal, true);
            setBottomBarOnPalletActive(rightPal, true);
        }

        refreshBottomBarLegs();
    }

    public void removeBottomBar() {
        bottomBeamOn = false;
        refreshBottomBarHeight();

        for (int i = 0; i < numPalletsWide; i++) {
            GameObject rightPal = right[i, 0];
            GameObject leftPal = left[i, 0];

            setBottomBarOnPalletActive(leftPal, false);
            setBottomBarOnPalletActive(rightPal, false);
        }

        refreshBottomBarLegs();
    }

    public Vector3 getColPos() {
        return new Vector3(getColXPos(), getAvgYPos(), getAvgZPos());
    }

    private void setBottomBarOnPalletActive(GameObject p, bool b) {
        if (p) {
            GameObject frameShelf = null;

            foreach (Transform frame in p.transform) {
                if (frame.name == "frameShelf")
                    frameShelf = frame.gameObject;
            }

            frameShelf.SetActive(b);
            refreshBottomBarLegs();
        }
    }

    public void refreshBottomBarHeight() {
        if (bottomBeamOn) {
            float barHeight = parameters.GetComponent<ParametersButton>().BottomBeamHeight * 0.001f; // * 0.001 to convert it to mm
            columnObject.transform.position = new Vector3(columnObject.transform.position.x, startHeight + barHeight, columnObject.transform.position.z);
        }
        else
            columnObject.transform.position = new Vector3(columnObject.transform.position.x, startHeight, columnObject.transform.position.z);
    }

    public void refreshBottomBarLegs() {
        float beamLength = parameters.GetComponent<ParametersButton>().BeamLength * 0.001f;
        float palsBetweenFrames = parameters.GetComponent<ParametersButton>().PalletsBetweenFrames;
        float beamDist = beamLength / palsBetweenFrames;

        extraRFrameClose.SetActive(bottomBeamOn && right[0, 0] != null);
        extraLFrameClose.SetActive(bottomBeamOn && left[0, 0] != null);
        extraRFrameFar.SetActive(bottomBeamOn && right[0, 0] != null);
        extraLFrameFar.SetActive(bottomBeamOn && left[0, 0] != null);

        //adjust frame scale and position
        float palWidth = parameters.GetComponent<ParametersButton>().PalletISO.getWidth();

        if (right[0, 0] != null) {
            Vector3 sideScale = new Vector3(right[0, 0].transform.localScale.x, right[0, 0].transform.localScale.y + 0.15f, right[0, 0].transform.localScale.z);
            extraRFrameFar.transform.localScale = sideScale;
            extraRFrameClose.transform.localScale = sideScale;

            float exFrameY = right[0, 0].transform.localScale.y / 2 + 0.075f;
            float frameRFarZ = right[numPalletsWide - 1, 0].transform.position.z;// + palWidth / 2;
            extraRFrameFar.transform.position = new Vector3(colXWithDeviation(ColumnSide.Right), exFrameY, frameRFarZ + beamDist / 2);
            float frameRCloseZ = right[0, 0].transform.position.z;// - palWidth / 2;
            //extraRFrameClose.transform.position = new Vector3(colXWithDeviation(ColumnSide.Right), exFrameY, frameRCloseZ);
            extraRFrameClose.transform.position = new Vector3(colXWithDeviation(ColumnSide.Right), exFrameY, frameRCloseZ - beamDist / 2);
        }

        if (left[0, 0] != null) {
            Vector3 sideScale = new Vector3(left[0, 0].transform.localScale.x, left[0, 0].transform.localScale.y + 0.15f, left[0, 0].transform.localScale.z);
            extraLFrameFar.transform.localScale = sideScale;
            extraLFrameClose.transform.localScale = sideScale;

            float exFrameY = left[0, 0].transform.localScale.y / 2 + 0.075f;
            float frameLFarZ = left[numPalletsWide - 1, 0].transform.position.z;// + palWidth / 2;
            extraLFrameFar.transform.position = new Vector3(colXWithDeviation(ColumnSide.Left), exFrameY, frameLFarZ + beamDist / 2);
            float frameLCloseZ = left[0, 0].transform.position.z;// - palWidth / 2;
            //extraLFrameClose.transform.position = new Vector3(colXWithDeviation(ColumnSide.Left), exFrameY, frameLCloseZ);
            extraLFrameClose.transform.position = new Vector3(colXWithDeviation(ColumnSide.Left), exFrameY, frameLCloseZ - beamDist / 2);
        }
    }


    private void createInitialPallets(GameObject columnObject) {
        left = new GameObject[maxColWidth, maxColHeight];
        right = new GameObject[maxColWidth, maxColHeight];

        //then move up the column placing pallets on either side of the imaginary column line
        for (int i = 0; i < numPalletsWide; i++) {
            //vertical pallets too
            for (int y = 0; y < numPalletsHigh; y++) {
                GameObject leftPallet = createPallet(ColumnSide.Left, i, y);
                leftPallet.transform.parent = columnObject.transform;
                left[i, y] = (leftPallet);
                GameObject rightPallet = createPallet(ColumnSide.Right, i, y);
                rightPallet.transform.parent = columnObject.transform;
                right[i, y] = (rightPallet);
            }
        }
    }

    private void createAisle(GameObject columnObject) {
        if (colNum == 0) return;

        Vector3 pos = new Vector3(colXWithDeviation(ColumnSide.Left) - 1.5f, .2f, numPalletsWide / 2);

        newAisleAtBase = (GameObject)UnityEngine.Object.Instantiate(aisleWhiteCarpet, pos, new Quaternion(0, 0, 0, 0));
        newAisleAtBase.transform.localScale = new Vector3(.21f, 1f, 1000);

        //newAisleAtBase.transform.parent = columnObject.transform;
        newAisleAtBase.renderer.material.mainTexture = aisleNormalTexture;
        Vector3 numberPos = new Vector3(colXWithDeviation(ColumnSide.Left) - 2.2f, .21f, numPalletsWide / 2 - 8);
        numberPlane = (GameObject)UnityEngine.Object.Instantiate(aisleNumberPlane, numberPos, new Quaternion(0, 90, 90, 0));
        numberPlane.transform.Rotate(0, 180, 0);
        //numberPlane.GetComponent<Renderer> ().sharedMaterial.shader = Shader.Find("GUI/Text Shader");

        numberPlane.GetComponent<TextMesh>().text = colNum.ToString();
        numberPlane.transform.parent = columnObject.transform;
    }


    private float colXWithDeviation(ColumnSide s) {
        float palletDepth = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        float adjColSpacing = parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
        float d = s == ColumnSide.Left ? getColXPos() - (palletDepth / 2 + (adjColSpacing / 2)) : getColXPos() + (palletDepth / 2 + (adjColSpacing / 2));
        return d;
    }


    public float getColXPos() {
        float adjColSpacing = parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
        float palletDepth = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        float aisleWidth = parameters.GetComponent<ParametersButton>().AisleWidth;
        return colNum * (adjColSpacing / 2 + palletDepth + aisleWidth);
    }


    public float getCustomColXPos(int i, ColumnSide s) {
        float adjColSpacing = parameters.GetComponent<ParametersButton>().AdjColumnSpacing;
        float palletDepth = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        float aisleWidth = parameters.GetComponent<ParametersButton>().AisleWidth;
        float cxp = i * (adjColSpacing / 2 + palletDepth + aisleWidth);
        float d = s == ColumnSide.Left ? cxp - (palletDepth / 2 + (adjColSpacing / 2)) : cxp + (palletDepth / 2 + (adjColSpacing / 2));
        return d;
    }


    public void refreshAllPalletAndFrameTransforms() {
        for (int z = 0; z < numPalletsWide; z++) {
            for (int y = 0; y < numPalletsHigh; y++) {
                GameObject leftPallet = left[z, y];
                if (leftPallet)
                    updatePalletTransform(leftPallet, ColumnSide.Left, y, z);

                GameObject rightPallet = right[z, y];
                if (rightPallet)
                    updatePalletTransform(rightPallet, ColumnSide.Right, y, z);
            }
        }
        updateAllFramesActivenessInCol();
    }

    private GameObject createPallet(ColumnSide sd, int z, int y) {
        vertBeamSpacing = 0.001f * parameters.GetComponent<ParametersButton>().VerticalBeamSpacing;

        //create pallet obj
        GameObject pal = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Pallet"));
        float palY = y * vertBeamSpacing;
        if (bottomBeamOn) palY += (0.001f * parameters.GetComponent<ParametersButton>().BottomBeamHeight);
        float beamLength = parameters.GetComponent<ParametersButton>().BeamLength * 0.001f;
        float palsBetweenFrames = parameters.GetComponent<ParametersButton>().PalletsBetweenFrames;
        float beamDist = beamLength / palsBetweenFrames;
        float palZ = baseZ + z * beamDist;
        Vector3 pos = new Vector3(colXWithDeviation(sd), palY, palZ);
        pal.transform.position = pos;
        pal.transform.parent = columnObject.transform;
        pal.GetComponent<PalletProperties>().Side = sd;
        pal.GetComponent<PalletProperties>().ColNum = colNum;
        pal.GetComponent<PalletProperties>().TargetScale = getParamScale();

        //create frames
        float distToPalEdge = (pal.transform.localScale.z) / 2;
        Vector3 sideScale = new Vector3(pal.transform.localScale.x, vertBeamSpacing + 0.15f, pal.transform.localScale.z);

        GameObject frameClose = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        frameClose.transform.position = new Vector3(pos.x, (pos.y + vertBeamSpacing / 2 + 0.12f), pos.z - beamDist);
        frameClose.transform.localScale = sideScale;
        frameClose.transform.parent = pal.transform;
        frameClose.name = "frameClose";

        GameObject frameFar = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        frameFar.transform.position = new Vector3(pos.x, (pos.y + vertBeamSpacing / 2 + 0.12f), pos.z + beamDist);
        frameFar.transform.localScale = sideScale;
        frameFar.transform.parent = pal.transform;
        frameFar.name = "frameFar";

        GameObject frameShelf = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameShelf"));
        frameShelf.transform.position = new Vector3(pos.x, pos.y - 0.06f, pos.z);
        frameShelf.transform.localScale = new Vector3(beamLength / palsBetweenFrames - 0.05f, pal.transform.localScale.y, pal.transform.localScale.z);
        frameShelf.transform.parent = pal.transform;
        frameShelf.name = "frameShelf";

        updateFrameActivenessOnPallet(pal, z);
        return pal;
    }

    private void updatePalletTransform(GameObject pal, ColumnSide sd, int y, int z) {
        vertBeamSpacing = 0.001f * parameters.GetComponent<ParametersButton>().VerticalBeamSpacing;
        float beamLength = parameters.GetComponent<ParametersButton>().BeamLength * 0.001f;
        float palsBetweenFrames = parameters.GetComponent<ParametersButton>().PalletsBetweenFrames;
        float palY = y * vertBeamSpacing;
        if (bottomBeamOn) palY += (0.001f * parameters.GetComponent<ParametersButton>().BottomBeamHeight);
        float palBeamLengthZ = beamLength / palsBetweenFrames;
        float palZ = baseZ + z * palBeamLengthZ;

        //pallet transform
        Vector3 pos = new Vector3(colXWithDeviation(sd), palY, palZ);
        pal.transform.position = pos;
        pal.GetComponent<PalletProperties>().TargetScale = getParamScale();
        pal.transform.parent = columnObject.transform;

        //update frames
        GameObject frameFar = null;
        GameObject frameClose = null;
        GameObject frameShelf = null;

        foreach (Transform frame in pal.transform) {
            if (frame.name == "frameClose")
                frameClose = frame.gameObject;
            else if (frame.name == "frameFar")
                frameFar = frame.gameObject;
            else if (frame.name == "frameShelf")
                frameShelf = frame.gameObject;
        }

        Vector3 sideScale = new Vector3(pal.transform.localScale.x, vertBeamSpacing + 0.15f, pal.transform.localScale.z);

        frameClose.transform.position = new Vector3(pos.x, (pos.y + vertBeamSpacing / 2 + 0.12f), pos.z - palBeamLengthZ / 2);
        frameClose.transform.localScale = sideScale;

        frameFar.transform.position = new Vector3(pos.x, (pos.y + vertBeamSpacing / 2 + 0.12f), pos.z + palBeamLengthZ / 2);
        frameFar.transform.localScale = sideScale;

        frameShelf.transform.position = new Vector3(pos.x, pos.y - 0.06f, pos.z);
        frameShelf.transform.localScale = new Vector3(beamLength / palsBetweenFrames / 2 - 0.05f, pal.transform.localScale.y, pal.transform.localScale.z);
    }


    public bool canAddPalletsOnEnd() {
        return numPalletsWide < maxColWidth;
    }

    public void addPalletsOnEnd(ColumnEnd c) {
        numPalletsWide++;
        //if close end then shift everything up by one
        if (c == ColumnEnd.Low) {
            for (int i = numPalletsWide - 1; i > 0; i--) {
                for (int j = 0; j < numPalletsHigh; j++) {
                    left[i, j] = left[i - 1, j];
                    right[i, j] = right[i - 1, j];
                }
            }
            baseZ -= parameters.GetComponent<ParametersButton>().PalletISO.getDepth(); ;
            //then add the new ones to the front
            for (int j = 0; j < numPalletsHigh; j++) {
                left[0, j] = createPallet(ColumnSide.Left, 0, j);
                right[0, j] = createPallet(ColumnSide.Right, 0, j);
            }
        }
        else {
            //otherwise just add them to the end.
            for (int j = 0; j < numPalletsHigh; j++) {
                left[numPalletsWide - 1, j] = createPallet(ColumnSide.Left, numPalletsWide - 1, j);
                right[numPalletsWide - 1, j] = createPallet(ColumnSide.Right, numPalletsWide - 1, j);
            }
        }
        updateAllFramesActivenessInCol();
    }

    public void DestroySelf() {
        GameObject.Destroy(newAisleAtBase);
        GameObject.Destroy(columnObject);
        GameObject.Destroy(colLowPoly);
    }

    public bool canDeletePalletsFromEnd() {
        return numPalletsWide > 1;
    }

    public void deletePalletsOnEnd(ColumnEnd end) {
        if (end == ColumnEnd.Low) {
            //destroy front ones
            for (int j = 0; j < numPalletsHigh; j++) {
                GameObject.Destroy(getPallet(ColumnSide.Left, 0, j));
                GameObject.Destroy(getPallet(ColumnSide.Right, 0, j));
            }
            //then shift everything down to keep the array tidy
            for (int i = 0; i < numPalletsWide - 1; i++) {
                for (int j = 0; j < numPalletsHigh; j++) {
                    left[i, j] = left[i + 1, j];
                    right[i, j] = right[i + 1, j];
                }
            }
        }
        else {
            //otherwise just destroy them.
            for (int j = 0; j < numPalletsHigh; j++) {
                GameObject.Destroy(getPallet(ColumnSide.Left, numPalletsWide - 1, j));
                GameObject.Destroy(getPallet(ColumnSide.Right, numPalletsWide - 1, j));
                left[numPalletsWide - 1, j] = null;
                right[numPalletsWide - 1, j] = null;
            }
        }

        numPalletsWide--;
        //updateAllFramesActivenessInCol();
        refreshAllPalletAndFrameTransforms();
    }

    public void updateAllFramesActivenessInCol() {
        for (int i = 0; i < numPalletsWide; i++) {
            for (int j = 0; j < numPalletsHigh; j++) {
                GameObject leftPal = getPallet(ColumnSide.Left, i, j);
                GameObject rightPal = getPallet(ColumnSide.Right, i, j);
                updateFrameActivenessOnPallet(leftPal, i);
                updateFrameActivenessOnPallet(rightPal, i);
            }
        }
        refreshBottomBarLegs();
    }

    private void updateFrameActivenessOnPallet(GameObject p, int x) {
        if (p != null) {
            GameObject frameFar = null;
            GameObject frameClose = null;

            foreach (Transform frame in p.transform) {
                if (frame.name == "frameClose")
                    frameClose = frame.gameObject;
                else if (frame.name == "frameFar")
                    frameFar = frame.gameObject;
            }

            frameFar.SetActive(false);
            frameClose.SetActive(false);

            //	Debug.Log (parameters.GetComponent<ParametersButton>().PalletsBetweenFrames);
            if ((x + 1) % parameters.GetComponent<ParametersButton>().PalletsBetweenFrames == 0)
                frameFar.SetActive(true);

            //column end frames always show
            if (x == 0)
                frameClose.SetActive(true);
            if (x == numPalletsWide - 1)
                frameFar.SetActive(true);
        }
    }

    public float getAvgZPos() {
        float minZ = baseZ;
        float maxZ = baseZ + numPalletsWide * parameters.GetComponent<ParametersButton>().PalletISO.getWidth();
        return (maxZ + minZ) / 2;
    }

    public float getAvgYPos() {
        float minY = 0;
        float maxY = 0;
        if (left[0, 0] != null) {
            minY = left[0, 0].transform.position.y;
            maxY = left[0, numPalletsHigh - 1].transform.position.y;
        }
        else {
            minY = right[0, 0].transform.position.y;
            maxY = right[0, numPalletsHigh - 1].transform.position.y;
        }
        return (maxY + minY) / 2 + parameters.GetComponent<ParametersButton>().VerticalBeamSpacing * 0.001f / 2;
    }

    public bool IsDying() {
        return isDying;
    }


    public bool canAddPalletsOnTop() {
        return numPalletsHigh < maxColHeight;
    }
    public void addPalletsOnTop() {
        numPalletsHigh++;

        for (int i = 0; i < numPalletsWide; i++) {
            if (colNum != 0) {
                GameObject leftPallet = createPallet(ColumnSide.Left, i, numPalletsHigh - 1);
                Left[i, numPalletsHigh - 1] = leftPallet;
            }
            if (colNum != parameters.GetComponent<ParametersButton>().NumAisles - 1) {
                GameObject rightPallet = createPallet(ColumnSide.Right, i, numPalletsHigh - 1);
                Right[i, numPalletsHigh - 1] = rightPallet;
            }
        }
    }

    public bool canDeletePalletsFromTop() {
        return numPalletsHigh > 1;
    }

    public void deletePalletsOnTop() {
        for (int i = 0; i < numPalletsWide; i++) {
            GameObject leftPallet = getPallet(ColumnSide.Left, i, numPalletsHigh - 1);
            GameObject rightPallet = getPallet(ColumnSide.Right, i, numPalletsHigh - 1);

            Left[i, numPalletsHigh - 1] = null;
            Right[i, numPalletsHigh - 1] = null;

            GameObject.Destroy(leftPallet);
            GameObject.Destroy(rightPallet);
        }
        numPalletsHigh--;
    }

    private Vector3 getParamScale() {
        float scX = parameters.GetComponent<ParametersButton>().PalletISO.getDepth();
        float scY = parameters.GetComponent<ParametersButton>().PalletHeight;
        float scZ = parameters.GetComponent<ParametersButton>().PalletISO.getWidth();
        return new Vector3(scX, scY, scZ);
    }

    public void destroyAll() {
        isDying = true;
        foreach (GameObject p in left)
            GameObject.Destroy(p);
        foreach (GameObject p in right)
            GameObject.Destroy(p);
    }

    //extra frames for when the bottom bar is active, so the pallets dont look like they are floating
    private void createExtraFrames() {
        extraLFrameClose = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        extraLFrameClose.transform.position = new Vector3(getColXPos(), 0, 0);
        extraLFrameClose.transform.localScale = new Vector3(1, 1, 1);
        extraLFrameClose.transform.parent = columnObject.transform;
        extraLFrameClose.name = "extraFrameClose";
        extraLFrameClose.SetActive(false);

        extraRFrameClose = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        extraRFrameClose.transform.position = new Vector3(getColXPos(), 0, 0);
        extraRFrameClose.transform.localScale = new Vector3(1, 1, 1);
        extraRFrameClose.transform.parent = columnObject.transform;
        extraRFrameClose.name = "extraFrameClose";
        extraRFrameClose.SetActive(false);

        extraLFrameFar = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        extraLFrameFar.transform.position = new Vector3(getColXPos(), 0, 0);
        extraLFrameFar.transform.localScale = new Vector3(1, 1, 1); ;
        extraLFrameFar.transform.parent = columnObject.transform;
        extraLFrameFar.name = "extraFrameFar";
        extraLFrameFar.SetActive(false);

        extraRFrameFar = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FrameMid"));
        extraRFrameFar.transform.position = new Vector3(getColXPos(), 0, 0);
        extraRFrameFar.transform.localScale = new Vector3(1, 1, 1); ;
        extraRFrameFar.transform.parent = columnObject.transform;
        extraRFrameFar.name = "extraFrameFar";
        extraRFrameFar.SetActive(false);
    }

    //Getters/Setters
    public GameObject getPallet(ColumnSide c, int x, int y) {
        if (left[x, y] != null && c == ColumnSide.Left)
            return left[x, y];
        else
            return right[x, y];
    }
    public int NumPalletsWide {
        get { return numPalletsWide; }
        set { numPalletsWide = value; }
    }
    public int NumPalletsHigh {
        get { return numPalletsHigh; }
        set { numPalletsHigh = value; }
    }

    public GameObject[,] Left {
        get { return left; }
        set { left = value; }
    }
    public GameObject[,] Right {
        get { return right; }
        set { right = value; }
    }
}