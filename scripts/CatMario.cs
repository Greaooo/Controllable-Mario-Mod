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
using LuigiController;

namespace CatMario
{
    public class Syobon : MonoBehaviour, Character
    {

        public GameObject mario;
        private GameObject pipeStandingOn;

        private bool brokeBlock = false;

        private bool controllable = true;
        public bool adult = false;
        public bool star = false;

        private float runAnimTime;
        private float jumpRange = 1f;

        private float cooldown = 1;
        private float fireSpriteCooldown = 0;
        private float fireShootingCooldown = 0;
        private float smallJumpCooldown = 0.15f;

        private float invincibilityFrames = 0;

        private bool moving;

        private int animNumb;
        private int shotFire;

        private float maxSpeed = 7;
        private const float SPRINT_SPEED = 12;

        private float moveSpeed = 500;

        protected bool canJump;

        public healthStates healthState;
        public PlayerActions currentAction;

        private Sprite curSprite;

        public Rigidbody2D rb;

        public SpriteRenderer sr;

        public CapsuleCollider2D col;

        public float jumpForce;
        public float jumpFalloff;

        private RaycastHit2D hit;

        public struct Sounds
        {
            public AudioClip jumpSoundSmall;
            public AudioClip jumpSoundAdult;
            public AudioClip deathSound;

            public AudioClip powerUpSound;
            public AudioClip fireballSound;
        }

        public struct Sprites
        {
            public Sprite curIdle, curRun1, curRun2, curRun3, curJump;

            public Sprite smallIdle, smallRun1, smallRun2, smallRun3, smallJump, smallDeath;
        }

        public Sprites AllSprites;
        public Sounds allSounds;

        private void FixedUpdate()
        {
            Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            rb.AddForce(mario.transform.right * moveDir * moveSpeed);

            RbStuff();
            HandleBreaking();
        }

        public void Update()
        {
            if (currentAction == PlayerActions.GoingIntoPipe)
            {
                GetComponent<SpriteRenderer>().sortingOrder = -1;

                rb.gravityScale = 0;

                col.enabled = false;

                return;
            }

            hit = CharacterCreatorLibrary.CanJump(jumpRange, transform.position);

            sr.sprite = curSprite;

            invincibilityFrames -= Time.deltaTime;
            fireSpriteCooldown -= Time.deltaTime;
            fireShootingCooldown -= Time.deltaTime;
            smallJumpCooldown -= Time.deltaTime;

            //cooldown for jumping
            if (smallJumpCooldown > 0)
            {
                canJump = false;
            }

            switch (healthState)
            {
                case healthStates.ALIVE:

                    HandleInput();
                    HandleJumping();

                    if (moving)
                    {
                        HandleAnim();
                    }

                    //if off the ground, set sprite to jumping sprite
                    if (!canJump)
                    {
                        curSprite = AllSprites.curJump;

                        GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }

                    break;

                case healthStates.DEAD:

                    GetComponent<SpriteRenderer>().sprite = AllSprites.smallDeath;
                    rb.gravityScale = 1;
                    rb.drag = 0;

                    break;
            }

            GetComponent<PhysicalBehaviour>().RefreshOutline();
        }

        public void HandleJumping()
        {
            if (healthState == healthStates.DEAD) { return; }

            //touching ground then i can jump, and set gravity back to normal
            if (hit)
            {
                canJump = true;
                brokeBlock = false;
            }
            else
            {
                canJump = false;
            }

            if (canJump && Input.GetKeyDown(KeyCode.W))
            {
                Jump(11, false, true);
            }

            if (hit.transform.TryGetComponent<Pipe>(out Pipe pipeController))
            {
                if (Input.GetKeyDown(KeyCode.S) && !pipeController.isSecondPhase)
                {
                    StartCoroutine(GoThroughPipe(pipeController));
                }
            }
        }

        IEnumerator GoThroughPipe(Pipe controller)
        {
            Vector3 newPosition = new Vector2(0, 0);

            currentAction = PlayerActions.GoingIntoPipe;

            while (transform.position.y > controller.transform.position.y - 1)
            {
                yield return new WaitForEndOfFrame();

                newPosition = controller.transform.up * -0.1f;

                transform.position = new Vector3(controller.transform.position.x + newPosition.x, transform.position.y + newPosition.y * Time.fixedDeltaTime);
            }

            transform.position = controller.secondPipe.transform.position;
            StartCoroutine(LeavePipe(controller.secondPipe.GetComponent<Pipe>()));

        }

