using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManagement : MonoBehaviour
{
    public int MaxHealth = 3;
    [HideInInspector] public int Health;
    public Animator PlayerAnim;
    public MeshRenderer _mr;
    
    private PlayerMovement _pm;
    private GameManager _gm;
    private ObstacleGenerator _og;
    private bool _isColliding;
    private bool _isInvinc;
    private Color _startColor;
    private void Start()
    {
        _pm = GetComponent<PlayerMovement>();
        _gm = FindObjectOfType<GameManager>();
        PlayerAnim = GetComponent<Animator>();
        _og = FindObjectOfType<ObstacleGenerator>();
        _startColor = _mr.material.color;
    }

    private void Update()
    {
        if (Health <= 0 && _gm.IsGameStarted)
        {
            StartCoroutine(_gm.StopGame());
        }
        _isColliding = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isColliding) return;
        _isColliding = true;
        if (other.gameObject.CompareTag("Enemy_car") && !_isInvinc)
        {
            Health -= 1;
            if (Health == 0) PlayerAnim.Play("Dead");
            else if (_gm.IsGameStarted) PlayerAnim.Play("Rotation");
        }

        if (other.gameObject.CompareTag("HealthUp"))
        {
            Health = Health + 1 > MaxHealth ? MaxHealth : Health + 1;
            StartCoroutine(_gm.UpgradeIcon("HealthUp"));
        }

        if (other.gameObject.CompareTag("Invincibility"))
        {
            StartCoroutine(Invincibility());
            StartCoroutine(_gm.UpgradeIcon("Invincibility"));
        }

        if (other.gameObject.CompareTag("RotateUpgrade"))
        {
            StartCoroutine(RotateUpgrade());
            StartCoroutine(_gm.UpgradeIcon("RotateUpgrade"));
        }
        _og.DeleteObstacle(other.gameObject);
    }

    private IEnumerator Invincibility()
    {
        _mr.material.color = Color.green;
        _isInvinc = true;
        yield return new WaitForSecondsRealtime(5f);
        _isInvinc = false;
        _mr.material.color = _startColor;
    }
    private IEnumerator RotateUpgrade()
    {
        _mr.material.color = Color.blue;
        _pm.Speed = _pm.StartSpeed + 3;
        yield return new WaitForSecondsRealtime(5f);
        _pm.Speed = _pm.StartSpeed;
        _mr.material.color = _startColor;
    }
    
}
