using UnityEngine;

public class Rotate : MonoBehaviour
{
	public float MinRotationSpeed = Mathf.PI;
	public float MaxRotationSpeed = Mathf.PI * 0.5f;
	float _angle = 0;
	float _rotationSpeed;
	private void OnEnable()
	{
		_angle = Random.value * Mathf.PI * 2;
		_rotationSpeed = Mathf.Lerp(MinRotationSpeed, MaxRotationSpeed, Random.value);
	}
	void Update()
	{
		_angle += Time.deltaTime * _rotationSpeed;
		transform.localRotation = Quaternion.Euler(0, 0, _angle * Mathf.Rad2Deg);
	}
}
