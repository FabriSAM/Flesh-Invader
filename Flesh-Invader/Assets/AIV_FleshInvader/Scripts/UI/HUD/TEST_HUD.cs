using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_HUD : MonoBehaviour
{
    private Button increaseHealth;
    private Button decreaseHealth;
    private Button increaseXP;
    private Button decreaseXP;

    private int maxHP = 10;
    private int currentHP = 0;
    private int maxXP = 10;
    private int currentXP = 0;

    private void Awake() {
        increaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-health");
        decreaseHealth = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-health");
        increaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("increase-xp");
        decreaseXP = GetComponent<UIDocument>().rootVisualElement.Q<Button>("decrease-xp");
    }

    void Start()
    {
        increaseHealth.clickable.clicked += delegate {
            currentHP++;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            Debug.Log("currentHP" + currentHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        decreaseHealth.clickable.clicked += delegate {
            currentHP--;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            Debug.Log("currentHP" + currentHP);
            GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated, EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
        };
        increaseXP.clickable.clicked += delegate {
            currentXP++;
            currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            Debug.Log("currentXP" + currentXP);
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(maxXP, currentXP));
        };
        decreaseXP.clickable.clicked += delegate {
            currentXP--;
            currentXP = currentXP = Mathf.Clamp(currentXP, 0, maxXP);
            Debug.Log("currentXP" + currentXP);
            GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated, EventArgsFactory.PlayerXPUpdatedFactory(maxXP, currentXP));
        };
    }
}
