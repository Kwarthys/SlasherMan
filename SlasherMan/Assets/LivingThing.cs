using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThing : MonoBehaviour
{
    public int maxLife;

    [SerializeField]
    protected int life;

    private void Start()
    {
        life = maxLife;
    }

    public void takeDamage(int amount)
    {
        life -= amount;
        if(life <= 0)
        {
            //ded
            Destroy(gameObject);
        }
    }
}
