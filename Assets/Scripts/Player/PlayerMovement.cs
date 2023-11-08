using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float StartSpeed;
    [HideInInspector] public float Speed;
    
    private GameManager _gm;
    private void Start()
    {
        Speed = StartSpeed;
        _gm = FindObjectOfType<GameManager>();
    }
    private void Update()
    {   
        if (_gm.IsGameStarted) Move();
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        var position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x + deltaX, -5, 5), 0.7f, 0);
        transform.position = position;
    }


}
