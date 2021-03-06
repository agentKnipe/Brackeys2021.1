﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mechanic : MonoBehaviour
{
    [Tooltip("Used to control the size of the poof animation used to obscure the player")]
    [SerializeField]
    public float size = 1f;

    [Tooltip("How long after the poof animation before this mechanic should start")]
    [SerializeField]
    public float delay = 0f;

    [Tooltip("Which key will activate the mechanic")]
    [SerializeField]
    public KeyCode key;

    [Tooltip("The cost to use the mechanic")]
    [SerializeField]
    public int cost = 1;
    public bool _inMechanic = false;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] hips_hups;

    protected Animator _mechanicAnimator;

    protected virtual void Start() {
        _mechanicAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        enabled = false;
    }


    /// <summary>
    /// Removes control from the character controller and sets the start_mechanic trigger in the
    /// animator
    /// </summary>
    public void StartMechanic() {
        _inMechanic = true;
        enabled = true;
        _audioSource.PlayOneShot(hips_hups[Random.Range(0, hips_hups.Length)]);
        _mechanicAnimator.SetTrigger("start_mechanic");
        onStartCallback();
    }

    public bool CanDoMechanic() {
        return !_inMechanic;
    }

    /// <summary>
    /// A callback function that is called when a mechanic is started to allow and entry point into
    /// the mechanic
    /// </summary>
    protected virtual void onStartCallback() {
        throw new System.Exception("Not Implemented");
    }

    /// <summary>
    /// A function that should be called when a mechanic is finished. It triggers the end_mechanic
    /// trigger in the animator and gives control back to the character controller
    /// </summary>
    protected void Finish() {
        _inMechanic = false;
        _mechanicAnimator.SetTrigger("end_mechanic");
        enabled = false;
    }
}
