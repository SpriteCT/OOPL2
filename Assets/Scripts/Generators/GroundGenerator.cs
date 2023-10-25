using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject GroundPrefab;
    public float maxSpeed;

    private float _speed;
    private List<GameObject> _grounds = new List<GameObject>();
    private int _maxGroundCount = 3;

    private void Update()
    {
        if (_speed == 0) return;
        foreach (GameObject ground in _grounds)
        {
            ground.transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        }

        if (_grounds[0].transform.position.z < -35f)
        {
            Destroy(_grounds[0]);
            _grounds.RemoveAt(0);
            CreateNextGround();
        }
    }

    private void Start()
    {
        ResetLevel();
        StartLevel();
    }

    private void CreateNextGround()
    {
        Vector3 pos = Vector3.zero;
        if (_grounds.Count > 0) pos = _grounds[^1].transform.position + new Vector3(0, 0, 50f);
        GameObject newGround = Instantiate(GroundPrefab, pos, quaternion.identity, transform);
        _grounds.Add(newGround);
    }

    public void ResetLevel()
    {
        while (_grounds.Count > 0)
        {
            Destroy(_grounds[0]);
            _grounds.RemoveAt(0);
        }

        for (int i = 0; i < _maxGroundCount; i++)
        {
            CreateNextGround();
        }
    }

    public void StartLevel()
    {
        _speed = maxSpeed;
    }

    public void StopLevel()
    {
        _speed = 0;
    }
}
