using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class ShowAndHideObject : MonoBehaviour
{
    [SerializeField] List<GameObject> hideObject;
    [SerializeField] List<GameObject> showObject;
    [SerializeField] GameObject button;
    [SerializeField] GameObject oneObjectHide;
    [SerializeField] GameObject oneObjectShow;
    DialogueRunner dialogueRunner;
    public bool setYarn = false;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (setYarn)
        {
            dialogueRunner.AddCommandHandler<string>("HideObject", HideInYarn);
            dialogueRunner.AddCommandHandler<string>("ShowObject", ShowInYarn);
            dialogueRunner.AddCommandHandler<string>("HideManyObject", HideManyObject);
        }
    }

    void Update()
    {
        if (PuzzleRail.Instance != null)
            if (PuzzleRail.Instance.win)
            {
                HideObject();
                ShowObject();
                button.SetActive(false);
                gameObject.SetActive(false);
            }
    }

    void ShowObject()
    {
        if (showObject != null)
            foreach (GameObject game in showObject)
            {
                game.SetActive(true);
            }
    }

    void HideObject()
    {
        if (hideObject != null)
        {
            foreach (GameObject game in hideObject)
            {
                game.SetActive(false);
            }
        }
    }

    public void HideInYarn(string objectParentName)
    {
        GameObject objectToHide = GameObject.Find(objectParentName);
        ShowAndHideObject hideScript = objectToHide.GetComponentInChildren<ShowAndHideObject>();
        if (hideScript.oneObjectHide != null)
        {
            hideScript.oneObjectHide.SetActive(false);
            Debug.Log($"Successfully hid: {objectParentName}");

        }
    }

    public void ShowInYarn(string objectName)
    {
        if (oneObjectShow != null)
        {
            if (objectName == oneObjectShow.name)
            {
                Debug.Log("Object to show exists in oneObjectShow.");
                GameObject objectToShow = GameObject.Find(objectName);
                if (objectToShow != null)
                {
                    Debug.Log("Found object to show: " + objectName);
                    objectToShow.SetActive(true);
                }
            }
        }
    }

    public void HideManyObject(string objectParentName)
    {
        GameObject objectToHide = GameObject.Find(objectParentName);
        ShowAndHideObject hideScript = objectToHide.GetComponentInChildren<ShowAndHideObject>();
        if (hideScript.hideObject != null)
        {
            foreach (GameObject game in hideScript.hideObject)
            {
                game.SetActive(false);
                Debug.Log($"Hide {game.name}");
            }
        }
    }

}
