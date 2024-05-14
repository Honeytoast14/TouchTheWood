using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PuzzleWall : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [Header("Tile Change")]
    [SerializeField] Tile tileChange;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3Int position;

    [Header("Dialogue")]
    [SerializeField] NPCData npcData;

    [Header("Animator")]
    Animator emoji;
    bool interactable = false;
    public bool destroyObject { get; set; } = false;
    TriggerEvent triggerEvent;
    public bool useSwitch { get; set; } = false;
    public SoundPlayer soundPlayer;

    void Start()
    {
        triggerEvent = FindObjectOfType<TriggerEvent>();
        GameObject emojis = GameObject.FindGameObjectWithTag("Emoji");

        if (emojis != null)
        {
            emoji = emojis.GetComponent<Animator>();
            if (emoji != null)
            {
                Debug.Log("Find Emoji");
                emoji.gameObject.SetActive(false);
            }
        }

        if (soundPlayer != null)
            soundPlayer.PlayerMusic();
    }

    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
            {
                if (!useSwitch)
                {
                    if (npcData != null)
                    {
                        triggerEvent.StartDialogue(npcData.dialogueName);
                    }
                    // useSwitch = true;
                }
            }

            if (useSwitch)
                tilemap.SetTile(position, tileChange);

            if (wall != null && destroyObject)
            {
                HideWall(wall);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        // TriggerEvent.Instance.StartDialogue("test");
        if (colision.gameObject.tag == "Player")
        {
            interactable = true;
            // Debug.Log("can interact");
        }
    }

    private void OnTriggerExit2D(Collider2D colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            interactable = false;
            // Debug.Log("cannot interact");
        }
    }

    void HideWall(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void SetEmoji(string animationName, string objectName)
    {
        StartCoroutine(SetEmojiTime(animationName));
        GameObject puzzleGameObject = GameObject.Find(objectName);
        if (puzzleGameObject != null)
        {
            PuzzleWall puzzleWall = puzzleGameObject.GetComponent<PuzzleWall>();
            if (puzzleWall != null)
            {
                puzzleWall.useSwitch = true;
                if (wall != null)
                    puzzleWall.destroyObject = true;
            }
        }
    }

    private IEnumerator SetEmojiTime(string animationName)
    {
        emoji.gameObject.SetActive(true);
        emoji.Play(animationName);
        yield return new WaitForSeconds(0.8f);
        emoji.gameObject.SetActive(false);
    }
}
