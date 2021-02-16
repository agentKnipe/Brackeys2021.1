using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slingshot : Mechanic
{
    public override void onStartCallback()
    {
        mechanicAnimator.SetBool("is_slingshotting", true);
        StartCoroutine(StopAnimation());
    }

    IEnumerator StopAnimation() {
        yield return new WaitForSeconds(10);
        mechanicAnimator.SetBool("is_slingshotting", false);
        Finish();
    }
}
