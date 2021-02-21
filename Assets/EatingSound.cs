using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] eatingSounds;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            audioSource.PlayOneShot(eatingSounds[Random.Range(0, eatingSounds.Length)]);
        }
    }
}
