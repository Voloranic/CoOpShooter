using System;
using UnityEngine;

public class DoSomethingWhenPlayerSpawns : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerMovement.LocalInstance != null)
        {
            print("Player is already spawned.");
        }
        else
        {
            PlayerMovement.OnAnyPlayerSpawned += Print;
        }
    }

    private void Print(object sender, EventArgs e)
    {
        print("Player Spawned!");
    }

}
