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
    public int cost;

    CharacterController controller;
    protected Animator mechanicAnimator;

    protected virtual void Start() {
        mechanicAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public virtual void onStartCallback() {
        throw new System.Exception("Not Implemented");
    }

    protected void Finish() {
        controller.enabled = true;
        mechanicAnimator.SetTrigger("end_mechanic");
    }
}
