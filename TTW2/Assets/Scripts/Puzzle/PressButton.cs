using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PressButton : MonoBehaviour
{
    [SerializeField] List<GameObject> walls;
    [SerializeField] AudioClip wallHideSound;
    [SerializeField] AudioClip wallShowSound;
    [SerializeField] NPCData nPCData = null;
    public float wallTime = 2f;
    public bool clearArea = false;
    public bool talkTroll = false;
    SoundPlayer soundPlayer;
    TriggerEvent triggerEvent;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        triggerEvent = FindObjectOfType<TriggerEvent>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!talkTroll)
            {
                soundPlayer.PlaySFX(soundPlayer.switchUsed);
                StartCoroutine(HideAndShowWalls());
            }
            else
            {
                triggerEvent.StartDialogue(nPCData.dialogueName);
            }
        }
    }

    IEnumerator HideAndShowWalls()
    {

        if (!clearArea)
        {
            soundPlayer.PlaySFX(wallHideSound);
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }

            yield return new WaitForSeconds(wallTime);

            // Show all walls
            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }

            soundPlayer.PlaySFX(wallShowSound);
        }
        else
        {
            soundPlayer.PlaySFX(soundPlayer.correct);
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }
}
