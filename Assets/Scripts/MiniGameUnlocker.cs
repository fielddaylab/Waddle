//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//This class sits in the overworld, one for each mini game, and handles the scene loading of the mini games
//after collecting enough pebbles

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameUnlocker : MonoBehaviour
{
    [SerializeField]
    string _sceneName;

    public string SceneName => _sceneName;

    MiniGameController _miniGame = null;

    public MiniGameController MiniGame
    {
        get { return _miniGame; }
        set { _miniGame = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
