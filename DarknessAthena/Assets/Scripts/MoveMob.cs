using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMob : MonoBehaviour
{
    private GameObject Player;
    private Transform Player_pos;

    private float distance_from_player;
    private float Angle_to_Hero;
    private float speed_max;
    private float Detection_Range;
    private float Auto_Attack_Range;

    private RaycastHit2D[] ray_list;

    private bool is_moving;
    private float time_before_re_moving;
    private float distance_from_torch;
    private float radius_torch;

    private GameObject Torch;


    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Player_pos = Player.GetComponent<Transform>();
        speed_max = 0.3f;
        Detection_Range = 3f;
        Auto_Attack_Range = 0.5f;
        is_moving = true;
        time_before_re_moving = 1f;
    }

    private bool in_light(float offset)
    {
        distance_from_torch = 0f;
    
        Torch = Player_pos.GetChild(0).gameObject;
        distance_from_torch = Vector2.Distance(Torch.GetComponent<Transform>().position,
            new Vector3(transform.position.x, transform.position.y - 0.02f, transform.position.z));
        radius_torch = Torch.GetComponent<CircleCollider2D>().radius;
        if (Torch.GetComponent<basic_torch>().state == true &&
            distance_from_torch + offset <= radius_torch) {
            return true;
        } else
            return false;
    }

    private bool is_player_in_sight()
    {
        if (distance_from_player > Detection_Range)
            return false;
        Angle_to_Hero = Mathf.Atan2(Player_pos.position.y - transform.position.y,
            Player_pos.position.x - transform.position.x);
        ray_list = Physics2D.RaycastAll(transform.position,
            new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero)));
        foreach (RaycastHit2D ray in ray_list) {
            if (ray.collider.tag == "Wall")
                return false;
        }
        return true;
    }

    private void Run_into_Player(float speed_boost)
    {
        Vector2 direction = new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero));
        transform.Translate(direction * (speed_max * speed_boost * Time.deltaTime));

        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.PingPong(Time.time * 100, 10) - 5);
    }

    private void Update_Moving()
    {
        if (!is_moving) {
            time_before_re_moving -= Time.deltaTime;
            if (time_before_re_moving <= 0f)
                is_moving = true;
        }
        distance_from_player = Vector2.Distance(Player_pos.position,
            transform.position);
        Debug.Log(distance_from_player);
        if ((is_player_in_sight() && is_moving && !in_light(-0.1f)) || distance_from_player <= Auto_Attack_Range) {
            Run_into_Player(1f);
        } else if (in_light(-0.1f)) {            
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Run_into_Player(-1f);
        }
    }

    void Update()
    {
        Update_Moving();
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player") {
            is_moving = false;
            time_before_re_moving = 1f;
        }
    }
}
