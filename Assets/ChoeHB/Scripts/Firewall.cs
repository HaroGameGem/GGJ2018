using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Firewall {

    [ShowInInspector] public int difficulty { get; private set; }
    public Firewall(int difficulty)
    {
        this.difficulty = difficulty;
    }

}
