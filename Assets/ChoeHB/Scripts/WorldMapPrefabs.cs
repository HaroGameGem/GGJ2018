using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldMapPrefabs : SingletonScriptableObject<SpriteStorage> {

#if UNITY_EDITOR
    [MenuItem("Custom/WorldMapPrefabs")]
    static void Select()
    {
        Selection.activeObject = instance;
    }
#endif


}