        IEnumerator LeavePipe(Pipe secondPipe)
        {
            Vector3 newPosition = new Vector2(0, 0);

            while (transform.position.y < secondPipe.transform.position.y + transform.localScale.y / 2 - 0.01f)
            {
                yield return new WaitForEndOfFrame();

                newPosition = secondPipe.transform.up * 0.1f;

                transform.position = new Vector3(secondPipe.transform.position.x + newPosition.x, transform.position.y + newPosition.y * Time.fixedDeltaTime);
            }

            currentAction = PlayerActions.Idle;
            col.enabled = true;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        public void HandleBreaking()
        {
            if(brokeBlock) { return; }

            //send out ray upwards
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, jumpRange);

            //if raycast hits a block, try to break it
            if (hit.transform.TryGetComponent<BlockClass>(out BlockClass script))
            {
                script.BlockHit(adult);
            }
        }

        public void HandleInput()
        {
            //Tell if moving
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                moving = true;
            }
            else
            {
                moving = false;
                currentAction = PlayerActions.Idle;
            }

            //sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                maxSpeed = SPRINT_SPEED;

                currentAction = PlayerActions.Running;
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                currentAction = PlayerActions.Walking;

                maxSpeed = 7;
            }

            //stop sliding around
            if (!moving && canJump)
            {
                rb.drag = 15;
            }
            else
            {
                rb.drag = 0;
            }

            //if not moving, set sprite to idle
            if (!moving && canJump)
            {
                curSprite = AllSprites.curIdle;
            }
            else if (!canJump)
            {
                curSprite = AllSprites.curJump;
            }

            //die in lava
            if (mario.GetComponent<PhysicalBehaviour>().IsInLava)
            {
                healthState = healthStates.DEAD;
            }
        }

        public void HandleAnim()
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-2, 2, 2);
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }

            if (canJump)
            {
                //decrease timer
                runAnimTime -= Time.deltaTime;

                //reset number if too high
                if (animNumb > 2) animNumb = 1;

                //change keyframe number
                if (runAnimTime <= 0)
                {
                    runAnimTime = 0.7f / maxSpeed;
                    animNumb++;
                }

                //set sprite to current keyframe using int and array
                switch (animNumb)
                {
                    case 1:
                        curSprite = AllSprites.curRun1;
                        break;
                    case 2:
                        curSprite = AllSprites.curIdle;
                        break;
                }
            }
        }

        public void Jump(float jumpForce, bool fallFast, bool playSound)
        {
            if (fallFast)
            {
                rb.gravityScale = 2;
            }

            if (playSound && !adult)
            {
                GetComponent<PhysicalBehaviour>().PlayClipOnce(clip: allSounds.jumpSoundSmall, volume: 1f);

            }
            else if (playSound && adult)
            {
                GetComponent<PhysicalBehaviour>().PlayClipOnce(clip: allSounds.jumpSoundAdult, volume: 1);
            }

            //cant jump again, so you cant infinitely jump in the air
            canJump = false;

            //add force upwards for jump
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        public void Hurt()
        {
            if (invincibilityFrames > 0) { return; }

            Die();
        }

        public void Die()
        {
            GetComponent<SpriteRenderer>().sprite = AllSprites.smallDeath;

            cooldown -= Time.fixedDeltaTime;

            GetComponent<PhysicalBehaviour>().PlayClipOnce(allSounds.deathSound, 1);

            rb.AddForce(transform.up * 150, ForceMode2D.Impulse);
            controllable = false;

            col.enabled = false;

            if (cooldown <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }

        public void SetInvincible(float time, bool llalalskdlk)
        {
            invincibilityFrames = time;
            star = llalalskdlk;
        }

        public void ShootFire()
        {
            if(fireShootingCooldown > 0) { return; }

            if (fireSpriteCooldown <= 0)
            {
                GameObject fireBall = Instantiate(ModAPI.FindSpawnable("Fire Ball").Prefab, transform.position + new Vector3(0.25f * (transform.localScale.x / transform.localScale.x), 0, 0), Quaternion.identity);
                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("Fire Ball"), fireBall);

                GetComponent<PhysicalBehaviour>().PlayClipOnce(allSounds.fireballSound, 1f);

                fireBall.GetComponent<FireBall>().characterThatThrew = mario;
                fireBall.GetComponent<Rigidbody2D>().AddForce(fireBall.transform.right * transform.localScale.x * (35 + rb.velocity.x) + -fireBall.transform.up * 35, ForceMode2D.Impulse);

                fireSpriteCooldown = 0.15f;

                shotFire++;
            }

            if (shotFire == 2)
            {
                shotFire = 0;
                fireShootingCooldown = 1.5f;
            }
        }

        public void RbStuff()
        {
            if (Input.GetKey(KeyCode.W)) { rb.gravityScale = 1; }
            else
            {
                rb.gravityScale = 2;
            }

            rb.mass = 16;
            rb.useAutoMass = false;

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

            PhysicsMaterial2D mat = new PhysicsMaterial2D();

            mat.friction = 0;
            mat.bounciness = 0;

            rb.sharedMaterial = mat;
        }
    }
}
