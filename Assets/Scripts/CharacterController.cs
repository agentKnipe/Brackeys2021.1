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

    private CircleCollider2D _circleCollider2D;

    private Animator _animator;

    private float _horizontalMove = 0f;

    private float yForce, xForce, temp = 0f;

    private Vector2 velocity;

    private Vector3 previousLocation;

    [SerializeField]
    private LayerMask _platformLayerMask;


    [Tooltip("How far ahead of the ant the raycasts will look for determining whether to stick to the next wall")]
    [SerializeField]
    float wallRayLength = 0.3f;

    [SerializeField]
    private LayerMask _waterLayerMask;
    [SerializeField]
    private float groundRayLength = 0.5f;
    [SerializeField]
    private float gravityForce = 6f;
    private SpriteRenderer _renderer;
    private bool canMove = true;


    // Start is called before the first frame update
    void Start(){
        yForce = gravityForce;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void ToggleMovement(bool toggle) {
        for(int i = 0; i < 4; i++) {
            CheckIfShouldRotate();
        }
        _rigidbody.velocity = Vector2.zero;
        canMove = toggle;
    }

    // Update is called once per frame
    void Update(){
        // For some reason the transform likes to rotate by -180 degrees on the x axis when its
        // upside down, which has very detrimental effects. This just ensure this doesnt happen.
        if(transform.localRotation.x == 1) {
            transform.Rotate(180f, 0, 180f);
        }

        _horizontalMove = Input.GetAxis("Horizontal");

        // If we aren't on the floor, set our rotation to be directly downward.
    }

    void FixedUpdate() {
        // Doing 4 to account for each direction. Almost certainly not the best way to do it, but
        // I plugged it in and it just worked!
        if(canMove) {
            for(int i = 0; i < 4; i++) {
                CheckIfShouldRotate();
            }
            Move();
            if(!isGrounded(out RaycastHit2D hit)) {
                _facingRight = true;
                _reverseFactor = 1;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                _gravityDirection = 0;
            }
        }
        _rigidbody.AddForce(new Vector2(xForce, yForce));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Water") {
            LevelManager.LevelManagerInstance.PlayerDied();
        }
    }

    private bool isGrounded(out RaycastHit2D raycastHit) {
        Vector2 gravityVector;
        gravityVector = _directionVectors[_gravityDirection];
        raycastHit = Physics2D.BoxCast(
            _circleCollider2D.bounds.center,
            _circleCollider2D.bounds.size,
            0f,
            gravityVector,
            groundRayLength,
            _platformLayerMask
        );

        Vector2 startCentre = _circleCollider2D.bounds.center;
        Vector2 inverseGrav = new Vector2(gravityVector.y, gravityVector.x);

        // Casting 3 rays, so that we can take the average of the normal so that the rotation doesnt go crazy when going over steepish terrain
        startCentre += inverseGrav * 0.1f;
        Debug.DrawRay(startCentre, gravityVector * groundRayLength, Color.blue);
        RaycastHit2D hit2 = Physics2D.BoxCast(
            startCentre,
            _circleCollider2D.bounds.size,
            0f,
            gravityVector,
            groundRayLength,
            _platformLayerMask
        );

        startCentre -= inverseGrav * 0.2f;
        Debug.DrawRay(startCentre, gravityVector * groundRayLength, Color.blue);
        RaycastHit2D hit3 = Physics2D.BoxCast(
            startCentre,
            _circleCollider2D.bounds.size,
            0f,
            gravityVector,
            groundRayLength,
            _platformLayerMask
        );

        raycastHit.normal = (hit2.normal + hit3.normal + raycastHit.normal) / 3f;

        // Draws a gizmo for the ray that is cast to determine whether we are grounded
        Debug.DrawRay(_circleCollider2D.bounds.center, gravityVector * groundRayLength, Color.blue);
        return raycastHit.collider != null;
    }

    private bool isFacingWall() {
        // Looking in both directions as there is a bug which makes the rotation do crazy stuff.
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            _circleCollider2D.bounds.center,
            _circleCollider2D.bounds.size/100,
            0f,
            -transform.right,
            wallRayLength,
            _platformLayerMask
        );

        RaycastHit2D raycastHit2 = Physics2D.BoxCast(
            _circleCollider2D.bounds.center,
            _circleCollider2D.bounds.size/100,
            0f,
            transform.right,
            wallRayLength,
            _platformLayerMask
        );

        // Draws a gizmo for the ray that is cast to determine whether we are next to a wall
        Debug.DrawRay(_circleCollider2D.bounds.center, -transform.right * wallRayLength, Color.blue);
        return raycastHit.collider != null || raycastHit2.collider != null;
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
                    handleRotation(false);
                } else {
                    handleRotation(true);
                }
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
            if (_facingRight)
                handleRotation(true);
            else
                handleRotation(false);
        } else {
            // If it is grounded, set its rotation to the normal of the ray hit
            transform.localRotation = Quaternion.FromToRotation(new Vector2(transform.up.x, transform.up.y), ray.normal) * transform.localRotation;
            xForce = gravityForce * -ray.normal.x;
            yForce = gravityForce * -ray.normal.y;
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

        _renderer.flipX = !_renderer.flipX;
    }
}

