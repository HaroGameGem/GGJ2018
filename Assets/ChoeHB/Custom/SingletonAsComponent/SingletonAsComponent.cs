﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonAsComponent<T> : SerializedMonoBehaviour where T : SingletonAsComponent<T>
{
    protected static T instance_;
    public static T instance
    {
        get
        {
            if (isDestroyed)
                return null;
            
            if (instance_ == null)
            {
                T newInstance;
                T[] ts = FindObjectsOfType<T>();
                if(ts.Length == 0)
                {
                    Debug.LogError(typeof(T).Name + "가 없어서 생성함");
                    newInstance = new GameObject().AddComponent<T>();
                    newInstance.name = typeof(T).Name;
                }

                else if(ts.Length == 1)
                {
                    newInstance = ts[0];
                }

                else
                {
                    Debug.Log(typeof(T).Name + "가 너무 많음");
                    ts.Skip(1).ForEach(t => Destroy(t.gameObject));
                    newInstance = ts[0];
                }
                return instance_;
            }
            return instance_;
        }

    }

    protected static bool isDestroyed { get { return isInitailized && !instance_; } }
    private static bool isInitailized;

    private static void Initialize(T newInstance)
    {
        if (instance_)
            throw new System.Exception("Already exist instance");

        instance_ = newInstance;
        isInitailized = true;
        instance_.Awake();
        DontDestroyOnLoad(newInstance.gameObject);

    }

    protected virtual void Awake()
    {
        if (instance_ == this)
            return;

        if (instance_ == null)
            Initialize(GetComponent<T>());

        else
            Destroy(gameObject);
    }

}
