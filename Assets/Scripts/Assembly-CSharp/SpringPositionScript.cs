using System.Collections.Generic;
using UnityEngine;

public class SpringPositionScript : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> Springs;

    [SerializeField]
	private PinScript pinScript;

	[SerializeField]
	public float pinSize;

	[SerializeField]
	private Material redM;

	[SerializeField]
	private Material greenM;

    private void Awake()
    {
        pinScript = transform.Find("Pin").GetComponent<PinScript>();
        pinScript.springPositionScript = this;
    }

    /* Skill  min max
       0-20    2   3
       20-40   2   4
       40-60   3   4
       60-80   3   5
       80-100  4   5
       100     4   6
    */
    public void SetSprings(int playerSkill)
    {
        pinScript.transform.localScale = new Vector3(1.05f, 1.05f, pinSize);

        int greenSizeMinimum = GetGreenSizeMin(playerSkill);
        int greenSizeMaximum = GetGreenSizeMax(playerSkill);

        int greenCount = Random.Range(greenSizeMinimum, greenSizeMaximum + 1);
        int greenPositionStart = Random.Range(0, Springs.Count - greenCount + 1);
        int greenPositionEnd = greenPositionStart + (greenCount - 1);

        //ѕроставл€ем сначала все красные позиции
        for (int i = 0; i < Springs.Count; i++)
        {
            Springs[i].tag = "RedSpring";
            Springs[i].GetComponent<MeshRenderer>().material = redM;
        }

        //ѕоверх красных позиций переписываем зеленые позиции
        for (int i = greenPositionStart; i <= greenPositionEnd; i++)
        {
            Springs[i].tag = "GreenSpring";
            Springs[i].GetComponent<MeshRenderer>().material = greenM;
        }

        for (int i = 0; i < Springs.Count; i++)
        {
            if (Springs[i].tag == "RedSpring")
                Springs[i].GetComponent<MeshRenderer>().enabled = false;
        }

    }

    private int GetGreenSizeMin(int playerSkill)
    {
        if (playerSkill < 40)
            return 2;

        if (playerSkill < 80)
            return 3;

        return 4;
    }

    private int GetGreenSizeMax(int playerSkill)
    {
        if (playerSkill < 20)
            return 3;

        if (playerSkill < 60)
            return 4;

        if (playerSkill < 100)
            return 5;

        return 6;
    }

    public void ResetSprings()
    {
        pinScript.ResetPin();
    }
}
