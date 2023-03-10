using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldManagement : MonoBehaviour
{
    public GameObject UILogic;
    
    public Sprite[] spriteArray;
    public GameObject plate;
    private int[] _numbers = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
    private int[,] _goalMatrix = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };
    private int[,] _gameMatrix = new int[4, 4];
    private int _matrixCreaterCounter = 0;
    private GameObject _selectedPlate;
    private Vector2 _mouseClickPosition;
    private int _currentFreeFirstNumberInArray;
    private int _currentFreeSecondNumberInArray;
    private int _currentFreeSecondNumberInArray2;

    private int _solveSumCounter;
    int checkSolvabilityMinorNumbersSum = 0;

    public bool _canMove = true;
    private float _movementDuration = 0.15f;
    private float elapsedTime;

    public delegate void MoveCounter();
    public static event MoveCounter ChangeMoves;


    public void Start()
    {
        ShuffleNumbers();
        CreateMatrix();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            UILogic.GetComponent<UILogic>().Victory();

        if (Input.GetMouseButtonDown(0) && _canMove)
        {
            _mouseClickPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit == true && hit.collider.gameObject.tag == "Plate")
            {
                Vector2 clickedPlate = hit.collider.gameObject.transform.position;
                _selectedPlate = hit.collider.gameObject;
                CheckPlate(clickedPlate);
            }
        }
    }
    public void ShuffleNumbers()
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            int tempNumber = _numbers[i];
            int randomNumber = _numbers[Random.Range(0, 15)];
            _numbers[i] = _numbers[randomNumber];
            _numbers[randomNumber] = tempNumber;
        }
    }
    public void CreateMatrix()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _gameMatrix[i, j] = _numbers[_matrixCreaterCounter];
                if (_gameMatrix[i, j] == 0)
                {
                    _currentFreeFirstNumberInArray = i;
                    _currentFreeSecondNumberInArray = j;
                }
                GameObject currentPlate = Instantiate(plate, transform.position = new Vector2(j, i * -1), transform.rotation);
                currentPlate.GetComponent<SpriteRenderer>().sprite = spriteArray[_gameMatrix[i, j]];
                currentPlate.name = _gameMatrix[i, j].ToString();
                _matrixCreaterCounter++;
            }
        }
        CheckSolvability();
    }

    public void CheckSolvability()
    {
        for (int i = 0; i < 15; i++)
        {
            int currentCheckSolvabilityNumber = _numbers[i];
            int step = 1;
            for (int x = 0; x < 15 - i; x++)
            {
                if (currentCheckSolvabilityNumber > _numbers[i + step] && _numbers[i + step] != 0)
                   checkSolvabilityMinorNumbersSum += 1;
                step += 1;
            }
            _solveSumCounter += checkSolvabilityMinorNumbersSum;
            Debug.Log(checkSolvabilityMinorNumbersSum);
            checkSolvabilityMinorNumbersSum = 0;
        }                           

        if ((_solveSumCounter + _currentFreeFirstNumberInArray + 1) % 2 == 0)
        {
            
            Debug.Log("–ˇ‰ Ò ÌÛÎÂÏ: " + (_currentFreeFirstNumberInArray + 1));
            Debug.Log("—ÛÏÏ‡:" + (_solveSumCounter + _currentFreeFirstNumberInArray + 1));
            
            Debug.Log("–≈ÿ¿≈ÃŒ");
        }
        else 
        {
            SceneManager.LoadScene(0);
        }
    }

    public void CheckPlate(Vector2 clickedPlate)
    {
        int currentPlateFirstNumberInArray = Mathf.Abs((int)(clickedPlate.y));
        int currentPlateSecondNumberInArray = ((int)(clickedPlate.x));
        if (currentPlateFirstNumberInArray == _currentFreeFirstNumberInArray && currentPlateSecondNumberInArray + 1 == _currentFreeSecondNumberInArray)
            MovePlate(currentPlateFirstNumberInArray, _currentFreeFirstNumberInArray, currentPlateSecondNumberInArray, _currentFreeSecondNumberInArray);
        if (currentPlateFirstNumberInArray == _currentFreeFirstNumberInArray && currentPlateSecondNumberInArray - 1 == _currentFreeSecondNumberInArray)
            MovePlate(currentPlateFirstNumberInArray, _currentFreeFirstNumberInArray, currentPlateSecondNumberInArray, _currentFreeSecondNumberInArray);
        if (currentPlateFirstNumberInArray + 1 == _currentFreeFirstNumberInArray && currentPlateSecondNumberInArray == _currentFreeSecondNumberInArray)
            MovePlate(currentPlateFirstNumberInArray, _currentFreeFirstNumberInArray, currentPlateSecondNumberInArray, _currentFreeSecondNumberInArray);
        if (currentPlateFirstNumberInArray - 1 == _currentFreeFirstNumberInArray && currentPlateSecondNumberInArray == _currentFreeSecondNumberInArray)
            MovePlate(currentPlateFirstNumberInArray, _currentFreeFirstNumberInArray, currentPlateSecondNumberInArray, _currentFreeSecondNumberInArray);
    }
    public void MovePlate(int plateFirstNum, int freeFirstNum, int plateSecondNum, int freeSecondNum)
    {
        ChangeMoves();
        
        int tempFirstNum = freeFirstNum;
        int tempSecondNum = freeSecondNum;
        _gameMatrix[_currentFreeFirstNumberInArray, _currentFreeSecondNumberInArray] = _gameMatrix[plateFirstNum, plateSecondNum];
        _gameMatrix[plateFirstNum, plateSecondNum] = 0;
        _currentFreeFirstNumberInArray = plateFirstNum;
        _currentFreeSecondNumberInArray = plateSecondNum;
        GameObject zeroPlate = GameObject.Find("0");
        _canMove = false;
        StartCoroutine(PlateMovement());
        GetComponent<PlateLerpMovement>().GetParams(_selectedPlate, zeroPlate);
        zeroPlate.transform.position = new Vector2(plateSecondNum, plateFirstNum * -1);
        GetComponent<AudioSource>().PlayDelayed(_movementDuration);
        CheckVictoryCondition();
    }
    public void CheckVictoryCondition()
    {
        int checkElementsCounter = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_gameMatrix[i, j] == _goalMatrix[i, j])
                    checkElementsCounter++;
                else return;
                if (checkElementsCounter == 16)
                {
                    UILogic.GetComponent<UILogic>().Victory();
                }
            }
        }
    }

    IEnumerator PlateMovement() 
    {
        yield return new WaitForSeconds(_movementDuration);
        _canMove = true;
    }
}