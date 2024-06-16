using UnityEngine;

public class StateMachine : MonoBehaviour
{

    private State[] states;
    private State backgroundState;
    private State activeState;

    public void Init(State[] states, State firstState, State backgroundState = null) {
        this.states = states;
        this.backgroundState = backgroundState;
        if (backgroundState != null) backgroundState.Init(this);
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
        if(backgroundState != null)
            backgroundState.OnUpdate();
        if (activeState == null) return;
            activeState.OnUpdate();
    }
}
