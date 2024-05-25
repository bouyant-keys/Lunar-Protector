using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Space]
    [SerializeField] Slider _baseHealth;
    [SerializeField] Image _healthBar;
    [SerializeField] Color _maxHealthColor;
    [SerializeField] Color _halfHealthColor;
    [SerializeField] Color _quarterHealthColor;
    [Space]
    [SerializeField] GameObject _tutorialInfo;
    [SerializeField] GameObject _waveTimer;
    [SerializeField] TMP_Text _waveTimerText;
    [SerializeField] GameObject _laserMinigameScreen;
    [SerializeField] GameObject _loseScreen;
    [SerializeField] TMP_Text _surviveTimeText;
    [SerializeField] GameObject _pauseScreen;

    Canvas _canvas;
    Vector3 _activeMenuPos;
    Vector3 _inactiveMenuPos;

    public static bool _isPaused = false;
    float _totalHealth = 100f;
    float _previousHealth;
    float _waveTime = 10f;
    float _waveStartTime;
    bool _timerIsActive = false;

    void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        _activeMenuPos = _canvas.transform.position;
        _inactiveMenuPos = new Vector3(_canvas.renderingDisplaySize.x * 2, _canvas.renderingDisplaySize.y / 2, 0f);

        _previousHealth = GameManager.BaseHealth;

        MoveElement(_waveTimer, false);
        MoveElement(_laserMinigameScreen, false);
        MoveElement(_pauseScreen, false);
        MoveElement(_loseScreen, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerIsActive) UpdateTimer();
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        MoveElement(_pauseScreen, _isPaused);

        if (_isPaused) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    public void LoseScreen(float surviveTime)
    {
        _surviveTimeText.text = $"{FormatNum(surviveTime)}";
        MoveElement(_loseScreen, true);
    }

    public void StartTimer()
    {
        _waveStartTime = Time.time;
        _timerIsActive = true;

        MoveElement(_waveTimer, true);
        MoveElement(_tutorialInfo, false);
    }
    public void NewWave()
    {
        _waveStartTime = Time.time;
    }
    public void StopTimer()
    {
        _timerIsActive = false;
    }

    public void AccessLaserMinigame(bool active)
    {
        if (active) MoveElement(_laserMinigameScreen, true);
        else MoveElement(_laserMinigameScreen, false);
    }

    public void UpdateHealth()
    {
        _baseHealth.value = GameManager.BaseHealth / _totalHealth;
        _previousHealth = GameManager.BaseHealth;

        if (_previousHealth >= _totalHealth / 2) _healthBar.color = _maxHealthColor;
        else if (_previousHealth >= _totalHealth / 4) _healthBar.color = _halfHealthColor;
        else if (_previousHealth > 0f) _healthBar.color = _quarterHealthColor;
    }

    private void UpdateTimer()
    {
        float timerNum = _waveTime - (Time.time - _waveStartTime);
        if (timerNum < 0f) timerNum = 0;

        _waveTimerText.text = FormatNum(timerNum) + "s";
    }

    private string FormatNum(float num)
    {
        float temp = Mathf.Floor(num * 10f) / 10f;
        return temp.ToString("F1"); //Formatted 000.0
    }

    private void MoveElement(GameObject element, bool active)
    {
        if (active) element.transform.position = _activeMenuPos;
        else element.transform.position = _inactiveMenuPos;
    }
}
