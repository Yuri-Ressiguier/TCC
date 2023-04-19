using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSFX : MonoBehaviour
{
    [field: SerializeField] private AudioSource _audioSource;

    public void PlaySFX(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }


}
