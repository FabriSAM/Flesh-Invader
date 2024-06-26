using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_EnemyStats : MonoBehaviour
{
    private VisualElement icon;
    private Label damages;
    private Label speed;
    private Label _class;
    private Label baseAttack;
    private Label passiveSkill;
    private VisualElement enemyStatsContainer;

    private void Awake() {
        icon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("icon");
        damages = GetComponent<UIDocument>().rootVisualElement.Q<Label>("damages-value");
        speed = GetComponent<UIDocument>().rootVisualElement.Q<Label>("speed-value");
        _class = GetComponent<UIDocument>().rootVisualElement.Q<Label>("class-value");
        baseAttack = GetComponent<UIDocument>().rootVisualElement.Q<Label>("base-attack-value");
        passiveSkill = GetComponent<UIDocument>().rootVisualElement.Q<Label>("passive-skill-value");
        enemyStatsContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("enemy-stats-container");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void PossessionExecuted(EventArgs message) {
        EventArgsFactory.PossessionExecutedParser(message, out EnemyInfo enemyInfo);
        damages.text = (Mathf.Floor(enemyInfo.CharStats.Damage * 10f) / 10f).ToString();
        speed.text = enemyInfo.CharStats.BaseSpeed.ToString();
        _class.text = enemyInfo.CharNarrativeStats.enemyTypeDescription.ToString();
        baseAttack.text = enemyInfo.CharNarrativeStats.baseAttackDescription.ToString();
        passiveSkill.text = enemyInfo.CharNarrativeStats.passiveSkillDescription.ToString();
        icon.style.backgroundImage = enemyInfo.CharNarrativeStats.icon;
        enemyStatsContainer.style.backgroundColor = enemyInfo.CharNarrativeStats.color;
    }
}
