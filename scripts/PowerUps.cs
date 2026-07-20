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
using CharactersScript;

namespace PowerUps
{
    public class Mushroom : MonoBehaviour, Item
    {
        private float flipCooldown = 0.25f;
        private float curCool = 0.25f;

        public int moveDir = 1;

        bool finishedStarting;

        void Start()
        {
            //StartCoroutine(OnStart(new Vector2(transform.position.x, transform.position.y + 1)));
        }

        public void FixedUpdate()
        {
            if (finishedStarting)
            {
                Move();
            }
        }

        public IEnumerator OnStart(Vector2 startPos)
        {
            float UpwardMovement = 2f;

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<SpriteRenderer>().sortingOrder = -1;

            while (transform.position.y < startPos.y)
            {
                yield return new WaitForEndOfFrame();

                transform.position += new Vector3(0, UpwardMovement*Time.deltaTime, 0);
            }

            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<SpriteRenderer>().sortingOrder = 0;

            finishedStarting = true;
        }

        public void Move()
        {
            curCool -= Time.fixedDeltaTime;

            GetComponent<Rigidbody2D>().AddForce(transform.right * 500 * moveDir);
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -2, 2), GetComponent<Rigidbody2D>().velocity.y);
           
            if (GetComponent<Rigidbody2D>().velocity.x == 0 && curCool <= 0)
            {
                //attempt to flip direction of movement on collision
                if (moveDir == 1)
                    moveDir = -1;
                else if (moveDir == -1)
                    moveDir = 1;

                curCool = flipCooldown;
            }
        }

        public void OnPickup(Mario characterTouched)
        {
            if(characterTouched.powerState != CharacterPowerUpStates.FIRE)
                characterTouched.PowerUp(CharacterPowerUpStates.TALL);

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);

            gameObject.SetActive(false);
        }

        public void LuigiPickup(Luigi l)
        {
            if (l.powerState != CharacterPowerUpStates.FIRE)
                l.PowerUp(CharacterPowerUpStates.TALL);

            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(!finishedStarting) { return; }

            if (col.transform.TryGetComponent<Mario>(out Mario mario))
            {
                OnPickup(mario);
            }
            if (col.transform.TryGetComponent<Luigi>(out Luigi luigi))
            {
                LuigiPickup(luigi);
            }

            if (col.transform.GetComponent<CharactersScript.Character>())
            {
                col.transform.GetComponent<CharactersScript.Character>().MushroomInteracted();
            }
        }
    }

    public class FireFlower : MonoBehaviour, Item
    {
        float animCooldown = 0.1f;
        int animNumb = 0;

        bool finishedStarting;

        public Sprite[] flowerSprites = new Sprite[3];

        void Start()
        {
            //StartCoroutine(OnStart(new Vector2(transform.position.x, transform.position.y + 1)));
        }

        void Update()
        {
            animCooldown -= Time.deltaTime;
            if (animCooldown <= 0)
            {
                animCooldown = 0.1f;
                animNumb++;
            }

            if (animNumb > 3)
            {
                animNumb = 0;
            }

            GetComponent<SpriteRenderer>().sprite = flowerSprites[animNumb];
        }

        public IEnumerator OnStart(Vector2 startPos)
        {
            float UpwardMovement = 2f;

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<SpriteRenderer>().sortingOrder = -1;

            while (transform.position.y < startPos.y)
            {
                yield return new WaitForEndOfFrame();

                transform.position += new Vector3(0, UpwardMovement * Time.deltaTime, 0);
            }

            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            finishedStarting = true;
        }

        public void OnPickup(Mario characterTouched)
        {
            if (!finishedStarting) { return; }

            characterTouched.PowerUp(CharacterPowerUpStates.FIRE);

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);

            GameObject.Destroy(gameObject);
        }
        public void LuigiPickup(Luigi l)
        {
            if (!finishedStarting) { return; }

            l.PowerUp(CharacterPowerUpStates.FIRE);

            GameObject.Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!finishedStarting) { return; }

            if (col.transform.TryGetComponent<Mario>(out Mario mario))
            {
                OnPickup(mario);
            }
            if (col.transform.TryGetComponent<Luigi>(out Luigi luigi))
            {
                LuigiPickup(luigi);
            }

        }
    }

    public class Star : MonoBehaviour, Item
    {
        public int moveDir = 1;
        bool finishedStarting;
        void Start()
        {
            StartCoroutine(OnStart(new Vector2(transform.position.x, transform.position.y + 1)));
        }

        public void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb);
            rb.velocity = new Vector2(3 * moveDir, rb.velocity.y);
        }

        public void OnPickup(Mario characterTouched)
        {

        }

        public void LuigiPickup(Luigi l)
        {

        }

        public IEnumerator OnStart(Vector2 startPos)
        {
            float UpwardMovement = 2f;

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<SpriteRenderer>().sortingOrder = -1;

            while (transform.position.y < startPos.y)
            {
                yield return new WaitForEndOfFrame();

                transform.position += new Vector3(0, UpwardMovement * Time.deltaTime, 0);
            }

            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            finishedStarting = true;
        }


        void OnCollisionEnter2D(Collision2D coll)
        {
            if (!finishedStarting) { return; }
            if (coll.transform.TryGetComponent<Luigi>(out Luigi l))
            {
                if(l) { l.SetInvincible(5, true); }

                GameObject.Destroy(gameObject);
            }
            if (coll.transform.TryGetComponent<Mario>(out Mario m))
            {
                if (m != null) { m.SetInvincible(5, true); }

                GameObject.Destroy(gameObject);
            }
            else
            {
                if(coll.transform.position.y < transform.position.y)
                    transform.GetComponent<Rigidbody2D>().AddForce(transform.up * 150, ForceMode2D.Impulse);
            }


        }
    }

    public class Coins : MonoBehaviour
    {
        public Sprite ONE, TWO, THREE, FOUR;

        public AudioClip pickUpSound;

        float cooldown;

        bool isPlaced;

        Vector2 curSpot, realSpot;

        int keyframe;

        void Update()
        {
            if (!isPlaced)
            {
                GetComponent<Collider2D>().enabled = false;

                curSpot =
                    Vector2.Lerp(transform.position, new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                    Mathf.RoundToInt(Global.main.MousePosition.y)), Time.deltaTime * 25);

                realSpot = new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                    Mathf.RoundToInt(Global.main.MousePosition.y));

                transform.position = curSpot;

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                transform.position = realSpot;
                
            }

            Animate();
            cooldown -= Time.deltaTime;

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) { PlaceBlock(); }
        }

        void PlaceBlock()
        {
            isPlaced = true;
            GetComponent<Collider2D>().enabled = true;
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.transform.GetComponent<Luigi>() || coll.transform.GetComponent<Mario>() || coll.transform.GetComponent<Syobon>())
            {
                GetComponent<PhysicalBehaviour>().PlayClipOnce(pickUpSound);

                GameObject powerUp = Instantiate(ModAPI.FindSpawnable("FakeCoin").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("FakeCoin"), powerUp);

                GameObject.Destroy(gameObject);
            }
        }

        void Animate()
        {
            if (cooldown <= 0)
            {
                keyframe++;
                cooldown = 0.25f;
            }

            if (keyframe > 3)
            {
                keyframe = 0;
            }

            switch (keyframe)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = ONE;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = TWO;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = THREE;
                    break;
                case 3:
                    GetComponent<SpriteRenderer>().sprite = FOUR;
                    break;
            }

            GetComponent<PhysicalBehaviour>().RefreshOutline();
        }
    }
}
