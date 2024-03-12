using System.Collections.Generic;
using UnityEngine;

public class RockSystem : MonoBehaviour
{
	public WorldSystem WorldSystem => _worldSystem;

	[SerializeField] RockComponent _RockPrefab;

	#region Constants

	/// <summary>
	/// The time at which the first rock will spawn.
	/// </summary>
	public const float c_FirstRockTime = 3;

	/// <summary>
	/// The initial time between rock spawns.
	/// </summary>
	public const float c_RockSpawnIntervalBegin = 2f;

	/// <summary>
	/// The minimum time between rock spawns.
	/// </summary>
	public const float c_RockSpawnIntervalEnd = 3 / 50f; // one every 3 frames, ~17 per second

	/// <summary>
	/// Time it takes to reduce the rock spawn interval to the minimum (10 per second). Reduced exponentially, not linearly.
	/// </summary>
	public const float c_RockSpawnIntervalDecrementDuration = 4 * 60;

	/// <summary>
	/// The smallest radius a rock can have. This value is interpolated with c_RockRadiusMax with a quadratic curve, i.e. most rocks will be closer to the minimum.
	/// </summary>
	public const float c_RockRadiusMin = 0.25f;

	/// <summary>
	/// The largest radius a rock can have. This value is interpolated with c_RockRadiusMin with a quadratic curve, i.e. most rocks will be closer to the minimum.
	/// </summary>
	public const float c_RockRadiusMax = .8f;

	/// <summary>
	/// The minimum rock speed.
	/// </summary>
	public const float c_RockSpeedMin = 1.5f;

	/// <summary>
	/// The maximum rock speed.
	/// </summary>
	public const float c_RockSpeedMax = 4;

	/// <summary>
	/// The interval at which a rock will be aimed at the player.
	/// </summary>
	public const float c_AimAtCatacterInterval = 2;

	/// <summary>
	/// Whether rocks will spawn with a variable radius.
	/// </summary>
	public const bool c_UseRockVariableRadius = true;

	#endregion

	#region Public Info

	/// <summary>
	/// The current rocks in the game.
	/// </summary>
	public List<RockComponent> Rocks = new();

	#endregion

	WorldSystem _worldSystem;

	float _lastSpawn; // the time at which the last rock was spawned
	float _rockSpawnInterval; // the current interval between rock spawns
	int _nextAimAtCatacterRockId; // the next rock id that will be aimed at the player
	int _nextRockId; // the next spawned rock's id

	public void DestroyRock(RockComponent rock)
	{
		var index = Rocks.IndexOf(rock);
		_DestroyRock(index);
	}
	void _DestroyRock(int index)
	{
		var rock = Rocks[index];
		Rocks.RemoveAt(index);
		Destroy(rock.gameObject);
	}

