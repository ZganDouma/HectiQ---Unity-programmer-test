using System.Linq;
using UnityEngine;


// This class is responsible for managing the shield behaviour of the AI
[RequireComponent(typeof(CharacterComponent))]
public class ShieldBehaviour : MonoBehaviour
{
    private CharacterComponent _character;
    private RockSystem _rockSystem;
    private BlockerSystem _blockerSystem;
    private WorldSystem _worldSystem;
    private GameConfiguration _gameConfiguration;

   public float shieldSearchRadius = 3f; //Search radius for a blocker usable as a shield
    private void Awake()
    {
        _character = GetComponent<CharacterComponent>();
        _worldSystem= FindAnyObjectByType<WorldSystem>();
        _rockSystem = _worldSystem.RockSystem;
        _blockerSystem = _worldSystem.BlockerSystem;
        _gameConfiguration = _worldSystem.GameConfiguration;
   }

public bool TryUseShield()
    {
        if (!_gameConfiguration.ShieldActive) return false;

        // Identify the nearest threatening rock
        var threateningRock = _rockSystem.Rocks
            .Select(rock => new { Rock = rock, Distance = Vector2.Distance(_character.Position, rock.transform.position) })
            .Where(rockInfo => rockInfo.Distance < shieldSearchRadius)
            .OrderBy(rockInfo => rockInfo.Distance)
            .FirstOrDefault();

        if (threateningRock == null) return false;

        // Find a blocker that can act as a shield between the AI and the rock
        foreach (var blocker in _blockerSystem.Blockers)
        {
            Vector2 toBlocker = blocker.transform.position - _character.Position;
            Vector2 toRock = threateningRock.Rock.transform.position - _character.Position;

            // Check if the blocker is between the AI and the rock
            if (Vector2.Dot(toBlocker.normalized, toRock.normalized) > 0.9f && toBlocker.magnitude < toRock.magnitude)
            {
                // Move towards blocker to use as a shield
                _character.MovementDirection = toBlocker.normalized;
                return true;
            }
        }

        return false;
    }
}
