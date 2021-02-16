using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour{
    // Let 1 = down, 2 = right, 3 = up, 4 = left
    private int _gravityDirection = 1;
    private float _facingDirection = 1f;
    private bool _isHorizontal = true;
    private int reverseFactor = 1;

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private Rigidbody2D _rigidBody2D;

    [SerializeField]
    private BoxCollider2D _boxCollider2D;

    public float yForce, xForce, temp = 0f;
    public readonly float fallSpeed = 50f;

    [SerializeField]
    private LayerMask _platformLayerMask;

    // Component Initialization
    void Awake() {
        _rigidBody2D = transform.GetComponent<Rigidbody2D>();
        _boxCollider2D = transform.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start(){
        // Initial Gravity Set (Downwards)
        yForce = -fallSpeed;
    }

    // Update is called once per frame
    void Update(){
        if (!isGrounded()) {
            if (_facingDirection < 0) {
                temp = -xForce;
                xForce = yForce;
                yForce = temp;
                handleRotation(true);
            } else if (_facingDirection > 0) {
                temp = xForce;
                xForce = -yForce;
                yForce = temp;
                handleRotation(false);
            }
        }
        _rigidBody2D.AddForce(new Vector2(xForce, yForce));
    }

    void FixedUpdate() {
        var input = Input.GetAxisRaw("Horizontal");
        if (_gravityDirection > 2) {
            reverseFactor = -1;
        } else {
            reverseFactor = 1;
        }
        if (_gravityDirection % 2 != 0) {
            _rigidBody2D.velocity = new Vector2(input * _speed * reverseFactor, 0);
        } else {
            _rigidBody2D.velocity = new Vector2(0, input * _speed * reverseFactor);
        }

        CheckIfShouldFlip(input);
    }
    
    void handleRotation(bool facingRight) {
        if (facingRight) {
            transform.Rotate(0f, 0f, 90f);
            _gravityDirection--;
            if (_gravityDirection < 1) _gravityDirection = 4;
            transform.Translate(new Vector2(_facingDirection/5, 0.1f));
        } else {
            transform.Rotate(0f, 0f, 90f);
            _gravityDirection++;
            if (_gravityDirection > 4) _gravityDirection = 1;
            transform.Translate(new Vector2(-_facingDirection/5, 0.1f));
        }       
    }

    bool isGrounded() {
        float extraHeightCompensation = 1f;
        Vector2 gravityVector;
        if (_gravityDirection == 1) {
            gravityVector = Vector2.down;
        } else if (_gravityDirection == 2) {
            gravityVector = Vector2.right;
        } else if (_gravityDirection == 3) {
            gravityVector = Vector2.up;
        } else {
            gravityVector = Vector3.left;
        }
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, gravityVector,
            extraHeightCompensation, _platformLayerMask);
        return raycastHit.collider != null;
    }

    private void CheckIfShouldFlip(float xInput) {
        var direction = 0.0f;

        if(xInput > 0) {
            direction = -1f;
        }
        else if(xInput < 0) {
            direction = 1f;
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
