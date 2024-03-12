using UnityEngine;
using UnityEngine.UI;

// This class is responsible for managing the game configuration
public class ConfigurationUI : MonoBehaviour
{
    [SerializeField] private SimpleAI simpleAI;

    [SerializeField] private GameConfiguration gameConfiguration;
    [SerializeField] private Toggle MovementType;
    [SerializeField] private Text MovementLabel;
    [SerializeField] private Toggle RockSpawnOverTime;
    [SerializeField] private Toggle CollectibleActive;
    [SerializeField] private Toggle BolckersActive;
    [SerializeField] private Toggle ShieldActive;
    [SerializeField] private Toggle CollectActive;

    [Header("ShieldBehaviourV")]
    [SerializeField] private Text ShieldBehaviourValue;

    [SerializeField] private Slider ShieldBehaviourSlider;

    [Header("CollectBehaviourV")]
    [SerializeField] private Text CollectBehaviourValue;

    [SerializeField] private Slider CollectBehaviourSlider;

    [Header("AvoidanceRadius")]
    [SerializeField] private Text AvoidanceRadiusValue;

    [SerializeField] private Slider AvoidanceRadiusSlider;

    private void Awake()
    {
   
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        simpleAI = FindAnyObjectByType<SimpleAI>();
        ShieldBehaviourSlider.value = simpleAI._shieldBehaviour.shieldSearchRadius;
        ShieldBehaviourValue.text = "Shield Behaviour Radius : " + ShieldBehaviourSlider.value;

        CollectBehaviourSlider.value = simpleAI._collectBehaviour._treasureRadius;
        CollectBehaviourValue.text = "Collect Behaviour Radius : " + CollectBehaviourSlider.value;

        AvoidanceRadiusSlider.value = simpleAI._dodgeBehaviour._avoidanceRadius;
        AvoidanceRadiusValue.text = "Avoidance Radius: " + AvoidanceRadiusSlider.value;

        RockSpawnOverTime.isOn = gameConfiguration.RocksSpawnFasterOverTime;
        CollectibleActive.isOn = gameConfiguration.CollectiblesActive;
        BolckersActive.isOn = gameConfiguration.BlockersActive;
        ShieldActive.isOn = gameConfiguration.ShieldActive;
        CollectActive.isOn = gameConfiguration.CollectActive;
        if (gameConfiguration.MovementMode == CharacterMovementMode.Velocity)
        {
            MovementType.isOn = true;
            MovementLabel.text = "Velocity";
        }
        else
        {
            MovementType.isOn = false;
            MovementLabel.text = "Acceleration";
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateConfiguration();
    }

    private void UpdateConfiguration()
    {
        simpleAI._shieldBehaviour.shieldSearchRadius = ShieldBehaviourSlider.value;
        ShieldBehaviourValue.text = "Shield Behaviour Radius : " + ShieldBehaviourSlider.value;

        simpleAI._collectBehaviour._treasureRadius = CollectBehaviourSlider.value;
        CollectBehaviourValue.text = "Collect Behaviour Radius : " + CollectBehaviourSlider.value;

        simpleAI._dodgeBehaviour._avoidanceRadius = AvoidanceRadiusSlider.value;
        simpleAI._collectBehaviour._avoidanceRadius = AvoidanceRadiusSlider.value;
        AvoidanceRadiusValue.text = "Avoidance Radius: " + AvoidanceRadiusSlider.value;

        gameConfiguration.RocksSpawnFasterOverTime = RockSpawnOverTime.isOn;
        gameConfiguration.CollectiblesActive = CollectibleActive.isOn;
        gameConfiguration.BlockersActive = BolckersActive.isOn;
        gameConfiguration.ShieldActive = ShieldActive.isOn;
        gameConfiguration.CollectActive = CollectActive.isOn;
        if (MovementType.isOn)
        {
            gameConfiguration.MovementMode = CharacterMovementMode.Velocity;
            MovementLabel.text = "Velocity";
        }
        else
        {
            gameConfiguration.MovementMode = CharacterMovementMode.Acceleration;
            MovementLabel.text = "Acceleration";
        }
    }
}