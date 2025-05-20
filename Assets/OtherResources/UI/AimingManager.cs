using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingManager : MonoBehaviour
{
    public GameObject cursorCrosshair;
    public MovementReborn movementReborn;

    void Update()
    {
        bool testInput = Input.GetKey(KeyCode.Mouse1); // aim

        cursorCrosshair.SetActive(testInput);
    }
    
}
