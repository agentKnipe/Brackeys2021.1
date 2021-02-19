using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slingshot : Mechanic
{
    [SerializeField]
    float _maxSlingForce = 8f;

    [SerializeField]
    float _slingForceMultiplier = 2f;

    [SerializeField]
    LayerMask _platformLayerMask;
    [SerializeField]
    float _airRotationSpeed;

    private bool _controlling = false;
    private Rigidbody2D _rb;
    private LineRenderer _lr;

    private Vector3 _mouseWorldPosition;
    CharacterController _controller;

    private bool _shot = false;


    protected override void Start() {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        _controller = GetComponent<CharacterController>();

    }

    private void Update() {
        if(_shot)
            transform.Rotate(0f, 0f, _airRotationSpeed * Time.deltaTime * (1f + _rb.velocity.magnitude));

        if(!_controlling)
            return;


        var mousePosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetButtonDown("Fire1")) {
            _lr.enabled = false;
            _shot = true;
            FireSlingshot();

            _controlling = false;
            _mechanicAnimator.SetBool("is_slingshotting", false);
            _inMechanic = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(_shot) {
            Finish();
            _controller.enabled = true;
            _controller.ToggleMovement(true);
        }
    }

    private void FixedUpdate() {
        var position = transform.position;

        var slingForce = CalcSlingForce();

        DrawTrajectory(position, slingForce);
    }

    private void FireSlingshot() {
        _controller.enabled = false;
        // Move it up a bit so it doesnt get stuck on the ground
        Vector3 position = gameObject.transform.up;
        position *= 0.5f;
        gameObject.transform.position += position;

        var slingForce = CalcSlingForce();

        _rb.velocity = slingForce;
    }

    protected override void onStartCallback(){
        _controller.ToggleMovement(false);
        _controlling = true;
        _shot = false;
        _mechanicAnimator.SetBool("is_slingshotting", true);

        _lr.enabled = true;
    }

    private Vector3 CalcSlingForce() {
        Vector3 position = gameObject.transform.up;
        position *= 0.5f;
        position += gameObject.transform.position;
        Debug.DrawLine(transform.position, position, Color.black);

        Vector2 direction = _mouseWorldPosition - position;
        var slingForce = direction * _slingForceMultiplier;
        slingForce = Vector2.ClampMagnitude(slingForce, _maxSlingForce);

        return slingForce;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, _maxSlingForce/_slingForceMultiplier);
    }

    private void DrawTrajectory(Vector2 startPos, Vector2 velocity) {
        var verts = 100;

        _lr.positionCount = verts;

        _lr.transform.position = startPos;

        _lr.sortingOrder = 1;
        _lr.material = new Material(Shader.Find("Sprites/Default"));
        _lr.material.color = Color.red;
        _lr.startWidth = .05f;
        _lr.endWidth = .01f;

        var pos = startPos;

        for (var i = 0; i < verts; i++) {
            var showLine = true;

            if(i > 5) { //ignore the first 5 to ensure we have something useful to test.
                var hit = Physics2D.Raycast(pos, Vector2.down, 50f, _platformLayerMask);
                //Debug.Log(hit.fraction * 1000);

                showLine = hit.fraction * 1000 > .1f;
            }

            if (showLine) {
                _lr.SetPosition(i, pos);

                velocity += Physics2D.gravity * Time.fixedDeltaTime;
                pos += velocity * Time.fixedDeltaTime;
            }
            else {
                //remove the extra vertex positions since we have hit the ground.
                _lr.positionCount = i;
                i = verts;

                break;
            }
        }
    }
}
