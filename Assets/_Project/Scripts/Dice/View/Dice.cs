using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Landing")]
    [SerializeField] private float landingRadius = 1f;
    [SerializeField] private float diceSeparation = 0.5f;
    [SerializeField] private int separationMaxIterations = 30;

    [HideInInspector] public int editorTargetFace = 1;

    private int pendingClipIndex = -1;

    public Vector3 PreparePosition(int targetFace, Transform landingTarget, List<Vector3> occupiedEndPositions)
    {
        if (throwClips == null || throwClips.Length == 0)
        {
            Debug.LogError("Dice: throwClips boş.");
            return transform.position;
        }

        pendingClipIndex = UnityEngine.Random.Range(0, throwClips.Length);

        int clipEndFace = (clipEndFaces != null && pendingClipIndex < clipEndFaces.Length)
                              ? clipEndFaces[pendingClipIndex] : clipEndFaces[0];
        AlignFaceUp(targetFace, clipEndFace);

        AnimationClip clip = throwClips[pendingClipIndex];
        Vector3 chosenEndPos = PositionForTarget(clip, landingTarget, occupiedEndPositions);

        return chosenEndPos;
    }


    public void LaunchAnimation(Action onComplete = null)
    {
        if (pendingClipIndex < 0 || throwClips == null || pendingClipIndex >= throwClips.Length)
        {
            return;
        }
        StartCoroutine(PlayThrowAnimation(pendingClipIndex, onComplete));
        pendingClipIndex = -1;
    }

    public void Throw(int targetFace, int clipIndex, Action onComplete = null)
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
        int clipEndFace = (clipEndFaces != null && clipIndex < clipEndFaces.Length)
                              ? clipEndFaces[clipIndex] : targetFace;
        AlignFaceUp(targetFace, clipEndFace);
        pendingClipIndex = clipIndex;
        LaunchAnimation(onComplete);
    }

    public Vector3 PositionAndThrow(int targetFace, Transform landingTarget, List<Vector3> occupiedEndPositions, Action onComplete = null)
    {
        Vector3 endPos = PreparePosition(targetFace, landingTarget, occupiedEndPositions);
        LaunchAnimation(onComplete);
        return endPos;
    }

    private IEnumerator PlayThrowAnimation(int clipIndex, Action onComplete)
    {
        AnimationClip clip = throwClips[clipIndex];
        string trigger = "A"+clipIndex.ToString();

        diceAnimator.SetFloat("AnimationSpeed", 1.75f * UnityEngine.Random.Range(0.75f, 1.33f));
        if (!string.IsNullOrEmpty(trigger))
            diceAnimator.SetTrigger(trigger);
        else
            diceAnimator.Play(clip.name, 0, 0f);

        yield return new WaitForSeconds(clip.length);
        onComplete?.Invoke();
    }
    private Vector3 PositionForTarget(AnimationClip clip, Transform landingTarget, List<Vector3> occupiedEndPositions)
    {
        if (landingTarget == null || diceAnimator == null) return transform.position;

        GameObject animGO = diceAnimator.gameObject;

        Vector3 rootSavedLocalPos = transform.localPosition;
        Quaternion rootSavedLocalRot = transform.localRotation;
        Vector3 animSavedLocalPos = animGO.transform.localPosition;
        Quaternion animSavedLocalRot = animGO.transform.localRotation;

        clip.SampleAnimation(animGO, clip.length);
        Vector3 endChildWorldPos = animGO.transform.position;

        transform.localPosition = rootSavedLocalPos;
        transform.localRotation = rootSavedLocalRot;
        animGO.transform.localPosition = animSavedLocalPos;
        animGO.transform.localRotation = animSavedLocalRot;

        Vector3 chosenTarget = Vector3.zero;
        bool found = false;

        for (int iter = 0; iter < separationMaxIterations; iter++)
        {
            Vector2 rnd = UnityEngine.Random.insideUnitCircle * landingRadius;
            Vector3 candidate = landingTarget.position + new Vector3(rnd.x, 0f, rnd.y);

            bool overlaps = false;
            if (occupiedEndPositions != null)
            {
                foreach (Vector3 occ in occupiedEndPositions)
                {
                    float d = new Vector2(candidate.x - occ.x, candidate.z - occ.z).magnitude;
                    if (d < diceSeparation) { overlaps = true; break; }
                }
            }
            if (!overlaps) { chosenTarget = candidate; found = true; break; }
        }

        if (!found)
        {
            Vector2 rnd = UnityEngine.Random.insideUnitCircle * landingRadius;
            chosenTarget = landingTarget.position + new Vector3(rnd.x, 0f, rnd.y);
        }

        Vector3 offset = chosenTarget - endChildWorldPos;
        offset.y = 0f;
        transform.position += offset;

        return chosenTarget;
    }
    private void AlignFaceUp(int targetFace, int clipEndFace)
    {
        if (diceVisual == null) return;
        Vector3 from = faceDirections[clipEndFace - 1].normalized;
        Vector3 to = faceDirections[targetFace - 1].normalized;
        diceVisual.localRotation = Quaternion.FromToRotation(from, to);
    }

