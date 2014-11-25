using UnityEngine;
using System.Text.RegularExpressions;
enum Mode { Default, Range, List };
public enum Location { none, aisleLocation, bayLocation, levelsLocation, verticalBeamSpacingLocation, bottomBeamHeightLocation, inboundOutboundStationLocation, pickUpLocation, dropOffLocation, pAndDLevelsLocation, pAndDWidthLocation, topLocation, sideLocation }
public enum InboundOutboundStation {sameEnd, oppositeEnds};
public enum PickUpDropOffLocation { dock, pAndD};

public class ParametersButton : MonoBehaviour {
    public Location currentCameraLocation;

    public GameObject factoryBuilder;

    public HighlightController highlightController;


    //Scroll Menu
    //public Vector2 scrollPosition = Vector2.zero;
    private int listSizes;
    private int compare;
    private int isoCount = 0;

	private bool SwitchFromTopOrSideToIso = false;

    //	GUIContent[] isoList = new GUIContent[5];
    ComboBox isoComboBoxControl;
    //	public GUIStyle listStyle;

    //string versions for storing + editing input box vals. Also default values.
    private string s_floorX = "3";
    private string s_floorZ = "2";
    private string s_numAisles = "4";
    private string s_numPalletsWide = "8";
    private string s_numPalletsHigh = "5";
    private string s_palletHeight = "1.1";
    private string s_palletsInColumn = "0";
    private string s_palletHeightFromGround = "0";
    private string s_aisleWidth = "3.1";
    private string s_palletColumnWidth = "0";
    private string s_adjColumnSpacing = "0.1";
    private string s_palletOverhang = "0";
    private string s_palletsBetweenFrames = "2";

    // DONT EDIT THESE VALUES. USE THE WRAPPERS OR THE STRINGS WONT UPDATE!
    //float versions because I dont want to keep writing float.Parse(x) in the calculations.
    private float floorX = 0;
    private float floorZ = 0;
    //private float numColumns = 0;
    private float numPalletsWide = 0;
    private float numPalletsHigh = 0;
    private float palletHeight = 0;
    private float palletsInColumn = 0;
    private float palletHeightFromGround = 0;
    private float aisleWidth = 0;
    private float palletColumnWidth = 0;
    private float adjColumnSpacing = 0;
    private float palletOverhang = 0;
    //private float palletsPer = 0;
    private int palletsBetweenFrames = 0;
    private ISO palletISO;

    //enables box editing by preventing the input box from auto-filling
    private bool editfloorX = true;
    private bool editfloorZ = true;
    private bool editnumPalletsWide = false;
    private bool editnumPalletsHigh = false;
    private bool editpalletHeight = false;
    private bool editpalletsInColumn = false;
    private bool editpalletHeightFromGround = false;
    private bool editpalletWidth = false;
    private bool editaisleWidth = false;
    private bool editpalletColumnWidth = false;
    private bool editadjColumnSpacing = false;
    private bool editpalletOverhang = false;


    public GameObject floor;

    //public Texture parametersButtonTexture;

    // LUKAS' IMPLEMENTATION
    public Texture crownLogo;
    private Mode currentMode = Mode.Default;
    private string currentRangeSubmenu;
    private int currentMin;
    private int currentMax;
    private int currentIncrement;
    private int currentInput;
    private string currentInputString = "Enter a Number";
    private int[] currentIntArray;

    private int sidebarWidth;// = 500;
    private int elementHeight;// = 70;
    private int sectionHeight;// = 50;
    private int logoWidth;// = 150;
    private int rightMenubuttonheight;

    public GUIStyle pageGUIStyle;
    public GUIStyle mainSectionHeadingsStyle;
    public GUIStyle elementStyle;
    public GUIStyle rangeButtonStyle;
    public GUIStyle toggleButtonStyle;
    public GUIStyle sectionHeadingStyle;
    public GUIStyle disabledElementStyle;
    public GUIStyle disabledRangeButtonStyle;
    public GUIStyle inputNumberStyle;
    public GUIStyle doneButtonStyle;
    public GUIStyle rangeStyle;
    public GUIStyle disabledRangeStyle;
	public GUIStyle selectedRangeStyle;
    public GUIStyle staticStyle;
    public GUIStyle touchToggleStyle;
    public GUIContent touchToggleContent;
    public GUIStyle ViewButtonStye;

    public Texture optionsTextureOn;
    public Texture optionsTextureOff;
    public Texture orangeBack;
    public Texture blackBack;
    public Texture greyBack;
	public Texture whiteBack;
    public Texture toggleOn;
    public Texture toggleOff;
    public Texture toggleOn2;
    public Texture toggleOff2;
    public Texture iso;
    public Texture top;
    public Texture side;
    public Texture viewBackground;

    private GUIContent[] pageHeadings;
    private GUIContent[] viewButtonsContent;
    private string[] domainSectionHeadings = new string[] { "Warehouse", "Operations", "Trucks" };

    private int selectedPage = 1;
    public int viewPerspective = 0; // 0 is normal(iso), 1 is top (ortho), 2 is front (ortho)
    private int selectedDomainSection = 0;

    private bool firstLevelBeamPresent = false;
    private int NumberOfAisles = 4;
    private int numberOfBaysDownAisle = 4;

    
    private int NumberOfLevels = 5;
    private int verticalBeamSpacing = 1600;
    private int bottomBeamHeight = 500;
    private int beamLength = 2700;

    private int pAndDLevels = 3;
    private int pAndDBeamsAcross = 1;

  
    
    private float ClearAisleWidth = 1901;

    InboundOutboundStation iOStation = InboundOutboundStation.sameEnd;

   

  
   
    PickUpDropOffLocation pickUpLocation = PickUpDropOffLocation.dock;

    
    PickUpDropOffLocation dropOffLocation = PickUpDropOffLocation.dock;

    
    // gets incremented every time I add a button, so we know where to draw next one
    private int yPos;

    // OUTPUT BOX STUFF
    public GUIStyle outputHeadingStyle;
    public GUIStyle outputValueStyle;
    public GUIStyle moreButtonStyle;
    private int rightYPos;
    private float outputWidth;// = 300;
    private int outputValueHeight;// = 110;
    private bool moreOptions = false;


    public Transform isoCameraPosition;
    public Camera UICamera;
    public Camera arrowCamera;
    public bool orthView = false;


    // scroll stuff
    private int selected = -1;
    private float scrollVelocity = 0f;
    private float timeTouchPhaseEnded = 0f;

    public Vector2 scrollPosition;

    public float inertiaDuration = 0.75f;
    // size of the window and scrollable list
    public int numRows;
    public Vector2 rowSize;
    public Vector2 windowMargin;
    public Vector2 listMargin;

    private Rect windowRect;   // calculated bounds of the window that holds the scrolling list
    private Vector2 listSize;  // calculated dimensions of the scrolling list placed inside the window
    // END SCROLL STUFF

