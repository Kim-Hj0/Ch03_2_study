using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;    //싱글톤 화.

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;    
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;
    private ObjectPool objectPool;

    private AudioSource musicAudioSource;   //오디오 소스, 오디오 리스너는 보통 카메라에 붙어있다. 3D일 경우, 거리에 따라 작아지고 커질 수 있다.
    public AudioClip musicClip; //오디오 클립이 실제 음원이라고 보면 됨.

    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);
    }

    public void ChangeBackGroundMusic(AudioClip music)  //동작
    {
        instance.musicAudioSource.Stop();   //정적인 메모리에서 일반 메모리로 내려온 상황.
        instance.musicAudioSource.clip = music;
        instance.musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = instance.objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);
    }
}
