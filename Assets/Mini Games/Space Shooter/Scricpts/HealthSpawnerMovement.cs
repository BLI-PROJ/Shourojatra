using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawnerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveHorizontal();
    }
    private void MoveHorizontal()
    {
        var deltaX = Random.Range(-5f, 5f) * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, -5, 5);
        transform.position = new Vector2(newXPos, transform.position.y);
    }
}
