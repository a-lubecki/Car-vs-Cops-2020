using UnityEngine;


public class MusicBehavior : MonoBehaviour {


    [SerializeField] private AudioClip clipMenu = null;
    [SerializeField] private AudioClip clipInGame = null;

    private AudioSource audioSourceMusic;


    protected void Awake() {

        audioSourceMusic = GetComponent<AudioSource>();
    }

    public void PlayMusicMenu() {

        ReplaceClip(clipMenu);
    }

    public void PlayMusicInGame() {

        ReplaceClip(clipInGame);
    }

    private void ReplaceClip(AudioClip clip) {

        var time = audioSourceMusic.time;
        audioSourceMusic.clip = clip;
        audioSourceMusic.Play();
        audioSourceMusic.time = time;
    }

}
