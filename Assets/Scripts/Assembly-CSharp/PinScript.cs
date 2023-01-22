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

	//pin в полете
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
		if (activePin && !pinLocked && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
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
		if (springPositionScript.CheckPinLocked(this))
		{
			Invoke("DeactivatePin", 0.25f);
			puzzleScript.progress++;
			soundController.PlayFixSound();
		}
		else
		{
			activePin = false;
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

	public void ResetPuzzle()
	{
		activePin = true;
		puzzleScript.DecreasePicksCount();
		puzzleScript.ResetPuzzle(false);
	}

	public void ResetPin(bool hideAfterReset)
	{
		pinLocked = false;
		activePin = false;
		springPositionScript.enabled = false;
		colliderActive = true;
		pinAnim.SetTrigger("Reset");
		pinAnim.speed = 1f;

		if (hideAfterReset)
			Invoke("HidePin", 0.25f);
	}

	private void HidePin()
    {
		GetComponent<MeshRenderer>().enabled = false;
	}
}
