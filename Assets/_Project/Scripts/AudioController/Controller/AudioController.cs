using System;
using UnityEngine;


public class AudioController : MonoBehaviour, IInitializable
{

    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private AudioSource audioSource;


    public void Initialize()
    {
        EventBus.Subscribe<OnDiceHitGround>(HandleDiceHitGround);
        EventBus.Subscribe<OnPlayerHitGround>(HandlePlayerHitGround);
    }

    public void Dispose()
    {
        EventBus.Unsubscribe<OnDiceHitGround>(HandleDiceHitGround);
        EventBus.Unsubscribe<OnPlayerHitGround>(HandlePlayerHitGround);
    }

    private void HandleDiceHitGround(OnDiceHitGround e)
    {
        Play(AudioType.DiceHitGround);
    }
    private void HandlePlayerHitGround(OnPlayerHitGround e)
    {
        Play(AudioType.Jump);
    }

    private void Play(AudioType type)
    {
        if (audioSource == null)
        {
            Debug.LogError($"[AudioController] AudioSource bulunamadı!");
            return;
        }
        AudioData data = audioConfig?.Get(type);
        if (data == null)
        {
            Debug.LogWarning($"[AudioController] AudioConfig'te {type} için veri bulunamadı!");
            return;
        }
        AudioClip clip = data.GetRandomClip();
        if (clip == null)
        {
            Debug.LogWarning($"[AudioController] AudioData'da {type} için clip bulunamadı!");
            return;
        }
        if (clip != null) audioSource.PlayOneShot(clip, data.Volume());
    }
}

