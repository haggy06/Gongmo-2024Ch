using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>(Path.Combine("Singleton", "AudioManager"))).GetComponent<AudioManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < 10; i++) // 10�� ���� ����Ŀ ����� ����
            {
                CreateSpeacker();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
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

        //LeanTween.value(0.5f, 1f, 0.5f).setOnUpdate((float value) => bgmSpeacker.volume = value);
    }

    public void PlaySFX(AudioClip clip)
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
