using System;
using UnityEngine;


public class AudioBehavior : MonoBehaviour {


    [SerializeField] private AudioClip clipMusicMenu = null;
    [SerializeField] private AudioClip clipMusicInGame = null;

    [SerializeField] private AudioClip[] clipsSounds = null;

    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSound;


    protected void Awake() {

        audioSourceMusic = GetComponents<AudioSource>()[0];
        audioSourceSound = GetComponents<AudioSource>()[1];
    }

    public void PlayMusicMenu() {

        ReplaceMusicClip(clipMusicMenu);
    }

    public void PlayMusicInGame() {

        ReplaceMusicClip(clipMusicInGame);
    }

    private void ReplaceMusicClip(AudioClip clip) {

        var time = audioSourceMusic.time;
        audioSourceMusic.clip = clip;
        audioSourceMusic.Play();
        audioSourceMusic.time = time;
    }

    public void PlaySound(string soundName) {

        //find sound by name
        AudioClip foundClip = null;

        foreach (var clip in clipsSounds) {

            if (clip.name.Equals(soundName)) {
                foundClip = clip;
                break;
            }
        }

        if (foundClip == null) {
            throw new ArgumentException("Sound " + soundName + " was not found");
        }

        audioSourceSound.PlayOneShot(foundClip);
    }

}
