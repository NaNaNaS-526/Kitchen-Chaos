using System;
using System.Threading;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private GameState _currentGameState;
    private float _waitingToStartTimer = 1.0f;
    private float _countdownToStartTimer = 3.0f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 60.0f;
    private bool _isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        _currentGameState = GameState.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (_currentGameState)
        {
            case GameState.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0.0f)
                {
                    _currentGameState = GameState.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case GameState.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0.0f)
                {
                    _currentGameState = GameState.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case GameState.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0.0f)
                {
                    _currentGameState = GameState.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case GameState.GameOver:
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                break;
        }

        print(_currentGameState);
    }

    public bool IsGamePlaying()
    {
        return _currentGameState == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _currentGameState == GameState.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return _currentGameState == GameState.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - _gamePlayingTimer / _gamePlayingTimerMax;
    }

    public void TogglePauseGame()
    {
        if (_currentGameState != GameState.GamePlaying) return;
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0.0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}