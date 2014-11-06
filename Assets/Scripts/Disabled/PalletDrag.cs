using UnityEngine;
using System.Collections;

public class PalletDrag : MonoBehaviour
{


    public Vector2 beginPosition = Vector2.zero,
            movePosition = Vector2.zero;
    public float movedDist = 0.0f,
            DistanceX = 0,
            DistanceY = 0;
    private GameObject factorybuilder;
    private float distanceToPlus, distanceToMinus;
    private float oldDistanceToMinus;
    public bool dragging = false;
    private GameObject mainCam;

    // Use this for initialization
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");

        factorybuilder = GameObject.Find("FactoryBuilderObj");
    }

    // Update is called once per frame
    void Update()
    {
        // resize rectangle
        if (transform.name == "DragTop")
        {
            this.transform.localScale = new Vector3(0.2f, 0.2f, factorybuilder.GetComponent<FactoryBuilder>().getColLength(0) / 20);
        }
        else
        {
            this.transform.localScale = new Vector3(0.2f, 0.2f, factorybuilder.GetComponent<FactoryBuilder>().getHighestY() / 20);
        }
       
    }

    void OnMouseDown()
    {
        dragging = false;
        factorybuilder.GetComponent<FactoryBuilder>().currentDragButton = this.transform.gameObject;
    }

    void OnMouseUp()
    {
        dragging = true;
        mainCam.GetComponent<MoveCamera>().setDraggingPallet(true);
    }

    void OnMouseDrag()
    {

    }
}
