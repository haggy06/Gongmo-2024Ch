using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    protected override void SceneChanged(Scene replacedScene, Scene newScene)
    {

    }

    [SerializeField]
    private AudioMixer audioMixer;

    [Space(5)]
    [SerializeField]
    private AudioMixerGroup sfxChannel;

    [Space(10)]
    [SerializeField]
    private AudioSource bgmSpeacker;
    [SerializeField]
    private Transform speackerBox;
    [SerializeField]
    private List<AudioSource> fbxSpeakers = new List<AudioSource>();

    private void Start()
    {
        string channelName;
        
        channelName = VolumChannel.BGM.ToString();
        audioMixer.SetFloat(channelName, DB(PlayerPrefs.GetFloat(channelName)));

        channelName = VolumChannel.SFX.ToString();
        audioMixer.SetFloat(channelName, DB(PlayerPrefs.GetFloat(channelName)));
    }
    public void ChangeVolume(VolumChannel channel, float volume) // volume은 1~10의 정수(자료형은 실수지만 아무튼)
    {
        string channelName = channel.ToString();

        PlayerPrefs.SetFloat(channelName, volume);
        audioMixer.SetFloat(channelName, DB(volume));
    }
    private float DB(float volume)
    {
        if (Mathf.Approximately(volume, 0f))
        {
            return -80f;
        }
        else
        {
            return 20 * Mathf.Log10(volume / 10f);
        }
    }

    public void ChangeBGM(AudioClip bgm)
    {
        bgmSpeacker.Pause();

        bgmSpeacker.clip = bgm;
        bgmSpeacker.Play();

        StopCoroutine("VolumeCor");
        StartCoroutine("VolumeCor");
    }
    private IEnumerator VolumeCor()
    {
        float progress = 0.5f;
        while (progress < 1f) 
        {
            progress += Time.deltaTime;
            bgmSpeacker.volume = progress;

            yield return null;
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource emptySource = null;
        foreach (AudioSource source in fbxSpeakers)
        {
            if (!source.isPlaying) // 사운드 재생 중이 아닐 경우
            {
                emptySource = source;

                break;
            }
        }

        if (!emptySource) // 빈 스피커가 없었다면
        {
            emptySource = CreateSpeacker(); // 새 스피커를 만들어 참조
        }

        emptySource.clip = clip;
        emptySource.volume = volume;
        emptySource.Play();
    }

    private AudioSource CreateSpeacker()
    {
        GameObject obj = new GameObject("Speaker " + fbxSpeakers.Count);
        obj.transform.parent = speackerBox;

        AudioSource speacker = obj.AddComponent<AudioSource>();

        speacker.outputAudioMixerGroup = sfxChannel;
        speacker.playOnAwake = false;
        speacker.loop = false;

        fbxSpeakers.Add(speacker);

        return speacker;
    }
}

public enum VolumChannel 
{
    BGM,
    SFX,
}
