using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class MechanicController : MonoBehaviour
{
    [Tooltip("Which key triggers the corresponding mechanic")]
    [SerializeField]
    List<KeyCode> keys;
    [SerializeField]
    List<Mechanic> mechanics;

    [SerializeField]
    GameObject poofEffect;

    Animator characterAnimator;

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        // Just ensures that the lists are of equal length. This is because Unity cannot serialize
        // dictionaries :/
        if(keys.Count != mechanics.Count) {
            Debug.Log("keys, animations and callbacks length not equal");
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        for(int i = 0; i < keys.Count; i++) {
            // Ensure that there is a key and animation for each place
            if(keys[i] == null || mechanics[i] == null) {
                throw new System.Exception("Key or callback #" + (i + 1).ToString() + " is not set");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < keys.Count; i++) {
            if(Input.GetKeyDown(keys[i])) {
                Mechanic m = mechanics[i];
                StartCoroutine(StartMechanic(m));
            }
        }
    }

    IEnumerator StartMechanic(Mechanic m) {
        GameObject effect = Instantiate(poofEffect, transform.position, transform.rotation);
        effect.transform.localScale *= m.size;
        yield return new WaitForSeconds(m.delay);
        characterAnimator.SetTrigger("start_mechanic");
        m.onStartCallback();
    }
}
