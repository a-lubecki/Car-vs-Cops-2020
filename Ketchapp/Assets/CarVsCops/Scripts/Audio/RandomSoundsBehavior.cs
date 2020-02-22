using System.Collections;
using UnityEngine;


public class RandomSoundsBehavior : MonoBehaviour {


    [SerializeField] private AudioClip[] randomClipsSounds = null;

    private AudioSource audioSourceSound;


    protected void Awake() {

        audioSourceSound = GetComponent<AudioSource>();
    }

    public void StartRandomSoundsPlaying() {

        StartCoroutine(PlayRandomSounds());
    }

    public void StopRandomSoundsPlaying() {

        StopAllCoroutines();
    }

    private IEnumerator PlayRandomSounds() {

        while (true) {

            var delay = UnityEngine.Random.Range(10, 20);
            yield return new WaitForSeconds(delay);

            var soundPos = UnityEngine.Random.Range(0, randomClipsSounds.Length);
            audioSourceSound.PlayOneShot(randomClipsSounds[soundPos]);
        }
    }

}
