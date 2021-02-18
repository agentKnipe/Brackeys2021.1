using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectectiveController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision) {
        //The player hit the objective, trigger win screen
        if(collision.collider.name == "Player") {
            LevelManager.LevelManagerInstance.LevelCleared();
        }
    }
}
