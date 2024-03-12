using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CollectibleComponent))]
public class CollectibleView : MonoBehaviour
{
	CollectibleComponent _collectibleComponent;
	SpriteRenderer[] _renderers;

	void Awake()
	{
		_collectibleComponent = GetComponent<CollectibleComponent>();
		_renderers = GetComponentsInChildren<SpriteRenderer>();
	}

	void Start()
	{
		Color c;
		switch(CollectibleSystem.GetCollectibleValueIndex(_collectibleComponent.ScoreValue))
		{
			case 0:
				c = new(0.809f, 1, 0.99045f);
				break;
			case 1:
				c = new(0.6461752f, 1, 0.514151f);
				break;
			case 2:
				c = new(0.9899541f, 1, 0);
				break;
			default: throw new NotImplementedException();
		}

		for (int i = 0; i < _renderers.Length; i++)
		{
			var renderer = _renderers[i];
			renderer.color = c;
		}
	}
}