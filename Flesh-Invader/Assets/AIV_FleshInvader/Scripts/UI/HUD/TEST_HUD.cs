using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_HUD : MonoBehaviour
{
    //buttons
    private Button increaseHealth;
    private Button decreaseHealth;
    private Button increaseXP;
    private Button decreaseXP;
    private Button possessBoss;
    private Button possessThief;
    private Button mission;
    private Button increaseLevel;

    //parameters
    private int maxHP = 10;
    private int currentHP = 0;
    private int maxXP = 10;
    private int currentXP = 0;
    private int totalMissionObjects = 5;
    private int currentMissionObjects = 0;
    private int currentLevel = 1;

    //icons
    [SerializeField]
    private Texture2D bossIcon;
    [SerializeField]
    private Texture2D thiefIcon;

    private void Awake() {
        increaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-health");
        decreaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-health");
        increaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-xp");
        decreaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-xp");
        possessBoss = GetComponent<UIDocument>().rootVisualElement.Q<Button>("possess-boss");
        possessThief = GetComponent<UIDocument>().rootVisualElement.Q<Button>("possess-thief");
        mission = GetComponent<UIDocument>().rootVisualElement.Q<Button>("found-mission-object");
        increaseLevel = GetComponent<UIDocument>().rootVisualElement.Q<Button>("next-level");
    }

    void Start()
    {
        increaseHealth.clickable.clicked += delegate {
            currentHP++;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        decreaseHealth.clickable.clicked += delegate {
            currentHP--;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        increaseXP.clickable.clicked += delegate {
            currentXP++;
            currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            LevelStruct level = new LevelStruct();
            level.CurrentLevel = 1;
            level.CurrentXP = currentXP;
            level.NextLevelXp = maxXP;  
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(level));
        };
        decreaseXP.clickable.clicked += delegate {
            currentXP--;
            currentXP = currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            LevelStruct level = new LevelStruct();
            level.CurrentLevel = 1;
            level.CurrentXP = currentXP;
            level.NextLevelXp = maxXP;
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(level));
        };
        possessBoss.clickable.clicked += delegate {
            EnemyStatistics enemyStatistics = new EnemyStatistics();
            enemyStatistics.EnemyType = EnemyType.Boss;
            enemyStatistics.Damage = 25;
            enemyStatistics.BaseSpeed = 10;
            EnemyNarrative enemyNarrative = new EnemyNarrative();
            enemyNarrative.icon = bossIcon;
            enemyNarrative.enemyTypeDescription = "Mafia Boss";
            enemyNarrative.baseAttackDescription = "Baciamo le mani";
            enemyNarrative.passiveSkillDescription = "Mafiosità";
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.CharStats = enemyStatistics;
            enemyInfo.CharNarrativeStats = enemyNarrative;
            GlobalEventSystem.CastEvent(EventName.PossessionExecuted, EventArgsFactory.PossessionExecutedFactory(enemyInfo));
        };
        possessThief.clickable.clicked += delegate {
            EnemyStatistics enemyStatistics = new EnemyStatistics();
            enemyStatistics.EnemyType = EnemyType.Thief;
            enemyStatistics.Damage = 34;
            enemyStatistics.BaseSpeed = 7;
            EnemyNarrative enemyNarrative = new EnemyNarrative();
            enemyNarrative.icon = thiefIcon;
            enemyNarrative.enemyTypeDescription = "Thief";
            enemyNarrative.baseAttackDescription = "I steel u";
            enemyNarrative.passiveSkillDescription = "Cleptomaniac";
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.CharStats = enemyStatistics;
            enemyInfo.CharNarrativeStats= enemyNarrative;
            GlobalEventSystem.CastEvent(EventName.PossessionExecuted, EventArgsFactory.PossessionExecutedFactory(enemyInfo));
        };
        mission.clickable.clicked += delegate {
            currentMissionObjects++;
            currentMissionObjects = Mathf.Clamp(currentMissionObjects, 0, totalMissionObjects);
            GlobalEventSystem.CastEvent(EventName.MissionUpdated, EventArgsFactory.MissionUpdatedFactory(totalMissionObjects, currentMissionObjects));
        };
        increaseLevel.clickable.clicked += delegate {
            currentLevel++;
            LevelStruct level = new LevelStruct();
            level.CurrentLevel = currentLevel;
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(level));
        };
    }
}
