using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    private int number_allumed;
    public TextMeshProUGUI txt;
    private bool key_getten;
    private bool key_deblock;
    public GameObject chest;
    public GameObject position;

    // Start is called before the first frame update
    void Start()
    {
        key_getten = false;
        key_deblock = false;
        number_allumed = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        number_allumed = 0;
        GameObject[] Lst_torch = GameObject.FindGameObjectsWithTag("Torch");
        foreach(GameObject trch in Lst_torch) {
            if (trch.GetComponent<basic_torch>().state == true && trch.name != "basic_torch")
                number_allumed += 1;
        }
        if (number_allumed == 4 && !key_deblock) {
            Instantiate(chest, position.GetComponent<Transform>().position, Quaternion.identity, position.transform);
            key_deblock = true;
        }
        if (number_allumed < 4) {
            txt.text = "Torches lit to unlock the key : " + number_allumed.ToString() + " / 4";
        } else if (!key_getten) {
            txt.text = "Find the key";
            if (GameObject.Find("Player").GetComponent<Key_script>().key_count > 0)
                key_getten = true;
        } else {
            txt.text = "Get out of the dungeon";
        }
    }
}
