using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour{
    // Let 0 = down, 1 = right, 2 = up, 3 = left
    private int _gravityDirection = 0;
    private Vector2[] _directionVectors = {Vector2.down, Vector2.right, Vector2.up, Vector2.left};

    private int _reverseFactor = 1;

    private bool _facingRight = false;

    [SerializeField]
    private float _speed = 5f;

    private Rigidbody2D _rigidbody;

    private BoxCollider2D _boxCollider2D;

    private Animator _animator;

    private float _horizontalMove = 0f;

    private float yForce, xForce, temp = 0f;

    private Vector2 velocity;

    private Vector3 previousLocation;

    [SerializeField]
    private LayerMask _platformLayerMask;

    [Tooltip("How far ahead of the ant the raycasts will look for determining whether to stick to the next wall")]
    [SerializeField]
    float forwardLookAhead = 1f;

    // Start is called before the first frame update
    void Start(){
        yForce = -5f;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update(){
        _horizontalMove = Input.GetAxis("Horizontal");
        // Doing 4 to account for each direction. Almost certainly not the best way to do it, but
        // I plugged it in and it just worked!
        for(int i = 0; i < 4; i++) {
            CheckIfShouldRotate();
        }
    }

    void FixedUpdate() {
        Move();
        _rigidbody.AddForce(new Vector2(xForce, yForce));
    }

    private bool isGrounded(out RaycastHit2D raycastHit) {
        Vector2 gravityVector;
        gravityVector = _directionVectors[_gravityDirection];
        raycastHit = Physics2D.BoxCast(
            _boxCollider2D.bounds.center,
            _boxCollider2D.bounds.size,
            0f,
            gravityVector,
            forwardLookAhead,
            _platformLayerMask
        );

        // Draws a gizmo for the ray that is cast to determine whether we are grounded
        Debug.DrawRay(_boxCollider2D.bounds.center, gravityVector * forwardLookAhead, Color.blue);
        return raycastHit.collider != null;
    }

    private bool isFacingWall() {

        Vector2 gravityVector;
        int index;
        if (_facingRight) {
            index = _gravityDirection + 1;
            if (index > 3) index = 0;
        } else {
            index = _gravityDirection - 1;
            if (index < 0) index = 3;
        }
        gravityVector = _directionVectors[index];
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            _boxCollider2D.bounds.center,
            _boxCollider2D.bounds.size/100,
            0f,
            gravityVector,
            forwardLookAhead,
            _platformLayerMask
        );

        // Draws a gizmo for the ray that is cast to determine whether we are next to a wall
        Debug.DrawRay(_boxCollider2D.bounds.center, gravityVector * forwardLookAhead, Color.blue);
        return raycastHit.collider != null;
    }

    private void Move() {
        // Handles the walking/running animation. Should likely be moved to a function but will
        // worry about it when/if there are more animations.
        if(_horizontalMove != 0) {
            _animator.SetBool("walking", true);
        } else {
            _animator.SetBool("walking", false);
        }
        if (_gravityDirection > 1) {
            _reverseFactor = -1;
        } else {
            _reverseFactor = 1;
        }
        // Set the velocity to the frame normalized time * the input on the x and maintain the y
        // velocity so that gravity works correctly.
        if (_gravityDirection % 2 == 0) {
            velocity = new Vector2(
                _horizontalMove * _speed * Time.fixedDeltaTime * _reverseFactor,
                _rigidbody.velocity.y
            );
        } else {
            velocity = new Vector2(
                _rigidbody.velocity.x,
                _horizontalMove * _speed * Time.fixedDeltaTime * _reverseFactor
            );
        }

        // Update the velocity with the player input velocity
        _rigidbody.velocity = velocity;
        if (Mathf.Abs(_horizontalMove) > 0) {
            if (isFacingWall()) {
                if (_facingRight) {
                    temp = xForce;
                    xForce = -yForce;
                    yForce = temp;
                    handleRotation(false);
                } else {
                    temp = -xForce;
                    xForce = yForce;
                    yForce = temp;
                    handleRotation(true);
                }
                transform.Rotate(0f, 0f, -90f);
            }
        }
        CheckIfShouldFlip(_horizontalMove);
    }

    private void handleRotation(bool isClockwiseRot) {
        if (isClockwiseRot) {
            _gravityDirection--;
            if (_gravityDirection < 0) _gravityDirection = 3;
        } else {
            _gravityDirection++;
            if (_gravityDirection > 3) _gravityDirection = 0;
        }
    }

    private void CheckIfShouldRotate() {
        if (!isGrounded(out RaycastHit2D ray)) {
            if (_facingRight) {
                temp = -xForce;
                xForce = yForce;
                yForce = temp;
                handleRotation(true);
            } else {
                temp = xForce;
                xForce = -yForce;
                yForce = temp;
                handleRotation(false);
            }
            transform.Rotate(0f, 0f, 90f);
        } else {
            // If it is ground, set its rotation to the normal of the ray hit
            transform.rotation = Quaternion.FromToRotation(transform.up, ray.normal) * transform.rotation;
        }
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

        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}

