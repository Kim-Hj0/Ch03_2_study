using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour    //���� Ŭ���� ��Ʈ���ϱ� ���ؼ� ����.
{
    private AudioSource _audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)   //� Ŭ��, ����� � ��ġ�� �÷��̸� �ϰ� ���ش�.
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke(); //�Ʒ��ʿ� �� Invoke������ ���⼭ Invoke ������ݴϴ�. //���� ��� �ϰ�.
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVolume;
        _audioSource.Play();
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);    //����ϰ�.

        Invoke("Disable", clip.length + 2); //���� ����. Ŭ�� +2 ���Ŀ� �۾��ض�. //���� �ٽ� �ϰ�.
    }

    public void Disable()
    {
        _audioSource.Stop();    //����
        gameObject.SetActive(false); //���� ����
    }
}
