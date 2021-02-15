using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour{
    private bool _facingRight = false;

    [SerializeField]
    private float _speed = 5f;

    private Rigidbody2D _rigidbody;

    private Animator _animator;

    // Start is called before the first frame update
    void Start(){
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
    }

    void FixedUpdate() {
        Move();
    }

    private void Move() {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Handles the walking/running animation. Should likely be moved to a function but will
        // worry about it when/if there are more animations.
        if(horizontalInput != 0) {
            _animator.SetBool("walking", true);
        } else {
            _animator.SetBool("walking", false);
        }

        // Set the velocity to the frame normalized time * the input on the x and maintain the y
        // velocity so that gravity works correctly.
        Vector2 velocity = new Vector2(
            horizontalInput * _speed * Time.deltaTime,
            _rigidbody.velocity.y
        );

        // Update the velocity with the player input velocity
        _rigidbody.velocity = velocity;

        CheckIfShouldFlip(velocity.x);
    }

    private void CheckIfShouldFlip(float xInput) {
        if(xInput > 0 && !_facingRight) {
            Flip();
        } else if(xInput < 0 && _facingRight) {
            Flip();
        }
    }

    private void Flip() {
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
    }
}
