using System.Collections.Generic;
using UnityEngine;

// This class is responsible for managing the emotional state of the AI
[RequireComponent(typeof(CharacterComponent))]
public class DodgeBehaviour : MonoBehaviour
{
    [SerializeField] public float _avoidanceRadius = 3f;
    private CharacterComponent _character;
    private WorldSystem _worldSystem;

    private void Awake()
    {
        _character = GetComponent<CharacterComponent>();
        _worldSystem = FindAnyObjectByType<WorldSystem>();
    }

    public bool TryDodge()
    {
        Vector2 moveDirection = Vector2.zero;
        bool isDodging = false;

        // Obtaining all active rocks in the scene
        var rocks = _worldSystem.RockSystem.Rocks;

        foreach (var rock in rocks)
        {
            Vector2 rockDirection = rock.Rigidbody.velocity.normalized;
            Vector2 rockToCharacter = _character.Position - rock.transform.position;
            float distanceToRock = rockToCharacter.magnitude;

            // Check if the rock is in the avoidance zone and moving towards the character
            if (distanceToRock < _avoidanceRadius && Vector2.Dot(rockDirection, rockToCharacter.normalized) > 0)
            {
                Vector2 avoidanceDirection = Vector2.Perpendicular(rockDirection).normalized;
                if (Vector2.Dot(avoidanceDirection, rockToCharacter.normalized) < 0)
                {
                    avoidanceDirection = -avoidanceDirection;
                }

                moveDirection += avoidanceDirection;
                isDodging = true;
                break; //We dodge the first threatening rock, priority for dodging
            }
        }

        if (isDodging)
        {
            moveDirection = AdjustDirectionForWorldBounds(moveDirection);
            moveDirection.Normalize();
            _character.MovementDirection = moveDirection;
        }

        return isDodging;
    }

    private Vector2 AdjustDirectionForWorldBounds(Vector2 moveDirection)
    {
        Vector3 characterPosition = _character.Position;
        float leftBound = _worldSystem.LeftPos;
        float rightBound = _worldSystem.RightPos;
        float bottomBound = _worldSystem.BottomPos;
        float topBound = _worldSystem.TopPos;

        if ((characterPosition.x <= leftBound && moveDirection.x < 0) ||
            (characterPosition.x >= rightBound && moveDirection.x > 0))
        {
            moveDirection.x = 0; // Prevent out-of-bounds horizontal movement
        }

        if ((characterPosition.y <= bottomBound && moveDirection.y < 0) ||
            (characterPosition.y >= topBound && moveDirection.y > 0))
        {
            moveDirection.y = 0; // Prevent out-of-bounds vertical movement
        }

        // If the adjusted direction is zero (the character is in a corner), choose a perpendicular direction to move away from the edge
        if (moveDirection == Vector2.zero)
        {
            if (characterPosition.x <= leftBound || characterPosition.x >= rightBound)
            {
                moveDirection.y = 1; // Move vertically if you are close to the left or right edge
            }
            else if (characterPosition.y <= bottomBound || characterPosition.y >= topBound)
            {
                moveDirection.x = 1; // Move horizontally if close to the bottom or top edge
            }
        }

        return moveDirection;
    }
}
