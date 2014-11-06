using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

    private GameObject factorybuilder;
	// Use this for initialization
	void Start () {
        this.transform.localScale = new Vector3(0, 0, 0);
        factorybuilder = GameObject.Find("FactoryBuilderObj");
	}
	
	// Update is called once per frame
	void Update () {
      //  print(this.transform.parent.gameObject.name);
        if (factorybuilder.GetComponent<FactoryBuilder>().currentDragButton != null)
        {
           
            if (factorybuilder.GetComponent<FactoryBuilder>().currentDragButton.name != this.transform.parent.gameObject.name)
            {
             
                this.transform.parent.GetComponent<PalletDrag>().dragging = false;
                this.transform.localScale = new Vector3(0, 0, 0);
                return;
            }
            else
            {
                if (this.transform.parent.gameObject.GetComponent<PalletDrag>().dragging)
                {

                    this.transform.localScale = new Vector3(.5f, .5f, 1);

                }
                else
                {

                    this.transform.localScale = new Vector3(0, 0, 0);

                }
            }
        }
       

        
      
	}
}
