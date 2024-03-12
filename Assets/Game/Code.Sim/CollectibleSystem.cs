using System.Collections.Generic;
using UnityEngine;

public class CollectibleSystem : MonoBehaviour
{
	WorldSystem _worldSystem;

	[SerializeField] CollectibleComponent _CollectiblePrefab;

	#region Constants

	/// <summary>
	/// The time at which the first collectible will spawn.
	/// </summary>
	public const float c_FirstCollectibleTime = 2;

	/// <summary>
	/// The interval between collectible spawns.
	/// </summary>
	public const float c_CollectibleSpawnInterval = 7;
	
	/// <summary>
	/// The average lifetime of a collectible.
	/// </summary>
	public const float c_CollectibleLifetime = 11;

	/// <summary>
	/// The randomness in the lifetime of a collectible. The actual lifetime will be between minus and plus this value.
	/// </summary>
	public const float c_CollectibleLifetimeRandomness = 2;

	/// <summary>
	/// The different possible score values for collectibles.
	/// </summary>
	public static readonly int[] c_CollectibleScoreValues = { 1, 5, 13 };

	/// <summary>
	/// The lowest possible score value for a collectible.
	/// </summary>
	public static int c_CollectibleScoreValueMin => c_CollectibleScoreValues[0];

	/// <summary>
	/// The highest possible score value for a collectible.
	/// </summary>
	public static int c_CollectibleScoreValueMax => c_CollectibleScoreValues[c_CollectibleScoreValues.Length-1];

	/// <summary>
	/// Given a score value, returns the index in the list of possible score values, or -1 if not found.
	/// </summary>
	public static int GetCollectibleValueIndex(int scoreValue)
	{
		for (int i = 0; i < c_CollectibleScoreValues.Length; i++)
		{
			if (c_CollectibleScoreValues[i] == scoreValue)
				return i;
		}
		return -1;
	}

	#endregion

	#region Public Info

	/// <summary>
	/// The current active collectibles.
	/// </summary>
	public List<CollectibleComponent> Collectibles = new();

	#endregion

	float _lastSpawn; // the last time a collectible was spawned

	public void DestroyCollectible(CollectibleComponent collectible)
	{
		var index = Collectibles.IndexOf(collectible);
		_DestroyCollectible(index);
	}
	void _DestroyCollectible(int index)
	{
		var collectible = Collectibles[index];
		Collectibles.RemoveAt(index);
		Destroy(collectible.gameObject);
	}

	void Awake()
	{
		_worldSystem = FindFirstObjectByType<WorldSystem>();
	}
	void Start()
	{
		_lastSpawn = _worldSystem.Time + c_FirstCollectibleTime - c_CollectibleSpawnInterval;
	}
	void FixedUpdate()
	{
		if (!_worldSystem.GameConfiguration.CollectiblesActive) return;

		float time = _worldSystem.Time;
		float timeSinceLastSpawn = time - _lastSpawn;

		if (timeSinceLastSpawn > c_CollectibleSpawnInterval)
		{
			int retries = 3;
			while (--retries >= 0)
			{
				if (_TrySpawnCollectible(time))
					break;
			}
		}

		for (int i = 0, l = Collectibles.Count; i < l; ++i)
		{
			var c = Collectibles[i];
			if (c.DeathTime < time)
			{
				_DestroyCollectible(i);
				--i;
				--l;
				break;
			}
		}
	}

	bool _TrySpawnCollectible(float time)
	{
		const int minTileDistanceFromBorder = 1;
		int tileMinX = _worldSystem.InsideLeftTileId - minTileDistanceFromBorder;
		int tileMaxX = _worldSystem.InsideRightTileId + minTileDistanceFromBorder;
		int tileMinY = _worldSystem.InsideBottomTileId + minTileDistanceFromBorder;
		int tileMaxY = _worldSystem.InsideTopTileId - minTileDistanceFromBorder;

		int randomX = Random.Range(tileMinX, tileMaxX + 1);
		int randomY = Random.Range(tileMinY, tileMaxY + 1);

		var pos = _worldSystem.CellCenterToWorld(randomX, randomY);
		
		var collider = Physics2D.OverlapCircle(pos + _CollectiblePrefab.Collider.offset, _CollectiblePrefab.Collider.radius);
		if (collider != null)
			return false;

		_lastSpawn = time;
		var o = Instantiate(_CollectiblePrefab, pos, Quaternion.identity);
		o.SpawnTime = time;
		o.DeathTime = time + c_CollectibleLifetime + Random.Range(-c_CollectibleLifetimeRandomness, c_CollectibleLifetimeRandomness);
		o.ScoreValue = c_CollectibleScoreValues[Random.Range(0, c_CollectibleScoreValues .Length)];
		Collectibles.Add(o);
		return true;
	}
}
