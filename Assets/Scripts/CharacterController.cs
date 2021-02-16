// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CharacterController : MonoBehaviour{
//     // Let 1 = down, 2 = right, 3 = up, 4 = left
//     private int _gravityDirection = 1;
//     private float _facingDirection = 1f;
//     private int _reverseFactor = 1;

//     [SerializeField]
//     private float _speed = 5f;

//     [SerializeField]
//     private Rigidbody2D _rigidBody2D;

//     [SerializeField]
//     private BoxCollider2D _boxCollider2D;

//     public float yForce, xForce, temp = 0f;
//     public readonly float fallSpeed = 50f;

//     [SerializeField]
//     private LayerMask _platformLayerMask;

//     // Component Initialization
//     void Awake() {
//         _rigidBody2D = transform.GetComponent<Rigidbody2D>();
//         _boxCollider2D = transform.GetComponent<BoxCollider2D>();
//     }

//     // Start is called before the first frame update
//     void Start(){
//         // Initial Gravity Set (Downwards)
//         yForce = -fallSpeed;
//     }

//     // Update is called once per frame
//     void Update(){
//         if (!isGrounded()) {
//             if (_facingDirection < 0) {
//                 temp = -xForce;
//                 xForce = yForce;
//                 yForce = temp;
//                 handleRotation(true);
//             } else if (_facingDirection > 0) {
//                 temp = xForce;
//                 xForce = -yForce;
//                 yForce = temp;
//                 handleRotation(false);
//             }
//         }
//         _rigidBody2D.AddForce(new Vector2(xForce, yForce));
//     }

//     void FixedUpdate() {
//         var input = Input.GetAxisRaw("Horizontal");
//         if (_gravityDirection > 2) {
//             _reverseFactor = -1;
//         } else {
//             _reverseFactor = 1;
//         }
//         if (_gravityDirection % 2 != 0) {
//             _rigidBody2D.velocity = new Vector2(input * _speed * _reverseFactor, 0);
//         } else {
//             _rigidBody2D.velocity = new Vector2(0, input * _speed * _reverseFactor);
//         }

//         CheckIfShouldFlip(input);
//     }
    
//     void handleRotation(bool facingRight) {
//         if (facingRight) {
//             transform.Rotate(0f, 0f, 90f);
//             _gravityDirection--;
//             if (_gravityDirection < 1) _gravityDirection = 4;
//             transform.Translate(new Vector2(_facingDirection/5, 0.1f));
//         } else {
//             transform.Rotate(0f, 0f, 90f);
//             _gravityDirection++;
//             if (_gravityDirection > 4) _gravityDirection = 1;
//             transform.Translate(new Vector2(-_facingDirection/5, 0.1f));
//         }       
//     }

//     bool isGrounded() {
//         float extraHeightCompensation = 1f;
//         Vector2 gravityVector;
//         if (_gravityDirection == 1) {
//             gravityVector = Vector2.down;
//         } else if (_gravityDirection == 2) {
//             gravityVector = Vector2.right;
//         } else if (_gravityDirection == 3) {
//             gravityVector = Vector2.up;
//         } else {
//             gravityVector = Vector3.left;
//         }
//         RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, gravityVector,
//             extraHeightCompensation, _platformLayerMask);
//         return raycastHit.collider != null;
//     }

//     private void CheckIfShouldFlip(float xInput) {
//         var direction = 0.0f;

//         if(xInput > 0) {
//             direction = -1f;
//         }
//         else if(xInput < 0) {
//             direction = 1f;
//         }
//         else {
//             direction = 0f;
//         }

//         if (direction != 0 && direction != _facingDirection) {
//             Flip();
//         }
//     }

//     private void Flip() {
//         _facingDirection *= -1;
//         transform.Rotate(0.0f, 180.0f, 0.0f);
//     }
// }



// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CharacterController : MonoBehaviour{
//     // Let 1 = down, 2 = right, 3 = up, 4 = left
//     private int _gravityDirection = 1;
//     private float _facingDirection = 1f;
//     private int _reverseFactor = 1;

//     private bool _facingRight = false;

//     [SerializeField]
//     private float _speed = 100f;

//     [SerializeField]
//     private Rigidbody2D _rigidBody2D;

//     [SerializeField]
//     private BoxCollider2D _boxCollider2D;

//     public float yForce, xForce, temp = 0f;
//     public readonly float fallSpeed = 50f;

//     private Rigidbody2D _rigidbody;

//     private Animator _animator;

//     private float _horizontalMove = 0f;

//     [SerializeField]
//     private LayerMask _platformLayerMask;

//     // Component Initialization
//     void Awake() {
//         _rigidBody2D = transform.GetComponent<Rigidbody2D>();
//         _boxCollider2D = transform.GetComponent<BoxCollider2D>();
//     }

//     // Start is called before the first frame update
//     void Start() {
//         // Initial Gravity Set (Downwards)
//         yForce = -fallSpeed;
//         _animator = GetComponent<Animator>();
//         _rigidbody = GetComponent<Rigidbody2D>();
//     }

//     // Update is called once per frame
//     void Update(){
//         if (!isGrounded()) {
//             if (_facingDirection < 0) {
//                 temp = -xForce;
//                 xForce = yForce;
//                 yForce = temp;
//                 handleRotation(true);
//             } else if (_facingDirection > 0) {
//                 temp = xForce;
//                 xForce = -yForce;
//                 yForce = temp;
//                 handleRotation(false);
//             }
//         }
//         _rigidBody2D.AddForce(new Vector2(xForce, yForce));
//         _horizontalMove = Input.GetAxisRaw("Horizontal");
//     }

