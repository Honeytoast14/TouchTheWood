using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class SwitchPlayTimeline : MonoBehaviour
{
    bool inZone = false;

    [Header("Timeline")]
    [SerializeField] PlayableDirector timeline;

    [Header("Tile Switch")]
    [SerializeField] Tile initialTile;
    [SerializeField] Tile tileChange;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3Int position;
    [SerializeField] float resetDelay = 1.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && inZone && GameController.Instance.state == GameState.FreeRoam)
        {
            if (timeline != null)
            {
                timeline.Play();
            }
            if (tilemap != null)
            {
                tilemap.SetTile(position, tileChange);
                StartCoroutine(ResetTileAfterDelay(resetDelay));
            }
        }
    }

    IEnumerator ResetTileAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(position, initialTile);
    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D colider)
    {
        if (colider.gameObject.tag == "Player")
        {
            inZone = false;
        }
    }
}
