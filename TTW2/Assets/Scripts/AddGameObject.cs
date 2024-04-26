using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGameObject : MonoBehaviour
{
    [SerializeField] GameObject parentObject;
    GameObject newObject;
    public static AddGameObject Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public void CreateChildObject(string objectName)
    {
        newObject = new GameObject(objectName);
        newObject.AddComponent<PuzzleTrigger>();

        Debug.Log("Add gameobject");
    }
}
