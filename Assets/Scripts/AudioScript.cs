using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{


    public AudioClip landingClip;
    public AudioClip jumpClip;
    public AudioClip squeezeClip;
    public AudioClip releaseClip;
    public AudioClip deathClip;
    public AudioClip wallkingClip;

    public List<AudioClip> dripClips;

    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private SpongeScript _spongeScript;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _spongeScript = GetComponent<SpongeScript>();
    }

    public void PlaySqueezeClip()
    {
        _audioSource.PlayOneShot(squeezeClip);
    }

    public void PlayReleaseClip()
    {
        _audioSource.PlayOneShot(releaseClip);
    }

    public void PlayJumpClip()
    {
        _audioSource.PlayOneShot(jumpClip);
    }

    public void PlayLandingClip()
    {
        _audioSource.PlayOneShot(landingClip);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        _audioSource.PlayOneShot(landingClip);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        _audioSource.PlayOneShot(jumpClip);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //check if the walking clip is already playing
        //if not, play it

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(wallkingClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
