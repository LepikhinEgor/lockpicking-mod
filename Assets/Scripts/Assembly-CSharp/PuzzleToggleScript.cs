// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// PuzzleToggleScript
using UnityEngine;

public class PuzzleToggleScript : MonoBehaviour
{
	[SerializeField]
	private PuzzleScript ps;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Kit"))
		{
			ps.hasKit = true;
			Object.Destroy(other.gameObject);
		}
		if (other.CompareTag("Door") && ps.hasKit && !ps.completed)
		{
			ps.pds = other.GetComponent<PuzzleDoorScript>();
			ps.Toggle();
		}
	}
}