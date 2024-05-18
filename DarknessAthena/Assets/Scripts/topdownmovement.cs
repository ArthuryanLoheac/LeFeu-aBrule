using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class topdownmovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x != 0 || moveInput.y != 0)
            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.PingPong(Time.time * 100, 10) - 5);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed * Time.deltaTime;
    }
}
