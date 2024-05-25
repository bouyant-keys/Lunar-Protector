using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager _uiManager;
    SFXManager _sfxManager;
    MoonBase _moonBase;
    AsteroidSpawner _asteroidSpawner;
    LaserMinigame _laserMinigame;
    LaserDish[] _laserDishes;
    List<LaserDish> _availableDishes = new List<LaserDish>();

    public static float BaseHealth = 100f;
    public static int _totalActiveAsteroids = 0;
    public static int _activeDishes = 0;
    int _maxActiveDishes = 1;
    int _maxActiveAsteroids = 2;
    int _totalAttacks = 0;

    bool _gameOver = false;
    bool _gracePeriodOver = false;
    bool _assessedDamage = false;
    bool _assignedDishes = false;
    float _gameStartTime;
    float _gameEndTime;
    float _survivalTime;

    float _timeBeforeNextAttack = 10f; //Seconds b4 next attack
    float _timeAtLastAttack;
    float _gracePeriodTime = 5f; //How long should starting grace period last
    float _asteroidDamage = 10f;
    
    void Start()
    {
        BaseHealth = 100f;
        _totalActiveAsteroids = 0;
        _activeDishes = 0;

        _uiManager = FindObjectOfType<UIManager>();
        _sfxManager = FindObjectOfType<SFXManager>();
        _laserMinigame = FindObjectOfType<LaserMinigame>();
        _laserDishes = FindObjectsOfType<LaserDish>();
         _moonBase = FindObjectOfType<MoonBase>();
        _asteroidSpawner = FindObjectOfType<AsteroidSpawner>(); //Use to confirm totalActiveAsteroids

        _uiManager.UpdateHealth();
        ResetAvailableDishes();

        StartCoroutine(StartGracePeriod());
    }

    void Update()
    {
        float timePassed = Time.time - _timeAtLastAttack;
        if (timePassed < _timeAtLastAttack) _assignedDishes = false;
        if (!_gameOver && timePassed > _timeBeforeNextAttack)
        {
            if (LaserMinigame._activePlayer) _laserMinigame.EndMinigame();
            CheckDamage();

            AdjustDifficulty();
            PickRandomDish();
        }
    }

    private IEnumerator StartGracePeriod()
    {
        yield return new WaitForSeconds(_gracePeriodTime);
        _gracePeriodOver = true;
        _gameStartTime = Time.time;
        _uiManager.StartTimer();
        PickRandomDish();
    }

    private void CheckDamage()
    {
        if (_totalActiveAsteroids == 0) return; //Deal no damage if all asteroids are gone
        else if (_assessedDamage) return; //If checkDamage has already been checked this turn

        BaseHealth -= _totalActiveAsteroids * _asteroidDamage;
        _totalActiveAsteroids = 0;
        _assessedDamage = true;

        _sfxManager.PlayClip(_sfxManager._baseHit);
        _uiManager.UpdateHealth();

        if (BaseHealth <= 0)
        {
            _gameEndTime = Time.time;
            GameOver();
        }
    }

    private void PickRandomDish()
    {
        if (_assignedDishes) return; //Cannot be run multiple times in one turn
        else if (!_gracePeriodOver) return;

        ResetAvailableDishes();
        for (int i = 0; i < _maxActiveDishes; i++)
        {
            int index = UnityEngine.Random.Range(0, _availableDishes.Count);
            _availableDishes[index].UnderAttack(); //Activated dish
            _availableDishes[index]._asteroids = AssignAsteroids(); //Assigns number of asteroids to dish
            _totalActiveAsteroids += _availableDishes[index]._asteroids; //Adds that number to total active asteroids
            _availableDishes.RemoveAt(index); //Removes that dish from being selected again
        }
        _assignedDishes = true;

        StartNewWave();
    }

    private int AssignAsteroids() //Assigns which dishes have how many asteroids
    {
        int asteroidNum = 0;
        if (_maxActiveDishes == 1) return _maxActiveAsteroids;

        asteroidNum = Mathf.FloorToInt(_maxActiveAsteroids/_maxActiveDishes);
        return asteroidNum;
    }

    private void ResetAvailableDishes()
    {
        for (int i = 0; i < _laserDishes.Length; i++) //Resets previously alerted dishes
        {
            _laserDishes[i].ResetState();
        }

        _availableDishes.RemoveRange(0, _availableDishes.Count);
        for (int i = 0; i < _laserDishes.Length; i++)
        {
            _availableDishes.Add(_laserDishes[i]);
        }
    }

    private void AdjustDifficulty()
    {
        if (_totalAttacks == 2)
        {
            _maxActiveDishes++;
            _maxActiveAsteroids += 2;
        }
        else if (_totalAttacks == 5)
        {
            _maxActiveAsteroids += 2;
        }
        else if (_totalAttacks == 10)
        {
            _maxActiveDishes++;
        }
        else if (_totalAttacks == 20)
        {
            _maxActiveAsteroids += 2;
        }
    }

    private void StartNewWave()
    {
        print("Starting next wave");
        _totalAttacks++;
        _assessedDamage = false;
        _timeAtLastAttack = Time.time;

        _uiManager.NewWave();
        _sfxManager.PlayClip(_sfxManager._dishInDanger);
    }

    public void GameOver()
    {
        if (LaserMinigame._activePlayer) _laserMinigame.EndMinigame();
        _gameOver = true;
        _survivalTime = _gameEndTime - _gameStartTime;
        _uiManager.StopTimer();
        _uiManager.LoseScreen(_survivalTime);
    }
}
