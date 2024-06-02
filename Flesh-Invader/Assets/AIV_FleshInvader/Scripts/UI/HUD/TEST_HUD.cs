using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_HUD : MonoBehaviour
{
    private Button increaseHealth;
    private Button decreaseHealth;
    private Button increaseXP;
    private Button decreaseXP;
    private Button possessBoss;
    private Button possessThief;

    private int maxHP = 10;
    private int currentHP = 0;
    private int maxXP = 10;
    private int currentXP = 0;

    private void Awake() {
        increaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-health");
        decreaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-health");
        increaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-xp");
        decreaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-xp");
        possessBoss = GetComponent<UIDocument>().rootVisualElement.Q<Button>("possess-boss");
        possessThief = GetComponent<UIDocument>().rootVisualElement.Q<Button>("possess-thief");
    }

    void Start()
    {
        increaseHealth.clickable.clicked += delegate {
            currentHP++;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            Debug.Log("currentHP: " + currentHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        decreaseHealth.clickable.clicked += delegate {
            currentHP--;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            Debug.Log("currentHP: " + currentHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        increaseXP.clickable.clicked += delegate {
            currentXP++;
            currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            Debug.Log("currentXP: " + currentXP);
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(maxXP, currentXP));
        };
        decreaseXP.clickable.clicked += delegate {
            currentXP--;
            currentXP = currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            Debug.Log("currentXP: " + currentXP);
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(maxXP, currentXP));
        };
        possessBoss.clickable.clicked += delegate {
            Debug.Log("Possess boss");
            EnemyStatistics enemyStatistics = new EnemyStatistics();
            enemyStatistics.EnemyType = EnemyType.Boss;
            enemyStatistics.Damage = 25;
            enemyStatistics.Speed = 10;
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.CharStats = enemyStatistics;
            GlobalEventSystem.CastEvent(EventName.PossessionExecuted, EventArgsFactory.PossessionExecutedFactory(enemyInfo));
        };
        possessThief.clickable.clicked += delegate {
            Debug.Log("Possess thief");
            EnemyStatistics enemyStatistics = new EnemyStatistics();
            enemyStatistics.EnemyType = EnemyType.Thief;
            enemyStatistics.Damage = 34;
            enemyStatistics.Speed = 7;
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.CharStats = enemyStatistics;
            GlobalEventSystem.CastEvent(EventName.PossessionExecuted, EventArgsFactory.PossessionExecutedFactory(enemyInfo));
        };
    }
}
