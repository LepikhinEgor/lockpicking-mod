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

	//[SerializeField]
	//private Text diffText;

	[SerializeField]
	private MeshRenderer tumblerMesh;

	[SerializeField]
	private Text timeText;

	[SerializeField]
	private bool isDebug;

	[SerializeField]
	private LockLevel lockLevel;

	[SerializeField]
	[Range(0, 100)]
	private int playerSkill;

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

	private bool moveRight;
	private bool moveLeft;

	private int picksCount = 5;

	private void Start()
	{
		initialAxisPosition = new Vector3(0.031F, pickAxis.localPosition.y, pickAxis.localPosition.z);
		puzzleActive = false;
		hitting = false;
		animating = false;
	}

	private void Update()
	{
		/*if (!hasKit)
        {
			pickAxis.gameObject.SetActive(false);
			return;
        }*/
		if (isDebug)
        {
			ProcessDebugControls();
		}

		if (Input.GetKeyDown(KeyCode.F2))  
		{
			hasKit = picksCount > 0;
			Toggle();
		}

		if (InputHitPin() && !hitting)
        {
			hitting = true;
			pickAnim.SetTrigger("Hit");
		}

		if (InputMoveRightStart())
		{
			moveRight = true;
		}

		if (InputMoveRightEnd())
		{
			moveRight = false;
		}

		if (InputMoveLeftStart())
		{
			moveLeft = true;
		}

		if (InputMoveLeftEnd())
		{
			moveLeft = false;
		}
	}

    private void FixedUpdate()
    {
        if (!hasKit || !puzzleActive)
            return;

        if (moveRight && pickAxis.transform.localPosition.x > initialAxisPosition.x - 0.016f)
        {
            pickAxis.transform.localPosition = pickAxis.transform.localPosition + Vector3.right * (0f - speed) * Time.deltaTime;
        }

        if (moveLeft && pickAxis.transform.localPosition.x < initialAxisPosition.x)
        {
            pickAxis.transform.localPosition = pickAxis.transform.localPosition + Vector3.right * speed * Time.deltaTime;
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

    }

    private bool InputHitPin()
    {
		return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0);
	}

	private bool InputMoveRightStart()
    {
		return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
	}

	private bool InputMoveRightEnd()
	{
		return Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow);
	}
	private bool InputMoveLeftStart()
    {
		return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
	}
	private bool InputMoveLeftEnd()
	{
		return Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow);
	}
	private void ProcessDebugControls()
    {
		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetPuzzle(_resetPick: true);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			CreatePuzzle();
		}
		if (Input.GetKeyDown(KeyCode.L) && !animating)
		{
			Toggle();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void CreatePuzzle()
	{
		ResetPuzzle(_resetPick: true);

		tumblerMesh.enabled = false;

		foreach (SpringPositionScript springPosition in springPositions)
		{
			springPosition.SetSprings(playerSkill);
		}

		switch (lockLevel)
		{
			case LockLevel.Easy:
				//diffText.text = "Easy";
				goal = 1;
				springPositions[1].gameObject.SetActive(false);
				springPositions[2].gameObject.SetActive(false);
				springPositions[3].gameObject.SetActive(false);
				springPositions[4].gameObject.SetActive(false);
				break;
			case LockLevel.Medium:
				//diffText.text = "Medium";
				goal = 2;
				springPositions[2].gameObject.SetActive(false);
				springPositions[3].gameObject.SetActive(false);
				springPositions[4].gameObject.SetActive(false);
				break;
			case LockLevel.Hard:
				//diffText.text = "Hard";
				goal = 3;
				springPositions[3].gameObject.SetActive(false);
				springPositions[4].gameObject.SetActive(false);
				break;
			case LockLevel.Expert:
				//diffText.text = "Expert";
				springPositions[4].gameObject.SetActive(false);
				goal = 4;
				break;
			case LockLevel.Master:
				//diffText.text = "Master";
				goal = 5;
				break;
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
			pickAxis.localPosition = initialAxisPosition;
		}

		if (picksCount <= 0)
        {
			pickAxis.gameObject.SetActive(false);

		}
		foreach (SpringPositionScript springPosition in springPositions)
		{
			springPosition.ResetSprings();
		}
	}

	public void DecreasePicksCount()
    {
		picksCount--;
    }
}
