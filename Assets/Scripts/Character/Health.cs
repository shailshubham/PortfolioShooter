using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    int permanentHp;
    // Start is called before the first frame update
    void Start()
    {
        permanentHp = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damageAmount)
    {
        health = health - damageAmount;
        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        health = permanentHp;
    }
}
