using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using CharacterAnimator;
using BlockStuff;
using Library;
using PowerUps;
using GreenKoopa;
using GoombaClass;
using MarioController;
using CatMario;
using LuigiController;
using LakituClass;
using Spiny;
using PiranhaPlant;

namespace CharactersScript
{
    public enum CharacterActionState
    {
        Idle,
        Walking,
        Running,
        Falling,
        Jumping,
    }

    public enum PowerUpState
    {
        Small,
        Adult,
        Fireflower,
        Star
    }

    public class CharacterMain : MonoBehaviour
    {
        public static void Main()
        {
            //Mario
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Mario New",
                    DescriptionOverride = "New and improved Mario, with completely rewritten code. (that doesnt suck)",
                    CategoryOverride = ModAPI.FindCategory("Mario Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/MarioIcon.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.localScale = new Vector3(2f, 2f, 1);

                        Instance.GetComponent<BoxCollider2D>().size = new Vector2(.35f, .45f);

                        MarioNEW script = Instance.AddComponent<MarioNEW>();

                        Rigidbody2D rb = Instance.GetComponent<Rigidbody2D>();

                        rb.sharedMaterial = new PhysicsMaterial2D()
                        {
                            friction = 0,
                            bounciness = 0
                        };

                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                        SpriteRenderer rend = Instance.GetComponent<SpriteRenderer>();

                        script.SetRend(rend);
                        script.SetCharacterRigidbody(rb);

                        script.phys_behaviour = Instance.GetComponent<PhysicalBehaviour>();

                        script.animator = Instance.AddComponent<CharacterAnimatorInstance>();

                        script.animator.SetCharacter(script);

                        script.SetCharacterMass(2);

                        InitMarioAnimations(script);

                        ModAPI.Notify("Init all animations");
                    }
                });
        }

        public static void InitMarioAnimations(MarioNEW script)
        {
            #region Small Animations:

            #region Creation:

            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Small_Idle, out CharacterAnimation idle);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Small_Walking, out CharacterAnimation walking);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Small_Jumping, out CharacterAnimation jump);

            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Universal_Death, out CharacterAnimation death);

            #endregion

            #region Change Settings:

            idle.SetLoopAnimation(true);
            walking.SetLoopAnimation(true);
            jump.SetLoopAnimation(true);

            #endregion

            #region Add Frames:

            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Small_Idle, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/idle.png"),
                _length = .01f
            });

            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Small_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/run/run0.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Small_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/run/run1.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Small_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/run/run2.png"),
                _length = .1f
            });

            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Small_Jumping, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/run/jump.png"),
                _length = .01f
            });

            #endregion

            #endregion

            #region Big Animations:

            #region Creation:
            
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Big_Idle, out CharacterAnimation idle_big);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Big_Walking, out CharacterAnimation walking_big);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Big_Jump, out CharacterAnimation jump_big);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Transition_To_Adult, out CharacterAnimation adult_transition);

            #endregion

            #region Change Settings:

            idle_big.SetLoopAnimation(true);
            walking_big.SetLoopAnimation(true);
            jump_big.SetLoopAnimation(true);

            adult_transition.SetCanBeOverriden(false);
            adult_transition.SetLoopAmountOfTimes(2);

            #endregion

            #region Adding Frames:
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Big_Idle, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultIdle.png"),
                _length = .01f
            });

            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Big_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun1.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Big_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun2.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Big_Walking, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun3.png"),
                _length = .1f
            });
            
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Big_Jump, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultJump.png"),
                _length = .01f
            });

            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Transition_To_Adult, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/idle.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation(MarioNEW.Animation_Names.Transition_To_Adult, new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultIdle.png"),
                _length = .1f
            });
            #endregion

            #endregion

            #region Fire Animations:

            #region Creation:
            
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Fire_Idle, out CharacterAnimation idlefire);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Fire_Walking, out CharacterAnimation walkingfire);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Fire_Jump, out CharacterAnimation jumpfire);

            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Transition_To_Fire_From_Small, out CharacterAnimation fire_transition);
            script.animator.CreateNewAnimation(MarioNEW.Animation_Names.Transition_To_Fire_From_Big, out CharacterAnimation fire_transition_adult);
            
            #endregion

            #region Change Settings:

            idlefire.SetLoopAnimation(true);
            walkingfire.SetLoopAnimation(true);
            jumpfire.SetLoopAnimation(true);

            fire_transition.SetCanBeOverriden(false);
            fire_transition.SetLoopAmountOfTimes(2);

            fire_transition_adult.SetCanBeOverriden(false);
            fire_transition_adult.SetLoopAmountOfTimes(2);

            #endregion

            #region Adding Frames:
            script.animator.AddFrameToAnimation("idle_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireIdle.png"),
                _length = 0.05f
            });

            script.animator.AddFrameToAnimation("walking_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun1.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation("walking_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun2.png"),
                _length = .1f
            });
            script.animator.AddFrameToAnimation("walking_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun3.png"),
                _length = .1f
            });

            script.animator.AddFrameToAnimation("jump_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireJump.png"),
                _length = 0.1f
            });

            script.animator.AddFrameToAnimation("trans_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/idle.png"),
                _length = 0.1f
            });
            script.animator.AddFrameToAnimation("trans_fire", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireIdle.png"),
                _length = 0.1f
            });

            script.animator.AddFrameToAnimation("trans_fire_adult", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/grown/AdultIdle.png"),
                _length = 0.1f
            });
            script.animator.AddFrameToAnimation("trans_fire_adult", new FrameInformation()
            {
                _sprite = ModAPI.LoadSprite("assets/ch/mario/fire/FireIdle.png"),
                _length = 0.1f
            });
            #endregion

            #endregion
        }
    }

    /*
     * public struct Sounds
        {
            public AudioClip jumpSoundSmall;
            public AudioClip jumpSoundAdult;
            public AudioClip deathSound;

            public AudioClip powerUpSound;
            public AudioClip fireballSound;
        }
      *IEnumerator GoThroughPipe(Pipe controller)
        {
            float yDisp = 0;

            currentAction = PlayerActions.GoingIntoPipe;

            while (transform.position.y > controller.transform.position.y - 1)
            {
                yield return new WaitForEndOfFrame();

                yDisp = -0.2f;

                transform.position = new Vector3(controller.transform.position.x, transform.position.y + yDisp * Time.fixedDeltaTime);
            }

            transform.position = controller.secondPipe.transform.position;
            StartCoroutine(LeavePipe(controller.secondPipe.GetComponent<Pipe>()));

        }

        IEnumerator LeavePipe(Pipe secondPipe)
        {
            float yDisp = 0;

            while (transform.position.y < secondPipe.transform.position.y + transform.localScale.y / 2 - 0.01f)
            {
                yield return new WaitForEndOfFrame();

                yDisp = 0.2f;

                transform.position = new Vector3(secondPipe.transform.position.x, transform.position.y + yDisp * Time.fixedDeltaTime);
            }

            currentAction = PlayerActions.Idle;
            col.enabled = true;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    */

    public interface IitemInteractable
    {
        void PowerUpInteracted(PowerUpState powerUp);

        void MushroomInteracted();

        void FireFlowerInteracted();

        void StarInteracted();
    }

    public abstract class Character : MonoBehaviour, IitemInteractable
    {
        public bool debugging = false;

        public static UnityAction<CharacterActionState, CharacterActionState> on_action_state_change;

        private GameObject character;

        [SerializeField]
        protected CharacterActionState character_state = CharacterActionState.Idle;

        [SerializeField]
        protected PowerUpState character_power_state = PowerUpState.Small;

        public PhysicalBehaviour phys_behaviour;

        public float acceleration_force = 40;
        public float sprinting_acceleration_force = 80;
        public float max_x_velocity = 8;
        public float max_sprinting_x_velocity = 12;
        public float max_walking_x_velocity = 8;
        public float friction = 0.8f;
        public float jump_force = 10;
        public float jump_detection_ray_length = 0.7f;

        [SerializeField]
        protected bool can_shoot_fire;

        protected bool sprinting;

        [SerializeField]
        protected float x_input;

        [SerializeField]
        private float x_velocity;

        [SerializeField]
        protected Rigidbody2D rigidbody;

        [SerializeField]
        protected SpriteRenderer rend;

        public CharacterAnimatorInstance animator;

        public bool canMove { get; private set; } = true;

        public virtual void SetRend(SpriteRenderer rend)
        {
            this.rend = rend;
            ModAPI.Notify("player sprite renderer set.");
        }

        public virtual void ReloadOutline()
        {
            phys_behaviour.RefreshOutline();
        }

        public virtual void SetCharacterMass(float mass)
        {
            rigidbody.mass = mass;
            GetComponent<Collider2D>().sharedMaterial = new PhysicsMaterial2D()
            {
                bounciness = 0
            };

            ModAPI.Notify("character mass set.");
        }

        public virtual void SetRendererSprite(Sprite sprite_to_set)
        {
            rend.sprite = sprite_to_set;
        }

        public virtual void SetCharacterObject(GameObject obj)
        {
            character = obj;
            ModAPI.Notify("Character object set.");
        }

        public virtual void SetCharacterRigidbody(Rigidbody2D rb)
        {
            rigidbody = rb;
        }

        public virtual void SetAcceleration(float accel)
        {
            acceleration_force = accel;
        }

        //put in fixed update
        protected virtual void Movement()
        {
            x_velocity += acceleration_force * x_input * Time.deltaTime;

            if (sprinting)
            {
                max_x_velocity = max_sprinting_x_velocity;
            }
            else
            {
                max_x_velocity = max_walking_x_velocity;
            }

            if (Mathf.Abs(x_velocity) <= 0.02f) { x_velocity = 0; }

            if(Mathf.Abs(x_velocity) > max_x_velocity) { x_velocity = Mathf.Sign(x_velocity) * max_x_velocity; }

            if (UseFriction())
            {
                x_velocity *= Time.fixedDeltaTime * friction / Time.fixedDeltaTime;
            }

            if (!canMove) { return; }

            rigidbody.velocity = new Vector2(x_velocity, rigidbody.velocity.y);
        }

        protected virtual bool UseFriction()
        {
            if(!CanJump()) { return false; }

            if(x_input < 0 && rigidbody.velocity.x < 0) { return false; }

            if (x_input > 0 && rigidbody.velocity.x > 0) { return false; }

            return true;
        }

        protected virtual bool CanJump()
        {
            return Physics2D.Raycast(character.transform.position,
                Vector2.down, jump_detection_ray_length);
        }

        protected virtual void Jump()
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jump_force);
        }

        public virtual void PowerUpInteracted(PowerUpState powerUp)
        {
            string StopSnooping = "Stop snooping... this is MY code! >:(";
        }

        public virtual void MushroomInteracted()
        {
            character_power_state = PowerUpState.Adult;
        }

        public virtual void FireFlowerInteracted()
        {
            character_power_state = PowerUpState.Fireflower;
        }

        public virtual void StarInteracted()
        {
            character_power_state = PowerUpState.Star;
        }

        protected virtual void HandleBreaking()
        {
            //should make this sometime...

            if (debugging)
            {
                ModAPI.Draw.Line(transform.position,
                    transform.position + Vector3.up * jump_detection_ray_length);
            }

            RaycastHit2D hit = Physics2D.Raycast(character.transform.position,
                Vector2.up, jump_detection_ray_length);

            //if(hit.transform.TryGetComponent<>)

        }

        protected virtual void StopMovement()
        {
            canMove = false;
        }

        protected virtual void StartMovement()
        {
            canMove = true;
        }

        protected virtual void SetActionState(CharacterActionState new_state)
        {
            if(new_state == character_state) { return; }

            on_action_state_change?.Invoke(character_state, new_state);
            character_state = new_state;
        }
    }

    public class MarioNEW : Character
    {
        public struct Animation_Names
        {
            public const string Small_Idle = "idle";
            public const string Small_Walking = "walking";
            public const string Small_Jumping = "jumping";

            public const string Universal_Death = "death"; //is universal as the player cannot die until in small state.

            public const string Big_Idle = "idle_big";
            public const string Big_Walking = "walking_big";
            public const string Big_Jump = "jump_big";
            public const string Transition_To_Adult = "trans_adult";

            public const string Fire_Idle = "idle_fire";
            public const string Fire_Walking = "walking_fire";
            public const string Fire_Jump = "jump_fire";

            public const string Transition_To_Fire_From_Small = "trans_fire";
            public const string Transition_To_Fire_From_Big = "trans_fire_adult";
        }

        public static readonly Animation_Names AnimationNames = new Animation_Names();

        void Start()
        {
            GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton(
                "debug", 
                "Switch debug", 
                "Flips debug on and off", 
                new UnityAction[1]
                {
                    () =>
                    {
                        SwitchDebugOnOff();
                    }
                }
            ));

            GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton(
                "change_powerup",
                "Change Power up",
                "Lets you change current powerup.",
                new UnityAction[1]
                {
                    () =>
                    {
                        Utils.OpenTextInputDialog("small", this, (script, value) =>
                            script.PowerUpSwitcher(value.ToLower()), 
                            "Change Mario's Power Up.",
                            "Input Power Up Name Here: ");
                    }
                }
            ));

            SetCharacterObject(gameObject);
            animator.onFrameStep += DebugFrame;

            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if(!debugging) { return; }

            ModAPI.Notify(type + " | " + condition + " | " + stackTrace);
        }

        void DebugFrame(int frameCount, FrameInformation frameInfo)
        {
            if(!debugging) { return; }
        }

        private void FixedUpdate()
        {
            Movement();
        }

        protected override void Movement()
        {
            base.Movement();
        }

        private void FlipCharacter(bool flipped)
        {
            if(!CanJump()) { return; }

            if (flipped)
            {
                transform.localScale = new Vector3(-2, 2, 0);
            }
            else
            {
                transform.localScale = new Vector3(2, 2, 0);
            }
        }

        public void Update()
        {
            x_input = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprinting = true;
            }
            else
            {
                sprinting = false;
            }

            if (x_input < 0)
            {
                FlipCharacter(true);
            }
            else if (x_input > 0)
            {
                FlipCharacter(false);
            }

            if (CanJump() && Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }

            if (!CanJump() && !Input.GetKey(KeyCode.W))
            {
                rigidbody.gravityScale = 3;
            }
            else
            {
                rigidbody.gravityScale = 1.25f;
            }

            if (debugging)
            {
                ModAPI.Draw.Line(transform.position, 
                    transform.position + Vector3.down * jump_detection_ray_length);
            }

            HandleAnimations();
        }

        private void HandleAnimations()
        {
            if (x_input == 0)
            {
                switch (character_power_state)
                {
                    case PowerUpState.Small:
                        animator.StartAnimation(Animation_Names.Small_Idle);
                        break;
                    case PowerUpState.Adult:
                        animator.StartAnimation(Animation_Names.Big_Idle);
                        break;
                    case PowerUpState.Fireflower:
                        animator.StartAnimation(Animation_Names.Fire_Idle);
                        break;
                }
            }
            if (x_input != 0)
            {
                switch (character_power_state)
                {
                    case PowerUpState.Small:
                        animator.StartAnimation(Animation_Names.Small_Walking);
                        break;
                    case PowerUpState.Adult:
                        animator.StartAnimation(Animation_Names.Big_Walking);
                        break;
                    case PowerUpState.Fireflower:
                        animator.StartAnimation(Animation_Names.Fire_Walking);
                        break;
                }
            }
            if (!CanJump())
            {
                switch (character_power_state)
                {
                    case PowerUpState.Small:
                        animator.StartAnimation(Animation_Names.Small_Jumping);
                        break;
                    case PowerUpState.Adult:
                        animator.StartAnimation(Animation_Names.Big_Jump);
                        break;
                    case PowerUpState.Fireflower:
                        animator.StartAnimation(Animation_Names.Fire_Jump);
                        break;
                }
            }

            if (sprinting)
            {
                animator.SetAnimatorSpeed(speed: 1.5f);
            }
            else
            {
                animator.SetAnimatorSpeed(speed: 1);
            }
        }

        public override void MushroomInteracted()
        {
            if(character_power_state == PowerUpState.Fireflower) { return; }

            base.MushroomInteracted();

            animator.StartAnimation(Animation_Names.Transition_To_Adult);

            BoxCollider2D coll = GetComponent<BoxCollider2D>();

            coll.size = new Vector2(coll.size.x, .9f);

            jump_detection_ray_length = 1.1f;

            StopMovement();

            Invoke(nameof(StartMovement), 0.5f);
        }

        public override void FireFlowerInteracted()
        {
            base.FireFlowerInteracted();

            Debug.Log("Fire Flower interacted");

            switch(character_power_state)
            {
                case PowerUpState.Small:
                    animator.StartAnimation("trans_fire");
                    break;
                case PowerUpState.Adult:
                    animator.StartAnimation("trans_fire_adult");
                    break;
            };

            BoxCollider2D coll = GetComponent<BoxCollider2D>();

            coll.size = new Vector2(coll.size.x, .9f);

            jump_detection_ray_length = 1.1f;

            StopMovement();

            Invoke(nameof(StartMovement), 0.5f);
        }

        private void PowerUpSwitcher(string input)
        {
            switch (input)
            {
                case "small":
                    character_power_state = PowerUpState.Small;
                    break;
                case "big":
                    MushroomInteracted();
                    break;
                case "fire":
                    FireFlowerInteracted();
                    break;
                case "star":
                    ModAPI.Notify("Can't change to star.");
                    break;
            }
        }

        void SwitchDebugOnOff()
        {
            debugging = !debugging;

            switch(debugging)
            {
                case true:
                    ModAPI.Notify("Debugging on.");
                    break;
                case false:
                    ModAPI.Notify("Debugging off.");
                    break;
            }
        }
    }
}
