using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMusic : MonoBehaviour
{
    public Button pauseButton;
    GameObject _musicManager;

    void Start()
    {
        _musicManager = GameObject.Find("MusicManager");
        pauseButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        _musicManager.GetComponent<SoundManager>().pauseMusic();
    }

}
