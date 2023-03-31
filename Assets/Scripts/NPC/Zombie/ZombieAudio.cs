using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip breathClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip feedClip;
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip screamClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = breathClip;
        audioSource.Play();
    }

    public void Feed()
    {
        audioSource.clip = feedClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Attack()
    {
        audioSource.clip = attackClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void Death()
    {
        audioSource.clip = deathClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void Hit()
    {
        audioSource.clip = hitClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void Scream()
    {
        audioSource.clip = screamClip;
        audioSource.loop = false;
        audioSource.Play();
    }


    public void Breath()
    {
        audioSource.clip = breathClip;
        audioSource.loop = true;
        audioSource.Play();
    }


}
