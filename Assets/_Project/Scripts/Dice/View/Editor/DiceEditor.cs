using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dice))]
public class DiceEditor : Editor
{
    private int editorClipIndex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Dice dice = (Dice)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("── Test Throw ──", EditorStyles.boldLabel);

        dice.editorTargetFace = EditorGUILayout.IntSlider("Hedef Yüz", dice.editorTargetFace, 1, 6);

        int maxClip = (dice.throwClips != null && dice.throwClips.Length > 0) ? dice.throwClips.Length - 1 : 0;
        editorClipIndex = EditorGUILayout.IntSlider("Animasyon İndeksi", editorClipIndex, 0, maxClip);

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

        if (GUILayout.Button($"Throw → Yüz {dice.editorTargetFace} Üste", GUILayout.Height(32)))
        {
            dice.Throw(dice.editorTargetFace, editorClipIndex);
        }

        EditorGUI.EndDisabledGroup();

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Throw butonu yalnızca Play Mode'da çalışır.", MessageType.Info);
        }
    }
}