#if UNITY_EDITOR
    private static readonly Color[] GizmoColors =
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.cyan
    };

    private void OnDrawGizmosSelected()
    {
        if (!showEndPoseGizmos || throwClips == null) return;

        // Mevcut transform'u kaydet
        Vector3 savedLocalPos = transform.localPosition;
        Quaternion savedLocalRot = transform.localRotation;
        // diceVisual'ın da state'ini kaydet (SampleAnimation onu da etkiler)
        Vector3 savedVisualLocalPos = diceVisual != null ? diceVisual.localPosition : Vector3.zero;
        Quaternion savedVisualLocalRot = diceVisual != null ? diceVisual.localRotation : Quaternion.identity;

        for (int i = 0; i < throwClips.Length; i++)
        {
            if (throwClips[i] == null) continue;

            // Clip'in son frame'ini sample et
            throwClips[i].SampleAnimation(gameObject, throwClips[i].length);

            Vector3 endPos = transform.position;
            Quaternion endRot = transform.rotation;

            // Restore et (diğer clip'ler için)
            transform.localPosition = savedLocalPos;
            transform.localRotation = savedLocalRot;
            if (diceVisual != null)
            {
                diceVisual.localPosition = savedVisualLocalPos;
                diceVisual.localRotation = savedVisualLocalRot;
            }

            Color c = i < GizmoColors.Length ? GizmoColors[i] : Color.white;
            Gizmos.color = c;

            // End pozisyonunu göster
            Gizmos.DrawWireSphere(endPos, gizmoArrowScale * 0.4f);
            UnityEditor.Handles.Label(endPos + Vector3.up * gizmoArrowScale * 0.6f, $"Clip {i}");

            // Her yüzün end-frame'deki world yönünü ok olarak çiz
            if (faceDirections == null || faceDirections.Length < 6) continue;

            for (int f = 0; f < 6; f++)
            {
                Vector3 worldDir = endRot * faceDirections[f].normalized;

                // Yukarıyı gösteren yüzü daha belirgin çiz
                bool isTop = Vector3.Dot(worldDir, Vector3.up) > 0.85f;

                UnityEditor.Handles.color = isTop ? Color.white : c;
                float scale = isTop ? gizmoArrowScale * 1.4f : gizmoArrowScale;

                if (worldDir.sqrMagnitude > 0.001f)
                {
                    UnityEditor.Handles.ArrowHandleCap(
                        0, endPos,
                        Quaternion.LookRotation(worldDir),
                        scale,
                        EventType.Repaint);

                    if (isTop)
                        UnityEditor.Handles.Label(endPos + worldDir * scale * 1.5f, $"Yukarı Yüz {f + 1}");
                }
            }
        }
    }
#endif
}
