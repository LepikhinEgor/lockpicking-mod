using UnityEngine;

public class PickAfterAnimationScript : MonoBehaviour
{
	[SerializeField]
	private PuzzleScript ps;

	private void Start()
	{
		InvokeRepeating("FixPosition", 0f, 0.2f);
	}

	public void FixPosition()
	{
		base.transform.localPosition = Vector3.zero;
	}

	public void StopHit()
	{
		ps.StopHit();
	}
}
