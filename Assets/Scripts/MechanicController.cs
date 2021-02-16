using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class MechanicController : MonoBehaviour
{
    [Tooltip("The mechanic that will be used when the key is pressed.")]
    [SerializeField]
    List<Mechanic> mechanics;

    [Tooltip("Is played when any of the provided mechanic keys are pressed")]
    [SerializeField]
    ParticleSystem poofEffect;

    CharacterController controller;
    Animator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if any of the keys are pressed, and if they are then starts the mechanic.
        // Likely needs a way to stop additional presses so that mechanics cant be used while in a
        // mechanic.
        foreach(Mechanic mec in mechanics) {
            if(Input.GetKeyDown(mec.key)) {
                StartCoroutine(StartMechanic(mec));
            }
        }
    }

    // A coroutine so that there can be a delay between the poof effect and the mechanic starting.
    IEnumerator StartMechanic(Mechanic m) {
        controller.enabled = false;
        ParticleSystem effect = Instantiate(poofEffect, transform.position, transform.rotation);
        effect.gameObject.transform.localScale *= m.size;
        yield return new WaitForSeconds(m.delay);
        characterAnimator.SetTrigger("start_mechanic");
        m.onStartCallback();
    }
}
