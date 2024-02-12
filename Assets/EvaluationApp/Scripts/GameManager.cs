using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public GameObject IntroductionPrefab;
    //public GameObject SubjectiveEvaluationPrefab;
    //public GameObject DirectionGuessingPrefab;

    public bool IsVR = true;
    public bool allowEvaluationNonVR = false;


    public GameObject introductionObject;
    public GameObject subjectiveObject;
    public GameObject directionGuessingObject;

    public GameObject roomModel;


    public enum EvaluationState
    {
        Introduction,
        SubjectiveEvaluation,
        DirectionGuessing,
        Complete
    }

    public EvaluationState evaluationState = 0;

    public List<GameObject> VRStuff;
    public List<GameObject> NonVRStuff;

    private static GameManager instance;


    public DataManager dataManager;

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
            dataManager = new DataManager();


            foreach(GameObject g in VRStuff)
            {
                g.SetActive(IsVR);
            }

            foreach (GameObject g in NonVRStuff)
            {
                g.SetActive(!IsVR);
            }

            if (allowEvaluationNonVR) VRStuff[VRStuff.Count - 1].SetActive(true);
            




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
        dataManager.InitializeSession();
        //HideRoomModel(0);


        switch(evaluationState)
        {
            case EvaluationState.Introduction:
                StartIntroduction(); break;
            case EvaluationState.SubjectiveEvaluation:
                StartSubjectiveEvaluation(); break;
            case EvaluationState.DirectionGuessing: 
                StartDirectionGuessing(); break;
            default: break;
        }

    }

    public void InitializeGame()
    {

    }

    public void StartIntroduction()
    {
        introductionObject.SetActive(true);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
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

    public void FinishSession()
    {
        SaveData();
    }

    public void SaveData()
    {
        dataManager.SaveSession();
    }

    public void ShowRoomModel(float time)
    {
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 1, time);
        }
    }

    public void HideRoomModel(float time)
    {
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 0, time);
            Debug.Log("Set Alpha");
        }
       
    }
}
