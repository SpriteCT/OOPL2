using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleGenerator : MonoBehaviour
{

    public float StartSpeed;
    public GameObject[] ObstaclesTypes;
    
    private List<GameObject> _obstacles = new List<GameObject>();
    private float _carSpeed;
    private GroundGenerator _gr;
    private bool _isPass;
    private PlayerMovement _pm;
    private GameManager _gm;

    private void Start()
    {
        _gr = FindObjectOfType<GroundGenerator>();
        _pm = FindObjectOfType<PlayerMovement>();
        _gm = FindObjectOfType<GameManager>();

    }
    private void Update()
    {
        if ( _obstacles.Count > 0 && _obstacles[0].transform.position.z < -15)
        {
            DeleteObstacle(_obstacles[0]);
            _isPass = false;
        }

        foreach (GameObject obstacle in _obstacles)
        {
            obstacle.transform.position -= new Vector3(0, 0, (_carSpeed + _gr.maxSpeed) * Time.deltaTime);
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, 0.7f, obstacle.transform.position.z);
            if (!_isPass && obstacle.transform.position.z < _pm.gameObject.transform.position.z && obstacle.CompareTag("Enemy_car"))
            {
                _gm.Score += 1;
                _isPass = true;
                _gm.IncreaseSpeed();
            }
        }
    }

    private void CreateObstacle()
    {
        Vector3 pos = new Vector3(Random.Range(-5f, 5f), 1f, 90f);
        int randomNumber = Random.Range(0, 100);
        int obstacleType;
        if (randomNumber >= 15) obstacleType = Random.Range(0, 2);
        else obstacleType = Random.Range(2, 5);
        GameObject new_obstacle = Instantiate(ObstaclesTypes[obstacleType], pos, Quaternion.Euler(0,180,0), transform);
        _obstacles.Add(new_obstacle);
    }

    IEnumerator GenerateOnTime()
    {
        while (_carSpeed != 0)
        {
            CreateObstacle();
            yield return new WaitForSeconds(Random.Range(0.5f,1.5f));
        }
    }

    public void StartGen()
    {
        _carSpeed = StartSpeed;
        StartCoroutine(GenerateOnTime());
    }

    public void Stop()
    {
        _carSpeed = 0;
    }

    public void DeleteObstacle(GameObject car)
    {
        Destroy(car);
        _obstacles.Remove(car);
    }
}
