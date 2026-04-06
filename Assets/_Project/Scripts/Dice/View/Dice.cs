using System.Collections;
using UnityEngine;

/// <summary>
/// Physics-based die.
/// - throwClips   : Unity Recorder'dan kaydedilmiş 5 atış animasyonu (Animator state ismi = clip ismi)
/// - diceVisual   : yüzlerin gerçekten çizildiği child küp transform'u
/// - faceDirections : her yüzün diceVisual local space'indeki normal yönü (index 0 = yüz 1)
///
/// Throw(targetFace, clipIndex) çağrılınca:
///   1) Animator ilgili clip'i oynatır
///   2) Clip bitince diceVisual döndürülür → istenen yüz world +Y'yi gösterir
/// </summary>
public class Dice : MonoBehaviour
{
    [SerializeField] private Animator diceAnimator;
    [SerializeField] private Transform diceVisual;

    public AnimationClip[] throwClips;

    [SerializeField] private string[] throwTriggers;

    [SerializeField] private int[] clipEndFaces;


    [SerializeField]
    private Vector3[] faceDirections = new Vector3[6]
    {
        Vector3.up,       // yüz 1
        Vector3.forward,  // yüz 2
        Vector3.right,    // yüz 3
        Vector3.left,     // yüz 4
        Vector3.back,     // yüz 5
        Vector3.down      // yüz 6
    };

    [Header("Gizmos")]
    [SerializeField] private bool showEndPoseGizmos = true;
    [SerializeField] private float gizmoArrowScale = 0.25f;
    [HideInInspector] public int editorTargetFace = 1;

    public void ThrowRandom(int targetFace)
    {
        if (throwClips == null || throwClips.Length == 0)
        {
            Debug.LogError("Dice: throwClips boş.");
            return;
        }
        int clipIndex = Random.Range(0, throwClips.Length);
        Throw(targetFace, clipIndex);
    }

    [ContextMenu("Throw — Use Editor Target Face")]
    private void ContextMenuThrow() => ThrowRandom(editorTargetFace);
    public void Throw(int targetFace, int clipIndex)
    {
        if (throwClips == null || clipIndex < 0 || clipIndex >= throwClips.Length || throwClips[clipIndex] == null)
        {
            Debug.LogError($"Dice: geçersiz clipIndex {clipIndex}");
            return;
        }
        if (targetFace < 1 || targetFace > 6)
        {
            Debug.LogError($"Dice: targetFace {targetFace} 1-6 aralığında olmalı");
            return;
        }
        StartCoroutine(ThrowRoutine(targetFace, clipIndex));
    }

    private IEnumerator ThrowRoutine(int targetFace, int clipIndex)
    {
        int clipEndFace = (clipEndFaces != null && clipIndex < clipEndFaces.Length)
                              ? clipEndFaces[clipIndex]
                              : targetFace; // bilinmiyorsa offset yok
        AlignFaceUp(targetFace, clipEndFace);

        AnimationClip clip = throwClips[clipIndex];
        string trigger = (throwTriggers != null && clipIndex < throwTriggers.Length)
                                    ? throwTriggers[clipIndex]
                                    : null;

        if (!string.IsNullOrEmpty(trigger))
            diceAnimator.SetTrigger(trigger);
        else
            diceAnimator.Play(clip.name, 0, 0f);

        yield return new WaitForSeconds(clip.length);
    }
    private void AlignFaceUp(int targetFace, int clipEndFace)
    {
        if (diceVisual == null) return;
        Vector3 from = faceDirections[clipEndFace - 1].normalized;
        Vector3 to = faceDirections[targetFace - 1].normalized;
        diceVisual.localRotation = Quaternion.FromToRotation(from, to);
    }


}
