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

    private void Awake() {
        icon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("icon");
        damages = GetComponent<UIDocument>().rootVisualElement.Q<Label>("damages");
        speed = GetComponent<UIDocument>().rootVisualElement.Q<Label>("speed");
        _class = GetComponent<UIDocument>().rootVisualElement.Q<Label>("class");
        baseAttack = GetComponent<UIDocument>().rootVisualElement.Q<Label>("base-attack");
        passiveSkill = GetComponent<UIDocument>().rootVisualElement.Q<Label>("passive-skill");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void PossessionExecuted(EventArgs message) {
        EventArgsFactory.PossessionExecutedParser(message, out EnemyInfo enemyInfo);
        damages.text = "Damages: " + Mathf.Floor(enemyInfo.CharStats.Damage * 10f) / 10f;
        speed.text = "Speed: " + enemyInfo.CharStats.BaseSpeed;
        _class.text = $"Class: {enemyInfo.CharNarrativeStats.enemyTypeDescription}";
        baseAttack.text = $"Base attack: {enemyInfo.CharNarrativeStats.baseAttackDescription}";
        passiveSkill.text = $"Passive skill: {enemyInfo.CharNarrativeStats.passiveSkillDescription}";
        icon.style.backgroundImage = enemyInfo.CharNarrativeStats.icon;
    }
}
