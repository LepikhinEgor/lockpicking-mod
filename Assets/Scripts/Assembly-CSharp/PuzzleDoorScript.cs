// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// PuzzleDoorScript
using UnityEngine;

public class PuzzleDoorScript : MonoBehaviour
{
	[SerializeField]
	private Rigidbody rb;

	public void UnlockDoor()
	{
		rb.constraints = RigidbodyConstraints.None;
	}
}
