// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// PuzzleScript
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PuzzleScript : MonoBehaviour
{
	[SerializeField]
	private List<SpringPositionScript> springPositions;

	[SerializeField]
	private Animator pickAnim;

	[SerializeField]
	private Animator puzzleAnim;

	[SerializeField]
	private Transform pickAxis;

	[SerializeField]
	private float speed;

	[SerializeField]
	private Text diffText;

	[SerializeField]
	private MeshRenderer tumblerMesh;

	[SerializeField]
	private Text timeText;

	public PuzzleDoorScript pds;

	public SpringPositionScript activeSpring;

	public int progress;

	public bool hasKit;

	public bool completed;

	private Vector3 initialAxisPosition;

	private int goal;

	private bool hitting;

	private bool puzzleActive;

	private bool animating;

	private float timeLeft;

	private void Start()
	{
		initialAxisPosition = new Vector3(0.029f, pickAxis.localPosition.y, pickAxis.localPosition.z);
		puzzleActive = false;
		hitting = false;
		animating = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))  
		{
			hasKit = true;
			Toggle();
		}
		if (hasKit)
		{
			if (puzzleActive)
			{
				if ((Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(1)) && !hitting)
				{
					hitting = true;
					pickAnim.SetTrigger("Hit");
				}
				if (Input.GetKey(KeyCode.D) && !hitting)
				{
					if (pickAxis.transform.localPosition.x > initialAxisPosition.x - 0.016f)
					{
						pickAxis.transform.localPosition = pickAxis.transform.localPosition + Vector3.right * (0f - speed) * Time.deltaTime;
					}
				}
				else if ((Input.GetKey(KeyCode.A) & !hitting) && pickAxis.transform.localPosition.x < initialAxisPosition.x)
				{
					pickAxis.transform.localPosition = pickAxis.transform.localPosition + Vector3.right * speed * Time.deltaTime;
				}
				if (Input.GetKeyDown(KeyCode.R))
				{
					ResetPuzzle(_resetPick: true);
				}
				if (Input.GetKeyDown(KeyCode.N))
				{
					CreatePuzzle();
				}
				if (!completed && progress == goal)
				{
					completed = true;
					tumblerMesh.enabled = true;
					puzzleActive = false;
					puzzleAnim.SetTrigger("Complete");
					if (pds != null)
					{
						pds.UnlockDoor();
					}
				}
				if (timeLeft > 0f)
				{
					timeLeft -= Time.deltaTime;
					timeText.text = Mathf.Round(timeLeft).ToString();
				}
				else
				{
					CreatePuzzle();
				}
			}
			if (Input.GetKeyDown(KeyCode.L) && !animating)
			{
				Toggle();
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void Toggle()
	{
		animating = true;
		if (puzzleActive)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	public void Show()
	{
		CreatePuzzle();
		puzzleAnim.SetTrigger("Show");
	}

	public void Hide()
	{
		puzzleActive = false;
		puzzleAnim.SetTrigger("Hide");
	}

	public void ClearAnimation()
	{
		puzzleActive = true;
		animating = false;
	}

	public void ClearAnimationCanMove()
	{
		animating = false;
	}

	public void CreatePuzzle()
	{
		ResetPuzzle(_resetPick: true);
		int num = Random.Range(0, 3);
		switch (num)
		{
			case 0:
				diffText.text = "Easy";
				tumblerMesh.enabled = false;
				timeLeft = 60f;
				goal = 3;
				break;
			case 1:
				diffText.text = "Medium";
				tumblerMesh.enabled = false;
				timeLeft = 50f;
				goal = 4;
				break;
			default:
				diffText.text = "Hard";
				tumblerMesh.enabled = false;
				timeLeft = 40f;
				goal = 5;
				break;
		}
		springPositions[3].gameObject.SetActive(value: true);
		springPositions[4].gameObject.SetActive(value: true);
		int num2 = 0;
		foreach (SpringPositionScript springPosition in springPositions)
		{
			num2++;
			springPosition.SetSprings();
			if (num == 0 && num2 > 2)
			{
				springPositions[3].gameObject.SetActive(value: false);
				springPositions[4].gameObject.SetActive(value: false);
				break;
			}
			if (num == 1 && num2 > 3)
			{
				springPositions[4].gameObject.SetActive(value: false);
				break;
			}
		}
	}

	public void StopHit()
	{
		hitting = false;
	}

	public void ResetPuzzle(bool _resetPick)
	{
		completed = false;
		progress = 0;
		if (_resetPick)
		{
			pickAxis.localPosition = new Vector3(0.029f, pickAxis.localPosition.y, pickAxis.localPosition.z);
		}
		foreach (SpringPositionScript springPosition in springPositions)
		{
			springPosition.ResetSprings();
		}
	}
}
