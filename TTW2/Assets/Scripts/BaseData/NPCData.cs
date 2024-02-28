using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCData", menuName = "NPC Data", order = 51)]
public class NPCData : ScriptableObject
{
    public string npcName;
    public string dialogueID;
    public bool hasPortrait;

    [HideInInspector]
    public string portraitPath;

    [CustomEditor(typeof(NPCData))]
    public class NPCDataEditor : Editor
    {
        SerializedProperty hasPortraitProp;
        SerializedProperty portraitPathProp;

        void OnEnable()
        {
            hasPortraitProp = serializedObject.FindProperty("hasPortrait");
            portraitPathProp = serializedObject.FindProperty("portraitPath");
        }

        public override void OnInspectorGUI()
        {
            NPCData npcData = (NPCData)target;

            serializedObject.Update();

            DrawDefaultInspector();

            if (hasPortraitProp.boolValue)
            {
                EditorGUILayout.PropertyField(portraitPathProp, new GUIContent("Portrait Path"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
