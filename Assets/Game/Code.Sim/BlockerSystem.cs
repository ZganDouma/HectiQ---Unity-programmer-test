using System.Collections.Generic;
using UnityEngine;

public class BlockerSystem : MonoBehaviour
{
	WorldSystem _worldSystem;

	[SerializeField] BlockerWarningComponent _BlockerWarningPrefab;
	[SerializeField] BlockerComponent _BlockerPrefab;

	#region Constants

	/// <summary>
	/// The time at which the first blocker will spawn. It will spawn as a warning, then transform into a blocker shortly after.
	/// </summary>
	public const float c_FirstBlockerTime = 5;

	/// <summary>
	/// The time between blocker spawns. It will spawn as a warning, then transform into a blocker shortly after.
	/// </summary>
	public const float c_BlockerSpawnInterval = 5;

	/// <summary>
	/// The average lifetime of a blocker.
	/// </summary>
	public const float c_BlockerLifetime = 10;

	/// <summary>
	/// The randomness in the lifetime of a blocker. The actual lifetime will be between minus and plus this value.
	/// </summary>
	public const float c_BlockerLifetimeRandomness = 2;

	/// <summary>
	/// The time it takes for a blocker warning to transform into a blocker.
	/// </summary>
	public const float c_BlockerWarningTransformationDelay = 2;

	#endregion

	#region Public Info

	/// <summary>
	/// The current active blockers.
	/// </summary>
	public List<BlockerComponent> Blockers = new();
	
	/// <summary>
	/// The current active blocker warnings.
	/// </summary>
	public List<BlockerWarningComponent> BlockerWarnings = new();

	#endregion

	float _lastSpawn; // the last time a blocker warning was spawned

	void Awake()
	{
		_worldSystem = FindFirstObjectByType<WorldSystem>();
	}
	void Start()
	{
		_lastSpawn = _worldSystem.Time + c_FirstBlockerTime - c_BlockerSpawnInterval;
	}
	void FixedUpdate()
	{
		if (!_worldSystem.GameConfiguration.BlockersActive) return;

		float time = _worldSystem.Time;
		float timeSinceLastSpawn = time - _lastSpawn;

		if (timeSinceLastSpawn > c_BlockerSpawnInterval)
		{
			int retries = 3;
			while (--retries >= 0)
			{
				if (_TrySpawnBlockerWarning(time))
					break;
			}
		}

		for (int i = 0, l = BlockerWarnings.Count; i < l; ++i)
		{
			var b = BlockerWarnings[i];
			var age = time - b.SpawnTime;
			if (age > c_BlockerWarningTransformationDelay)
			{
				Destroy(b.gameObject);
				BlockerWarnings.RemoveAt(i);
				--i;
				--l;

				float lifetimeRandomness = (Random.value * c_BlockerLifetimeRandomness * 2 - c_BlockerLifetimeRandomness);

				var o = Instantiate(_BlockerPrefab, b.transform.position, Quaternion.identity);
				o.SpawnTime = time;
				o.DeathTime = time + c_BlockerLifetime + lifetimeRandomness;
				Blockers.Add(o);
			}
		}

		for (int i = 0, l = Blockers.Count; i < l; ++i)
		{
			var b = Blockers[i];
			if (time >= b.DeathTime)
			{
				Destroy(b.gameObject);
				Blockers.RemoveAt(i);
				--i;
				--l;
			}
		}
	}

	bool _TrySpawnBlockerWarning(float time)
	{
		const int minTileDistanceFromBorder = 2;
		int tileMinX = _worldSystem.InsideLeftTileId - minTileDistanceFromBorder;
		int tileMaxX = _worldSystem.InsideRightTileId + minTileDistanceFromBorder;
		int tileMinY = _worldSystem.InsideBottomTileId + minTileDistanceFromBorder;
		int tileMaxY = _worldSystem.InsideTopTileId - minTileDistanceFromBorder;

		int randomX = Random.Range(tileMinX, tileMaxX + 1);
		int randomY = Random.Range(tileMinY, tileMaxY + 1);

		var pos = _worldSystem.CellCenterToWorld(randomX, randomY);

		var collider = Physics2D.OverlapBox(pos + _BlockerWarningPrefab.Collider.offset, _BlockerWarningPrefab.Collider.size, 0f);
		if (collider != null)
			return false;

		_lastSpawn = time;
		var o = Instantiate(_BlockerWarningPrefab, pos, Quaternion.identity);
		o.SpawnTime = time;
		o.TransformationTime = time + c_BlockerWarningTransformationDelay;
		BlockerWarnings.Add(o);
		return true;
	}
}
