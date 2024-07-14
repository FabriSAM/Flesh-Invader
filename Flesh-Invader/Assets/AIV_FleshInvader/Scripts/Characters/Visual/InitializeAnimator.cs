using UnityEngine;

public class InitializeAnimator : MonoBehaviour, IAnimatorInitializer
{
    [SerializeField]
    private AnimatorCharacterType characterType;

    private const string animatorWeaponType = "CurrentWeaponType";

    public void InitializeAnimatorParams(Visual visual)
    {
        if (visual == null) return;
        visual.SetAnimatorParameter(animatorWeaponType, (int)characterType);
    }
}
