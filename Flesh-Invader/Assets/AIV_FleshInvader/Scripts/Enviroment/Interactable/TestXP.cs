using UnityEngine;

public class TestXP : InteractableBase
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerState.Get().LevelController.SetXP(100);
    }

    protected override void OnOpen()
    {
        
    }
}
