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

namespace LakituClass
{
    class Lakitu : MonoBehaviour
    {
        GameObject currentTarget;

        public struct LakituSprites
        {
            public Sprite idle;
            public Sprite grabbing;
        }

        public enum State
        {
            IDLE,
            GRABBING
        }

        public State currentState = State.IDLE;

        public LakituSprites sprites;

        List<GameObject> dropped = new List<GameObject>();

        public Rigidbody2D rb;
        public SpriteRenderer rend;

        bool hasTarget;
        bool tooClose;

        const float MOVEMENT_THRESHOLD = 4f;
        float moveSpeed = 30;
        float closenessCountdown = 2f;
        float grabCooldown = 1f;

        Vector2 moveDir;
        Vector2 sinCos;

        void Start()
        {
            rb.drag = 2;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }

        void FixedUpdate()
        {
            FindTarget();

            if (Vector2.Distance(transform.position, currentTarget.transform.position) > MOVEMENT_THRESHOLD)
            {
                rb.AddForce((moveDir + sinCos) * moveSpeed);
                tooClose = false;
            }
            else
            {
                tooClose = true;
            }

            sinCos = new Vector2(Mathf.Cos(Time.time / 2), Mathf.Sin(Time.time));
            
            foreach (GameObject obj in dropped)
            {
                Physics2D.IgnoreCollision(obj.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

        void Update()
        {
            if (hasTarget)
            {
                moveDir = new Vector2(currentTarget.transform.position.x - transform.position.x, 
                    currentTarget.transform.position.y + 4 - transform.position.y);
            }
            else
            {
                moveDir = transform.position;
            }

            grabCooldown -= Time.deltaTime;

            if (tooClose)
            {
                closenessCountdown -= Time.deltaTime;
                currentState = State.GRABBING;
            }
            else
            {
                closenessCountdown = 2;
            }

            if (closenessCountdown <= 0)
            {
                DropSpiny();
            }

            if (grabCooldown <= 0)
            {
                currentState = State.IDLE;
            }

            Animate();
        }

        void Animate()
        {
            switch (currentState)
            {
                case State.IDLE:

                    rend.sprite = sprites.idle;

                    break;

                case State.GRABBING:

                    rend.sprite = sprites.grabbing;

                    break;
            }

            if (hasTarget)
            {
                if (currentTarget.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(2, 2, 2);
                }
                else if ((currentTarget.transform.position.x < transform.position.x))
                {
                    transform.localScale = new Vector3(-2, 2, 2);
                }
            }

            GetComponent<PhysicalBehaviour>().RefreshOutline();
        }

        void DropSpiny()
        {
            GameObject Spiny = Instantiate(ModAPI.FindSpawnable("Spiny").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("Spiny"), Spiny);

            dropped.Add(Spiny);

            Spiny.GetComponent<Rigidbody2D>().AddForce(transform.up * 25, ForceMode2D.Impulse);

            closenessCountdown = 4;
            grabCooldown = 0.3f;
        }

        void FindTarget()
        {
            

            if (GameObject.Find("Mario"))
            {
                currentTarget = GameObject.Find("Mario");
                hasTarget = true;
            }
            else if (GameObject.Find("Luigi"))
            {
                currentTarget = GameObject.Find("Luigi");
                hasTarget = true;
            }
            else
            {
                hasTarget = false;
                currentTarget = null;
            }

        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            coll.transform.TryGetComponent<Mario>(out Mario mario);
            coll.transform.TryGetComponent<Luigi>(out Luigi luigi);
            coll.transform.TryGetComponent<Syobon>(out Syobon syobon);

            if (mario)
            {
                if (HurtCharacter(mario.transform.position))
                    mario.Hurt();
                else
                    Hurt();
            }
            if (luigi)
            {
                if (HurtCharacter(luigi.transform.position))
                    luigi.Hurt();
                else
                    Hurt();
            }
            if (syobon)
            {
                if (HurtCharacter(syobon.transform.position))
                    syobon.Hurt();
                else
                    Hurt();
            }
        }

        bool HurtCharacter(Vector2 position)
        {
            if(position.y > transform.position.y + 0.2f)
            {
                return false;
            }

            return true;
        }
         
        void Hurt()
        {

        }
    }
}
