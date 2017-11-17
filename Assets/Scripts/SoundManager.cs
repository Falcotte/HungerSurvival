using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource fxSource;
    public AudioSource musicSource;
    public static SoundManager Instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	void Awake () {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip Clip)
    {
        fxSource.clip = Clip;
        fxSource.Play();

    }

    public void RandomizeFx(params AudioClip[] Clips)
    {
        int randomIndex = Random.Range(0, Clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        fxSource.pitch = randomPitch;
        fxSource.clip = Clips[randomIndex];
        fxSource.Play();
    }
	
}
