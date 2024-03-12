using UnityEngine;

[RequireComponent(typeof(CharacterComponent))]
public class PlayerCharacterController : MonoBehaviour
{
	private Animator _animator;
	private CharacterComponent _characterComponent;

	private void Start()
	{
		_characterComponent = GetComponent<CharacterComponent>();
		_animator = GetComponentInChildren<Animator>();
		_animator.SetInteger("Direction", 0);
	}

	private void Update()
	{
		Vector2 dir = Vector2.zero;
		if (Input.GetKey(KeyCode.A)) dir.x += -1;
		if (Input.GetKey(KeyCode.D)) dir.x += 1;
		if (Input.GetKey(KeyCode.W)) dir.y += 1;
		if (Input.GetKey(KeyCode.S)) dir.y += -1;
		dir.Normalize();

		_animator.SetBool("IsMoving", dir.magnitude > 0);

		_characterComponent.MovementDirection = dir;
	}
}