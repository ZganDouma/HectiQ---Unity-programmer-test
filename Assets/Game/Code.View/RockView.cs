using UnityEngine;

[RequireComponent (typeof(RockComponent))]
public class RockView : MonoBehaviour
{
	public Transform Pivot;
	
	RockComponent _rock;

	void Awake()
	{
		_rock = GetComponent<RockComponent> ();
	}
	void LateUpdate()
	{
		var r = _rock.Radius * 2;
		Pivot.localScale = new Vector3(r, r, r);
	}
}