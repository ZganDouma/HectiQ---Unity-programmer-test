using System.Linq;
using UnityEngine;

// this class is responsible for managing the Collect Behaviour of the AI
public class CollectBehaviour : MonoBehaviour
{
    public CollectibleSystem _collectibleSystem { get; set; }
    public RockSystem _rockSystem { get; set; }
    private WorldSystem _worldSystem;
    [SerializeField] public float _treasureRadius = 5f;
    [SerializeField] public float _avoidanceRadius = 1.5f; // Avoidance distance for rocks

    private CharacterComponent _character;
    private GameConfiguration _gameConfiguration;

    // Start is called before the first frame update
    private void Awake()
    {
        _worldSystem = FindAnyObjectByType<WorldSystem>();
        _rockSystem = _worldSystem.RockSystem;
        _gameConfiguration = _worldSystem.GameConfiguration;
        _collectibleSystem = _worldSystem.CollectibleSystem;
        _character = GetComponent<CharacterComponent>();
    }

    public bool TryCollect()
    {
        if (!_gameConfiguration.CollectActive) return false;

        var closestTreasure = _collectibleSystem.Collectibles
            .OrderBy(treasure => Vector2.Distance(_character.Position, treasure.transform.position))
            .FirstOrDefault(treasure => Vector2.Distance(_character.Position, treasure.transform.position) < _treasureRadius);

        if (closestTreasure != null)
        {
            Vector2 collectDirection = (closestTreasure.transform.position - _character.Position).normalized;

            // Check if a rock is in the way
            var obstacleRock = _rockSystem.Rocks
                .FirstOrDefault(rock => IsRockInPath(rock, collectDirection));

            if (obstacleRock != null)
            {
                // Adjust the direction to avoid the rock
                Vector2 avoidanceDirection = GetAvoidanceDirection(obstacleRock);
                _character.MovementDirection = avoidanceDirection;
            }
            else
            {
                _character.MovementDirection = collectDirection;
            }
            return true;
        }
        else
        {
            _character.MovementDirection = Vector2.zero; //Stand still if no treasure is nearby
            return false;
        }
    }

    private bool IsRockInPath(RockComponent rock, Vector2 direction)
    {
        Vector2 toRock = rock.transform.position - _character.Position;
        return Vector2.Dot(direction, toRock.normalized) > 0.9 && toRock.magnitude < _avoidanceRadius;
    }

    private Vector2 GetAvoidanceDirection(RockComponent rock)
    {
        Vector2 toRock = rock.transform.position - _character.Position;
        return Vector2.Perpendicular(toRock).normalized;
    }
}