using UnityEngine;

public class PaletteGenerator : MonoBehaviour {
public Color[] colourArray = new Color[256];
   
    public void Awake()
    {
        Texture2D colourPalette = new Texture2D(256, 10, TextureFormat.ARGB32, false);
       
        for(int x = 0; x < 256; x++){
            for(int y = 0; y < 10; y++){
                colourPalette.SetPixel(x,y,colourArray[x]);
            }
        }
        colourPalette.filterMode = FilterMode.Point;
        colourPalette.wrapMode = TextureWrapMode.Clamp;
        colourPalette.Apply();
        renderer.material.SetTexture("_ColorRamp",colourPalette);
    }
}