	void Awake()
	{
		_worldSystem = FindFirstObjectByType<WorldSystem>();
	}
	void Start()
	{
		_lastSpawn = WorldSystem.Time + c_FirstRockTime - c_RockSpawnIntervalBegin;
		_rockSpawnInterval = c_RockSpawnIntervalBegin;
	}
	void FixedUpdate()
	{
		float time = WorldSystem.Time;

		float timeSinceStart = WorldSystem.Time - WorldSystem.StartTime;
		float timeSinceLastSpawn = time - _lastSpawn;

		for (int i = 0, l = Rocks.Count; i < l; ++i)
		{
			var rock = Rocks[i];

			var outOfRangeThreshold = c_RockRadiusMax * 2;

			var pos = rock.Rigidbody.position;
			if (pos.x < WorldSystem.LeftPos - outOfRangeThreshold
				|| pos.x > WorldSystem.RightPos + outOfRangeThreshold
				|| pos.y < WorldSystem.BottomPos - outOfRangeThreshold
				|| pos.y > WorldSystem.TopPos + outOfRangeThreshold)
			{
				_DestroyRock(i);
				--i;
				--l;
			}
		}

		if (WorldSystem.GameConfiguration.RocksSpawnFasterOverTime)
		{
			// desmos: \left(1-\frac{x}{a}\right)^{3}
			float spawnIntervalProgression = 1 - timeSinceStart / c_RockSpawnIntervalDecrementDuration;
			spawnIntervalProgression = Mathf.Clamp01(spawnIntervalProgression * spawnIntervalProgression * spawnIntervalProgression * spawnIntervalProgression);
			_rockSpawnInterval = Mathf.Lerp(c_RockSpawnIntervalEnd, c_RockSpawnIntervalBegin, spawnIntervalProgression);
		}

		if (timeSinceLastSpawn > _rockSpawnInterval)
		{
			_lastSpawn = WorldSystem.Time;

			bool bAimAtCatacter = _nextRockId == _nextAimAtCatacterRockId;
			if (bAimAtCatacter)
			{
				_nextAimAtCatacterRockId = _nextRockId + Mathf.Max(3, (int)(1 / _rockSpawnInterval * c_AimAtCatacterInterval));
				//Debug.Log($"AimAtCatacter: {_nextRockId}");
				//Debug.Log($"NextRockAtCatacter: {_nextAimAtCatacterRockId}");
			}
			float worldCenterX = (WorldSystem.LeftPos + WorldSystem.RightPos) * .5f;
			float worldCenterY = (WorldSystem.BottomPos + WorldSystem.TopPos) * .5f;

			bool bTopOrBottom = Random.Range(0, 2) == 0;

			Vector3 targetPosMin, targetPosMax;
			Vector3 spawnPos;
			if (bTopOrBottom)
			{
				bool bBottom = Random.Range(0, 2) == 0;

				Vector2Int randomSpawnTileId = default;
				randomSpawnTileId.x = Random.Range(WorldSystem.OutsideLeftTileId, WorldSystem.OutsideRightTileId);
				randomSpawnTileId.y = bBottom ? WorldSystem.OutsideBottomTileId : WorldSystem.OutsideTopTileId;

				targetPosMin = new(WorldSystem.LeftPos, worldCenterY);
				targetPosMax = new(WorldSystem.RightPos, worldCenterY);

				spawnPos = WorldSystem.CellCenterToWorld(randomSpawnTileId);
				spawnPos.y += bBottom ? -c_RockRadiusMax : c_RockRadiusMax;
			}
			else
			{
				bool bLeft = Random.Range(0, 2) == 0;

				Vector2Int randomSpawnTileId = default;
				randomSpawnTileId.x = bLeft ? WorldSystem.OutsideLeftTileId : WorldSystem.OutsideRightTileId;
				randomSpawnTileId.y = Random.Range(WorldSystem.OutsideBottomTileId, WorldSystem.OutsideTopTileId);

				targetPosMin = new(worldCenterX, WorldSystem.BottomPos);
				targetPosMax = new(worldCenterX, WorldSystem.TopPos);

				spawnPos = WorldSystem.CellCenterToWorld(randomSpawnTileId);
				spawnPos.x += bLeft ? -c_RockRadiusMax : c_RockRadiusMax;
			}

			Vector3 targetPos = bAimAtCatacter ? WorldSystem.Character.Rigidbody.position : Vector3.Lerp(targetPosMin, targetPosMax, Random.value);
			Vector3 dir = (targetPos - spawnPos).normalized;
			float speed = Mathf.Lerp(c_RockSpeedMin, c_RockSpeedMax, Random.value);
			float radius = c_RockRadiusMin;
			if (c_UseRockVariableRadius)
			{
				var r1 = Random.value;
				r1 = r1 * r1;
				radius = Mathf.Lerp(c_RockRadiusMin, c_RockRadiusMax, r1);
			}

			var rock = Instantiate(_RockPrefab, spawnPos, Quaternion.identity);
			rock.Id = _nextRockId;
			rock.RockSystem = this;
			rock.Rigidbody.velocity = dir * speed;
			rock.Radius = radius;

			Rocks.Add(rock);

			_nextRockId = _nextRockId + 1;
		}
	}
}