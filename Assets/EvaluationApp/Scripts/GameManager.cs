using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public GameObject IntroductionPrefab;
    //public GameObject SubjectiveEvaluationPrefab;
    //public GameObject DirectionGuessingPrefab;


    public GameObject introductionObject;
    public GameObject subjectiveObject;
    public GameObject directionGuessingObject;


    public bool hasCompletedIntroduction = false;
    public bool hasCompletedSubjectiveEvaluation = false;
    public bool hasCompletedDirectionGuessing = false;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            



            StartNewSession();
        }
        InitializeGame();

    }

    private void Start()
    {
        SetupWorldCamera();
    }

    private void Update()
    {
    }

    private void SetupWorldCamera()
    {
        Canvas[] canvasList = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasList)
        {
            canvas.worldCamera = Camera.main;
            Debug.Log(Camera.main);
        }
    }

    public void StartNewSession()
    {
        introductionObject.SetActive(true);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
    }

    public void InitializeGame()
    {

    }

    public void StartSubjectiveEvaluation()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(true);
        directionGuessingObject.SetActive(false);
    }

    public void StartDirectionGuessing()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(true);
    }
}
