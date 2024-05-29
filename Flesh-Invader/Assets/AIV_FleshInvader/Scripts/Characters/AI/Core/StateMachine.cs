using UnityEngine;

public class StateMachine : MonoBehaviour
{

    private State[] states;
    private State activeState;

    public void Init(State[] states, State firstState) {
        this.states = states;
        foreach(State state in this.states) {
            state.Init(this);
        }
        SwapState(firstState);
    }

    public void SwapState(State toState) {
        if (activeState != null) activeState.OnExit();
        activeState = toState;
        activeState.OnEnter();
    }

    private void Update() {
        if (activeState == null) return;
        activeState.OnUpdate();
    }
}
