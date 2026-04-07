using UnityEngine;

public class DiceAnimationListener : MonoBehaviour
{
    public void OnDiceHitGround()
    {
        EventBus.Raise(new OnDiceHitGround());
    }
}
