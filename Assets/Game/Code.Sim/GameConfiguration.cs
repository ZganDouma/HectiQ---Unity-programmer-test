using UnityEngine;

public enum CharacterMovementMode
{
	Velocity,
	Acceleration,
}

// Options:
// - Movement mode
// - Blockers or no blockers
// - Collectibles

// todo:
// - expose some basics (write a simple AI?)
// - summary for all public parameters
// - collectible to trigger instead of collider

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 1)]
public class GameConfiguration : ScriptableObject
{
	[Header("Catacter")]
	[Tooltip("In Velocity mode, the mdesired direction is instantly transformed into the maximum velocity. In Acceleration mode, it takes time to reach maximum velocity.")]
	public CharacterMovementMode MovementMode;

	[Header("Rocks")]
	[Tooltip("When enabled, rocks spawn faster over time. This is suicidal, doesn't yield extra points in the game, and depresses most people.")]
	public bool RocksSpawnFasterOverTime;

	[Header("Blockers")]
	[Tooltip("When enabled, pillars appear that block rocks. They also kill you, though, but at least they don't move.")]
	public bool BlockersActive;

	[Header("Collectibles")]
	[Tooltip("When enabled, collectibles appear with various values. This gives you points and makes you feel good.")]
	public bool CollectiblesActive;

    [Header("ShieldBehaviour")]
    [Tooltip("When enabled, ShieldBehaviour Make the AI work with blockers ")]
    public bool ShieldActive;

    [Header("CollectBehaviour")]
    [Tooltip("When enabled, ShieldBehaviour Make the AI collect treasures")]
    public bool CollectActive;
}