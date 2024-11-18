using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance;

    public Sound[] musicSound, sfxSound;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic("Music");
    }

    public void PlayMusic(string name)
    {
        Sound music = Array.Find(musicSound, musicNeedToFind => musicNeedToFind.name == name);

        if (music == null)
        {
            Debug.Log("Khong tim thay am thanh");
        }
        else
        {
            musicSource.clip = music.audioClip;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        Sound sfx = Array.Find(sfxSound, sfxNeedToFind => sfxNeedToFind.name == name);

        if (sfx == null)
        {
            Debug.Log("Khong tim thay am thanh");
        }
        else
        {
            sfxSource.PlayOneShot(sfx.audioClip);
        }
    }
}
