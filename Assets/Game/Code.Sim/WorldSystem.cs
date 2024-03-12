using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldSystem : MonoBehaviour
{
	public GameConfiguration GameConfiguration;

	#region Level Data

	/// <summary>
	/// The colliders that define the level's boundaries.
	/// </summary>
	public Collider2D LeftCollider, RightCollider, BottomCollider, TopCollider;

	/// <summary>
	/// The tilemap used for rendering and object spawning in the level.
	/// </summary>
	public Tilemap Tilemap;

	/// <summary>
	/// The left-most boundary of the level rectangle.
	/// </summary>
	public float LeftPos { get; private set; }
	/// <summary>
	/// The right-most boundary of the level rectangle.
	/// </summary>
	public float RightPos { get; private set; }
	/// <summary>
	/// The bottom-most boundary of the level rectangle.
	/// </summary>
	public float BottomPos { get; private set; }
	/// <summary>
	/// The top-most boundary of the level rectangle.
	/// </summary>
	public float TopPos { get; private set; }

	/// <summary>
	/// The first tile id x-coordinate inside the level rectangle, from the left.
	/// </summary>
	public int InsideLeftTileId { get; private set; }
	/// <summary>
	/// The first tile id x-coordinate inside the level rectangle, from the right.
	/// </summary>
	public int InsideRightTileId { get; private set; }
	/// <summary>
	/// The first tile id y-coordinate inside the level rectangle, from the bottom.
	/// </summary>
	public int InsideBottomTileId { get; private set; }
	/// <summary>
	/// The first tile id y-coordinate inside the level rectangle, from the top.
	/// </summary>
	public int InsideTopTileId { get; private set; }

	/// <summary>
	/// The first tile-id x-coordinate outside the level rectangle, on the left.
	/// </summary>
	public int OutsideLeftTileId => InsideLeftTileId - 1;
	/// <summary>
	/// The first tile-id x-coordinate outside the level rectangle, on the right.
	/// </summary>
	public int OutsideRightTileId => InsideRightTileId + 1;
	/// <summary>
	/// The first tile-id y-coordinate outside the level rectangle, at the bottom.
	/// </summary>
	public int OutsideBottomTileId => InsideBottomTileId - 1;
	/// <summary>
	/// The first tile-id y-coordinate outside the level rectangle, at the top.
	/// </summary>
	public int OutsideTopTileId => InsideTopTileId + 1;

	/// <summary>
	/// The size of the cells in the tilemap.
	/// </summary>
	public Vector3 CellSize => Tilemap.cellSize;

	#endregion

	#region Game Objects

	/// <summary>
	/// The only character in the game. It's a cat, called Ton. He's single.
	/// </summary>
	public CharacterComponent Character { get; private set; }
	/// <summary>
	/// The system that manages the rocks in the game. Look here for information on rocks (not minerals).
	/// </summary>
	public RockSystem RockSystem { get; private set; }
	/// <summary>
	/// The system that manages the blocker warnings and blockers in the game.
	/// </summary>
	public BlockerSystem BlockerSystem { get; private set; }
	/// <summary>
	/// The system that manages the collectibles in the game.
	/// </summary>
	public CollectibleSystem CollectibleSystem { get; private set; }

	#endregion

	/// <summary>
	/// The current game time. It will stop when the character dies. Also part of score.
	/// </summary>
	public float Time { get; private set; }
	/// <summary>
	/// The time at which the game started.
	/// </summary>
	public float StartTime { get; private set; }
	/// <summary>
	/// The score, defined as Time + the value of all collectibles collected.
	/// </summary>
	public int Score => (int)Time + _collectibleScore;
	/// <summary>
	/// Are we dead?
	/// </summary>
	public bool GameOver => _bCatacterDead;

	#region Cell Helpers

	/// <summary>
	/// Converts a 2d cell coordinate to a world position. For this game's purposes, z can be ignored.
	/// </summary>
	public Vector3 CellToWorld(int x, int y) => Tilemap.CellToWorld(new(x, y));
	/// <summary>
	/// Converts a 2d cell coordinate to a world position. For this game's purposes, z can be ignored.
	/// </summary>
	public Vector3 CellToWorld(Vector2Int v) => Tilemap.CellToWorld(new(v.x, v.y));

	/// <summary>
	/// Returns the world position of the center of the cell as opposed to the corner.
	/// </summary>
	public Vector2 CellCenterToWorld(int x, int y) => Tilemap.CellToWorld(new(x, y)) + Tilemap.cellSize * .5f;
	/// <summary>
	/// Returns the world position of the center of the cell as opposed to the corner.
	/// </summary>
	public Vector2 CellCenterToWorld(Vector2Int v) => Tilemap.CellToWorld(new(v.x, v.y)) + Tilemap.cellSize * .5f;

	/// <summary>
	/// Transforms a 2d world position to a 2d cell coordinate.
	/// </summary>
	public (int x, int y) WorldToCell(float x, float y)
	{
		var cellPos = Tilemap.WorldToCell(new Vector3(x, y, 0));
		return (cellPos.x, cellPos.y);
	}

	#endregion

	#region Public Methods so that we can be notified of game events.

	public void NotifyCatacterDeath()
	{
		_bCatacterDead = true;
	}
	public void NotifyCollectibleCollected(CollectibleComponent c)
	{
		_collectibleScore += c.ScoreValue;
	}

	#endregion

	bool _bCatacterDead;
	int _collectibleScore;

	private void Awake()
	{
		Character = FindFirstObjectByType<CharacterComponent>();
		RockSystem = FindFirstObjectByType<RockSystem>();
		BlockerSystem = FindFirstObjectByType<BlockerSystem>();
		CollectibleSystem = FindFirstObjectByType<CollectibleSystem>();

		float halfCellSizeX = .5f * Tilemap.cellSize.x;
		float halfCellSizeY = .5f * Tilemap.cellSize.y;
		Vector3 leftPos = LeftCollider.transform.position;
		Vector3 rightPos = RightCollider.transform.position;
		Vector3 bottomPos = BottomCollider.transform.position;
		Vector3 topPos = TopCollider.transform.position;

		InsideLeftTileId = Tilemap.WorldToCell(leftPos).x + 1;
		InsideRightTileId = Tilemap.WorldToCell(rightPos).x - 1;
		InsideBottomTileId = Tilemap.WorldToCell(bottomPos).y + 1;
		InsideTopTileId = Tilemap.WorldToCell(topPos).y - 1;

		leftPos.x += halfCellSizeX;
		rightPos.x -= halfCellSizeX;
		bottomPos.y += halfCellSizeY;
		topPos.y -= halfCellSizeY;

		LeftPos = leftPos.x;
		RightPos = rightPos.x;
		BottomPos = bottomPos.y;
		TopPos = topPos.y;
	}
	private void Start()
	{
		Time = UnityEngine.Time.fixedTime;
		StartTime = Time;
	}
	void FixedUpdate()
	{
		if (!_bCatacterDead)
			Time = UnityEngine.Time.fixedTime;
	}
}
