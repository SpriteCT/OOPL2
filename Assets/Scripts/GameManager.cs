using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Canvas StartMenu;
    [SerializeField] public TextMeshProUGUI HealthBar;
    [SerializeField] public TextMeshProUGUI ScoreBar;
    [SerializeField] public TextMeshProUGUI HighScoreBar;
    [SerializeField] public Canvas InGameBar;
    [SerializeField] public int Score;
    [SerializeField] public GameObject HealthUpUpgrade;
    [SerializeField] public GameObject InvincibilityUpgrade;
    [SerializeField] public GameObject RotateUpgrade;
    
    [HideInInspector] public bool IsGameStarted;
    
    private float _gameSpeed;
    private int _highscore;
    private ObstacleGenerator _og;
    private GroundGenerator _gr;
    private PlayerManagement _pm;

    void Start()
    {
        _og = FindObjectOfType<ObstacleGenerator>();
        _gr = FindObjectOfType<GroundGenerator>();
        _pm = FindObjectOfType<PlayerManagement>();
        _gr.ResetLevel();
        IsGameStarted = false;
        InGameBar.gameObject.SetActive(false);
        _gameSpeed = 1;
    }

    private void Update()
    {
        ScoreBar.text = $"Score: {Score}";
        HealthBar.text = $"Health: {_pm.Health}";
        if (IsGameStarted) Time.timeScale = Mathf.Sqrt(_gameSpeed);
    }

    public IEnumerator StopGame()
    {
        _og.Stop();
        IsGameStarted = false;
        _gameSpeed = 1;
        yield return new WaitForSeconds(4f);
        _gr.StopLevel();
        StartMenu.gameObject.SetActive(true);
        InGameBar.gameObject.SetActive(false);
        _highscore = Score > _highscore ? Score : _highscore;
        if (_highscore > 0)   HighScoreBar.gameObject.SetActive(true);
        HighScoreBar.text = $"HighScore: {_highscore}";
        Score = 0;
    }
    

    public void StartGame()
    {
        _gr.StartLevel();
        _pm.Health = _pm.MaxHealth;
        _og.StartGen();
        IsGameStarted = true;
        InGameBar.gameObject.SetActive(true);
        _pm.PlayerAnim.Play("Idle");
    }

    public void IncreaseSpeed()
    {
        _gameSpeed += 0.01f;
    }

    public IEnumerator UpgradeIcon(string upgradeType)
    {
        GameObject obj;
        if (upgradeType == "RotateUpgrade")
        {
            obj = RotateUpgrade;
        }
        else if(upgradeType == "HealthUp")
        {
            obj = HealthUpUpgrade;
        }
        else
        {
            obj = InvincibilityUpgrade;
        }
        obj.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        obj.SetActive(false);
    }
}
