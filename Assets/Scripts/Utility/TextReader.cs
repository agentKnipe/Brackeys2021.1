using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextReader : MonoBehaviour {
    private static TextReader _textReader;

    [SerializeField]
    private TextAsset _speechFile;

    private List<string> _speechLines;


    public static TextReader Instance {
        get {
            return _textReader;
        }
    }

    private void Awake() {
        if (_textReader != null && _textReader == this) {
            Destroy(this.gameObject);
        }
        else {
            _textReader = this;
        }
    }

    private void Start() {
        _speechLines = new List<string>();

        using(var reader = new StreamReader(new MemoryStream(_speechFile.bytes))) {
            while (reader.Peek() > 0) {
                _speechLines.Add(reader.ReadLine());
            }
        }
    }

    public string RandomSpeechGet() {
        var rand = Random.Range(0, _speechLines.Count);

        return _speechLines[rand];
    }

}
