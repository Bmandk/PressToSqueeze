using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{


    public List<AudioClip> landingWetClips;
    public List<AudioClip> landingDryClips;
    public List<AudioClip> jumpClips;
    public List<AudioClip> squeezeClips;
    public List<AudioClip> releaseClips;
    public List<AudioClip> deathClips;
    public List<AudioClip> wallkingDryClips;
    public List<AudioClip> wallkingWetClips;


    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private SpongeScript _spongeScript;

    private float lastLandingTime;



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
        
        if (landingDryClips.Count == 0 && landingDryClips.Count == 0)
        {
            return;
        }
        if(lastLandingTime == null)
        {
            lastLandingTime = Time.time;
        }

        if (!_spongeScript.changingSize)
        {

            float wetDryNum = Random.Range(0, 1);

            List<AudioClip> landingClips = new List<AudioClip>();

            if (wetDryNum < _spongeScript.currentWaterAmount)
            {
                //play wet clip
                landingClips = landingWetClips;
            }
            else
            {
                //play dry clip
                landingClips = landingDryClips;
            }

            //choose a walking clip
            int clipIndex = Random.Range(0, landingClips.Count);

            AudioClip landingClip = landingClips[clipIndex];


            _audioSource.PlayOneShot(landingClip);
            lastLandingTime = Time.time;
        }
    }

    public void PlayWalkingClip()
    {
        if(wallkingDryClips.Count == 0 && wallkingWetClips.Count == 0)
        {
            return;
        }
        if (!_audioSource.isPlaying && Mathf.Abs(_rigidbody2D.velocity.x) > 0.01)
        {


            float wetDryNum = Random.Range(0, 1);

            List<AudioClip> wallkingClips = new List<AudioClip>();

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

        //Debug.Log(_rigidbody2D.velocity.x);

        if (!_audioSource.isPlaying && Mathf.Abs(_rigidbody2D.velocity.x) > 0.01)
        {


            float wetDryNum = Random.Range(0, 1);

            List<AudioClip> wallkingClips = new List<AudioClip>();

            //Debug.Log(_spongeScript.currentWaterAmount);
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
