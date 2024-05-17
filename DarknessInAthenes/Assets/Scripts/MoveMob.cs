using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMob : MonoBehaviour
{
    private GameObject Player;
    private Transform Player_pos;
    private float distance_from_player;
    private RaycastHit2D ray;
    private float Angle_to_Hero;
    private float speed_max;
    private float speed;
    private float Detection_Range;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Player_pos = Player.GetComponent<Transform>();
        speed_max = 0.25f;
        Detection_Range = 1.5f;
    }

    private bool is_player_in_sight()
    {
        distance_from_player = Vector2.Distance(Player_pos.position,
            transform.position);
        if (distance_from_player > Detection_Range)
            return false;
        Angle_to_Hero = Mathf.Atan2(Player_pos.position.y - transform.position.y,
            Player_pos.position.x - transform.position.x);
        ray = Physics2D.Raycast(transform.position,
            new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero)));
        if (ray.collider && ray.collider.gameObject.tag == "Player")
            return true;
        return false;
    }

    private void Run_into_Player()
    {
        float step = speed_max * Time.deltaTime;
    
        transform.position = Vector2.MoveTowards(transform.position, Player_pos.position, step);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_player_in_sight())
            Run_into_Player();
    }
}