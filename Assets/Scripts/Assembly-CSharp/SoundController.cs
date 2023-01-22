using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    List<AudioClip> fixSounds;

    [SerializeField]
    List<AudioClip> failSounds;

    [SerializeField]
    List<AudioClip> openSounds;

    [SerializeField]
    List<AudioClip> skripSoundsSounds;

    [SerializeField]
    List<AudioClip> keyInsertSounds;

    public void PlayFixSound()
    {
        int soundNum = Random.Range(0, fixSounds.Count);
        audioSource.PlayOneShot(fixSounds[soundNum]);
    }

    public void PlayOpenSound()
    {
        int soundNum = Random.Range(0, openSounds.Count);
        audioSource.PlayOneShot(openSounds[soundNum]);
    }

    public void PlayFailSound()
    {
        int soundNum = Random.Range(0, failSounds.Count);
        audioSource.PlayOneShot(failSounds[soundNum]);
    }

    public void PlayKeyInsertSound()
    {
        int soundNum = Random.Range(0, keyInsertSounds.Count);
        audioSource.PlayOneShot(keyInsertSounds[soundNum]);
    }
}
