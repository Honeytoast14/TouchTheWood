using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject panel;
    bool inZone = false;
    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {

        if (inZone)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                panel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.X))
                panel.SetActive(false);
        }
        else
        {

            panel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = false;
        }
    }
}
