using UnityEngine;

public class State
{

    private StateMachine owner;

    private StateAction[] actions;
    private Transition[] transitions;

    public void Init (StateMachine owner) {
        this.owner = owner;
    }

    public void SetUpMe(StateAction[] actions) {
        this.actions = actions;
    }

    public void SetUpMe (Transition[] transitions) {
        this.transitions = transitions;
    }

    public void OnEnter () {
        foreach(StateAction action in actions) {
            action.OnEnter();
        }
        foreach(Transition transition in transitions) {
            transition.OnEnter();
        }
    }

    public void OnExit() {
        foreach (StateAction action in actions) {
            action.OnExit();
        }
        foreach (Transition transition in transitions) {
            transition.OnExit();
        }
    }

    public void OnUpdate () {
        foreach(StateAction action in actions) {
            action.OnUpdate();
        }
        foreach(Transition transition in transitions) {
            if (transition.Validate()) {
                owner.SwapState(transition.ToState);
                return;
            }
        }
    }

}
