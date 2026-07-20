using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using CharactersScript;
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

//Mario "assets/ch/mario/"
//Luigi "assets/ch/luigi/"
//Peach "assets/ch/peach/"

namespace Mod
{
    /*
    public struct CharacterUTILS
    {
        public static AssetBundle LoadAssetBundle(string path)
        {
            string text = Path.Combine(ModAPI.Metadata.MetaLocation, path);

            if (ModResourceCache.TryGet<AssetBundle>(text, out AssetBundle bundle))
            {
                return bundle;
            }

            if(!File.Exists(text))
            {
                Debug.LogError("No bundle found.");
                return null;
            }

            AssetBundle newAssetbundle = AssetBundle.LoadFromFile(text);

            if (newAssetbundle == null) 
            {
                Debug.LogError("No bundle found.");
            }

            ModResourceCache.Cache(text, newAssetbundle);
            return newAssetbundle;
        }
    }
    */

    public enum healthStates
    {
        DEAD,
        ALIVE
    }

    public enum CharacterPowerUpStates
    {
        SMALL,
        TALL,
        FIRE,
        STAR
    }

    public enum EnemyHealthStates
    {
        STANDARD,
        SQUASHED,
        DEAD
    }

    public enum PlayerActions
    {
        Jumping,
        Idle,
        Walking,
        Running,
        GoingIntoPipe,
        OnSpring
    }

    public class Mod : MonoBehaviour
    {
        public static void Main()
        {
            ModAPI.Notify("A for left, D for right, W for jump, F to shoot fireballs, E or Q to place blocks, and left click to deselect current block");

            ModAPI.RegisterCategory("Mario Category", "All the mario characters!", ModAPI.LoadSprite("assets/tabs/mario.png"));
            ModAPI.RegisterCategory("Enemy Category", "All the mario enemies!", ModAPI.LoadSprite("assets/tabs/enemy.png"));
            ModAPI.RegisterCategory("Boss Category", "All the mario enemies!", ModAPI.LoadSprite("assets/tabs/boss.png"));
            ModAPI.RegisterCategory("Blocks Category", "All the mario blocks", ModAPI.LoadSprite("assets/tabs/blocks.png"));
            ModAPI.RegisterCategory("Power Up Category", "All the mario Power Ups", ModAPI.LoadSprite("assets/tabs/power.png"));

            #region Entities

            #region Playable Characters

            //Mario
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Mario -CM",
                    DescriptionOverride = "Mario, from mario",
                    CategoryOverride = ModAPI.FindCategory("Mario Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/MarioIcon.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<Rigidbody2D>().mass = 1;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/ch/mario/idle.png");
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                        Instance.AddComponent<CapsuleCollider2D>();
                        UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());

                        Instance.AddComponent<Mario>();
                        Instance.AddComponent<EditableTintBehaviour>();

                        Mario mScript = Instance.GetComponent<Mario>();

                        mScript.transform.name = "Mario";
                        mScript.transform.tag = "character";
                        mScript.mario = Instance.gameObject;

                        mScript.healthState = healthStates.ALIVE;
                        mScript.powerState = CharacterPowerUpStates.SMALL;

                        mScript.rb = Instance.GetComponent<Rigidbody2D>();
                        mScript.col = Instance.GetComponent<CapsuleCollider2D>();
                        mScript.sr = Instance.GetComponent<SpriteRenderer>();

                        mScript.jumpForce = 170;
                        mScript.jumpFalloff = 2;

                        Instance.GetComponent<PhysicalBehaviour>().PlaySliderSound = false;

                        Instance.transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.33f, .45f);
                        Instance.transform.localScale = new Vector3(2, 2, 2);

                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;

                        //get all sounds
                        mScript.allSounds = new Mario.Sounds()
                        {
                            deathSound = ModAPI.LoadSound("assets/sounds/mariodie.wav"),
                            fireballSound = ModAPI.LoadSound("assets/sounds/shootFire.wav"),
                            jumpSoundAdult = ModAPI.LoadSound("assets/sounds/ajump.wav"),
                            jumpSoundSmall = ModAPI.LoadSound("assets/sounds/sjump.wav"),
                            powerUpSound = ModAPI.LoadSound("assets/sounds/grabbedP.wav"),
                        };

