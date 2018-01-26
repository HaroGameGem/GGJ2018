using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugNumpad : MonoBehaviour {

    public NumPad numpad;
    int floaton;
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            numpad.Float();
            floaton=1;
        }

        if (Input.GetKeyDown(KeyCode.Space)&&floaton==1)
        {
            int rand = Random.Range(1, 5);
            numpad.Active(rand, (result) => Debug.Log(result));
        }
	}
}
