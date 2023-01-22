using UnityEngine;

public class PinScript : MonoBehaviour
{
	public SpringPositionScript springPositionScript;

	private PuzzleScript puzzleScript;

	[SerializeField]
	private Animator pinAnim;

	[SerializeField]
	private BoxCollider pinCollider;

	SoundController soundController;

	public bool activePin;

	public bool pinLocked;

	public bool colliderActive = true;

	private void Start()
	{
		activePin = false;
		pinLocked = false;
		colliderActive = true;
		puzzleScript = GetComponentInParent<PuzzleScript>();
		soundController = GameObject.FindObjectOfType<SoundController>();
	}

	private void Update()
	{
		if (activePin && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && !pinLocked))
		{
			StopPin();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (colliderActive)
		{
			if (other.CompareTag("Pick") && !activePin)
			{
				HitPin();
			}
			if (other.CompareTag("RedSpring"))
			{
				pinLocked = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (colliderActive && other.CompareTag("RedSpring"))
		{
			pinLocked = false;
		}
	}

	//Ударили по пину отмычкой
	public void HitPin()
	{
		springPositionScript.enabled = true;
		activePin = true;
		colliderActive = false;
		pinAnim.SetTrigger("Hit");
		puzzleScript.activeSpring = springPositionScript;
	}

	public void StopPin()
	{
		pinAnim.speed = 0f;
		colliderActive = true;
		pinLocked = true;
		//CheckPin();
		Debug.Log(springPositionScript.CheckPinLocked(this));
		if (springPositionScript.CheckPinLocked(this))
		{
			activePin = false;
			puzzleScript.progress++;
			puzzleScript.StopHit();
			soundController.PlayFixSound();
		}
		else
		{
			soundController.PlayFailSound();
			Invoke("ResetPuzzle", 0.25f);
		}
	}

	public void DeactivatePin()
	{
		activePin = false;
		springPositionScript.enabled = false;
		colliderActive = true;
		puzzleScript.StopHit();
	}

	public void CheckPin()
	{
		if (pinLocked)
		{
			activePin = false;
			puzzleScript.progress++;
			puzzleScript.StopHit();
			soundController.PlayFixSound();
		}
		else
		{
			ResetPuzzle();
		}
	}

	public void ResetPuzzle()
	{
		puzzleScript.DecreasePicksCount();
		puzzleScript.ResetPuzzle(false);
	}

	public void ResetPin()
	{
		pinLocked = false;
		activePin = false;
		springPositionScript.enabled = false;
		colliderActive = true;
		pinAnim.SetTrigger("Reset");
		pinAnim.speed = 1f;
	}
}
