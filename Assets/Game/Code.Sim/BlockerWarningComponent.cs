using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlockerWarningComponent : MonoBehaviour
{
	/// <summary>
	/// The time at which this blocker warning was spawned.
	/// </summary>
	public float SpawnTime { get; set; }

	/// <summary>
	/// The time this blocker warning will transform into an actual blocker and start blocking rocks (and maybe killing the player).
	/// </summary>
	public float TransformationTime { get; set; }

	/// <summary>
	/// The collider for this blocker warning, assigned in the editor. Identical to the collider in BlockerComponent, here for informative purposes.
	/// </summary>
	public BoxCollider2D Collider;
}
