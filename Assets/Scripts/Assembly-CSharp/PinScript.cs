// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// PinScript
using UnityEngine;

public class PinScript : MonoBehaviour
{
	[SerializeField]
	private SpringPositionScript sps;

	[SerializeField]
	private PuzzleScript ps;

	[SerializeField]
	private Animator pinAnim;

	[SerializeField]
	private BoxCollider pinCollider;

	public bool activePin;

	public bool pinLocked;

	public bool colliderActive = true;

	public bool resetPick;

	private void Start()
	{
		activePin = false;
		pinLocked = false;
		colliderActive = true;
	}

	private void Update()
	{
		if (activePin && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.S)) && !pinLocked)
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

	public void HitPin()
	{
		sps.enabled = true;
		activePin = true;
		colliderActive = false;
		pinAnim.SetTrigger("Hit");
		ps.activeSpring = sps;
	}

	public void StopPin()
	{
		pinAnim.speed = 0f;
		colliderActive = true;
		pinLocked = true;
		Invoke("CheckPin", 0.25f);
	}

	public void DeactivatePin()
	{
		activePin = false;
		sps.enabled = false;
		colliderActive = true;
	}

	public void CheckPin()
	{
		if (pinLocked)
		{
			activePin = false;
			ps.progress++;
		}
		else
		{
			ResetPuzzle(resetPick);
		}
	}

	public void ResetPuzzle(bool _resetPick)
	{
		ps.ResetPuzzle(_resetPick);
	}

	public void ResetPin()
	{
		pinLocked = false;
		activePin = false;
		sps.enabled = false;
		colliderActive = true;
		pinAnim.SetTrigger("Reset");
		pinAnim.speed = 1f;
	}
}
