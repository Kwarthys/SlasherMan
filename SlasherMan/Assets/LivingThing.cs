using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThing : MonoBehaviour
{
    public int maxLife;

    public GameObject deathAnimation;

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
            if(deathAnimation != null)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            }

            //ded
            Destroy(gameObject);
        }
    }
}
