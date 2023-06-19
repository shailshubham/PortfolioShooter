using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Health playerHealth;
    Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        healthBar = GetComponent<Slider>(); 
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerHealth.health;
    }
}
