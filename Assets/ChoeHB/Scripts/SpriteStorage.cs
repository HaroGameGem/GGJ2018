using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteStorage : SingletonScriptableObject<SpriteStorage> {

    public Sprite line;

#if UNITY_EDITOR
    [MenuItem("Custom/SpriteStorage")]
    static void Select()
    {
        Selection.activeObject = instance;
    }
#endif


}
