using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class TimelineController : MonoBehaviour
{
    IsometricPlayerMovementController playerController;
    DialogueRunner dialogueRunner;
    TriggerEvent trigger;
    [SerializeField] PlayableDirector timeline;
    // [SerializeField] GameObject objectSprite;
    public bool setYarn = false;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        trigger = FindObjectOfType<TriggerEvent>();
        playerController = FindObjectOfType<IsometricPlayerMovementController>();

        if (setYarn)
        {
            dialogueRunner.AddCommandHandler<string>("PlayTimeline", StartTimelineByYarn);
            dialogueRunner.AddCommandHandler<string>("StopTimeline", StopTimeline);
            dialogueRunner.AddCommandHandler<string>("ResumeTimeline", ResumeTimeline);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            StartTimeline();
    }

    public void StartTimeline()
    {
        if (timeline != null)
            timeline.Play();
    }

    public void StartTimelineByYarn(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            playableDirector.Play();
        }
    }

    public void SetToFreeRoam()
    {
        GameController.Instance.state = GameState.FreeRoam;
        playerController.ResumeMoving();
    }

    public void SetToTimeline()
    {
        GameController.Instance.state = GameState.Timeline;
        playerController.StopMoving();
    }
    public void SetToDialogue()
    {
        GameController.Instance.state = GameState.Dialogue;
    }

    public void StopTimeline(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            if (playableDirector != null)
            {
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
            }
        }
    }

    public void ResumeTimeline(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            if (playableDirector != null)
            {
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
        }
    }

    public void SendToMainTitle()
    {
        SceneManager.LoadScene("TitleGame");
    }

    public void StartDialogueTinmeline(NPCData npcData)
    {
        trigger.StartDialogue(npcData.dialogueName);
    }

    public void SetVarToFirstFloor()
    {
        var variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();

        bool beetlePermission;
        variableStorage.TryGetValue("$beetlePermission", out beetlePermission);
        variableStorage.SetValue("$beetlePermission", beetlePermission = true);
    }

    public void DestroyPlayer()
    {
        GameObject player = GameObject.Find("EssentialObjects");
        Destroy(player);
    }
}
