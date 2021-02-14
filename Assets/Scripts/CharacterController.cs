using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour{
    private float _facingDirection = 1f;

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
    }

    void FixedUpdate() {
        var input = Input.GetAxis("Horizontal");
        _rigidBody.velocity = new Vector2(input * _speed, 0);

        Debug.Log(input);

        CheckIfShouldFlip(input);
    }


    private void CheckIfShouldFlip(float xInput) {
        var direction = 0.0f;

        if(xInput > 0) {
            direction = 1f;
        }
        else if(xInput < 0) {
            direction = -1f;
        }
        else {
            direction = 0f;
        }

        if (direction != 0 && direction != _facingDirection) {
            Flip();
        }
    }

    private void Flip() {
        _facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
