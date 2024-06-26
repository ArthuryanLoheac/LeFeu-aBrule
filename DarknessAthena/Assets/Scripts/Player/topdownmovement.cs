using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class topdownmovement : MonoBehaviour
{
    private float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    private PauseCheck PauseManager;
    private LifePlayer life;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0.8f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
        life = this.gameObject.GetComponent<LifePlayer>();
    }

    void Update_move()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x != 0 || moveInput.y != 0)
            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.PingPong(Time.time * 100, 10) - 5);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        moveInput.Normalize();

        rb2d.MovePosition(transform.position + new Vector3(moveInput.x, moveInput.y, 0)
            * moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PauseManager.IsPlaying && life.Life > 0f)
            Update_move();
    }
}
