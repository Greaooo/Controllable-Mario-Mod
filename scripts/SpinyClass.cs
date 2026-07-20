using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using Mod;
using MarioController;
using CatMario;
using LuigiController;

namespace Spiny
{
    class Spiny : MonoBehaviour
    {
        enum SpinyStates
        {
            WALKING,
            FALLING,
            DEAD
        }

        SpinyStates state;

        public Rigidbody2D rb;

        public SpriteRenderer rend;

        bool onGround()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
        }

        int keyframe;
        int moveDir = 1;

        float moveSpeed = 500;
        float keyCooldown = 0;

        float flipCooldownTime;

        public struct Sprites
        {
            public Sprite idle1;
            public Sprite idle2;

            public Sprite egg1;
            public Sprite egg2;
        }

        public Sprites sprites;

        void Start()
        {
            rb.freezeRotation = true;
        }

        void FixedUpdate()
        {
            if (state == SpinyStates.WALKING)
            {
                rb.AddForce(transform.right * moveDir * moveSpeed);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -2, 2), rb.velocity.y);
            }

            TestForCollision();
        }

        void Update()
        {
            Animate();
        }

        public void TestForCollision()
        {
            flipCooldownTime -= Time.deltaTime;

            if (rb.velocity.x == 0 && flipCooldownTime <= 0)
            {
                moveDir = -moveDir;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                flipCooldownTime = 0.1f;
            }
        }

        void Animate()
        {
            keyCooldown -= Time.deltaTime;

            if (onGround() && state != SpinyStates.DEAD)
            {
                state = SpinyStates.WALKING;
            }
            else
            {
                state = SpinyStates.FALLING;
            }

            if (keyframe > 1)
            {
                keyframe = 0;
            }

            if(keyCooldown <= 0)
            {
                keyframe++;
                keyCooldown = 0.5f;
            }

            if (state == SpinyStates.WALKING)
            {
                switch (keyframe)
                {
                    case 0:
                        rend.sprite = sprites.idle1;
                        break;
                    case 1:
                        rend.sprite = sprites.idle2;
                        break;
                }
            }
            if (state == SpinyStates.FALLING)
            {
                switch (keyframe)
                {
                    case 0:
                        rend.sprite = sprites.egg1;
                        break;
                    case 1:
                        rend.sprite = sprites.egg2;
                        break;
                }
            }
            if (state == SpinyStates.DEAD)
            {
                switch (keyframe)
                {
                    case 0:
                        rend.sprite = sprites.egg1;
                        break;
                    case 1:
                        rend.sprite = sprites.egg2;
                        break;
                }
            }
        }

        public void Hurt()
        {
            state = SpinyStates.DEAD;

            GetComponent<PhysicalBehaviour>().ShowOutline = false;

            transform.localScale = new Vector3(2, 2, 2);
            rb.AddForce(Vector2.up * 75, ForceMode2D.Impulse);

            transform.rotation = Quaternion.Euler(0, 0, 180);

            GetComponent<BoxCollider2D>().enabled = false;

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            coll.transform.TryGetComponent<Mario>(out Mario mario);
            coll.transform.TryGetComponent<Luigi>(out Luigi luigi);
            coll.transform.TryGetComponent<Syobon>(out Syobon syobon);

            if (mario)
            {
                mario.Hurt();
            }
            if (luigi)
            {
                luigi.Hurt();
            }
            if (syobon)
            {
                syobon.Hurt();
            }
        }
    }
}
