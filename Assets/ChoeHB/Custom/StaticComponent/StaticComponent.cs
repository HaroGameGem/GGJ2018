using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticComponent<T> : SerializedMonoBehaviour where T : StaticComponent<T> {
    protected static T instance_;
    public static T instance
    {
        get
        {
            if (instance_ == null)
                instance_ = FindObjectOfType<T>();

            if (instance_ == null)
                Debug.LogError("Can't Find " + typeof(T).Name);

            return instance_;
        }
    }
}
