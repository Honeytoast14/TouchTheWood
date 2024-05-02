using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHideObject : MonoBehaviour
{
    [SerializeField] List<GameObject> hideObject;
    [SerializeField] List<GameObject> showObject;
    [SerializeField] GameObject button;

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
}
