using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class basic_torch : MonoBehaviour
{
    private float max_fuel = 100f;
    private float fuel = 100f;
    private float consumption_rate = 1f; //consumption of fuel per second, set to zero to make it infinite
    private float max_radius = 1.5f;
    public bool state = false;
    private CircleCollider2D hitbox;
    private GameObject torch;
    public Light2D light;

    void Start()
    {
        torch = gameObject;
        hitbox = torch.GetComponent<CircleCollider2D>();
    }
    public void switch_torch_state()
    {
        state = !state;
    }
    private void update_torch_radius(float time_spent)
    {
        if (state && fuel > 0)
            fuel -= time_spent * consumption_rate;
        hitbox.radius = (fuel * max_radius) / max_fuel;
    }
    void Update()
    {
        update_torch_radius(Time.deltaTime);
        light.intensity = Mathf.Log(Mathf.PingPong(Time.time, 1) + 2f);
    }
}
