using UnityEngine;

public class TestXP : InteractableBase
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXP(100);
    }

    protected override void OnOpen()
    {
        
    }
}
