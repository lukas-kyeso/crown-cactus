using UnityEngine;
using System.Collections;
using SimpleJSON;

public class JSONReciever : MonoBehaviour {

	HighlightController highlightController;
    ParametersButton parametersButton;
	string jsonString = "";

    bool bottomBeamPresent;
    int levels;
    int bottomBeamHeight;
    int beamLength;
    int pAndDBeamsAcross;
    int palletsPerBeam;
    int aisles;
    string pickUpLocationForPutaway;
    string inboundAndOutboundStation;
    int verticalBeamSpacing;
    int baysDownAisles;
    string dropOffLocationForRetrieval;
    int pAndDLevels;
   
   string test = @"{
  ""bottomBeamPresent"" : false,
  ""levels"" : 4,
  ""bottomBeamHeight"" : 200,
  ""beamLength"" : 2700,
  ""pAndDBeamsAcross"" : 1,
  ""palletsPerBeam"" : 2,
  ""aisles"" : 1,
  ""pickUpLocationForPutaway"" : ""P&D"",
  ""inboundAndOutboundStation"" : ""Same End"",
  ""verticalBeamSpacing"" : 1600,
  ""baysDownAisles"" : 8,
  ""dropOffLocationForRetrieval"" : ""P&D"",
  ""pAndDLevels"" : 6
}";
	// Use this for initialization
	void Start () {
        parametersButton = GameObject.Find("ParametersButton").GetComponent<ParametersButton>();
		highlightController = GameObject.Find ("ParametersButton").GetComponent<ParametersButton> ().highlightController;
        
	}
	


	void ParseCameraView(string camera_view){
		switch (camera_view) {
		case "bottomBeamPresent":
			parametersButton.currentCameraLocation = Location.bottomBeamHeightLocation;
			if (parametersButton.FirstLevelBeamPresent) highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
			else highlightController.SetHighlightArea(highlightSection.none);
			break;
		case "levels":
			highlightController.SetHighlightArea(highlightSection.NumberOfLevels);
			parametersButton.currentCameraLocation = Location.levelsLocation;
			break;
		case "bottomBeamHeight":
			highlightController.SetHighlightArea(highlightSection.BottomBeamHeight);
			parametersButton.currentCameraLocation = Location.bottomBeamHeightLocation;
			break;
		case "beamLength":
			highlightController.SetHighlightArea(highlightSection.BeamLength);
			parametersButton.currentCameraLocation = Location.verticalBeamSpacingLocation;
			break;
		case "pAndDBeamsAcross":
			parametersButton.currentCameraLocation = Location.pAndDWidthLocation;
			parametersButton.invokeRefreshPAndDBeamsAcrossHighlighting();
			break;
		case "palletsPerBeam":
			parametersButton.currentCameraLocation = Location.verticalBeamSpacingLocation;
			parametersButton.invokeRefreshPalletsPerBeamHighlighting();
			// INVOKE refreshPalletsPerBeamHighlighting
			break;
		case "aisles":
			highlightController.SetHighlightArea(highlightSection.NumberOfAisles);
			parametersButton.currentCameraLocation = Location.aisleLocation;
			break;
		case "pickUpLocationForPutaway":
			parametersButton.currentCameraLocation = Location.pickUpLocation;
			parametersButton.invokeRefresPickUpHighlighting();
			// invoke
			break;
		case "inboundAndOutboundStation":
			parametersButton.currentCameraLocation = Location.inboundOutboundStationLocation;
			parametersButton.invokeRefreshInboundOutboundHighlighting();
			// invoke
			break;
		case "verticalBeamSpacing":
			highlightController.SetHighlightArea(highlightSection.VerticalBeamSpacing);
			parametersButton.currentCameraLocation = Location.verticalBeamSpacingLocation;
			break;
		case "baysDownAisles":
			highlightController.SetHighlightArea(highlightSection.NumberOfBaysDownAisle);
			parametersButton.currentCameraLocation = Location.bayLocation;
			break;
		case "dropOffLocationForRetrieval":
			parametersButton.currentCameraLocation = Location.dropOffLocation;
			parametersButton.invokeRefreshDropOffHighlighting();
			// invoke
			break;
		case "pAndDLevels":
			parametersButton.currentCameraLocation = Location.pAndDLevelsLocation;
			parametersButton.invokeRefreshPAndDLevelsHighlighting();
			// invoke
			break;
		}
	}
	void ParseJSON(string string_parameter){
        var JSONObject = JSON.Parse(string_parameter);

        bottomBeamPresent = JSONObject["bottomBeamPresent"].AsBool;
        print(bottomBeamPresent);

        levels = JSONObject["levels"].AsInt;
        print(levels);

        bottomBeamHeight = JSONObject["bottomBeamHeight"].AsInt;
        print(bottomBeamHeight);

        beamLength = JSONObject["beamLength"].AsInt;
        print(beamLength);

        pAndDBeamsAcross = JSONObject["pAndDBeamsAcross"].AsInt;
        print(pAndDBeamsAcross);

        palletsPerBeam = JSONObject["palletsPerBeam"].AsInt;
        print(palletsPerBeam);

        aisles = JSONObject["aisles"].AsInt;
        print(aisles);

        pickUpLocationForPutaway = JSONObject["pickUpLocationForPutaway"].Value;
        print(pickUpLocationForPutaway);

        inboundAndOutboundStation = JSONObject["inboundAndOutboundStation"].Value;
        print(inboundAndOutboundStation);

        verticalBeamSpacing = JSONObject["verticalBeamSpacing"].AsInt;
        print(verticalBeamSpacing);

        baysDownAisles = JSONObject["baysDownAisles"].AsInt;
        print(baysDownAisles);

        dropOffLocationForRetrieval = JSONObject["dropOffLocationForRetrieval"].Value;
        print(dropOffLocationForRetrieval);

        pAndDLevels = JSONObject["pAndDLevels"].AsInt;
        print(pAndDLevels);

        UpdateScene();
	}

    void UpdateScene()
    {
        parametersButton.FirstLevelBeamPresent = bottomBeamPresent;

        if (levels != 0) parametersButton.NumLevels = levels;

        if (bottomBeamHeight != 0) parametersButton.BottomBeamHeight = bottomBeamHeight;

        if (beamLength != 0) parametersButton.BeamLength = beamLength;

        if (pAndDBeamsAcross != 0) parametersButton.PAndDBeamsAcross = pAndDBeamsAcross;

        if (palletsPerBeam != 0) parametersButton.PalletsBetweenFrames = palletsPerBeam;

        if (aisles != 0) parametersButton.NumAisles = aisles + 1;

        if (pickUpLocationForPutaway == "P&D") parametersButton.PickUpLocation = PickUpDropOffLocation.pAndD;
        else parametersButton.PickUpLocation = PickUpDropOffLocation.dock;

        if (inboundAndOutboundStation == "Same End") parametersButton.IOStation = InboundOutboundStation.sameEnd;
        else parametersButton.IOStation = InboundOutboundStation.oppositeEnds;

        if (verticalBeamSpacing != 0) parametersButton.VerticalBeamSpacing = verticalBeamSpacing;

        if (baysDownAisles != 0) parametersButton.NumberOfBaysDownAisle = baysDownAisles;

        if (dropOffLocationForRetrieval == "P&D") parametersButton.DropOffLocation = PickUpDropOffLocation.pAndD;
        else parametersButton.DropOffLocation = PickUpDropOffLocation.dock;

        if (pAndDLevels != 0) parametersButton.PAndDLevels = pAndDLevels;
        
    }
}
