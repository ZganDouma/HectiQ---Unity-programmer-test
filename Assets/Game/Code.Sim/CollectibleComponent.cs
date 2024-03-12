using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CollectibleComponent : MonoBehaviour
{
	/// <summary>
	/// The time at which this collectible was spawned.
	/// </summary>
	public float SpawnTime { get; set; }

	/// <summary>
	/// The time at which this collectible will disappear.
	/// </summary>
	public float DeathTime { get; set; }

	/// <summary>
	/// The score value of this collectible; the number of points the will get for collecting it.
	/// </summary>
	public int ScoreValue { get; set; }

	/// <summary>
	/// The radius of this collectible. If the character collider touches this radius, the collectible is collected.
	/// </summary>
	public float Radius
	{
		get => Collider.radius;
		set => Collider.radius = value;
	}

	public CircleCollider2D Collider;

	WorldSystem _worldSystem;
	LayerMask _scoreLayerMask;
	bool _bDead;

	void Awake()
	{
		_worldSystem = FindFirstObjectByType<WorldSystem>();

		Collider = GetComponent<CircleCollider2D>();

		_scoreLayerMask = LayerMask.NameToLayer("Catacter");
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (_bDead) return;

		if(collider.gameObject.layer == _scoreLayerMask)
		{
			_bDead = true;
			_worldSystem.NotifyCollectibleCollected(this);
			_worldSystem.CollectibleSystem.DestroyCollectible(this);
		}
	}
}