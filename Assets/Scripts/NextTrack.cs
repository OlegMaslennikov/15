using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTrack : MonoBehaviour
{
    public Button nextTrackButton;
    GameObject _musicManager;

void Start()
    {
        _musicManager = GameObject.Find("MusicManager");
        nextTrackButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        _musicManager.GetComponent<SoundManager>().ChangeTrack();
    }
}
