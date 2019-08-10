using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayDeathClip()
    {
        audioSource.PlayOneShot(deathClip);
    }
}
