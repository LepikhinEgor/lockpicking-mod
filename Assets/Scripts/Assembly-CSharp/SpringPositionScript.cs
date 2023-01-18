// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SpringPositionScript
using System.Collections.Generic;
using UnityEngine;

public class SpringPositionScript : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> Springs;

	[SerializeField]
	private GameObject pinObj;

	[SerializeField]
	private int initialRedMinimum;

	[SerializeField]
	private int initialRedMaximum;

	[SerializeField]
	private int greenSizeMinimum;

	[SerializeField]
	private int greenSizeMaximum;

	[SerializeField]
	public float pinSize;

	[SerializeField]
	private Material redM;

	[SerializeField]
	private Material greenM;

	public void SetSprings()
	{
		int num = Random.Range(initialRedMinimum, initialRedMaximum);
		pinObj.transform.localScale = new Vector3(1.05f, 1.05f, pinSize);
		for (int i = 0; i < num; i++)
		{
			Springs[i].tag = "RedSpring";
			Springs[i].GetComponent<MeshRenderer>().material = redM;
		}
		int num2 = Random.Range(greenSizeMinimum, greenSizeMaximum);
		for (int j = num; j < num + num2; j++)
		{
			Springs[j].tag = "GreenSpring";
			Springs[j].GetComponent<MeshRenderer>().material = greenM;
		}
		for (int k = num + num2; k < 10; k++)
		{
			Springs[k].tag = "RedSpring";
			Springs[k].GetComponent<MeshRenderer>().material = redM;
		}
	}

	public void ResetSprings()
	{
		pinObj.GetComponent<PinScript>().ResetPin();
	}
}
