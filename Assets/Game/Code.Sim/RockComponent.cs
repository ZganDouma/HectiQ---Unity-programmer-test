using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class RockComponent : MonoBehaviour
{
	public int Id;

	/// <summary>
	/// The radius of this rock. If the character collider touches this radius, the character dies.
	/// </summary>
	public float Radius
	{
		get => _collider.radius; 
		set => _collider.radius = value;
	}

	/// <summary>
	/// The speed of this rock.
	/// </summary>
	public float Speed => _rigidbody.velocity.magnitude;

	/// <summary>
	/// The velocity of this rock.
	/// </summary>
	public Vector3 Velocity => _rigidbody.velocity;

	/// <summary>
	/// The current position of this rock.
	/// </summary>
	public Vector3 Position => _rigidbody.position;

	public Rigidbody2D Rigidbody => _rigidbody;
	public CircleCollider2D Collider => _collider;

	public RockSystem RockSystem { get; set; }

	Rigidbody2D _rigidbody;
	CircleCollider2D _collider;

	LayerMask _surviveLayer;
	bool _bDead;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_surviveLayer = LayerMask.NameToLayer("Catacter");
	}
	private void FixedUpdate()
	{
		if (RockSystem.WorldSystem.GameOver)
			Rigidbody.velocity = Vector3.zero;
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(_bDead) return;

		bool bDeath = collision.gameObject.layer != _surviveLayer.value;
		if (bDeath)
		{
			_bDead = true; // protect against double collision in a frame
			RockSystem.DestroyRock(this);
		}
	}
}
