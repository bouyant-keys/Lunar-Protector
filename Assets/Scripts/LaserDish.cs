using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDish : MonoBehaviour
{
    //LaserDish should recieve input from the player when InDanger or Broken, then activate one of either minigames
    enum DishState { Idle, InDanger, Broken };
    [SerializeField] DishState _currentState;

    LaserMinigame _laserMinigame;
    RepairMinigame _repairMinigame;
    [SerializeField] SpriteRenderer _stateIndicator;
    [SerializeField] Color _idleColor;
    [SerializeField] Color _dangerColor;
    [SerializeField] Color _brokenColor;

    public GameObject _dish;
    public GameObject _laser;
    public int _asteroids = 0;

    private void Start()
    {
        _laser.SetActive(false);
        _laserMinigame = FindObjectOfType<LaserMinigame>();
        _currentState = DishState.Idle;
        _stateIndicator.color = _idleColor;
    }

    public void UnderAttack()
    {
        print("UnderAttack");
        _currentState = DishState.InDanger;
        _stateIndicator.color = _dangerColor;
    }
    public void ThreatNeutalized()
    {
        GameManager._activeDishes--;
        _currentState = DishState.Idle;
        _stateIndicator.color = _idleColor;
    }
    public void ResetState()
    {
        _currentState = DishState.Idle;
        _stateIndicator.color = _idleColor;
    }

    //May remove Repair Minigame!
    public void Broken()
    {
        _currentState = DishState.Broken;
        _stateIndicator.color = _brokenColor;
    }
    public void Fixed()
    {
        _currentState = DishState.Idle;
        _stateIndicator.color = _idleColor;
    }


    public void StartMinigame()
    {
        if (_currentState == DishState.InDanger)
        {
            _laserMinigame.PlayMinigame(this, _asteroids);
        }
        else if (_currentState == DishState.Broken)
        {
            _repairMinigame.PlayMinigame(this);
        }
        else
        {
            return;
        }
    }
}
