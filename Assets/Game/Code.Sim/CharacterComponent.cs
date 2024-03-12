using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CharacterComponent : MonoBehaviour
{
	[SerializeField] GameObject DeadCatacter;

	public Rigidbody2D Rigidbody => _rigidbody;
	public CircleCollider2D Collider => _collider;

	#region Constants

	/// <summary>
	/// Used in mode Velocity and Acceleration
	/// </summary>
	public const float c_MaxSpeed = 4;

	/// <summary>
	/// Used in mode Acceleration only
	/// </summary>
	public const float c_Acceleration = 4;

	#endregion

	#region Controller Values

	/// <summary>
	/// The desired movement direction for this character. This value is to be modified by any character controllers, be it player or AI. <br></br>
	/// If the movement mode is set to Velocity, this value will be multiplied by c_MaxSpeed and used as the rigidbody's velocity. <br></br>
	/// If the movement mode is set to Acceleration, this value will be multiplied by c_Acceleration and used to accelerate the rigidbody. <br></br>
	/// If the magnitude of this vector is greater than 1, it will be normalized, otherwise it will be used as-is.
	/// </summary>
	public Vector2 MovementDirection;

	#endregion

	#region Properties

	/// <summary>
	/// The position of the character.
	/// </summary>
	public Vector3 Position => _rigidbody.position;
	/// <summary>
	/// The velocity of the character. This is not immediately modified when MovementDirection is changed, but is modified in the FixedUpdate.
	/// </summary>
	public Vector3 Velocity => _rigidbody.velocity;
	public float Speed => _rigidbody.velocity.magnitude;
	public float Radius => _collider.radius;

	#endregion

	WorldSystem _worldSystem;

	Rigidbody2D _rigidbody;
	CircleCollider2D _collider;
	LayerMask _deathMask;
	bool _bDead;

	void Awake()
	{
		_deathMask = LayerMask.NameToLayer("Rock");
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_worldSystem = FindFirstObjectByType<WorldSystem>();
	}
	void FixedUpdate()
	{
		var velocity = _rigidbody.velocity;

		if(MovementDirection.sqrMagnitude > 1)
			MovementDirection = MovementDirection.normalized;

		switch (_worldSystem.GameConfiguration.MovementMode)
		{
			case CharacterMovementMode.Acceleration:
				velocity += MovementDirection * c_Acceleration * Time.fixedDeltaTime;
				velocity = Vector3.ClampMagnitude(velocity, c_MaxSpeed);
				break;
			case CharacterMovementMode.Velocity:
				velocity = c_MaxSpeed * MovementDirection;
				break;
			default:
				throw new NotImplementedException();
		}
		_rigidbody.velocity = velocity;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_bDead) return;

		bool bDie = collision.gameObject.layer == _deathMask.value;
		if (bDie)
		{
			_bDead = true;
			gameObject.SetActive(false);
			_worldSystem.NotifyCatacterDeath();
			Instantiate(DeadCatacter, transform.position, transform.rotation);
		}
	}
}
