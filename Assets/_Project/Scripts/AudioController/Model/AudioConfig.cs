using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/Audio Config")]
public class AudioConfig : ScriptableObject
{
    public List<AudioData> AudioDatas = new List<AudioData>();

    public AudioData Get(AudioType type) => AudioDatas.Find(a => a.audioType == type);
}
