using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using Mod;
using Library;
using MarioController;
using LuigiController;
using CatMario;

namespace GoombaClass
{
    public class Goomba : BasicEnemy
    {
        float flipCooldownTime = 0;
        bool playedSound = false;

        float soundCool = 0;

        private void FixedUpdate()
        {
            if (state != EnemyHealthStates.SQUASHED)
            {
                Walk();
            }
        }

        private void Update()
        {
            if (state != EnemyHealthStates.SQUASHED)
            {
                Animate();
            }

            if (state == EnemyHealthStates.SQUASHED)
            {
                deathCooldown -= Time.deltaTime;
                enemy.GetComponent<SpriteRenderer>().sprite = squashed;

                if (deathCooldown <= 0) Death();
            }

            TestForCollision();

            if (GetComponent<PhysicalBehaviour>().IsInLava) { Death(); }
        }

        public override void Walk()
        {
            rb.AddForce(enemy.right * 500 * walkDir);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }

        public override void Hurt(float cooldown)
        {
            state = EnemyHealthStates.SQUASHED;
            GetComponent<PhysicalBehaviour>().PlayClipOnce(squashSound, 0.25f);

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);
        }

        public override void Animate()
        {
            animCooldown -= Time.deltaTime;

            if (animCooldown <= 0)
            {
                animNumb++;
                animCooldown = 0.2f;
            }

            if (animNumb > 2)
            {
                animNumb = 1;
            }

            switch (animNumb)
            {
                case 1:
                    GetComponent<SpriteRenderer>().sprite = run1;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = run2;
                    break;
            }
        }

        public override void Death()
        {
            GameObject.Destroy(gameObject);
        }

        public override void TestForCollision()
        {
            flipCooldownTime -= Time.deltaTime;

            if (rb.velocity.x == 0 && flipCooldownTime <= 0)
            {
                walkDir = -walkDir;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                flipCooldownTime = 0.3f;
            }
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.transform.GetComponent<Mario>() || coll.transform.GetComponent<Luigi>() || coll.transform.GetComponent<Syobon>())
            {
                if (coll.transform.position.y <= transform.position.y + 0.5f)
                {
                    coll.transform.GetComponent<Character>().Hurt();
                }
                else if (coll.transform.position.y > transform.position.y + 0.5f)
                {
                    Hurt(5);
                    coll.transform.GetComponent<Character>().Jump(5, true, false);
                    coll.transform.GetComponent<Character>().SetInvincible(0.5f, false);
                }

                if (coll.transform.GetComponent<Mario>().star || coll.transform.GetComponent<Luigi>().star)
                {
                    Hurt(0);
                }
            }
        }
    }
}
