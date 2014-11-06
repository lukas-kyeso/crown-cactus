using UnityEngine;
using System.Collections;
using System.Xml;

public class xmlParser : MonoBehaviour {
	public TextAsset xmlDoc;
	private XmlDocument xmlLoader;
	private string filepath;
	private XmlElement rootElement;
	private XmlNodeList warehouseContent;
	private XmlNodeList operationContent;
	private XmlNodeList trucksContent;

	// Use this for initialization
	void Start () {
		loader();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void loader(){
		if (xmlDoc != null) {
			// Create Document to hold the xml in memory
			xmlLoader = new XmlDocument();
			// load xml by path
//			string filepath = Application.dataPath + @"/Resources/crownMenu.xml";
//			xmlLoader.Load(filepath);
			// load xml by element in game scene
			xmlLoader.LoadXml (xmlDoc.text);
			// Find all the document elements of root element
			rootElement = xmlLoader.DocumentElement;
			// Find all the documnet element tags with xmlLoader.GetElementsByTagName("xxxxx");
//			XmlNodeList elemList = xmlLoader.GetElementsByTagName("menu");
			parser();

		} else {
			print("Need to drag the xml file from resource folder to XML within Main Camera!");
		}
	}

	void parser(){
		foreach(XmlNode childNode in rootElement)
		{
			if(childNode.Name.Equals("warehouse"))
			{
//				XmlNodeList warehouseContent = xmlLoader.GetElementsByTagName("warehouse");
				warehouseContent = childNode.ChildNodes;
				foreach(XmlNode warehouseInfo in warehouseContent)
				{
//					print ("warehouse info: " + warehouseInfo.Name);
					if(warehouseInfo.Name == "button")
					{
						XmlNodeList buttonCotent = warehouseInfo.ChildNodes;
						switch(warehouseInfo.Attributes["name"].Value)
						{
							case "range":
                                //	print ("Range button");
								foreach(XmlNode buttonInfo in buttonCotent)
								{
									if(buttonInfo.Name == "caption")
									{
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "min"){
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "max"){
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "increment"){
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "unit"){
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "default"){
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
								}
								break;
							case "list":
                                //	print ("List button");
								foreach(XmlNode buttonInfo in buttonCotent)
								{
									if(buttonInfo.Name == "caption")
									{
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "option")
									{
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "default")
									{
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
								}
								break;
							// toggle button still need more twist
							case "toggle":
                                //	print ("Toggle button");
								foreach(XmlNode buttonInfo in buttonCotent)
								{
									if(buttonInfo.Name == "caption")
									{
									//	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									if(buttonInfo.Name == "default")
									{
                                        //	print (buttonInfo.Name +" : "+ buttonInfo.InnerText);
									}
									// This is the sub-button inside toggle button
									if(buttonInfo.Name == "button")
									{
										XmlNodeList sButtonContent = buttonInfo.ChildNodes;
										foreach(XmlNode sButtonInfo in sButtonContent)
										{
											if(sButtonInfo.Name == "caption")
											{
                                                //		print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
											if(sButtonInfo.Name == "min")
											{
                                                //		print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
											if(sButtonInfo.Name == "max")
											{
                                                //	print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
											if(sButtonInfo.Name == "increment")
											{
                                                //		print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
											if(sButtonInfo.Name == "unit")
											{
                                                //	print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
											if(sButtonInfo.Name == "default")
											{
                                                //	print ("(Sub-button)" + " " + sButtonInfo.Name + " : " + sButtonInfo.InnerText);
											}
										}
									}
//									XmlNodeList sButtonContent = buttonInfo.ChildNodes;
//									foreach(XmlNode sButtonInfo in sButtonContent){
//										switch(sButtonInfo.Attributes["name"].Value)
//										{
//											case "list":
//												break;
//										}
//									}
								}
								break;
						}

						// OLD PARSER (KEEP IN CASE NEW ONE NOT WORK FULLY)
//						XmlNodeList buttonCotent = warehouseInfo.ChildNodes;
//						foreach(XmlNode buttonInfo in buttonCotent)
//						{
//							print (buttonInfo.InnerText);
//							XmlNodeList buttonTypes = buttonInfo.ChildNodes;
//							foreach(XmlNode buttonType in buttonTypes)
//							{
//							if(buttonInfo.Name == "type"){
//								if(buttonInfo.InnerText == "toggle")
//								{
//									print ("Toggle button");
//								}
//								if(buttonInfo.InnerText == "range")
//								{
//									print ("Range button");
//								}
//								if(buttonInfo.InnerText == "list")
//								{
//									print ("List button");
//								}
//								if(buttonInfo.Name == "caption"){
//									
//								}
//								if(buttonInfo.Name == "min"){
//									
//								}
//								if(buttonInfo.Name == "max"){
//									
//								}
//								if(buttonInfo.Name == "increment"){
//									
//								}
//								if(buttonInfo.Name == "unit"){
//									
//								}
//								if(buttonInfo.Name == "default"){
//									
//								}
//								if(buttonInfo.Name == "option"){
//									
//								}
//							}
//						}
					}
					if(warehouseInfo.Name == "title")
					{
						// Write string on menu
                        //	print (warehouseInfo.Name +" : "+ warehouseInfo.InnerText);
					}
					if(warehouseInfo.Name == "caption")
					{
						// top bar - Tabs name
                        //	print (warehouseInfo.Name +" : "+ warehouseInfo.InnerText);
					}
					if(warehouseInfo.Name == "line")
					{
						// Draw a line break;
                        //	print ("=====================================================");
					}
				}

			}
			if(childNode.Name.Equals("operations"))
			{
				operationContent = childNode.ChildNodes;
				foreach(XmlNode operationInfo in operationContent)
				{
//					print ("operation info: "+operationInfo.Name);
					// Do something
				}
			}
			if(childNode.Name.Equals("trucks"))
			{
				trucksContent = childNode.ChildNodes;
				foreach(XmlNode trucksInfo in trucksContent)
				{
//					print ("trucks info: "+trucksInfo.Name);
					// Do something
				}
			}

		}

	}

	void rangeButton(){

	}

	void listButton(){

	}

	void toggleButton(){

	}

	void OnGUI(){

	}
}
