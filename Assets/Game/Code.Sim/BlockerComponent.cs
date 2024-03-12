using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlockerComponent : MonoBehaviour
{
	/// <summary>
	/// The time at which this blocker was spawned.
	/// </summary>
	public float SpawnTime { get; set; }

	/// <summary>
	/// The time at which this blocker will die and stop blocking rocks.
	/// </summary>
	public float DeathTime { get; set; }

	/// <summary>
	/// The collider for this blocker, assigned in the editor.
	/// </summary>
	public BoxCollider2D Collider;
}
