using UnityEngine;
using System.Collections;

public class PalletProperties : MonoBehaviour {
	private int column = 0;

	public int getColumn(){
		return column;
	}

	public void setColumn(int i){
		column = i;
	}
}
