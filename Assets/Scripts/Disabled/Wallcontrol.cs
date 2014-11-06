using UnityEngine;
using System.Collections;

public class Wallcontrol : MonoBehaviour {
    private GameObject floor;

    // Use this for initialization
    void Start() {
        floor = GameObject.Find("Factory Floor");
    }

    // Update is called once per frame
    void Update() {
        float floorX = floor.transform.position.x;
        float floorZ = floor.transform.position.z;
        float floorXScale = floor.transform.localScale.x;
        float floorZScale = floor.transform.localScale.z;
        Vector3 pos = transform.position;
        Vector3 sc = transform.localScale;

        switch (name) {
            case "BottomWall":
                pos.x = floorX + floorXScale / 0.2f;
                sc.x = floorXScale*10;
                break;
            case "LeftWall":
                pos.z = floorZ + floorZScale / 0.2f;
				sc.z = floorZScale*10 + 0.1f;
                break;
            case "TopWall":
                pos.x = floorX + floorXScale / 0.2f;
                pos.z = floorZ + floorZScale / 0.1f;
                sc.x = floorXScale*10;
                break;
            case "RightWall":
                pos.x = floorX + floorXScale / 0.1f;
                pos.z = floorZ + floorZScale / 0.2f;
                sc.z = floorZScale*10 + 0.1f;
                break;
            default:
                break;
        }

        transform.position = pos;
        transform.localScale = sc;
    }
}
