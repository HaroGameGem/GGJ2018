using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Firewall {

    public event Action OnUpdateDifficulty;

    public int difficulty { get; private set; }

    public Firewall(int difficulty)
    {
        this.difficulty = difficulty;
    }
    
    public void UpdateDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        if (OnUpdateDifficulty != null)
            OnUpdateDifficulty();
    }

}