//     void FixedUpdate() {
//         var input = Input.GetAxisRaw("Horizontal");
//         if (_gravityDirection > 2) {
//             _reverseFactor = -1;
//         } else {
//             _reverseFactor = 1;
//         }
//         if (_gravityDirection % 2 != 0) {
//             _rigidBody2D.velocity = new Vector2(input * _speed * _reverseFactor, 0);
//         } else {
//             _rigidBody2D.velocity = new Vector2(0, input * _speed * _reverseFactor);
//         }
//         Move();
//     }

//     void handleRotation(bool facingRight) {
//         if (facingRight) {
//             transform.Rotate(0f, 0f, 90f);
//             _gravityDirection--;
//             if (_gravityDirection < 1) _gravityDirection = 4;
//             transform.Translate(new Vector2(_facingDirection/2, 0.1f));
//         } else {
//             transform.Rotate(0f, 0f, 90f);
//             _gravityDirection++;
//             if (_gravityDirection > 4) _gravityDirection = 1;
//             transform.Translate(new Vector2(-_facingDirection/2, 0.1f));
//         }       
//     }

//     bool isGrounded() {
//         float extraHeightCompensation = 1f;
//         Vector2 gravityVector;
//         if (_gravityDirection == 1) {
//             gravityVector = Vector2.down;
//         } else if (_gravityDirection == 2) {
//             gravityVector = Vector2.right;
//         } else if (_gravityDirection == 3) {
//             gravityVector = Vector2.up;
//         } else {
//             gravityVector = Vector3.left;
//         }
//         RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, gravityVector,
//             extraHeightCompensation, _platformLayerMask);
//         return raycastHit.collider != null;
//     }

//     private void Move() {
//         // Handles the walking/running animation. Should likely be moved to a function but will
//         // worry about it when/if there are more animations.
//         if(_horizontalMove != 0) {
//             _animator.SetBool("walking", true);
//         } else {
//             _animator.SetBool("walking", false);
//         }

//         // Set the velocity to the frame normalized time * the input on the x and maintain the y
//         // velocity so that gravity works correctly.
//         Vector2 velocity = new Vector2(
//             _horizontalMove * _speed * Time.fixedDeltaTime,
//             _rigidbody.velocity.y
//         );

//         // Update the velocity with the player input velocity
//         _rigidbody.velocity = velocity;

//         CheckIfShouldFlip(velocity.x);
//     }

//     private void CheckIfShouldFlip(float xInput) {
//         if(xInput > 0 && !_facingRight) {
//             Flip();
//         } else if(xInput < 0 && _facingRight) {
//             Flip();
//         }
//     }

//     private void Flip() {
//         _facingRight = !_facingRight;

//         // Multiply the player's x local scale by -1.
// 		Vector3 scale = transform.localScale;
// 		scale.x *= -1;
// 		transform.localScale = scale;
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour{
    // Let 0 = down, 1 = right, 2 = up, 3 = left
    private int _gravityDirection = 0;
    private Vector2[] _directionVectors = {Vector2.down, Vector2.right, Vector2.up, Vector2.left};

    private int _reverseFactor = 1;

    private int _rotationCooldown = 0;

    private bool _facingRight = false;

    [SerializeField]
    private float _speed = 5f;

    private Rigidbody2D _rigidbody;

    private BoxCollider2D _boxCollider2D;

    private Animator _animator;

    private float _horizontalMove = 0f;

    public float yForce, xForce, temp = 0f;

    [SerializeField]
    private LayerMask _platformLayerMask;

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
        Move();
    }

    void FixedUpdate() {
        if (_rotationCooldown > 0) _rotationCooldown--;
        if (_rotationCooldown == 0) CheckIfShouldRotate();
        _rigidbody.AddForce(new Vector2(xForce, yForce));
    }

    private bool isGrounded() {
        float extraHeightCompensation = 1f;
        Vector2 gravityVector;
        gravityVector = _directionVectors[_gravityDirection];
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            _boxCollider2D.bounds.center,
            _boxCollider2D.bounds.size,
            0f,
            gravityVector,
            extraHeightCompensation,
            _platformLayerMask
        );
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
        Vector2 velocity;

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
        Debug.Log(velocity);

        // Update the velocity with the player input velocity
        _rigidbody.velocity = velocity;

        CheckIfShouldFlip(_horizontalMove);
    }

    private void handleRotation() {
        if (_facingRight) {
            transform.Rotate(0f, 0f, 90f);
            _gravityDirection--;
            if (_gravityDirection < 0) _gravityDirection = 3;
        } else {
            transform.Rotate(0f, 0f, 90f);
            _gravityDirection++;
            if (_gravityDirection > 3) _gravityDirection = 0;
        }
        _rotationCooldown = 10;  
    }

    private void CheckIfShouldRotate() {
        if (!isGrounded()) {
            if (_facingRight) {
                temp = -xForce;
                xForce = yForce;
                yForce = temp;
                handleRotation();
            } else {
                temp = xForce;
                xForce = -yForce;
                yForce = temp;
                handleRotation();
            }
            return;
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

