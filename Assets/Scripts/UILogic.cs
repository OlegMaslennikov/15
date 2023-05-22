using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;
using TMPro;


public class UILogic : MonoBehaviour
{
    private int _seconds = 0;
    private int _minutes = 0;
    private float _SessionTimeElapsed;
    private string _strMinutes;
    private string _strSeconds;

    public GameObject menuPanel;
    public GameObject victoryPanel;
    public GameObject victoryPanelTMP;
    public GameObject gameLogic;

    private int _moveCounter = 0;

    private bool _gamePaused = false;

    public void Start()
    {
        FieldManagement.ChangeMoves += ChangeMoveCounter;
    }

    void Update()
    {
        if (!_gamePaused)
        {
            _SessionTimeElapsed += Time.deltaTime;
        }
        GameObject timer = GameObject.Find("Timer");
        _seconds = (int)_SessionTimeElapsed - _minutes * 60;
        if (_seconds == 60)
        {
            _minutes += 1;
            _seconds = 0;
        }

        if (_seconds < 10)
        {
            _strSeconds = "0" + (_seconds.ToString());
        }
            else _strSeconds = _seconds.ToString();

        if (_minutes < 10)
        {
            _strMinutes = "0" + (_minutes.ToString());
        }
        else _strMinutes = _minutes.ToString();

        timer.GetComponent<TMP_Text>().text = ("Time: " + _strMinutes + ":" + _strSeconds); 
        
    }

    public void ChangeMoveCounter()
    {
        _moveCounter += 1;
        GameObject scores = GameObject.Find("MoveCounter");
        scores.GetComponent<TMP_Text>().text = ("Moves: " + _moveCounter.ToString());
    }

    public void SetMenuActive()
    {
        menuPanel.active = true;
    }          
    
    public void SetMenuInactive()
    {
        menuPanel.active = false;
    }
    
    public void Pause()
    {
        _gamePaused = true;
        gameLogic.GetComponent<FieldManagement>()._canMove = false;
    }
    public void Unpause()
    {
        _gamePaused = false;
        gameLogic.GetComponent<FieldManagement>()._canMove = true;
    }

    public void Victory()
    {
        Pause();
        victoryPanel.active = true;
        victoryPanelTMP.GetComponent<TMP_Text>().text = ("15! Сделано ходов: " + _moveCounter + ". Потрачено времени: " + _minutes + ":" + _seconds);
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
        {
            { "turns", _moveCounter},
            { "timeInSeconds",  _minutes * 60 + _seconds }
        });
    }
}
