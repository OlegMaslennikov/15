using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateLerpMovement : MonoBehaviour
{
    private Vector2 _selectedPosition;
    private Vector2 _zeroPosition;
    private bool _canMove = false;
    private float _movementDuration = 0.15f;
    private float _elapsedTime;
    private GameObject _selectedPlate;


    public void GetParams(GameObject selected, GameObject zero)
    {
        _selectedPlate = selected;
        _selectedPosition = selected.transform.position;
        _zeroPosition = zero.transform.position;
        _canMove = true;
        _elapsedTime = 0f;
    }
    void Update()
    {
       if (_canMove == true)
        {
            _elapsedTime += Time.deltaTime;
            float completionPercentage = _elapsedTime / _movementDuration;
            _selectedPlate.transform.position = Vector2.Lerp(_selectedPosition, _zeroPosition, completionPercentage);
            if (completionPercentage == 1)
                _canMove = false;
        } 
    }
}
