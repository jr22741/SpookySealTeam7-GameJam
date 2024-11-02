using System;
using UnityEngine;
using System.Collections;
public class SoundsMngr : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip music;
    
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        playBackgroundMusic();

    }

    IEnumerator playBackgroundMusic() {

        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();

        GetComponent<AudioSource>().volume = 0.3f;
        yield return new WaitForSeconds(audioSource.clip.length);

        yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 50.0f));
        playBackgroundMusic();
    }

}