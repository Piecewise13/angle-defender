using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{

    public class ShootingRangeDummyScript : MonoBehaviour, Damageable
    {

        public float maxHealth;
        private float respawnStartTime;
        public float respawnTime;


        public float health { get; set; }
        public bool isDead { get; set; }

        public Animator anim;

        public void Start()
        {
            health = maxHealth;
        }

        public void Update()
        {
            if (!isDead)
            {
                return;
            }
            
            if (respawnStartTime + respawnTime < Time.time)
            {
                Respawn();
            }
        }


        public void Death()
        {
            anim.SetTrigger("Death");
            isDead = true;
            respawnStartTime = Time.time;
        }

        public void Respawn()
        {
            anim.SetTrigger("Respawn");
            isDead = false;
            health = maxHealth;

        }

        public virtual void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
        {
            damageGiven = damage;
            crit = false;
            GiveDamage(damage);

        }

        public virtual void GiveDamage(float damage)
        {
            health -= damage;
            if (health <= 0f)
            {

                Death();
            }
        }

    }
}