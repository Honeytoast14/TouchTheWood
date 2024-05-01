using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        StartCoroutine(SetTutorial());
    }
    IEnumerator SetTutorial()
    {
        panel.SetActive(false);
        yield return new WaitForSeconds(2f);
        panel.SetActive(true);
        yield return new WaitForSeconds(4f);
        panel.SetActive(false);
    }
}
