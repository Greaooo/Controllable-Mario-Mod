using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using BlockStuff;
using Mod;
using Library;
using MarioController;
using CatMario;
using LuigiController;
using GoombaClass;
using GreenKoopa;

namespace GreenKoopa
{
    public class Koopa : BasicEnemy
    {
        public float shellTime = 5;

        public bool hitInShell = false;
        public bool red;

        float flipCooldownTime;

        private void FixedUpdate()
        {
            if (state != EnemyHealthStates.SQUASHED)
            {
                Walk();
                Animate();

                if (walkDir == 0)
                {
                    walkDir = -1;
                }
            }
        }

        private void Update()
        {
            if (state == EnemyHealthStates.DEAD) { return; }

            TestForCollision();

            if (state == EnemyHealthStates.STANDARD)
            {
                enemy.GetComponent<CapsuleCollider2D>().size = new Vector2(0.43f, .66f);
                maxSpeed = 2;
            }

            if (state == EnemyHealthStates.SQUASHED)
            {
                Shell();
            }

            if (GetComponent<PhysicalBehaviour>().IsInLava) { Death(); }
        }

        public void Shell()
        {
            shellTime -= Time.deltaTime;

            if (shellTime <= 0)
            {
                state = EnemyHealthStates.STANDARD;
            }

            maxSpeed = 4f;

            GetComponent<SpriteRenderer>().sprite = squashed;
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.43f, 0.35f);

            Walk();

        }

        public override void Walk()
        {
            rb.AddForce(Vector2.right * 500 * walkDir);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }

        public override void Animate()
        {
            if (state == EnemyHealthStates.DEAD) { return; }

            if (state != EnemyHealthStates.SQUASHED || state != EnemyHealthStates.DEAD)
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
            state = EnemyHealthStates.DEAD;

            GetComponent<PhysicalBehaviour>().ShowOutline = false;
            GetComponent<PhysicalBehaviour>().PlayClipOnce(deathSound, 1f);

            transform.localScale = new Vector3(2, 2, 2);
            GetComponent<SpriteRenderer>().sprite = squashed;
            rb.AddForce(Vector2.up * 75, ForceMode2D.Impulse);

            transform.rotation = Quaternion.Euler(0, 0, 180);

            GetComponent<CapsuleCollider2D>().enabled = false;

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);
        }

        public override void Hurt(float cooldown)
        {
            state = EnemyHealthStates.SQUASHED;
            GetComponent<PhysicalBehaviour>().PlayClipOnce(squashSound, 1);
            walkDir = 0;
            shellTime = cooldown;
        }

        public override void TestForCollision()
        {
            flipCooldownTime -= Time.deltaTime;

            if (rb.velocity.x == 0 && flipCooldownTime <= 0)
            {
                walkDir = -walkDir;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                flipCooldownTime = 0.1f;
            }
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.transform.TryGetComponent<Syobon>(out Syobon cat))
            {
                if (coll.transform.position.y <= transform.position.y + 0.5f)
                {
                    coll.transform.GetComponent<Character>().Hurt();
                }
                else if (coll.transform.position.y > transform.position.y + 0.5f)
                {
                    Hurt(5);
                    coll.transform.GetComponent<Character>().Jump(9, true, false);
                    coll.transform.GetComponent<Character>().SetInvincible(0.5f, false);

                    if (state == EnemyHealthStates.SQUASHED)
                    {
                        if (coll.transform.position.x < transform.position.x - 0.3f)
                        {
                            walkDir = 1;
                            transform.localScale = new Vector3(2, 2, 2);
                        }
                        if (coll.transform.position.x > transform.position.x + 0.3f)
                        {
                            walkDir = -1;
                            transform.localScale = new Vector3(-2, 2, 2);
                        }
                    }
                }

                if (cat.star)
                {
                    Death();
                }
            }

            if (coll.transform.TryGetComponent<Mario>(out Mario mario))
            {
                if (coll.transform.position.y <= transform.position.y + 0.5f)
                {
                    coll.transform.GetComponent<Character>().Hurt();
                }
                else if (coll.transform.position.y > transform.position.y + 0.5f)
                {
                    Hurt(5);
                    coll.transform.GetComponent<Character>().Jump(9, true, false);
                    coll.transform.GetComponent<Character>().SetInvincible(0.5f, false);

                    if (state == EnemyHealthStates.SQUASHED)
                    {
                        if (coll.transform.position.x < transform.position.x - 0.3f)
                        {
                            walkDir = 1;
                            transform.localScale = new Vector3(2, 2, 2);
                        }
                        if (coll.transform.position.x > transform.position.x + 0.3f)
                        {
                            walkDir = -1;
                            transform.localScale = new Vector3(-2, 2, 2);
                        }
                    }
                }

                if (mario.star)
                {
                    Death();
                }
            }

            if (coll.transform.TryGetComponent<Luigi>(out Luigi luigi))
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

                    if (state == EnemyHealthStates.SQUASHED && walkDir == 0)
                    {
                        if (coll.transform.position.x < transform.position.x - 0.3f)
                        {
                            walkDir = 1;
                            transform.localScale = Vector3.one * 2;
                        }
                        if (coll.transform.position.x > transform.position.x + 0.3f)
                        {
                            walkDir = -1;
                            transform.localScale = new Vector3(-2, 2, 2);
                        }
                    }
                    else if(state == EnemyHealthStates.SQUASHED && walkDir != 0)
                    {
                        walkDir = 0;
                    }
                }

                if (luigi.star)
                {
                    Death();
                }
            }

            if (coll.transform.TryGetComponent<Goomba>(out Goomba gomb))
            {
                if (coll.transform.position.y <= transform.position.y + 0.5f && state == EnemyHealthStates.SQUASHED)
                {
                    gomb.Hurt(2);
                }
            }

            if (coll.transform.TryGetComponent<Koopa>(out Koopa kop))
            {
                if (coll.transform.position.y <= transform.position.y + 0.5f && state == EnemyHealthStates.SQUASHED)
                {
                    kop.Death();
                }
            }

            if (coll.transform.TryGetComponent<BlockClass>(out BlockClass block))
            {
                block.BlockHit(true);
            }
        }
    }
}
