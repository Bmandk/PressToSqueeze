using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{


    public List<AudioClip> landingClips;
    public List<AudioClip> jumpClips;
    public List<AudioClip> squeezeClips;
    public List<AudioClip> releaseClips;
    public List<AudioClip> deathClips;
    public List<AudioClip> wallkingDryClips;
    public List<AudioClip> wallkingWetClips;


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
        if(squeezeClips.Count == 0)
        {
            return;
        }
        int clipIndex = Random.Range(0, squeezeClips.Count);

        _audioSource.PlayOneShot(squeezeClips[clipIndex]);
    }

    public void PlayReleaseClip()
    {
        if (releaseClips.Count == 0)
        {
            return;
        }
        int clipIndex = Random.Range(0, releaseClips.Count);
        _audioSource.PlayOneShot(releaseClips[clipIndex]);
    }

    public void PlayJumpClip()
    {

        if (jumpClips.Count == 0)
        {
            return;
        }
        int clipIndex = Random.Range(0, jumpClips.Count);
        _audioSource.PlayOneShot(jumpClips[clipIndex]);
    }

    public void PlayLandingClip()
    {
        
        if (landingClips.Count == 0)
        {
            return;
        }

        int clipIndex = Random.Range(0, landingClips.Count);

        _audioSource.PlayOneShot(landingClips[clipIndex]);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayLandingClip();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        PlayJumpClip();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //check if the walking clip is already playing
        //if not, play it

        Debug.Log(_rigidbody2D.velocity.x);

        if (!_audioSource.isPlaying && Mathf.Abs(_rigidbody2D.velocity.x) > 0.01)
        {


            float wetDryNum = Random.Range(0, 1);

            List<AudioClip> wallkingClips = new List<AudioClip>();

            Debug.Log(_spongeScript.currentWaterAmount);
            if (wetDryNum < _spongeScript.currentWaterAmount)
            {
                //play wet clip
                wallkingClips = wallkingWetClips;
            }
            else
            {
                //play dry clip
                wallkingClips = wallkingDryClips;
            }

            //choose a walking clip
            int clipIndex = Random.Range(0, wallkingClips.Count);

            AudioClip wallkingClip = wallkingClips[clipIndex];


            _audioSource.PlayOneShot(wallkingClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}