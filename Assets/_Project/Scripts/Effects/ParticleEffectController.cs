using UnityEngine;

public class ParticleEffectController : MonoBehaviour, IInitializable
{
    [SerializeField] private ParticleSystem collectParticleSystem;
    [SerializeField] private ParticleSystem diceRollParticleSystem;

    public void Initialize()
    {
        EventBus.Subscribe<OnTileRewardCollected>(HandleRewardCollected);
        EventBus.Subscribe<OnDiceRolled>(HandleDiceRolled);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnTileRewardCollected>(HandleRewardCollected);
        EventBus.Unsubscribe<OnDiceRolled>(HandleDiceRolled);
    }

    private void HandleRewardCollected(OnTileRewardCollected e)
    {
        if (!e.reward.isEmpty)
            collectParticleSystem?.Play();
    }

    private void HandleDiceRolled(OnDiceRolled e)
    {
        diceRollParticleSystem?.Play();
    }
}

