using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour    //사운드 클립을 컨트롤하기 위해서 만듬.
{
    private AudioSource _audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)   //어떤 클립, 볼룸과 어떤 피치로 플레이를 하게 해준다.
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke(); //아래쪽에 쓴 Invoke때문에 여기서 Invoke 취소해줍니다. //예약 취소 하고.
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVolume;
        _audioSource.Play();
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);    //등록하고.

        Invoke("Disable", clip.length + 2); //지연 실행. 클립 +2 이후에 작업해라. //예약 다시 하고.
    }

    public void Disable()
    {
        _audioSource.Stop();    //스톱
        gameObject.SetActive(false); //전원 오프
    }
}
