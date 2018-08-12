using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int Score;
    
    private Text _text;
    
    private void Awake ()
    {
        _text = GetComponent <Text> ();
        Score = 0;
    }
    
    private void Update ()
    {
        _text.text = "Score: " + Score;
    }
}