    // view buttons
    public Transform topViewPoint;
    public Transform sideViewPoint;

    // hidden logo stats box
    private bool showStats = false;
    private bool enableTouchControls = false;



    // Use this for initialization
    void Start() {

        // scale ui according to screen size
        SetGUIScaling();

        // initialize section heading strings and pallet object
        pageHeadings = new GUIContent[] { new GUIContent(optionsTextureOn), new GUIContent("Inputs"), new GUIContent("Report") };
        viewButtonsContent = new GUIContent[] { new GUIContent(iso), new GUIContent(top), new GUIContent(side) };
        palletISO = new ISO("nothing", 1, 1);

        highlightController = new HighlightController();
    }

    void SetGUIScaling() {
        // Test change elements width and height for all resolution
        sidebarWidth = Screen.width / 4;
        elementHeight = Screen.height / 20;
        sectionHeight = Screen.height / 20;
        outputWidth = Screen.width / 5.5f;
        outputValueHeight = Screen.width / 15;

        // GUIStyle font size chagne base on 
        if (Screen.width <= 1024) {
            rangeButtonStyle.fontSize = 22 / 2;
            rangeButtonStyle.padding.right = 40 / 2;
            //	toggleButtonStyle.padding.right = 20/2;
            pageGUIStyle.fontSize = 24 / 2;
            mainSectionHeadingsStyle.fontSize = 22 / 2;
            elementStyle.fontSize = 22 / 2;
            elementStyle.padding.left = 30 / 2;
            rangeStyle.fontSize = 22 / 2;
            sectionHeadingStyle.fontSize = 16 / 2;
            sectionHeadingStyle.padding.left = 30 / 2;
            disabledElementStyle.fontSize = 22 / 2;
            disabledElementStyle.padding.left = 30 / 2;
            disabledRangeButtonStyle.fontSize = 22 / 2;
            disabledRangeButtonStyle.padding.right = 40 / 2;
            inputNumberStyle.fontSize = 22 / 2;
            doneButtonStyle.fontSize = 24 / 2;
            doneButtonStyle.padding.right = 30 / 2;
            rangeStyle.fontSize = 22 / 2;
			selectedRangeStyle.fontSize = 22 / 2;
            disabledRangeStyle.fontSize = 22 / 2;
            staticStyle.fontSize = 22 / 2;
            staticStyle.padding.top = 8 / 2;
            outputValueStyle.fontSize = 44 / 2;
            outputHeadingStyle.fontSize = 22 / 2;
            rightMenubuttonheight = 50 / 2;
            touchToggleStyle.fontSize = 22 / 2;
            logoWidth = 150 / 2;

        }
        else if (Screen.width > 1024) {
            rangeButtonStyle.fontSize = 22;
            rangeButtonStyle.padding.right = 40;
            //	toggleButtonStyle.padding.right = 20;
            pageGUIStyle.fontSize = 24;
            mainSectionHeadingsStyle.fontSize = 22;
            elementStyle.fontSize = 22;
            elementStyle.padding.left = 30;
            rangeStyle.fontSize = 22;
            sectionHeadingStyle.fontSize = 16;
            sectionHeadingStyle.padding.left = 30;
            disabledElementStyle.fontSize = 22;
            disabledElementStyle.padding.left = 30;
            disabledRangeButtonStyle.fontSize = 22;
            disabledRangeButtonStyle.padding.right = 40;
            inputNumberStyle.fontSize = 22;
            doneButtonStyle.fontSize = 24;
            doneButtonStyle.padding.right = 30;
            rangeStyle.fontSize = 22;
			selectedRangeStyle.fontSize = 22;
            disabledRangeStyle.fontSize = 22;
            staticStyle.fontSize = 22;
            staticStyle.padding.top = 8;
            outputValueStyle.fontSize = 44;
            outputHeadingStyle.fontSize = 22;
            rightMenubuttonheight = 50;
            touchToggleStyle.fontSize = 22;
            logoWidth = 150;
        }
    }


    // Update is called once per frame
    void Update() {



        if (currentCameraLocation == Location.topLocation || currentCameraLocation == Location.sideLocation) {
            Camera.main.orthographic = true;
            UICamera.orthographic = true;
            arrowCamera.orthographic = true;
        }
        else {
            Camera.main.orthographic = false;
            UICamera.orthographic = false;
            arrowCamera.orthographic = false;
        }

        if (!editfloorX) s_floorX = floor.transform.localScale.x.ToString(); //if the input box isnt being used, fill it with the values from the floor object
        if (!editfloorZ) s_floorZ = floor.transform.localScale.z.ToString();
        convertInputStringsToFloats();

		//Debug.Log ("touchCount == "+ Input.touchCount);
//		if (Input.touchCount == 1) {
//			print ("1");
//		}
//		if (Input.touchCount == 2) {
//			print ("2");
//		}

        // scroll
        if (Input.touchCount != 1) {
			//print ("touchCount != 1, touchCount == "+ Input.touchCount);
            selected = -1;

            if (scrollVelocity != 0.0f) {
                // slow down over time
                float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
                if (scrollPosition.y <= 0 || scrollPosition.y >= (numRows * rowSize.y - listSize.y)) {
                    // bounce back if top or bottom reached
                   // scrollVelocity = -scrollVelocity / 2;
                }

                float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
                scrollPosition.y += frameVelocity * Time.deltaTime;

                // after N seconds, we've stopped
                if (t >= 1.0f) scrollVelocity = 0.0f;
            }
            return;
        }
        Touch touch = Input.touches[0];
        bool fInsideList = IsTouchInsideList(touch.position);

		// touch has began touching list - called once
        if (touch.phase == TouchPhase.Began && fInsideList) {;
            scrollVelocity = 0.0f;
        }
		// touch has gone off of the list - called constantly
        else if (touch.phase == TouchPhase.Canceled || !fInsideList) {
			selected = -1;
        }
        else if (touch.phase == TouchPhase.Moved && fInsideList) {
            // dragging
		//	print ("touch phase is dragging");
            selected = -1;
			scrollVelocity = (int)(touch.deltaPosition.y / touch.deltaTime);
            scrollPosition.y += touch.deltaPosition.y;
        }
		else if (touch.phase == TouchPhase.Ended) {
            // Was it a tap, or a drag-release?
            if (selected > -1 && fInsideList) {
				selected = TouchToRowIndex(touch.position);
//                Debug.Log("Player selected row " + selected);
            }
            else {
                // impart momentum, using last delta as the starting velocity
                // ignore delta < 10; precision issues can cause ultra-high velocity
                if (Mathf.Abs(touch.deltaPosition.y) >= 10)
                    scrollVelocity = (int)(touch.deltaPosition.y / touch.deltaTime);

                timeTouchPhaseEnded = Time.time;
            }
        }
        // end scroll
    }

