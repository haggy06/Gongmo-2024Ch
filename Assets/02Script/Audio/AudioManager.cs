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
    private AudioSource bgmSpeacker;

    [SerializeField]
    private Transform speackerBox;
    [SerializeField]
    private AudioMixerGroup sfxChannel;
    [SerializeField]
    private List<AudioSource> fbxSpeakers = new List<AudioSource>();

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
            if (!source.isPlaying) // ���� ��� ���� �ƴ� ���
            {
                emptySource = source;

                break;
            }
        }

        if (!emptySource) // �� ����Ŀ�� �����ٸ�
        {
            emptySource = CreateSpeacker(); // �� ����Ŀ�� ����� ����
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
