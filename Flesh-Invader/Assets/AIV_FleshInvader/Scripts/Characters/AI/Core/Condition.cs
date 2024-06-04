public abstract class Condition : ExecutableNode
{

    public virtual bool Validate () {
        return true;
    }

}
