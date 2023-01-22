using System.Collections.Generic;
using UnityEngine;

public class SpringPositionScript : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> Springs;

    [SerializeField]
	public PinScript pinScript;

	[SerializeField]
	public float pinSize;

	[SerializeField]
	private Material redM;

	[SerializeField]
	private Material greenM;

    [SerializeField]
    private float springItemSize;

    [SerializeField]
    private float lockTargetMinY;
    [SerializeField]
    private float lockTargetMaxY;

    private void Awake()
    {
        pinScript = transform.Find("Pin").GetComponent<PinScript>();
        pinScript.springPositionScript = this;
        springItemSize = Springs[0].GetComponent<BoxCollider>().size.y;
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
            Springs[i].GetComponent<MeshRenderer>().enabled = false;
        }

        //ѕоверх красных позиций переписываем зеленые позиции
        for (int i = greenPositionStart; i <= greenPositionEnd; i++)
        {
            Springs[i].tag = "GreenSpring";
            Springs[i].GetComponent<MeshRenderer>().material = greenM;
            Springs[i].GetComponent<MeshRenderer>().enabled = true;
        }

        lockTargetMaxY = Springs[greenPositionStart].transform.position.y + springItemSize / 2 - 0.01377f;
        lockTargetMinY = Springs[greenPositionEnd].transform.position.y - springItemSize / 2 - 0.01377f;
    }

    public bool CheckPinLocked(PinScript pin)
    {
        float pinSizeY = pin.GetComponent<BoxCollider>().size.y;

        float pinYMin = pin.transform.position.y - pinSizeY / 2;
        float pinYMax = pin.transform.position.y + pinSizeY / 2;

        return pinYMin > lockTargetMinY && pinYMax < lockTargetMaxY;
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