                        //get all sprites
                        mScript.AllSprites = new Mario.Sprites()
                        {
                            //small mario sprites
                            smallIdle = ModAPI.LoadSprite("assets/ch/mario/idle.png"),
                            smallRun1 = ModAPI.LoadSprite("assets/ch/mario/run/run0.png"),
                            smallRun2 = ModAPI.LoadSprite("assets/ch/mario/run/run1.png"),
                            smallRun3 = ModAPI.LoadSprite("assets/ch/mario/run/run2.png"),
                            smallJump = ModAPI.LoadSprite("assets/ch/mario/run/jump.png"),
                            smallDeath = ModAPI.LoadSprite("assets/ch/mario/marioDeath.png"),

                            //starting Sprites
                            curIdle = ModAPI.LoadSprite("assets/ch/mario/idle.png"),
                            curRun1 = ModAPI.LoadSprite("assets/ch/mario/run/run0.png"),
                            curRun2 = ModAPI.LoadSprite("assets/ch/mario/run/run1.png"),
                            curRun3 = ModAPI.LoadSprite("assets/ch/mario/run/run2.png"),
                            curJump = ModAPI.LoadSprite("assets/ch/mario/run/jump.png"),

                            //adult sprites
                            adultIdle = ModAPI.LoadSprite("assets/ch/mario/grown/AdultIdle.png"),
                            adultRun1 = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun1.png"),
                            adultRun2 = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun2.png"),
                            adultRun3 = ModAPI.LoadSprite("assets/ch/mario/grown/AdultRun3.png"),
                            adultJump = ModAPI.LoadSprite("assets/ch/mario/grown/AdultJump.png"),

                            //fire sprites
                            fireIdle = ModAPI.LoadSprite("assets/ch/mario/fire/FireIdle.png"),
                            fireRun1 = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun1.png"),
                            fireRun2 = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun2.png"),
                            fireRun3 = ModAPI.LoadSprite("assets/ch/mario/fire/FireRun3.png"),
                            fireJump = ModAPI.LoadSprite("assets/ch/mario/fire/FireJump.png"),
                            fireShoot = ModAPI.LoadSprite("assets/ch/mario/fire/shootingFire.png")
                        };
                    }
                });

            

            //make an easter egg that unlocks cat mario

            /*
            //catMario
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Cat Mario -CM",
                    DescriptionOverride = "meow",
                    CategoryOverride = ModAPI.FindCategory("Mario Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/CatIcon.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/ch/cat/idle.png");
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                        Instance.AddComponent<CapsuleCollider2D>();
                        UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());

                        Syobon mScript = Instance.AddComponent<Syobon>();

                        mScript.mario = Instance.gameObject;

                        mScript.healthState = healthStates.ALIVE;

                        mScript.rb = Instance.GetComponent<Rigidbody2D>();
                        mScript.col = Instance.GetComponent<CapsuleCollider2D>();
                        mScript.sr = Instance.GetComponent<SpriteRenderer>();

                        mScript.jumpForce = 170;
                        mScript.jumpFalloff = 2;

                        Instance.GetComponent<PhysicalBehaviour>().PlaySliderSound = false;

                        Instance.transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.33f, .93f);
                        Instance.transform.localScale = new Vector3(2, 2, 2);

                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;

                        //get all sounds
                        mScript.allSounds = new Syobon.Sounds()
                        {
                            deathSound = ModAPI.LoadSound("assets/sounds/mariodie.wav"),
                            jumpSoundAdult = ModAPI.LoadSound("assets/sounds/ajump.wav"),
                            jumpSoundSmall = ModAPI.LoadSound("assets/sounds/sjump.wav"),
                            powerUpSound = ModAPI.LoadSound("assets/sounds/grabbedP.wav"),
                        };

                        //get all sprites
                        mScript.AllSprites = new Syobon.Sprites()
                        {
                            //small mario sprites
                            smallIdle = ModAPI.LoadSprite("assets/ch/cat/idle.png"),
                            smallRun1 = ModAPI.LoadSprite("assets/ch/cat/run.png"),
                            smallJump = ModAPI.LoadSprite("assets/ch/cat/jump.png"),
                            smallDeath = ModAPI.LoadSprite("assets/ch/cat/dead.png"),

                            //starting Sprites
                            curIdle = ModAPI.LoadSprite("assets/ch/cat/idle.png"),
                            curRun1 = ModAPI.LoadSprite("assets/ch/cat/run.png"),
                            curJump = ModAPI.LoadSprite("assets/ch/cat/jump.png"),
                        };
                    }
                });
            */

            //Luigi
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Luigi -CM",
                    DescriptionOverride = "Luigi, from mario",
                    CategoryOverride = ModAPI.FindCategory("Mario Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("Textures/LuigiIcon.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/ch/mario/idle.png");
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                        Instance.AddComponent<CapsuleCollider2D>();
                        UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());

                        Instance.AddComponent<Luigi>();

                        Luigi lScript = Instance.GetComponent<Luigi>();

                        lScript.luigi = Instance.gameObject;
                        lScript.transform.name = "Luigi";
                        lScript.transform.tag = "character";

                        lScript.healthState = healthStates.ALIVE;
                        lScript.powerState = CharacterPowerUpStates.SMALL;

                        lScript.rb = Instance.GetComponent<Rigidbody2D>();
                        lScript.col = Instance.GetComponent<CapsuleCollider2D>();
                        lScript.sr = Instance.GetComponent<SpriteRenderer>();

                        lScript.jumpForce = 200;
                        lScript.jumpFalloff = 1.75f;

                        Instance.GetComponent<PhysicalBehaviour>().PlaySliderSound = false;

                        Instance.transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.33f, .45f);
                        Instance.transform.localScale = new Vector3(2, 2, 2);

                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;

                        //get all sounds
                        lScript.allSounds = new Luigi.Sounds()
                        {
                            deathSound = ModAPI.LoadSound("assets/sounds/mariodie.wav"),
                            fireballSound = ModAPI.LoadSound("assets/sounds/shootFire.wav"),
                            jumpSoundAdult = ModAPI.LoadSound("assets/sounds/ajump.wav"),
                            jumpSoundSmall = ModAPI.LoadSound("assets/sounds/sjump.wav"),
                            powerUpSound = ModAPI.LoadSound("assets/sounds/grabbedP.wav"),
                        };

                        //get all sprites
                        lScript.AllSprites = new Luigi.Sprites()
                        {
                            //small mario sprites
                            smallIdle = ModAPI.LoadSprite("assets/ch/luigi/idle.png"),
                            smallRun1 = ModAPI.LoadSprite("assets/ch/luigi/run/run1.png"),
                            smallRun2 = ModAPI.LoadSprite("assets/ch/luigi/run/run2.png"),
                            smallRun3 = ModAPI.LoadSprite("assets/ch/luigi/run/run3.png"),
                            smallJump = ModAPI.LoadSprite("assets/ch/luigi/run/jump.png"),
                            smallDeath = ModAPI.LoadSprite("assets/ch/luigi/death.png"),

                            //starting Sprites
                            curIdle = ModAPI.LoadSprite("assets/ch/luigi/idle.png"),
                            curRun1 = ModAPI.LoadSprite("assets/ch/luigi/run/run1.png"),
                            curRun2 = ModAPI.LoadSprite("assets/ch/luigi/run/run2.png"),
                            curRun3 = ModAPI.LoadSprite("assets/ch/luigi/run/run3.png"),
                            curJump = ModAPI.LoadSprite("assets/ch/luigi/run/jump.png"),

                            //adult sprites
                            adultIdle = ModAPI.LoadSprite("assets/ch/luigi/grown/luigiAIdle.png"),
                            adultRun1 = ModAPI.LoadSprite("assets/ch/luigi/grown/luigiA1.png"),
                            adultRun2 = ModAPI.LoadSprite("assets/ch/luigi/grown/luigiA2.png"),
                            adultRun3 = ModAPI.LoadSprite("assets/ch/luigi/grown/luigiA3.png"),
                            adultJump = ModAPI.LoadSprite("assets/ch/luigi/grown/luigiAJump.png"),

                            //fire sprites
                            fireIdle = ModAPI.LoadSprite("assets/ch/luigi/fire/fireI.png"),
                            fireRun1 = ModAPI.LoadSprite("assets/ch/luigi/fire/fire1.png"),
                            fireRun2 = ModAPI.LoadSprite("assets/ch/luigi/fire/fire2.png"),
                            fireRun3 = ModAPI.LoadSprite("assets/ch/luigi/fire/fire3.png"),
                            fireJump = ModAPI.LoadSprite("assets/ch/luigi/fire/fireJ.png"),
                            fireShoot = ModAPI.LoadSprite("assets/ch/luigi/fire/fireS.png")
                        };
                    }

                });

            #endregion

            #region Enemies

            //Goomba
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Goomba",
                    DescriptionOverride = "Mario's most iconic enemy, i think.",
                    CategoryOverride = ModAPI.FindCategory("Enemy Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("Textures/Enemies/Goomba/GoombaPrev.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                        Instance.AddComponent<CapsuleCollider2D>();
                        UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());

                        Instance.transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.43f, .43f);
                        Instance.transform.localScale = new Vector3(2, 2, 2);
                        Instance.name = "enemy";

                        Instance.AddComponent<Goomba>();

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Enemies/Goomba/Goomba1.png");

                        Instance.GetComponent<Goomba>().enemy = Instance.transform;
                        Instance.GetComponent<Goomba>().rb = Instance.GetComponent<Rigidbody2D>();

                        Instance.GetComponent<Goomba>().run1 = ModAPI.LoadSprite("Textures/Enemies/Goomba/Goomba1.png");
                        Instance.GetComponent<Goomba>().run2 = ModAPI.LoadSprite("Textures/Enemies/Goomba/Goomba2.png");
                        Instance.GetComponent<Goomba>().squashed = ModAPI.LoadSprite("Textures/Enemies/Goomba/Goomba3.png");

                        Instance.GetComponent<Goomba>().squashSound = ModAPI.LoadSound("assets/sounds/stomp.wav");
                    }
                });

            //Koopa
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Koopa",
                    DescriptionOverride = "Mario's second most iconic enemy, i think.",
                    CategoryOverride = ModAPI.FindCategory("Enemy Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("Textures/Enemies/Koopa/KoopaPrev.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                        Instance.AddComponent<CapsuleCollider2D>();
                        UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());

                        Instance.transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.43f, .66f);
                        Instance.transform.localScale = new Vector3(2, 2, 2);
                        Instance.name = "enemy";

                        Instance.AddComponent<Koopa>();
                        Instance.AddComponent<Koopa>().state = EnemyHealthStates.STANDARD;

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Enemies/Koopa/Koopa1.png");

                        Instance.GetComponent<Koopa>().enemy = Instance.transform;
                        Instance.GetComponent<Koopa>().rb = Instance.GetComponent<Rigidbody2D>();

                        Instance.GetComponent<Koopa>().Hurt(0.01f);

                        Instance.GetComponent<Koopa>().run1 = ModAPI.LoadSprite("Textures/Enemies/Koopa/Koopa1.png");
                        Instance.GetComponent<Koopa>().run2 = ModAPI.LoadSprite("Textures/Enemies/Koopa/Koopa2.png");
                        Instance.GetComponent<Koopa>().squashed = ModAPI.LoadSprite("Textures/Enemies/Koopa/Koopa3.png");

                        Instance.GetComponent<Koopa>().squashSound = ModAPI.LoadSound("assets/sounds/stomp.wav");
                        Instance.GetComponent<Koopa>().deathSound = ModAPI.LoadSound("assets/sounds/kick.wav");
                    }
                });

            //lakitu
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Lakitu",
                DescriptionOverride = "He likes clouds",
                CategoryOverride = ModAPI.FindCategory("Enemy Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/enemy/lak/idle.png"),
                NameToOrderByOverride = "2",

                AfterSpawn = (Instance) =>
                {
                    Instance.transform.localScale = Vector3.one * 2;
                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(.43f, .43f);
                    Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                    Lakitu script = Instance.AddComponent<Lakitu>();

                    script.rb = Instance.GetComponent<Rigidbody2D>();

                    script.sprites = new Lakitu.LakituSprites()
                    {
                        idle = ModAPI.LoadSprite("assets/enemy/lak/idle.png"),
                        grabbing = ModAPI.LoadSprite("assets/enemy/lak/grab.png")
                    };

                    script.rend = Instance.GetComponent<SpriteRenderer>();
                }

            });

            //spiny
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Spiny",
                DescriptionOverride = "YEEEOUCHHHHH",
                CategoryOverride = ModAPI.FindCategory("Enemy Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/enemy/spiny/walk1.png"),
                NameToOrderByOverride = "2",

                AfterSpawn = (Instance) =>
                {
                    Instance.transform.localScale = Vector3.one * 2;
                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(.43f, .43f);
                    Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                    Spiny.Spiny script = Instance.AddComponent<Spiny.Spiny>();

                    script.rb = Instance.GetComponent<Rigidbody2D>();

                    script.sprites = new Spiny.Spiny.Sprites()
                    {
                        idle1 = ModAPI.LoadSprite("assets/enemy/spiny/walk1.png"),
                        idle2 = ModAPI.LoadSprite("assets/enemy/spiny/walk2.png"),
                        egg1 = ModAPI.LoadSprite("assets/enemy/spiny/egg1.png"),
                        egg2 = ModAPI.LoadSprite("assets/enemy/spiny/egg2.png")
                    };

                    script.rend = Instance.GetComponent<SpriteRenderer>();
                    script.rend.sprite = ModAPI.LoadSprite("assets/enemy/spiny/walk1.png");
                }
            });

            //piranha plant
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "piranha plant",
                DescriptionOverride = "How does this thing hurt mario???",
                CategoryOverride = ModAPI.FindCategory("Enemy Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/enemy/plant/plant1.png"),
                NameToOrderByOverride = "2",

                AfterSpawn = (Instance) =>
                {
                    Instance.transform.localScale = Vector3.one * 2;
                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(.43f, .43f);

                    Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                    Instance.GetComponent<BoxCollider2D>().enabled = false;

                    UnityEngine.Object.Destroy(Instance.GetComponent<Rigidbody2D>());

                    PipePlant script = Instance.AddComponent<PipePlant>();

                    Sprite[] bothSprites = {

                        ModAPI.LoadSprite("assets/enemy/plant/plant1.png"),
                        ModAPI.LoadSprite("assets/enemy/plant/plant2.png"),
                    };

                    script.TwoSprites = bothSprites;
                    script.rend = Instance.GetComponent<SpriteRenderer>();
                }
            });

            #endregion

            //fireball
            ModAPI.Register(new Modification()
            {
                NameOverride = "Fire Ball",
                DescriptionOverride = "The most iconic fireball... not really a cool title.",
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),

                ThumbnailOverride = ModAPI.LoadSprite("assets/Items/fire/fireBall.png"),
                NameToOrderByOverride = "15",
                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/Items/fire/fireBall.png");
                    Instance.AddComponent<FireBall>();

                    UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());
                    Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                    Instance.AddComponent<CircleCollider2D>();
                    Instance.transform.localScale = new Vector3(2, 2, 2);
                    Instance.GetComponent<FireBall>().col = Instance.GetComponent<CircleCollider2D>();
                    Instance.GetComponent<FireBall>().rb = Instance.GetComponent<Rigidbody2D>();
                }
            });

            #region bosses
            //Bowser
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Bowser",
                    DescriptionOverride = "Bowser. He needs no introduction",
                    CategoryOverride = ModAPI.FindCategory("Boss Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/enemy/bowserprev.png"),

                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");
                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, .9f);
                        Instance.transform.localScale = new Vector3(-2, 2, 1);
                        Instance.name = "enemy";

                        Instance.AddComponent<Bowser>();
                        Instance.GetComponent<Bowser>().rb = Instance.GetComponent<Rigidbody2D>();
                        Instance.GetComponent<Bowser>().bows1 = ModAPI.LoadSprite("assets/enemy/Bowser.png");
                        Instance.GetComponent<Bowser>().bows2 = ModAPI.LoadSprite("assets/enemy/Bowser2.png");

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/enemy/Bowser.png");


                    }
                });

            //bowser fire
            ModAPI.Register(new Modification()
            {
                NameOverride = "Bowser Fire",
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/Items/fire/fire1.png"),
                NameToOrderByOverride = "15",

                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/Items/fire/fire1.png");
                    Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soft");

                    Instance.transform.localScale = new Vector3(-2, 2, 1);
                    Instance.FixColliders();

                    Instance.GetComponent<Collider2D>().isTrigger = true;

                    BFire bf = Instance.AddComponent<BFire>();
                    bf.rb = Instance.GetComponent<Rigidbody2D>();
                }
            });
            #endregion

            #endregion

            BlockMain.CreateBlocks();

            CharacterMain.Main();

            #region Items

            //Coin
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Coin",
                DescriptionOverride = "Golden coin. Must weigh a lot.",
                CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/Items/coin/coinPrev.png"),
                NameToOrderByOverride = "2",
                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/Items/coin/coin0.png");

                    Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                    Instance.GetComponent<Collider2D>().isTrigger = true;

                    Instance.AddComponent<Coins>();

                    Instance.GetComponent<Coins>().ONE = ModAPI.LoadSprite("assets/Items/coin/coin0.png");
                    Instance.GetComponent<Coins>().TWO = ModAPI.LoadSprite("assets/Items/coin/coin1.png");
                    Instance.GetComponent<Coins>().THREE = ModAPI.LoadSprite("assets/Items/coin/coin2.png");
                    Instance.GetComponent<Coins>().FOUR = ModAPI.LoadSprite("assets/Items/coin/coin3.png");

                    Instance.GetComponent<Coins>().pickUpSound = ModAPI.LoadSound("assets/sounds/coin.wav");

                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(0.43f, 0.43f);
                    Instance.transform.localScale = new Vector3(2, 2, 2);
                }
            });

            //fire flower
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Fire Flower",
                DescriptionOverride = "Spicy",
                //CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/Items/firePrev.png"),
                NameToOrderByOverride = "2",
                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Items/FireFlower.png");
                    FireFlower s = Instance.AddComponent<FireFlower>();

                    for (int i = 0; i < 3; i++)
                    {
                        s.flowerSprites[i] = ModAPI.LoadSprite("assets/Items/flower/" + i + ".png");
                    }

                    Instance.GetComponent<Rigidbody2D>().freezeRotation = true;

                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(0.43f, 0.43f);
                    Instance.transform.localScale = new Vector3(2, 2, 2);
                }
            });

            //mushroom
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Mushroom",
                DescriptionOverride = "I wonder what happens to you when you eat this..",
                //CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/Items/mushPrev.png"),
                NameToOrderByOverride = "2",
                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Items/mushroom.png");
                    Instance.AddComponent<Mushroom>();
                    Instance.GetComponent<Mushroom>().moveDir = 1;

                    Instance.GetComponent<Rigidbody2D>().freezeRotation = true;

                    Instance.GetComponent<BoxCollider2D>().size = new Vector2(0.43f, 0.43f);
                    Instance.transform.localScale = new Vector3(2, 2, 2);
                }
            });

            #endregion

            #region Fx
            //fire fx
            ModAPI.Register(new Modification()
            {
                NameOverride = "FireFx",
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/fx/fFx2.png"),

                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/fx/fFx2.png");
                    Instance.AddComponent<Fx>();

                    UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());
                    UnityEngine.Object.Destroy(Instance.GetComponent<Rigidbody2D>());
   
                    Instance.transform.localScale = new Vector3(2, 2, 2);

                }
            });

            //points fx
            ModAPI.Register(new Modification()
            {
                NameOverride = "100",
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),

                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/fx/100.png");
                    Instance.AddComponent<PointsFx>();

                    UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());
                    UnityEngine.Object.Destroy(Instance.GetComponent<Rigidbody2D>());

                    Instance.transform.localScale = new Vector3(1, 1, 1);

                }
            });

            //coin fx
            ModAPI.Register(new Modification()
            {
                NameOverride = "FakeCoin",
                //CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),

                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/Items/coin/coin0.png");
                    Instance.AddComponent<CoinFx>();
                    Instance.GetComponent<CoinFx>().sound = ModAPI.LoadSound("assets/sounds/coin.wav");

                    UnityEngine.Object.Destroy(Instance.GetComponent<BoxCollider2D>());
                    UnityEngine.Object.Destroy(Instance.GetComponent<Rigidbody2D>());

                    Instance.transform.localScale = new Vector3(2, 2, 2);

                }
            });

            #endregion

            
        }
    }

    //mario controller

    public interface Character
    {
        void Hurt();

        void SetInvincible(float time, bool starGrabbed);

        void Jump(float jumpForce, bool fallFast, bool playSound);
    }

    public abstract class BasicEnemy : MonoBehaviour
    {
        public Transform enemy;

        public AudioClip squashSound, deathSound;

        public Rigidbody2D rb;

        public Sprite run1, run2;

        public Sprite squashed;

        public float lRange = 0.6f, rRange = 0.6f, maxSpeed = 2;

        public int walkDir = 1;

        public float deathCooldown = 0.3f;
        public float animCooldown = 0.1f;
        public int animNumb;

        public EnemyHealthStates state;

        public abstract void Walk();

        public abstract void Animate();

        public abstract void Hurt(float cooldown);

        public abstract void Death();

        public abstract void TestForCollision();
    }

    public class SquareDraw : MonoBehaviour
    {
        private Vector3 startPos;
        private Vector3 endPos;

        private bool placed;

        public GameObject square;

        public Sprite backgroundSprite;

        private void Update()
        {
            if (!placed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = new Vector3(Global.main.MousePosition.x, Global.main.MousePosition.y, 0);
                }

                if (Input.GetMouseButton(0))
                {
                    endPos = new Vector3(Global.main.MousePosition.x, Global.main.MousePosition.y, 0);

                    float width = Mathf.Abs(startPos.x - endPos.x);
                    float height = Mathf.Abs(startPos.y - endPos.y);

                    square.transform.localScale = new Vector3(width, height, 1);
                    square.transform.position = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                placed = true;

                GetComponent<SpriteRenderer>().sortingOrder = -5;
            }
        }
    }

    //Item Stuff
    public interface Item
    {
        void OnPickup(Mario characterTouched);
        void LuigiPickup(Luigi l);

        IEnumerator OnStart(Vector2 startPosition);
    }

    public class FireBall : MonoBehaviour 
    {
        public Rigidbody2D rb;
        public CircleCollider2D col;

        public int bounceAmount;

        public GameObject characterThatThrew;

        Transform light;

        public void Start()
        {
            ModAPI.CreateLight(transform, Color.red, 0.5f, 1f);
        }

        public void FixedUpdate()
        {
            Physics2D.IgnoreCollision(col, characterThatThrew.GetComponent<CapsuleCollider2D>());

            rb.gravityScale = 2;

            if(bounceAmount > 2)
            {
                GameObject.Destroy(gameObject);

            }
        }

        void Update()
        {
            if (rb.velocity == Vector2.zero)
            {
                GameObject.Destroy(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (!coll.transform.GetComponent<BasicEnemy>())
            {
                rb.AddForce(Vector2.up * 110, ForceMode2D.Impulse);
                bounceAmount++;
            }

            if (coll.gameObject.layer == 9 && !coll.transform.GetComponent<BasicEnemy>() && !coll.transform.GetComponent<Mario>() && !coll.transform.GetComponent<Luigi>() && !coll.transform.GetComponent<FireBall>() && !coll.transform.GetComponent<Bowser>())
            {
                coll.transform.GetComponent<PhysicalBehaviour>().Ignite();

                GameObject fx = Instantiate(ModAPI.FindSpawnable("FireFx").Prefab);
                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("FireFx"), fx);

                fx.transform.position = transform.position;

                GameObject.Destroy(gameObject);
            }

            if(coll.transform.GetComponent<BasicEnemy>())
            {
                if(coll.transform.GetComponent<Goomba>()) { coll.transform.GetComponent<BasicEnemy>().Hurt(0); }
                if(coll.transform.GetComponent<Koopa>()) { coll.transform.GetComponent<BasicEnemy>().Death(); }

                GameObject fx = Instantiate(ModAPI.FindSpawnable("FireFx").Prefab);
                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("FireFx"), fx);

                fx.transform.position = transform.position;

                GameObject.Destroy(gameObject);
            }

            if (coll.transform.GetComponent<Spiny.Spiny>())
            {
                coll.transform.GetComponent<Spiny.Spiny>().Hurt();
            }
        }
    }

    public class Fx : MonoBehaviour
    {
        float cooldown = 0.125f;

        void Update()
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public class Bowser : MonoBehaviour
    {
        float stateCooldown = 2;
        float curCooldown;

        float fireCooldown;
        float animateTime;
        float fireCur;

        bool canMoveRight = false;
        bool canMoveLeft = true;

        public Sprite bows1, bows2;

        int stateToRun;
        int whereInHead;
        int animKey;

        int health = 10;

        public Rigidbody2D rb;

        public void Update()
        {
            curCooldown -= Time.deltaTime;
            fireCur -= Time.deltaTime;
            if (curCooldown <= 0)
            {
                ReRoll();

                switch (stateToRun) 
                {
                    case 0:
                        curCooldown = 2;
                        break;
                    case 1:
                        if(!canMoveRight) { ReRoll(); return; }
                        StartCoroutine(MoveRight());
                        break;
                    case 2:
                        if (!canMoveLeft) { ReRoll(); return; }
                        StartCoroutine(MoveLeft());
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Fireball();
                        break;
                }
            }

            Animate();

            if (health <= 0) StartCoroutine(Death());
        }

        void Animate()
        {
            if (animKey > 1)
            {
                animKey = 0;
            }

            animateTime -= Time.deltaTime;

            if (animateTime <= 0)
            {
                animateTime = 0.5f;
                animKey++;
            }

            switch (animKey)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = bows1;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = bows2;
                    break;
            }
        }

        void ReRoll()
        {
            stateToRun = UnityEngine.Random.Range(0, 5);
        }

        IEnumerator MoveRight()
        {

            if(!canMoveRight) { yield return null; }

            whereInHead++;
            canMoveRight = false;
            canMoveLeft = true;
            

            while (curCooldown > 0 && stateToRun == 1)
            {
                yield return new WaitForFixedUpdate();

                rb.velocity = new Vector2(0.5f, rb.velocity.y);
            }

            curCooldown = 2f;
            yield return null;
        }

        IEnumerator MoveLeft()
        {

            whereInHead--;
            canMoveRight = true;
            canMoveLeft = false;
            curCooldown = 0.5f;

            while (curCooldown > 0 && stateToRun == 2)
            {
                yield return new WaitForFixedUpdate();

                rb.velocity = new Vector2(-0.5f, rb.velocity.y);
            }

            curCooldown = 2f;
            yield return null;
        }
        void Jump()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddForce(Vector2.up * 150, ForceMode2D.Impulse);
            curCooldown = 3;
        }

        void Fireball()
        {
            curCooldown = 3;
            GameObject fireball = Instantiate(ModAPI.FindSpawnable("Metal Cube").Prefab);
            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("Bowser Fire"), fireball);
            fireball.transform.position = transform.position + transform.right * -2;
            Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        IEnumerator Death()
        {
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(15);
            GameObject.Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.transform.GetComponent<Mario>() || coll.transform.GetComponent<Luigi>())
            {
                coll.transform.GetComponent<Character>().Hurt();
            }
            if (coll.transform.GetComponent<FireBall>())
            {
                health--;
                GameObject.Destroy(coll.gameObject);
            }
        }
    }

    public class BFire : MonoBehaviour
    {
        public Rigidbody2D rb;

        public int moveDir = -1;

        void Update()
        {
            GetComponent<Collider2D>().isTrigger = true;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(moveDir * 4, 0);
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.transform.GetComponent<Mario>() || coll.transform.GetComponent<Luigi>())
            {
                coll.transform.GetComponent<Character>().Hurt();
            }
        }
    }

    public class Pipe : MonoBehaviour
    {
        Vector2 curSpot, realSpot, offset;

        bool isPlaced = false;
        bool dragGrabbed = false;
        public bool isSecondPhase = false;
        public bool finishedPhase = false;
        public bool twoParter;

        float dragCooldown = 0.3f;
        float cool;

        public GameObject firstPipe;
        public GameObject secondPipe;
        GameObject lastExtension;
        List<GameObject> allExtensions = new List<GameObject>();

        public GameObject draggyThing;

        public int pipeHeight;


        public void Update()
        {
            cool -= Time.deltaTime;

            if (!isPlaced)
            {
                curSpot =
                    Vector2.Lerp(transform.position, new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                    Mathf.RoundToInt(Global.main.MousePosition.y)), Time.deltaTime * 25);

                realSpot = new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                    Mathf.RoundToInt(Global.main.MousePosition.y));

                transform.position = curSpot;

                if (!isSecondPhase)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StageOne();
                    }
                }
                else
                {
                    ModAPI.Draw.Line(firstPipe.transform.position + new Vector3(0, -1f, 0), transform.position + new Vector3(0, -1f, 0));
                    if (Input.GetMouseButtonDown(0) && twoParter)
                    {
                        StageTwo();
                    }
                }
            }
            else
            {
                if (Global.main.ShowLimbStatus)
                {
                    draggyThing.transform.position = transform.position + new Vector3(0, 1, 0);
                    draggyThing.SetActive(true);
                    HandleDragging();
                }
                else
                {
                    draggyThing.SetActive(false);
                }
            }
        }

        private void StageOne()
        {
            transform.position = realSpot;

            if (twoParter)
            {
                secondPipe = Instantiate(ModAPI.FindSpawnable("Metal Cube").Prefab);
                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("Two-way Pipe"), secondPipe);

                secondPipe.GetComponent<Pipe>().isSecondPhase = true;
                secondPipe.GetComponent<Pipe>().firstPipe = gameObject;

            }

            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<BoxCollider2D>().enabled = true;

            isPlaced = true;
        }

        private void StageTwo()
        {
            transform.position = realSpot;

            firstPipe.GetComponent<Pipe>().finishedPhase = true;
            GetComponent<Pipe>().finishedPhase = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            isPlaced = true;
        }

        private void HandleDragging()
        {
            if (Vector2.Distance(draggyThing.transform.position, Global.main.MousePosition) <= 0.55f)
            {
                draggyThing.transform.localScale =
                    Vector3.Lerp(draggyThing.transform.localScale, new Vector3(1.5f, 1.5f), Time.deltaTime * 15);

                if (Input.GetMouseButton(0))
                {
                    dragGrabbed = true;
                }
            }
            else
            {
                draggyThing.transform.localScale =
                    Vector3.Lerp(draggyThing.transform.localScale, new Vector3(1f, 1f), Time.deltaTime * 15);
            }
            if (Input.GetMouseButtonUp(0))
            {
                dragGrabbed = false;
            }
            if (dragGrabbed)
            {
                if (Global.main.MousePosition.y >= draggyThing.transform.position.y + 0.5f)
                {
                    IncreaseHeight();
                }
                if (Global.main.MousePosition.y <= draggyThing.transform.position.y - 0.5f && pipeHeight > 0)
                {
                    DecreaseHeight();
                }
            }
        }

        private void IncreaseHeight()
        {
            transform.position += new Vector3(0, 1);

            GameObject extension = Instantiate(ModAPI.FindSpawnable("Metal Cube").Prefab);
            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("Pipe Bottom"), extension);

            extension.transform.position = transform.position - new Vector3(0, 2);
            lastExtension = extension;

            allExtensions.Add(extension);

            pipeHeight++;
        }

        private void DecreaseHeight()
        {
            transform.position += new Vector3(0, -1);

            allExtensions.Remove(lastExtension);
            GameObject.Destroy(lastExtension);

            if(allExtensions.Count > 0)
                lastExtension = allExtensions.ToArray()[allExtensions.Count-1];

            pipeHeight--;
        }
    }
    
    public class PointsFx : MonoBehaviour
    {
        float timer = 1;

        void Update()
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                GameObject.Destroy(gameObject);
            }

            transform.position += transform.up * 0.75f * Time.deltaTime;
        }
    }

    public class CoinFx : MonoBehaviour, Item
    {
        public AudioClip sound;

        float timer = 0.75f;
        float moveAmount = 4;

        public void LuigiPickup(Luigi l)
        {
            throw new NotImplementedException();
        }

        public void OnPickup(Mario characterTouched)
        {
            throw new NotImplementedException();
        }

        public IEnumerator OnStart(Vector2 startPosition)
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            GetComponent<PhysicalBehaviour>().PlayClipOnce(sound);
            GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            moveAmount -= Time.deltaTime * 3;

            if (timer <= 0)
            {
                GameObject powerUp = Instantiate(ModAPI.FindSpawnable("100").Prefab,
                new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

                CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("100"), powerUp);

                GameObject.Destroy(gameObject);
            }

            transform.position += transform.up * moveAmount * Time.deltaTime;
        }
    }

    public class Spring : MonoBehaviour
    {
        Vector2 curSpot, realSpot, offset;

        bool isPlaced = false;

        public enum CompressionStates
        {
            COMPRESSED,
            EXTENDED,
            IDLE
        }

        public struct SpringSprites
        {
            public Sprite idle;
            public Sprite compressed;
            public Sprite extended;
        }

        public SpringSprites sprites;
        CompressionStates state = CompressionStates.IDLE;

        void Update()
        {
            if (!isPlaced)
            {
                if (!Input.GetKey(KeyCode.LeftAlt))
                {
                    realSpot = new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                    Mathf.RoundToInt(Global.main.MousePosition.y)+0.5f);
                }
                else
                {
                    realSpot = Global.main.MousePosition;
                }

                curSpot = Vector2.Lerp(curSpot, realSpot, Time.deltaTime * 15);

                transform.position = curSpot;

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject.Destroy(gameObject);
                }

                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    curSpot = Global.main.MousePosition;
                }
            }
            else
            {
                transform.position = realSpot;
            }

            if (Input.GetKey(KeyCode.Q)|| Input.GetKey(KeyCode.E)) { PlaceBlock(); }
        }

        void PlaceBlock()
        {
            isPlaced = true;

            GetComponent<BoxCollider2D>().enabled = true;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (state != CompressionStates.IDLE)
            {
                return;
            }

            StartCoroutine(CollisionRoutine(coll, GetComponent<BoxCollider2D>()));
        }

        IEnumerator CollisionRoutine(Collision2D coll, BoxCollider2D bx)
        {
            coll.transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb);
            coll.transform.TryGetComponent<Mario>(out Mario mario);
            coll.transform.TryGetComponent<Luigi>(out Luigi luigi);

            #region Compress

            state = CompressionStates.COMPRESSED;
            GetComponent<SpriteRenderer>().sprite = sprites.compressed;

            coll.transform.position = new Vector3(coll.transform.position.x, coll.transform.position.y - 0.3f, 0);

            if (mario)
            {
                mario.currentAction = PlayerActions.OnSpring;
            }
            if (luigi)
            {
                luigi.currentAction = PlayerActions.OnSpring;
            }

            rb.velocity = new Vector2(0, rb.velocity.y);

            bx.size = new Vector2(0.5f, 0.25f);
            bx.offset = new Vector2(0, -0.3f);

            yield return new WaitForSeconds(0.85f);

            #endregion

            #region Extend

            rb.velocity = new Vector2(rb.velocity.x, 15);

            state = CompressionStates.EXTENDED;
            GetComponent<SpriteRenderer>().sprite = sprites.extended;

            bx.enabled = false;

            yield return new WaitForSeconds(0.2f);

            #endregion

            #region Regulate
            state = CompressionStates.IDLE;
            GetComponent<SpriteRenderer>().sprite = sprites.idle;

            if (mario)
            {
                mario.currentAction = PlayerActions.Idle;
            }
            if (luigi)
            {
                luigi.currentAction = PlayerActions.Idle;
            }

            bx.enabled = true;
            bx.size = new Vector2(0.5f, 0.5f);
            bx.offset = new Vector2(0, 0f);
            #endregion

        }
    }

    public class BreakableFloor : MonoBehaviour
    {
        Vector2 curSpot, realSpot, offset;

        bool isPlaced = false;

        void Update()
        {
            if (!isPlaced)
            {
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

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) { PlaceBlock(); }
        }

        void PlaceBlock()
        {
            isPlaced = true;

            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}