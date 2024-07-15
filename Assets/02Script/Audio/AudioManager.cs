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

            for (int i = 0; i < 10; i++) // 10개 정도 스피커 만들고 시작
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
