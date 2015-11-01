using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemonBehavior : Mobile {

	public int[] reqMats;
	public string[] reqMatsName;
	//public Dictionary<string,int> neededMats = new Dictionary<string,int >();

	public struct summMaterials
	{
		public GameObject mat;
		public int numOfMat;
	}
}
