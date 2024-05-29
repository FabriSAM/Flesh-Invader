public class Transition
{

    private State fromState;
    public State FromState {
        get { return fromState; }
    }
    private State toState;
    public State ToState {
        get { return toState; }
    }
    private Condition[] conditions;

    public void SetUpMe (State fromState, State toState, Condition[] conditions) {
        this.fromState = fromState;
        this.toState = toState;
        this.conditions = conditions;
    }

    public void OnEnter () {
        foreach(Condition condition in conditions) {
            condition.OnEnter();
        }
    }

    public void OnExit () {
        foreach(Condition condition in conditions) {
            condition.OnExit();
        }
    }

    public bool Validate () {
        foreach(Condition condition in conditions) {
            if (!condition.Validate()) return false;
        }
        return true;
    }

}
