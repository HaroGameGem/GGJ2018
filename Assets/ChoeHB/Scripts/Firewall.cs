using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using FlyingWormConsole3;

[Serializable]
public class Firewall {

    public event Action OnUpdateDifficulty;

    private int difficulty_;
    public int difficulty
    {
        get { return difficulty_; }
        set
        {
            difficulty_ = value;
            if (OnUpdateDifficulty != null)
                OnUpdateDifficulty();
        }
    }

    public Firewall(int difficulty)
    {
        this.difficulty = difficulty;
    }
    
    public void Debuffed()
    {
        difficulty--;
    }

}
