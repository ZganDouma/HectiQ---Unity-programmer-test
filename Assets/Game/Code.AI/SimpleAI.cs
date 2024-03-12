using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterComponent))]
public class SimpleAI : MonoBehaviour
{
    private CharacterComponent _character;

    private CircleCollider2D _collider;

    public DodgeBehaviour _dodgeBehaviour;
    public CollectBehaviour _collectBehaviour;
    public ShieldBehaviour _shieldBehaviour;
    private EmotionalBehaviour _emotionalBehaviour;

    private float _radius;

    [SerializeField] private Text debugText;

    private void Start()
    {
        _character = GetComponent<CharacterComponent>();
        _collider = _character.Collider;

        _dodgeBehaviour = GetComponent<DodgeBehaviour>();
        _collectBehaviour = GetComponent<CollectBehaviour>();
        _shieldBehaviour = GetComponent<ShieldBehaviour>();
        _emotionalBehaviour = GetComponent<EmotionalBehaviour>();
        Assert.IsTrue(_collider.offset == Vector2.zero);
    }

    private void FixedUpdate()

    {
        bool tryUseShield = _shieldBehaviour.TryUseShield();
        bool tryDodge = _dodgeBehaviour.TryDodge();
        bool tryCollect = false; // Set to false by default

        if (!tryUseShield && !tryDodge)
        {
            // Attempt to collect only if neither shield nor dodge is used
            tryCollect = _collectBehaviour.TryCollect();
        }

        // Update debug text based on actions performed and update emotional state
        UpdateDebugTextAndEmotional(tryUseShield, tryDodge, tryCollect);
    }

    private void UpdateDebugTextAndEmotional(bool isUsingShield, bool isDodging, bool isCollecting)
    {
        if (isUsingShield && isDodging)
        {
            _emotionalBehaviour.UpdateFear(0.7f);
            debugText.text = "Shield and Dodge";
        }
        else if (isUsingShield && !isCollecting && !isDodging)
            debugText.text = "Idle and using Shield";
        else if (isUsingShield)
        {
            debugText.text = "Shield";
            _emotionalBehaviour.UpdateFear(0.1f);
        }
        else if (isDodging)
        {
            debugText.text = "Dodge";
            _emotionalBehaviour.UpdateFear(0.5f);
        }
        else if (isCollecting)
        {
            debugText.text = "Collect";
            _emotionalBehaviour.UpdateExcitement(0.1f);
        }
        else
        {
            _emotionalBehaviour.ResetEmotion();

            debugText.text = "Idle";
        }
    }
}