    void DrawTitleBar() {
        GUI.DrawTexture(new Rect(0, yPos, Screen.width, elementHeight), blackBack);
        GUI.Label(new Rect(Screen.width / 2 - (sidebarWidth / 2), yPos, sidebarWidth, elementHeight), "TSP  Estimator Visualisation Tool", pageGUIStyle);
    }
    void OnGUI() {

        windowRect = new Rect(0, 0, sidebarWidth, Screen.height + elementHeight);
        listSize = new Vector2(windowRect.width, windowRect.height);


        yPos = 0;
        detectFocusChange();

      //  DrawTitleBar();


       // selectedPage = GUI.SelectionGrid(new Rect(0, yPos, sidebarWidth, elementHeight), selectedPage, pageHeadings, 3, pageGUIStyle);

        yPos += elementHeight;
        if (selectedPage == 0) {
            pageHeadings = new GUIContent[] { new GUIContent(optionsTextureOn), new GUIContent("Inputs"), new GUIContent("Report") };
        }
        else if (selectedPage == 1) {
            pageHeadings = new GUIContent[] { new GUIContent(optionsTextureOff), new GUIContent("Inputs"), new GUIContent("Report") };
       //     showDomainBoxes();
      //      showLeftMenu();

        }
        else if (selectedPage == 2) {
            pageHeadings = new GUIContent[] { new GUIContent(optionsTextureOff), new GUIContent("Inputs"), new GUIContent("Report") };
        }

        //output box
 //       showRightOutput();

        //View button

        //if (showStats) {
        //    GUI.DrawTexture(new Rect(Screen.width - outputWidth, Screen.height - ((elementHeight / 2f) + elementHeight * 1.3f + (elementHeight * 2)), outputWidth, elementHeight / 2), viewBackground);
        //    viewPerspective = GUI.SelectionGrid(new Rect(Screen.width - outputWidth, Screen.height - ((elementHeight / 2f) + elementHeight * 1.3f + (elementHeight * 2)), outputWidth, elementHeight / 2), viewPerspective, viewButtonsContent, 3, ViewButtonStye);
        //}
        //else {
        //    GUI.DrawTexture(new Rect(Screen.width - outputWidth, Screen.height - ((elementHeight / 2f) + elementHeight * 1.3f), outputWidth, elementHeight / 2), viewBackground);
        //    viewPerspective = GUI.SelectionGrid(new Rect(Screen.width - outputWidth, Screen.height - ((elementHeight / 2f) + elementHeight * 1.3f), outputWidth, elementHeight / 2), viewPerspective, viewButtonsContent, 3, ViewButtonStye);
        //}


        yPos += elementHeight;

        // iso
        if (viewPerspective == 0) {
			if(!SwitchFromTopOrSideToIso){
				currentCameraLocation = Location.none;
				SwitchFromTopOrSideToIso = true;
			}
            //sideCount = 0;
            //topCount = 0;
            viewButtonsContent = new GUIContent[] { new GUIContent(iso), GUIContent.none, GUIContent.none };
            if (isoCount == 0) {

                Camera.main.GetComponent<MoveCamera>().disableTouches = false;
                Camera.main.transform.position = isoCameraPosition.transform.position;
                isoCount++;
            }
        }
        // top
        else if (viewPerspective == 1)
        {
            currentCameraLocation = Location.topLocation;
			SwitchFromTopOrSideToIso = false;
            isoCount = 0;
            //   sideCount = 0;
            viewButtonsContent = new GUIContent[] { GUIContent.none, new GUIContent(top), GUIContent.none };
      //      if (topCount == 0)
            {
                Camera.main.GetComponent<MoveCamera>().disableTouches = true;	// No one finger move touch



                float temp = ((NumAisles * (BeamLength / 1000) - 1) * (NumberOfBaysDownAisle / 2)) / 1 + 1;
                if (temp < 5)
                {
                    temp = 5;
                }
                Camera.main.orthographicSize = temp;
                UICamera.orthographicSize = temp;
                arrowCamera.orthographicSize = temp;
                Camera.main.transform.position = topViewPoint.position;
                Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);

    //            topCount++;
            }
        }
        // side
        else if (viewPerspective == 2)
        {
            currentCameraLocation = Location.sideLocation;
			SwitchFromTopOrSideToIso = false;
            isoCount = 0;
      //      topCount = 0;
            viewButtonsContent = new GUIContent[] { GUIContent.none, GUIContent.none, new GUIContent(side) };
            //     if (sideCount == 0)
            {
				Camera.main.GetComponent<MoveCamera>().disableTouches = true;	// No one finger move touch
                float tempWide = ((NumAisles - 1) * (numberOfBaysDownAisle / 2)) / 1;
                if (tempWide < 5)
                {
                    tempWide = 5;
                }

                float tempHigh = NumberOfLevels * VerticalBeamSpacing / 1000 + 2;
                if (tempHigh < 5)
                {
                    tempHigh = 5;
                }

                if (tempHigh > tempWide) tempWide = tempHigh;

                Camera.main.orthographicSize = tempWide;
                UICamera.orthographicSize = tempWide;
                arrowCamera.orthographicSize = tempWide;
                Camera.main.transform.position = sideViewPoint.position;
                Camera.main.transform.localEulerAngles = sideViewPoint.localEulerAngles;

                //        sideCount++;
            }
        }
        // bottom bar
		bottomBar ();
        
    }

	private void bottomBar(){
		if (GUI.Button(new Rect(Screen.width - logoWidth, Screen.height - elementHeight / 1.15f, logoWidth, elementHeight), crownLogo, GUIStyle.none)) {
			showStats = !showStats;
			if (showStats) factoryBuilder.GetComponent<FactoryBuilder>().calculateTotalNumberOfPallets();
		}
		
		if (showStats) {
			GUI.Label(new Rect(Screen.width - outputWidth, Screen.height - elementHeight * 3, outputWidth, elementHeight), "FPS: " + 1 / Time.smoothDeltaTime + "\nTotal Pallets: " + factoryBuilder.GetComponent<FactoryBuilder>().getTotalNumberOfPallets(), staticStyle);
			GUI.Label(new Rect(Screen.width - outputWidth, Screen.height - elementHeight * 2, outputWidth, elementHeight), "", touchToggleStyle);
			if (enableTouchControls) {
				touchToggleContent.image = toggleOn2;
			}
			else {
				touchToggleContent.image = toggleOff2;
			}
			//enableTouchControls = GUI.Toggle(new Rect(Screen.width - outputWidth, Screen.height - elementHeight * 2, outputWidth, elementHeight), enableTouchControls, touchToggleContent, touchToggleStyle);
		}
	}
    private void showRightOutput() {
        rightYPos = elementHeight;
        GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Trucks Required", outputHeadingStyle);
        rightYPos += sectionHeight;

        GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "8.5", outputValueStyle);
        rightYPos += outputValueHeight;

        GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 1), greyBack);
        rightYPos += 1;

        GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Battery Hours", outputHeadingStyle);
        rightYPos += sectionHeight;

        GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "8.4", outputValueStyle);
        rightYPos += outputValueHeight;

        GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 2), greyBack);
        rightYPos += 2;

        if (!moreOptions) {
            if (GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, rightMenubuttonheight), "More...", moreButtonStyle)) {
                moreOptions = true;
            }
        }
        else {
            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Operational Cost Per \nPutaway Load", outputHeadingStyle);
            rightYPos += sectionHeight;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "$0.84", outputValueStyle);
            rightYPos += outputValueHeight;

            GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 1), greyBack);
            rightYPos += 1;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Operational Cost Per \nRetrieval Load", outputHeadingStyle);
            rightYPos += sectionHeight;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "$0.83", outputValueStyle);
            rightYPos += outputValueHeight;

            GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 2), greyBack);
            rightYPos += 2;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Putaways Per Hour \nPer Truck", outputHeadingStyle);
            rightYPos += sectionHeight;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "24.4", outputValueStyle);
            rightYPos += outputValueHeight;

            GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 2), greyBack);
            rightYPos += 2;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, sectionHeight), "Est. Retrievals Per Hour \nPer Truck", outputHeadingStyle);
            rightYPos += sectionHeight;

            GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, outputValueHeight), "24.6", outputValueStyle);
            rightYPos += outputValueHeight;

            GUI.DrawTexture(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, 2), greyBack);
            rightYPos += 2;

            if (GUI.Button(new Rect(Screen.width - outputWidth, rightYPos, outputWidth, rightMenubuttonheight), "Less...", moreButtonStyle)) {
                moreOptions = false;
            }
        }
    }

    private void showLeftMenu() {
        // clicked warehouse
        if (selectedDomainSection == 0) {
            if (currentMode == Mode.Default) {
                ShowWarehouseWindow();
            }
            else if (currentMode == Mode.Range) {
                DoRangeWindow();
            }

			else if (currentMode == Mode.List) {
				DoListWindow();
			}
        }
        // clicked operations
		if (selectedDomainSection == 1) {
			GUI.DrawTexture(new Rect(0, elementHeight*2, sidebarWidth, Screen.height), whiteBack);
		}
		// clicked trucks
		if (selectedDomainSection == 2) {
			GUI.DrawTexture(new Rect(0, elementHeight*2, sidebarWidth, Screen.height), whiteBack);
		}
    }
	
    private void showDomainBoxes() {
        // domain selection (warehouse, operations, report)
        GUI.DrawTexture(new Rect(0, yPos, sidebarWidth, elementHeight), orangeBack);
        selectedDomainSection = GUI.SelectionGrid(new Rect(0, yPos, sidebarWidth, elementHeight), selectedDomainSection, domainSectionHeadings, 3, mainSectionHeadingsStyle);
        yPos += elementHeight;
    }

    private void detectFocusChange() {
        string focus = GUI.GetNameOfFocusedControl();
        editfloorX = (focus == "floorX");
        editfloorZ = (focus == "floorZ");
        editnumPalletsHigh = (focus == "numPalletsHigh");
        editnumPalletsWide = (focus == "numPalletsWide");
        editpalletWidth = (focus == "palletWidth");
        editpalletHeight = (focus == "palletHeight");
        editpalletHeightFromGround = (focus == "palletHeightFromGround");
        editaisleWidth = (focus == "aisleWidth");
        editpalletColumnWidth = (focus == "palletColumnWidth");
        editadjColumnSpacing = (focus == "adjColumnSpacing");
        editpalletOverhang = (focus == "palletOverhang");
    }

    private void convertInputStringsToFloats() {
        floorX = 0;
        float.TryParse(s_floorX, out floorX);
        floorZ = 0;
        float.TryParse(s_floorZ, out floorZ);
        // numColumns = 0;
        // float.TryParse(s_numColumns, out numColumns);
        numPalletsWide = 0;
        float.TryParse(s_numPalletsWide, out numPalletsWide);
        numPalletsHigh = 0;
        float.TryParse(s_numPalletsHigh, out numPalletsHigh);
        palletHeight = 0;
        float.TryParse(s_palletHeight, out palletHeight);
        palletsInColumn = 0;
        float.TryParse(s_palletsInColumn, out palletsInColumn);
        palletHeightFromGround = 0;
        float.TryParse(s_palletHeightFromGround, out palletHeightFromGround);
        aisleWidth = 0;
        float.TryParse(s_aisleWidth, out aisleWidth);
        palletColumnWidth = 0;
        float.TryParse(s_palletColumnWidth, out palletColumnWidth);
        adjColumnSpacing = 0;
        float.TryParse(s_adjColumnSpacing, out adjColumnSpacing);
        palletOverhang = 0;
        float.TryParse(s_palletOverhang, out palletOverhang);
        palletsBetweenFrames = 0;
        int.TryParse(s_palletsBetweenFrames, out palletsBetweenFrames);
    }

    private string removeLetters(string mystring) {
        var temp = "";
        foreach (char c in mystring) {
            if (char.IsDigit(c) || c == '.') {
                temp += c;
            }
        }
        return temp;
    }

    void ShowWarehouseWindow() {
        numRows = 21;

        Rect rScrollFrame = new Rect(0, yPos, sidebarWidth, Screen.height - (elementHeight * 2));
        Rect rList = new Rect(0, 0, rowSize.x, numRows * elementHeight);	// why -60 cuz 3 of the rows is sectionHeight, each is 20 less than element

        scrollPosition = GUI.BeginScrollView(rScrollFrame, scrollPosition, rList, GUIStyle.none, GUIStyle.none);
        //	scrollPosition = GUI.BeginScrollView (rScrollFrame, scrollPosition, rList, true,true);

        int mainYPos = 0;
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, sectionHeight), "WAREHOUSE SIZE", sectionHeadingStyle);
        mainYPos += sectionHeight;

        // Warehouse menu
        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Number of Aisles", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), (NumberOfAisles - 1).ToString(), rangeButtonStyle)) {
        //    viewPerspective = 0;

            highlightController.SetHighlightArea(highlightSection.NumberOfAisles);
            currentCameraLocation = Location.aisleLocation;

            currentRangeSubmenu = "Number of Aisles";
            currentMin = 2;
            currentMax = 99;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;

        }

        mainYPos += elementHeight;
        // Warehouse menu
        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Number of Bays Down Aisle", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), numberOfBaysDownAisle.ToString(), rangeButtonStyle)) {
            //viewPerspective = 0;

            highlightController.SetHighlightArea(highlightSection.NumberOfBaysDownAisle);
            currentCameraLocation = Location.bayLocation;

            currentRangeSubmenu = "Number of Bays Down Aisle";
            currentMin = 2;	// Should change to have 1 when it is fix
            currentMax = 25;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;

        }
        mainYPos += elementHeight;
        // Warehouse menu
        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Number of Levels", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), NumPalletsHigh.ToString(), rangeButtonStyle)) {
            //viewPerspective = 0;

            highlightController.SetHighlightArea(highlightSection.NumberOfLevels);
            currentCameraLocation = Location.levelsLocation;

            currentRangeSubmenu = "Number of Levels";
            currentMin = 1;
            currentMax = 17;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;
        }
        mainYPos += elementHeight;

        GUI.DrawTexture(new Rect(0, mainYPos, sidebarWidth, 1), greyBack);
        mainYPos += 1;

        // NEW SECTION
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, sectionHeight), "RACKING", sectionHeadingStyle);
        mainYPos += sectionHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Vertical Beam Spacing", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), verticalBeamSpacing.ToString() + "mm", rangeButtonStyle)) {
            //viewSelect = 0;

            highlightController.SetHighlightArea(highlightSection.VerticalBeamSpacing);
            currentCameraLocation = Location.verticalBeamSpacingLocation;

            currentRangeSubmenu = "Vertical Beam Spacing";
            currentMin = 1000;
            currentMax = 3000;
            currentIncrement = 50;
            currentMode = Mode.Range;
            numRows = ((currentMax - currentMin) / currentIncrement) + 1;

        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Bottom Beam Present", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), firstLevelBeamPresent ? toggleOn : toggleOff, toggleButtonStyle)) {
            firstLevelBeamPresent = !firstLevelBeamPresent;
            factoryBuilder.GetComponent<FactoryBuilder>().setBottomBarOn(firstLevelBeamPresent);
            currentCameraLocation = Location.bottomBeamHeightLocation;
            if (!firstLevelBeamPresent)
            {
                highlightController.SetHighlightArea(highlightSection.none);
            }
            else
            {
                currentCameraLocation = Location.bottomBeamHeightLocation;
                highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
            }
            
        }
        if (firstLevelBeamPresent) {
            mainYPos += elementHeight;
            GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Bottom Beam Height", elementStyle);
            //            GUI.Button(new Rect(250, yPos, sidebarWidth / 2, elementHeight), "500mm", rangeButtonStyle);

            if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), bottomBeamHeight.ToString() + "mm", rangeButtonStyle)) {
                currentCameraLocation = Location.bottomBeamHeightLocation;
				highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
                currentRangeSubmenu = "Bottom Beam Height";
                currentMin = 200;
                currentMax = 1000;
                currentIncrement = 50;
                currentMode = Mode.Range;
                numRows = ((currentMax - currentMin) / currentIncrement) + 1;
            }
            mainYPos += elementHeight;
        }
        else {
            mainYPos += elementHeight;
            GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Bottom Beam Height", disabledElementStyle);
            GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), bottomBeamHeight.ToString() + "mm", disabledRangeButtonStyle);
            mainYPos += elementHeight;
        }

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Beam Length", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), beamLength.ToString() + "mm", rangeButtonStyle)) {
            highlightController.SetHighlightArea(highlightSection.BeamLength);
            currentCameraLocation = Location.verticalBeamSpacingLocation;
            currentRangeSubmenu = "Beam Length";
            currentMin = 1200;
            currentMax = 4000;
            currentIncrement = 50;
            currentMode = Mode.Range;
            numRows = ((currentMax - currentMin) / currentIncrement) + 1;
        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "No. of Pallets per Beam", elementStyle);

        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), palletsBetweenFrames.ToString(), rangeButtonStyle)) {
            currentCameraLocation = Location.verticalBeamSpacingLocation;
            currentRangeSubmenu = "No. of Pallets per Beam";
            currentMin = 1;
            currentMax = 4;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;
 
            Invoke("refreshPalletsPerBeamHighlighting", 1f);
        }
        mainYPos += elementHeight;


        // NEW SECTION
        GUI.DrawTexture(new Rect(0, mainYPos, sidebarWidth, 1), greyBack);
        mainYPos += 1;

        GUI.Button(new Rect(0, mainYPos, sidebarWidth, sectionHeight), "AISLES", sectionHeadingStyle);
        mainYPos += sectionHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Clear Aisle Width", disabledElementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), ClearAisleWidth.ToString() + "mm", disabledRangeButtonStyle)) {
            //currentOptionString = "Clear Aisle Width";
            //currentMin = 50;
            //currentMax = 2000;
            //currentIncrement = 50;
            //currentMode = Mode.Range;
        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Frame Width", disabledElementStyle);
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), "2700mm", disabledRangeButtonStyle);
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Row Spacer", disabledElementStyle);
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Pallet Overhand", disabledRangeButtonStyle);
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Pallet Overhang", disabledElementStyle);
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), "5mm", disabledRangeButtonStyle);
        mainYPos += elementHeight;

        // NEW SECTION
        GUI.DrawTexture(new Rect(0, mainYPos, sidebarWidth, 1), greyBack);
        mainYPos += 1;
        GUI.Button(new Rect(0, mainYPos, sidebarWidth, sectionHeight), "P&D", sectionHeadingStyle);
        mainYPos += sectionHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Inbound and Outbound Station", elementStyle);
		if(GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), IOStationToString , rangeButtonStyle)){
			currentCameraLocation = Location.inboundOutboundStationLocation;
			currentRangeSubmenu = "Inbound and Outbound Station";
			currentMode = Mode.List;
			numRows = 2;

            Invoke("refreshInboundOutboundHighlighting", .2f);
		}
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Pick Up Location for Putaway", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), PickUpLocationString, rangeButtonStyle))
        {
            currentCameraLocation = Location.pickUpLocation;
            currentRangeSubmenu = "Pick Up Location for Putaway";
            currentMode = Mode.List;
            numRows = 2;

            Invoke("refreshPickUpHighlighting", .2f);
        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "Drop Off Locatiion for Retrieval", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), DropOffLocationString, rangeButtonStyle))
        {
            currentCameraLocation = Location.dropOffLocation;
            currentRangeSubmenu = "Drop Off Location for Retrieval";
            currentMode = Mode.List;
            numRows = 2;

            Invoke("refreshDropOffHighlighting", .2f);
        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "No. of P&D Beams Across", elementStyle);
        if(GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), PAndDBeamsAcross.ToString(), rangeButtonStyle)){
            currentCameraLocation = Location.pAndDWidthLocation;
            currentRangeSubmenu = "No. of P&D Beams Across";
            currentMin = 1;
            currentMax = NumberOfBaysDownAisle;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;

            Invoke("refreshPAndDBeamsAcrossHighlighting", .2f);
        }
        mainYPos += elementHeight;

        GUI.Label(new Rect(0, mainYPos, sidebarWidth, elementHeight), "No. of P&D Levels", elementStyle);
        if (GUI.Button(new Rect(0, mainYPos, sidebarWidth, elementHeight), pAndDLevels.ToString(), rangeButtonStyle))
        {
            currentCameraLocation = Location.pAndDLevelsLocation;
            currentRangeSubmenu = "No. of P&D Levels";
            currentMin = 1;
            currentMax = NumberOfLevels;
            currentIncrement = 1;
            currentMode = Mode.Range;
            numRows = currentMax - currentMin + 1;

            Invoke("refreshPAndDLevelsHighlighting", .2f);
        }
        mainYPos += elementHeight;

        GUI.EndScrollView();
    }

	void DoListWindow() {
		int newYPos = elementHeight * 2;
		GUI.Label(new Rect(0, newYPos, sidebarWidth, elementHeight), currentRangeSubmenu, mainSectionHeadingsStyle);

        if (GUI.Button(new Rect(sidebarWidth / 2 + sidebarWidth / 4, newYPos, sidebarWidth / 4, elementHeight), "Done", doneButtonStyle))
        {
            // resets camera back to being narrow (cutting off where the menu starts)
            Camera.main.rect = new Rect(.25f, -0.05f, 1, 1);

            currentMode = Mode.Default;

            // return if didnt change input box
            if (currentInputString == "Enter a Number")
            {
                currentMode = Mode.Default;
                return;
            }

            // return if wahterver is in input box isnt a number
            if (!Regex.IsMatch(currentInputString, @"^\d+$"))
            {
                currentInputString = "Enter a Number";
                currentMode = Mode.Default;
                return;
            }
            int.TryParse(currentInputString, out currentInput);
        }

		newYPos += elementHeight;
		newYPos -= 1;
		if (currentRangeSubmenu == "Inbound and Outbound Station"){
			// makes camera wider because we want to show pallets under the menu (this menu is very short, we don't have this problem with long menus because they cover the black space)
			Camera.main.rect = new Rect(0,-0.05f,1,1);

			if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "Same End", iOStation == InboundOutboundStation.sameEnd ? selectedRangeStyle : rangeStyle ) && scrollVelocity == 0)
			{
				
				iOStation = InboundOutboundStation.sameEnd;
                Invoke("refreshInboundOutboundHighlighting", .2f);
			}
			newYPos += elementHeight;
            if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "Opposite End", iOStation == InboundOutboundStation.oppositeEnds ? selectedRangeStyle : rangeStyle) && scrollVelocity == 0)
			{
			
				iOStation = InboundOutboundStation.oppositeEnds;
                Invoke("refreshInboundOutboundHighlighting", .2f);
			}     
		}

        if (currentRangeSubmenu == "Pick Up Location for Putaway")
        {
            // makes camera wider because we want to show pallets under the menu (this menu is very short, we don't have this problem with long menus because they cover the black space)
            Camera.main.rect = new Rect(0, -0.05f, 1, 1);

            if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "DOCK", pickUpLocation == PickUpDropOffLocation.dock ? selectedRangeStyle : rangeStyle) && scrollVelocity == 0)
            {
                
                pickUpLocation = PickUpDropOffLocation.dock;
                Invoke("refreshPickUpHighlighting", .2f);
            }
            newYPos += elementHeight;
            if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "P&D", pickUpLocation == PickUpDropOffLocation.pAndD ? selectedRangeStyle : rangeStyle) && scrollVelocity == 0)
            {
                
                pickUpLocation = PickUpDropOffLocation.pAndD;
                Invoke("refreshPickUpHighlighting", .2f);
            }  
        }

        if (currentRangeSubmenu == "Drop Off Location for Retrieval")
        {
            // makes camera wider because we want to show pallets under the menu (this menu is very short, we don't have this problem with long menus because they cover the black space)
            Camera.main.rect = new Rect(0, -0.05f, 1, 1);

            if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "DOCK", dropOffLocation == PickUpDropOffLocation.dock ? selectedRangeStyle : rangeStyle) && scrollVelocity == 0)
            {
               
                dropOffLocation = PickUpDropOffLocation.dock;
                Invoke("refreshDropOffHighlighting", .2f);
            }
            newYPos += elementHeight;
            if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), "P&D", dropOffLocation == PickUpDropOffLocation.pAndD ? selectedRangeStyle : rangeStyle) && scrollVelocity == 0)
            {
                
                dropOffLocation = PickUpDropOffLocation.pAndD;
              
                Invoke("refreshDropOffHighlighting", .2f);
            }
        }
	}
    void DoRangeWindow() {
        int newYPos = elementHeight * 2;

        GUI.Label(new Rect(0, newYPos, sidebarWidth, elementHeight), currentRangeSubmenu, mainSectionHeadingsStyle);



        if (GUI.Button(new Rect(sidebarWidth / 2 + sidebarWidth / 4, newYPos, sidebarWidth / 4, elementHeight), "Done", doneButtonStyle)) {

            currentMode = Mode.Default;

            // return if didnt change input box
            if (currentInputString == "Enter a Number") {
                currentMode = Mode.Default;
                return;
            }

            // return if wahterver is in input box isnt a number
            if (!Regex.IsMatch(currentInputString, @"^\d+$")) {
                currentInputString = "Enter a Number";
                currentMode = Mode.Default;
                return;
            }
            int.TryParse(currentInputString, out currentInput);
            
			 // Range list menu
                if (currentRangeSubmenu == "Number of Aisles") {
					factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfColumns(currentInput + 1);
					NumberOfAisles = currentInput + 1;

                    //viewPerspective = 0;

                    // refreshes hgihlights
                    highlightController.SetHighlightArea(highlightSection.NumberOfAisles);
                }
                // Range list menu
                if (currentRangeSubmenu == "Number of Bays Down Aisle") {
					factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfPalletsDownAisle(currentInput * PalletsBetweenFrames);
					numberOfBaysDownAisle = currentInput;

                //    viewPerspective = 0;

                    // refreshes hgihlights
                    Invoke("refreshLengthHighlighting", 1f);
                   
  
                }
                // Range list menu
                if (currentRangeSubmenu == "Number of Levels") {
                    // Lukas
					factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfLevels(currentInput);
                    NumberOfLevels = currentInput;

                 //   viewPerspective = 0;

                    // refreshes hgihlights
                    Invoke("refreshHeightHighlighting",1f);

                }
                if (currentRangeSubmenu == "Vertical Beam Spacing") {
					verticalBeamSpacing = currentInput;
					factoryBuilder.GetComponent<FactoryBuilder>().refreshAllPalletAndFrameTransforms();
					highlightController.SetHighlightArea(highlightSection.VerticalBeamSpacing);
                }
                if (currentRangeSubmenu == "Bottom Beam Height") {
					bottomBeamHeight = currentInput;
                    factoryBuilder.GetComponent<FactoryBuilder>().refreshBottomBar();
					highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
                }
                if (currentRangeSubmenu == "Beam Length") {
					BeamLength = currentInput;
					factoryBuilder.GetComponent<FactoryBuilder>().refreshAllPalletAndFrameTransforms();
					highlightController.SetHighlightArea(highlightSection.BeamLength);
				}
                if (currentRangeSubmenu == "No. of Pallets per Beam") {
					PalletsBetweenFrames = currentInput;
                    while (BeamLength < PalletsBetweenFrames * 1000) BeamLength += 100;
                    factoryBuilder.GetComponent<FactoryBuilder>().setPalletsPerBayInColumn();
					factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfPalletsDownAisle(numberOfBaysDownAisle * PalletsBetweenFrames);
                    Invoke("refreshPalletsPerBeamHighlighting", 1f);
                }
                if (currentRangeSubmenu == "No. of P&D Beams Across")
                {
                    PAndDBeamsAcross = currentInput;
                    Invoke("refreshPAndDBeamsAcrossHighlighting", .2f);
                }
                if (currentRangeSubmenu == "No. of P&D Levels")
                {
                    PAndDLevels = currentInput;
                    Invoke("refreshPAndDLevelsHighlighting", .2f);
                }
			if (currentRangeSubmenu == "Clear Aisle Width") ClearAisleWidth = currentInput;
                currentMode = Mode.Default;

        }
        newYPos += elementHeight;

        newYPos -= 1;

		GUI.DrawTexture(new Rect(0, newYPos, sidebarWidth, Screen.height), whiteBack);

        currentInputString = GUI.TextField(new Rect(0, newYPos, sidebarWidth, elementHeight), currentInputString, inputNumberStyle);

        newYPos += elementHeight;
        // NEW SECTION
        GUI.Label(new Rect(0, newYPos, sidebarWidth, sectionHeight), "OR SELECT FROM THE LIST", sectionHeadingStyle);
        newYPos += sectionHeight;


        Rect rScrollFrame = new Rect(0, newYPos, sidebarWidth, Screen.height - (elementHeight * 4 + sectionHeight));
        Rect rList = new Rect(0, 0, sidebarWidth, numRows * elementHeight);

        scrollPosition = GUI.BeginScrollView(rScrollFrame, scrollPosition, rList, GUIStyle.none, GUIStyle.none);

        newYPos = 0;


        for (int i = currentMin; i <= currentMax; i = i + currentIncrement) {
			bool useSelectedRangeStyle = false;

            GUIStyle rangeButtonStyle = rangeStyle;

            // show disabled button if beam length too short for number of pallets to fit inside

          //  bool enabled = true;

//            if (currentOptionString == "Beam Length" && i < PalletsBetweenFrames * 1000)
//            {
//                rangeButtonStyle = disabledRangeStyle;
//                enabled = false;
//            }

			// show the currently selected value as highlighted
			switch(currentRangeSubmenu){
			case "Number of Aisles":
				if(NumberOfAisles == i + 1) useSelectedRangeStyle = true;
				break;

			case "Number of Bays Down Aisle":
				if(NumberOfBaysDownAisle == i)  useSelectedRangeStyle = true;
				break;

			case "Number of Levels":
				if(NumberOfLevels == i)  useSelectedRangeStyle = true;
				break;
			
			case "Vertical Beam Spacing":
				if(VerticalBeamSpacing == i)  useSelectedRangeStyle = true;
				break;

			case "Bottom Beam Height":
				if(BottomBeamHeight == i)  useSelectedRangeStyle = true;
				break;
			
			case "Beam Length":
				if(BeamLength == i)  useSelectedRangeStyle = true;
				break;

			case "No. of Pallets per Beam":
				if(PalletsBetweenFrames == i)  useSelectedRangeStyle = true;
				break;
            case "No. of P&D Beams Across":
			    if(PAndDBeamsAcross == i)  useSelectedRangeStyle = true;
				break;
            case "No. of P&D Levels":
			    if(pAndDLevels == i)  useSelectedRangeStyle = true;
				break;
			}

            // apply style " selected range style"
			if(useSelectedRangeStyle){
				rangeButtonStyle = selectedRangeStyle;
			}

			// draw button with style according to whether its clickable, disabled or the current selection (code to figure this out is above)
			if (GUI.Button(new Rect(0, newYPos, sidebarWidth, sectionHeight), i.ToString(), rangeButtonStyle ) && scrollVelocity == 0)
            {
                // needed if graying gout some values
        //        if (!enabled) return;

                // Range list menu
                if (currentRangeSubmenu == "Number of Aisles") {
                    factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfColumns(i + 1);
                    NumberOfAisles = i + 1;

                //    viewPerspective = 0;

                    // refreshes hgihlights
                    highlightController.SetHighlightArea(highlightSection.NumberOfAisles);
                }
                // Range list menu
                if (currentRangeSubmenu == "Number of Bays Down Aisle") {
                    factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfPalletsDownAisle(i * PalletsBetweenFrames);
                    numberOfBaysDownAisle = i;
                    if (PAndDBeamsAcross > i) PAndDBeamsAcross = i;
               //     viewPerspective = 0;

                    // refreshes hgihlights
                    Invoke("refreshLengthHighlighting", 1f);
                   
  
                }
                // Range list menu
                if (currentRangeSubmenu == "Number of Levels") {
                    // Lukas
                    factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfLevels(i);
                    NumberOfLevels = i;
                    if (PAndDLevels > i) PAndDLevels = i;
                 //   viewPerspective = 0;

                    // refreshes hgihlights
                    Invoke("refreshHeightHighlighting",1f);

                }
                if (currentRangeSubmenu == "Vertical Beam Spacing") {
                    verticalBeamSpacing = i;
					factoryBuilder.GetComponent<FactoryBuilder>().refreshAllPalletAndFrameTransforms();
					highlightController.SetHighlightArea(highlightSection.VerticalBeamSpacing);
                }
                if (currentRangeSubmenu == "Bottom Beam Height") {
                    bottomBeamHeight = i;
                    factoryBuilder.GetComponent<FactoryBuilder>().refreshBottomBar();
					highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
                }
                if (currentRangeSubmenu == "Beam Length") {
					BeamLength = i;
					factoryBuilder.GetComponent<FactoryBuilder>().refreshAllPalletAndFrameTransforms();
					highlightController.SetHighlightArea(highlightSection.BeamLength);
				}
                if (currentRangeSubmenu == "No. of Pallets per Beam") {
                    PalletsBetweenFrames = i;
                    while (BeamLength < PalletsBetweenFrames * 1000) BeamLength += 100;
                    factoryBuilder.GetComponent<FactoryBuilder>().setPalletsPerBayInColumn();
					factoryBuilder.GetComponent<FactoryBuilder>().refreshNumberOfPalletsDownAisle(numberOfBaysDownAisle * PalletsBetweenFrames);
                    Invoke("refreshPalletsPerBeamHighlighting", 1f);
                }
                if (currentRangeSubmenu == "No. of P&D Beams Across")
                {
                    PAndDBeamsAcross = i;
                    Invoke("refreshPAndDBeamsAcrossHighlighting", 1f);
                }
                if (currentRangeSubmenu == "No. of P&D Levels")
                {
                    PAndDLevels = i;
                    Invoke("refreshPAndDLevelsHighlighting", 1f);
                }
                
                if (currentRangeSubmenu == "Clear Aisle Width") ClearAisleWidth = i;
             //   currentMode = Mode.Default;
            }


            newYPos += sectionHeight;

			GUI.DrawTexture(new Rect(0, newYPos, sidebarWidth, 3), greyBack);
            yPos += 1;
        }

        GUI.EndScrollView();

    }
    void refreshHeightHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.NumberOfLevels);
    }
    void refreshLengthHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.NumberOfBaysDownAisle);
    }

    void refreshPAndDLevelsHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.NumberOfPAndDLevels);
    }
    void refreshPAndDBeamsAcrossHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.NumberOfPAndDBeamsAcross);
    }
    void refreshPickUpHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.PickUp);
    }
    void refreshDropOffHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.DropOff);
    }
    void refreshPalletsPerBeamHighlighting()
    {
        highlightController.SetHighlightArea(highlightSection.NumberOfPalletsPerBeam);
    }
   	
	void refreshInboundOutboundHighlighting()
	{
		highlightController.SetHighlightArea (highlightSection.InboundOutbound);
	}
    private int TouchToRowIndex(Vector2 touchPos) {
        float y = Screen.height - touchPos.y;  // invert y coordinate
        y += scrollPosition.y;  // adjust for scroll position
        y -= windowMargin.y;    // adjust for window y offset
        y -= listMargin.y;      // adjust for scrolling list offset within the window
        int irow = (int)(y / rowSize.y);

        irow = Mathf.Min(irow, numRows);  // they might have touched beyond last row
        return irow;
    }

    bool IsTouchInsideList(Vector2 touchPos) {
        Vector2 screenPos = new Vector2(touchPos.x, Screen.height - touchPos.y);  // invert y coordinate
        Rect rAdjustedBounds = new Rect(listMargin.x + windowRect.x, listMargin.y + windowRect.y, listSize.x, listSize.y);

        return rAdjustedBounds.Contains(screenPos);
    }

    public class ISO {
        string name;
        float width;
        float length;
        public ISO(string name, float width, float length) {
            this.name = name;
            this.width = width;
            this.length = length;
        }

        public string getName() { return name; }
        public float getWidth() { return width; }
        public float getDepth() { return length; }
    }

    public float FloorX {
        get { return floorX; }
        set { floorX = value; }
    }

    public float FloorZ {
        get { return floorZ; }
        set { floorZ = value; }
    }

    public float NumAisles {
        get { return NumberOfAisles; }
        set {
            NumberOfAisles = (int)value;
            s_numAisles = value.ToString();
        }
    }

    public int NumLevels {
        get { return NumberOfLevels; }
        set { NumberOfLevels = value; }
    }

    public float NumPalletsWide {
        get { return numPalletsWide; }
        set {
            numPalletsWide = value;
            s_numPalletsWide = value.ToString();
        }
    }

    public float NumPalletsHigh {
        get { return numPalletsHigh; }
        set {
            numPalletsHigh = value;
            s_numPalletsHigh = value.ToString();
        }
    }

    public float PalletHeight {
        get { return palletHeight; }
        set { palletHeight = value; }
    }

    public float PalletsInColumn {
        get { return palletsInColumn; }
        set { palletsInColumn = value; }
    }

    public float PalletHeightFromGround {
        get { return palletHeightFromGround; }
        set { palletHeightFromGround = value; }
    }

    public float AisleWidth {
        get { return aisleWidth; }
        set { aisleWidth = value; }
    }

    public float TheClearAisleWidth {
        get { return ClearAisleWidth; }
        set { ClearAisleWidth = value; }
    }

    public float PalletColumnWidth {
        get { return palletColumnWidth; }
        set { palletColumnWidth = value; }
    }

    public float AdjColumnSpacing {
        get { return adjColumnSpacing; }
        set { adjColumnSpacing = value; }
    }

    public float PalletOverhang {
        get { return palletOverhang; }
        set { palletOverhang = value; }
    }

    public bool FirstLevelBeamPresent {
        get { return firstLevelBeamPresent; }
        set { firstLevelBeamPresent = value; }
    }

    public int BottomBeamHeight {
        get { return bottomBeamHeight; }
        set { bottomBeamHeight = value; }
    }

    public int VerticalBeamSpacing {
        get { return verticalBeamSpacing; }
        set { verticalBeamSpacing = value; }
    }

    public int BeamLength {
        get { return beamLength; }
        set { beamLength = value; }
	}

    public ISO PalletISO {
        get { return palletISO; }
        set { palletISO = value; }
    }

    public int PalletsBetweenFrames {
        get { return palletsBetweenFrames; }
        set {
            palletsBetweenFrames = value;
            s_palletsBetweenFrames = value.ToString();
        }
    }
    public int NumberOfBaysDownAisle
    {
        get { return numberOfBaysDownAisle; }
        set { numberOfBaysDownAisle = value; }
    }
    public bool EnableTouchControls {
        get { return enableTouchControls; }
        set { enableTouchControls = value; }
    }

	public string IOStationToString{
		get{ if(iOStation == InboundOutboundStation.sameEnd) return "Sam...";
			else return "Opp...";
		}
	}

    public string PickUpLocationString{
        get{
            if (pickUpLocation == PickUpDropOffLocation.dock) return "DOCK";
            else return "P&D";
        }
    }

  
    public string DropOffLocationString
    {
        get
        {
            if (dropOffLocation == PickUpDropOffLocation.dock) return "DOCK";
            else return "P&D";
        }
    }
    public int PAndDLevels
    {
        get { return pAndDLevels; }
        set { pAndDLevels = value; }
    }
    public int PAndDBeamsAcross
    {
        get { return pAndDBeamsAcross; }
        set { pAndDBeamsAcross = value; }
    }

    public PickUpDropOffLocation DropOffLocation
    {
        get { return dropOffLocation; }
        set { dropOffLocation = value; }
    }
    public PickUpDropOffLocation PickUpLocation
    {
        get { return pickUpLocation; }
        set { pickUpLocation = value; }
    }

    public InboundOutboundStation IOStation
    {
        get { return iOStation; }
        set { iOStation = value; }
    }
}