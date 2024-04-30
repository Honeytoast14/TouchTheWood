using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public sealed class EssentialObjects : MonoBehaviour
{
    private static readonly Dictionary<Type, Component> disallowMultipleInstancesOfTypes = new Dictionary<Type, Component>();

    [SerializeField]
    private Component disallowMultipleInstancesOf; // drag EventSystem component to this field

    private void Awake()
    {
        if (transform.parent != null)
        {
            Debug.LogError("DontDestroyOnLoad can only be attached to GameObjects in the root of the scene hierarchy.", this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        if (disallowMultipleInstancesOf != null)
        {
            var type = disallowMultipleInstancesOf.GetType();
            if (disallowMultipleInstancesOfTypes.TryGetValue(type, out var firstInstance) && firstInstance != null)
            {
                Destroy(gameObject); // destroy duplicate instances
                // Debug.Log(gameObject.name);
            }
            else
            {
                disallowMultipleInstancesOfTypes[type] = disallowMultipleInstancesOf;
            }
        }
    }

    // automatically assigns any other non-transform component found on the same GameObject
    // to the 'Disallow Multiple Instances Of' field.
    private void Reset() => disallowMultipleInstancesOf = GetComponents<Component>().Where(c => c != transform && c != this).FirstOrDefault();

    private void OnValidate()
    {
        if (transform.parent != null)
        {
            Debug.LogError("DontDestroyOnLoad can only be attached to GameObjects in the root of the scene hierarchy.", this);
        }
    }
}
