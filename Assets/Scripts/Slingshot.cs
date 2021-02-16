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

    private bool _controlling = false;
    private Rigidbody2D _rb;

    protected override void Start() {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(!_controlling)
            return;

        if(Input.GetButtonDown("Fire1")) {
            // Move it up a bit so it doesnt get stuck on the ground
            Vector3 position = gameObject.transform.position;
            position.y += 0.5f;
            gameObject.transform.position = position;

            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 direction = worldPos - gameObject.transform.position;
            Vector2 slingForce = direction * _slingForceMultiplier;
            slingForce = Vector2.ClampMagnitude(slingForce, _maxSlingForce);

            _rb.velocity = slingForce;

            _controlling = false;
            mechanicAnimator.SetBool("is_slingshotting", false);
            StartCoroutine(StopMechanic());
        }



    }

    public override void onStartCallback()
    {
        _controlling = true;
        mechanicAnimator.SetBool("is_slingshotting", true);
    }

    private IEnumerator StopMechanic() {
        yield return new WaitForSeconds(1.5f);
        if(!_controlling) {
            Finish();
        }
    }
}
