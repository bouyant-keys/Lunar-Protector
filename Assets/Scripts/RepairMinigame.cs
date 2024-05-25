using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairMinigame : MonoBehaviour
{
    UIManager _uiManager;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void PlayMinigame(LaserDish laserDish) //Starts Minigame
    {
    }

    public void EndMinigame()
    {
    }
}
