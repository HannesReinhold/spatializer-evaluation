using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject IntroductionPrefab;
    public GameObject SubjectiveEvaluationPrefab;
    public GameObject DirectionGuessingPrefab;


    private GameObject introductionObject;
    private GameObject subjectiveObject;
    private GameObject directionGuessingObject;


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

    public void StartNewSession()
    {
        if(introductionObject == null) introductionObject = Instantiate(IntroductionPrefab);
    }

    public void InitializeGame()
    {

    }

    public void StartSubjectiveEvaluation()
    {
        if (subjectiveObject == null) subjectiveObject = Instantiate(SubjectiveEvaluationPrefab);
        if (introductionObject != null) introductionObject.SetActive(false);
    }

    public void StartDirectionGuessing()
    {
        if(directionGuessingObject == null) directionGuessingObject = Instantiate(DirectionGuessingPrefab);
        if(introductionObject != null) introductionObject.SetActive(false);
    }
}
