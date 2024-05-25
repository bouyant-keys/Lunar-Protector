using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMinigame : MonoBehaviour
{
    AsteroidSpawner _spawner;
    UIManager _uiManager;
    SFXManager _sfxManager;
    CameraManager _camManager;
    LaserDish _currentLaserDish;

    [SerializeField] GameObject _minigameDisplay;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _laser;

    public static bool _activePlayer = false;
    [SerializeField] float _rotationPerSec = 1.5f;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _sfxManager = FindObjectOfType<SFXManager>();
        _camManager = FindObjectOfType<CameraManager>();
        _spawner = FindObjectOfType<AsteroidSpawner>();
        _laser.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!_activePlayer) return;

        float rotateForce = Input.GetAxis("Horizontal") * _rotationPerSec;
        _player.transform.Rotate(Vector3.forward * -rotateForce);
        _currentLaserDish._dish.transform.Rotate(Vector3.forward * -rotateForce);
    }

    private void Update()
    {
        if (!_activePlayer) return;

        if (Input.GetKeyDown(KeyCode.Space)) //Fires laser
        {
            _laser.SetActive(true);
            _currentLaserDish._laser.SetActive(true);
            _sfxManager.PlayLongClip(_sfxManager._firingLaser);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _laser.SetActive(false);
            _currentLaserDish._laser.SetActive(false);
            _sfxManager.Stop();
        }
    }

    public void PlayMinigame(LaserDish laserDish, int asteroidNum) //Starts Minigame
    {
        _currentLaserDish = laserDish;
        SwitchActivePlayer();
        _sfxManager.PlayPlayerClip();

        if (!_spawner.SpawnAsteroid(asteroidNum)) EndMinigame();
        _uiManager.AccessLaserMinigame(true);
    }

    public void EndMinigame()
    {
        _laser.SetActive(false);
        _currentLaserDish._laser.SetActive(false);
        _sfxManager.StopPlayerClip();
        _sfxManager.Stop();

        _currentLaserDish.ThreatNeutalized();
        SwitchActivePlayer();
        _uiManager.AccessLaserMinigame(false);
    }

    private void SwitchActivePlayer()
    {
        PlayerController._activePlayer = !PlayerController._activePlayer;
        _activePlayer = !PlayerController._activePlayer;

        _camManager.SwitchPriority();
    }
}
