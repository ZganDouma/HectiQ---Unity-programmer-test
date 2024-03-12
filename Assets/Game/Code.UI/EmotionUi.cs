using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionUi : MonoBehaviour
{
    [SerializeField] private EmotionalBehaviour emotionalBehaviour;

    [SerializeField] private List<EmotionalType> emotionalTypes;

    [SerializeField] private Image spriteIcon;
    [SerializeField] private Text stateText;

    private Vector3 normalSize = new Vector3(1, 1, 1);
    private Vector3 enlargedSize = new Vector3(1.5f, 1.5f, 1);
    private float scaleSpeed = 2f;
    private float timeToChange = 1;

    // Update is called once per frame
    private void Update()
    {
        spriteIcon.sprite = emotionalTypes.Find(x => x.state == emotionalBehaviour.CurrentState).sprite;
        stateText.text = emotionalBehaviour.CurrentState.ToString();
        AnimateIconScaling();
    }

    private void AnimateIconScaling()
    {
        Vector3 targetScale = Time.time % (2 * timeToChange) < timeToChange ? normalSize : enlargedSize;
        spriteIcon.transform.localScale = Vector3.Lerp(spriteIcon.transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }
}

[System.Serializable]
public struct EmotionalType
{
    public Sprite sprite;
    public EmotionalState state;
}