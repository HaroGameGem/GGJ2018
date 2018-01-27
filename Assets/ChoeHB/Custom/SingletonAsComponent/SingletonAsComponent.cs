using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonAsComponent<T> : SerializedMonoBehaviour where T : SingletonAsComponent<T>
{
    protected static bool isDestroyed;
    protected static T instance_;
    public static T instance
    {
        get
        {
            if (instance_)
                return instance_;

            T newInstance;
            T[] ts = FindObjectsOfType<T>();

            if (ts.Length == 0)
            {
                Debug.LogError(typeof(T).Name + "가 없어서 생성함");
                newInstance = new GameObject().AddComponent<T>();
                newInstance.name = typeof(T).Name;
            }

            else if (ts.Length == 1)
            {
                newInstance = ts[0];
            }

            else
            {
                Debug.Log(typeof(T).Name + "가 너무 많음");
                ts.Skip(1).ForEach(t => Destroy(t.gameObject));
                newInstance = ts[0];
            }
            DontDestroyOnLoad(newInstance.gameObject);
            instance_ = newInstance;
            return instance_;
        }

    }

    protected virtual void Awake()
    {
        T t = GetComponent<T>();
        if (instance == null)
            instance_ = t;

        else if (instance != t)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (instance_ == GetComponent<T>())
            isDestroyed = true;
    }


}
