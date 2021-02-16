using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour{
    private static LevelManager _levelManager;

    //TODO: this feels hacky, maybe a better way to track this?
    private List<int> _destroyedInstances = new List<int>();

    [SerializeField]
    private Text _antCount;

    public int AntCount = 5;

    public static LevelManager LevelManagerInstance { 
        get {
            return _levelManager;
        } 
    }

    private void Awake() {
        if(_levelManager != null && _levelManager == this) {
            Destroy(this.gameObject);
        }
        else {
            _levelManager = this;
        }
    }

    // Start is called before the first frame update
    void Start(){
        //_antCount = GetComponent<Text>();

        UpdateCount();
    }

    // Update is called once per frame
    void Update() { 
        
    }

    public void CollectAnt(int instanceID) {
        if (!_destroyedInstances.Contains(instanceID)) {
            _destroyedInstances.Add(instanceID);

            AntCount += 1;

            UpdateCount();
        }
    }

    public void ExpendAnts(int ants) {
        AntCount -= ants;

        UpdateCount();
    }

    private void UpdateCount() {
        _antCount.text = AntCount.ToString();
    }
}
