using System;
using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private BoardConfigSO boardConfig;
    [SerializeField] private GameConfigSO gameConfig;

    [Header("Jump Settings")]
    [SerializeField] private float bounceHeight = 0.8f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem landingParticles;

    private Coroutine moveCoroutine;

    public void MoveToTile(int tileIndex, Action onLanded = null)
    {
        Vector3 target = new Vector3(tileIndex * boardConfig.tileSpacing, 0f, 0f);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(AnimateMove(target, onLanded));
    }

    private IEnumerator AnimateMove(Vector3 target, Action onLanded)
    {
        Vector3 start = transform.position;
        float duration = gameConfig.playerMoveDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 p = Vector3.Lerp(start, target, t);
            p.y += Mathf.Sin(t * Mathf.PI) * bounceHeight;
            transform.position = p;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        landingParticles?.Play();
        EventBus.Raise(new OnPlayerHitGround());
        onLanded?.Invoke();
    }
}
