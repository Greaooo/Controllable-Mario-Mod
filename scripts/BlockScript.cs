using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Mod;
using PowerUps;

namespace BlockStuff
{
    public class BlockMain : MonoBehaviour
    {
        public static void CreateBlocks()
        {
            #region Blocks

            #region Overworld blocks

            //floor block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Floor Block",
                    DescriptionOverride = "Some type of... rocky floor?",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/ow/Floor.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/ow/Floor.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //empty block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Empty Block",
                    DescriptionOverride = "A hollow metal block.",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/ow/emptyBlock.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/ow/emptyBlock.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //metal block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Metal Block",
                    DescriptionOverride = "NOT a hollow metal block.",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/ow/Metal.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/ow/Metal.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //rock block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Rock Block",
                    DescriptionOverride = "what do you think it is",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/ow/Rock.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/ow/Rock.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //brick block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Brick Block",
                    DescriptionOverride = "Very brittle brick block.",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/ow/Brick.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<BreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/ow/Brick.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<BreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            #endregion

            #region Underworld blocks

            //floor block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Underworld Floor Block",
                    DescriptionOverride = "Some type of... rocky floor? blue?",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/uw/Floor.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/uw/Floor.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //metal block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Underworld Metal Block",
                    DescriptionOverride = "blue",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/uw/Metal.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/uw/Metal.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //rock block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Underworld Rock Block",
                    DescriptionOverride = "what do you think it is blue edition",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/uw/Rock.png"),
                    NameToOrderByOverride = "0",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/uw/Rock.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //brick block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Undworld Brick Block",
                    DescriptionOverride = "Very brittle brick block... but blue!!11!!1!",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/uw/Brick.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<BreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/uw/Brick.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<BreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            #endregion

            #region Underworld blocks

            //floor block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Dungeon Floor Block",
                    DescriptionOverride = "Some type of... rocky floor? blue?",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/dg/Floor.png"),
                    NameToOrderByOverride = "2",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/dg/Floor.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //metal block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Dungeon Metal Block",
                    DescriptionOverride = "description #12350200104010424891023002",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/dg/Metal.png"),
                    NameToOrderByOverride = "2",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/dg/Metal.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //rock block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Dungeon Rock Block",
                    DescriptionOverride = "do i really have to write every single description",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/dg/Rock.png"),
                    NameToOrderByOverride = "2",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<UnbreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/dg/Rock.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<UnbreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            //brick block
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Dungeon Brick Block",
                    DescriptionOverride = "SCARY BRICK BLOCK AHHHHH",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/dg/Brick.png"),
                    NameToOrderByOverride = "2",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<BreakableBlock>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Rock");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/dg/Brick.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        Instance.GetComponent<BreakableBlock>().SetBlockInfo(new BlockInfo()
                        {
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                            blockType = BlockType.REG
                        });
                    }
                });

            #endregion

            #endregion

            #region Item Blocks
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Power Up (Holding a Mushroom)",
                    DescriptionOverride = "Power Up block",
                    CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/PowerUpMushroom.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<ItemBlock>();

                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");
                        Instance.GetComponent<ItemBlock>().SetItem("Mushroom");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Blocks/powerUp.png");

                        Instance.GetComponent<BlockClass>().SetBlockInfo(new BlockInfo()
                        {
                            block1 = ModAPI.LoadSprite("Textures/Blocks/powerUp.png"),
                            block2 = ModAPI.LoadSprite("Textures/Blocks/powerUp1.png"),
                            block3 = ModAPI.LoadSprite("Textures/Blocks/powerUp2.png"),
                            empty = ModAPI.LoadSprite("assets/blocks/ow/emptyBlock.png"),
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                        });

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;
                    }
                });

            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Power Up (Holding a Fire Flower)",
                    DescriptionOverride = "Power Up block with a fire flower inside.",
                    CategoryOverride = ModAPI.FindCategory("Power Up Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/PowerUpFire.png"),
                    NameToOrderByOverride = "1",
                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Instance.AddComponent<ItemBlock>();

                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow Metal");
                        Instance.GetComponent<ItemBlock>().SetItem("Mushroom");

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.45f, 0.45f);
                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Textures/Blocks/powerUp.png");

                        Instance.GetComponent<BlockClass>().SetBlockInfo(new BlockInfo()
                        {
                            block1 = ModAPI.LoadSprite("Textures/Blocks/powerUp.png"),
                            block2 = ModAPI.LoadSprite("Textures/Blocks/powerUp1.png"),
                            block3 = ModAPI.LoadSprite("Textures/Blocks/powerUp2.png"),
                            empty = ModAPI.LoadSprite("assets/blocks/ow/emptyBlock.png"),
                            outline = ModAPI.LoadSprite("assets/blocks/outline.png"),
                        });

                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;
                    }
                });
            #endregion

            #region Background Blocks



            #endregion

            //Spring tile
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                    NameOverride = "Spring",
                    DescriptionOverride = "Building block for all mario levels",
                    CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/Spring/spring.png"),
                    NameToOrderByOverride = "0",

                    AfterSpawn = (Instance) =>
                    {
                        Instance.transform.position = Global.main.MousePosition;
                        Spring spring = Instance.AddComponent<Spring>();
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Brick");

                        spring.sprites = new Spring.SpringSprites()
                        {
                            idle = ModAPI.LoadSprite("assets/blocks/Spring/spring.png"),
                            compressed = ModAPI.LoadSprite("assets/blocks/Spring/spring1.png"),
                            extended = ModAPI.LoadSprite("assets/blocks/Spring/spring2.png")
                        };

                        Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.45f);

                        Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/Spring/spring.png");
                        Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                        Instance.GetComponent<BoxCollider2D>().enabled = false;

                        UnityEngine.Object.Destroy(Instance.GetComponent<Rigidbody2D>());
                    }
                });

            #region Pipe
            //pipe
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Two-way Pipe",
                DescriptionOverride = "Pipe",
                CategoryOverride = ModAPI.FindCategory("Blocks Category"),
                ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/PipeIcon.png"),
                NameToOrderByOverride = "55",

                AfterSpawn = (Instance) =>
                {
                    Instance.transform.position = Global.main.MousePosition;

                    Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(1, 0.8f);
                    Instance.transform.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.2f);
                    Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);

                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/pipe.png");
                    Instance.GetComponent<Rigidbody2D>().freezeRotation = true;
                    Instance.GetComponent<BoxCollider2D>().enabled = false;
                    Instance.GetComponent<Rigidbody2D>().gravityScale = 0;
                    Instance.GetComponent<Rigidbody2D>().sharedMaterial = new PhysicsMaterial2D()
                    {
                        bounciness = 0,
                        friction = 0
                    };

                    Instance.AddComponent<Pipe>();
                    Instance.GetComponent<Pipe>().twoParter = true;
                    Instance.GetComponent<Pipe>().draggyThing = new GameObject();
                    Instance.GetComponent<Pipe>().draggyThing.transform.parent = Instance.transform;
                    Instance.GetComponent<Pipe>().draggyThing.AddComponent<SpriteRenderer>();
                    Instance.GetComponent<Pipe>().draggyThing.SetActive(false);
                    Instance.GetComponent<Pipe>().draggyThing.
                        GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/pipeDragging.png");

                }
            });

            //pipeBottom
            ModAPI.Register(new Modification()
            {
                OriginalItem = ModAPI.FindSpawnable("Metal Cube"),
                NameOverride = "Pipe Bottom",
                DescriptionOverride = "dont spawn this.",
                ThumbnailOverride = ModAPI.LoadSprite("assets/blocks/extension.png"),
                NameToOrderByOverride = "55",

                AfterSpawn = (Instance) =>
                {
                    Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("assets/blocks/extension.png");
                    Instance.GetComponent<Rigidbody2D>().isKinematic = true;
                    Instance.transform.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                    Instance.transform.localScale = new Vector3(2.2f, 2.2f, 2);
                }
            });

            #endregion
        }
    }

    public class BlockPositions : MonoBehaviour
    {
        public static BlockPositions Main;

        private Dictionary<Vector2, bool> BlockDictionary = new Dictionary<Vector2, bool>();

        void Update()
        {
            Main = this;
        }

        public void AddBlock(Vector2 position)
        {
            BlockDictionary.TryGetValue(position, out bool containsBlock);

            if (containsBlock) { ModAPI.Notify("Couldn't add block, spot already taken."); return; }

            BlockDictionary.Add(position, true);
        }

        public void RemoveBlock(Vector2 position)
        {
            BlockDictionary.TryGetValue(position, out bool containsBlock);

            if (!containsBlock) { ModAPI.Notify("Can't remove missing block."); return; }

            BlockDictionary.Remove(position);
        }

        public bool GetBlock(Vector2 position)
        {
            BlockDictionary.TryGetValue(position, out bool containsBlock);

            return containsBlock;
        }
    }

    public enum BlockType
    {
        REG,
        BREAKABLE,
        OPENABLE
    }

    public struct BlockInfo
    {
        public Sprite block1, block2, block3, empty, outline;

        public BlockType blockType;
    }

    public class BlockClass : MonoBehaviour
    {
        protected BlockInfo information;

        protected UnityAction onBlockPlaced;
        protected UnityAction<bool> onBlockHit;

        private bool canBounce = true;
        public Vector2 RoundedMousePosition { get; private set; }
        public Vector2 PlacedPosition { get; private set; }
        public Vector2 FirstBlockPosition { get; private set; }

        protected bool blockPlaced;

        protected GameObject block;

        public virtual void SetBlockInfo(BlockInfo newInfo)
        {
            information = newInfo;
        }

        public virtual void NotPlaced()
        {
            if (GameObject.Find("Block Dictionary") == null)
            {
                GameObject blockpos = new GameObject("Block Dictionary");
                blockpos.AddComponent<BlockPositions>();

                ModAPI.Notify("Created block dictionary");
            }

            RoundedMousePosition = new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
            Mathf.RoundToInt(Global.main.MousePosition.y));

            PlacedPosition = RoundedMousePosition;

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                PlacedPosition = Global.main.MousePosition;
            }

            block.transform.position = PlacedPosition;

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                Place();
            }

            if (Input.GetMouseButtonDown(0))
            {
                DestroyBlock();
            }
        }

        public virtual void Place()
        {
            PlacedPosition = RoundedMousePosition;

            blockPlaced = true;

            block.transform.position = RoundedMousePosition;

            block.GetComponent<BoxCollider2D>().enabled = true;
            block.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            BlockPositions.Main.AddBlock(PlacedPosition);

            onBlockPlaced?.Invoke();
        }

        public virtual void BlockHit(bool adult)
        {
            onBlockHit?.Invoke(adult);
            ModAPI.Notify("block hit");
        }

        public void DestroyBlock()
        {
            block.GetComponent<PhysicalBehaviour>().Disintegrate();
        }

        protected void BumpUpwards()
        {
            if(canBounce == false) { return; }

            float realY = transform.position.y;

            transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            StartCoroutine(BumpAnimation(realY));
        }

        private IEnumerator BumpAnimation(float y)
        {
            

            while (transform.position.y > y)
            {
                yield return new WaitForEndOfFrame();

                canBounce = false;

                transform.position = new Vector3(block.transform.position.x, block.transform.position.y - Time.deltaTime * 5);
                ModAPI.Notify(y - block.transform.position.y);
            }

            transform.position = new Vector2(transform.position.x, y);

            Invoke(nameof(AllowBumping), .5f);
        }

        private void AllowBumping()
        {
            canBounce = true;
        }
    }

    //class for unbreakable blocks that can only be placed.
    public class UnbreakableBlock : BlockClass
    {
        void Start()
        {
            block = this.gameObject;
        }

        private void Update()
        {
            if (!blockPlaced)
            {
                NotPlaced();
            }
        }
    }

    public class BreakableBlock : BlockClass
    {
        void Start()
        {
            block = this.gameObject;
            onBlockHit += Break;
        }

        private void Update()
        {
            if (!blockPlaced)
            {
                NotPlaced();
            }
        }

        void Break(bool adult)
        {
            if (!adult) { BumpUpwards(); return; }

            Destroy(block);
        }
    }

    public class ItemBlock : BlockClass
    {
        private string itemName = null;

        bool spitOut = false;

        private bool placed = false;

        void Start()
        {
            block = this.gameObject;
            onBlockHit += SpitOutPowerUp;
        }

        private void Update()
        {
            if (!blockPlaced)
            {
                NotPlaced();
            }
        }

        public void SetItem(string newName)
        {
            itemName = newName;
        }

        void SpitOutPowerUp(bool adult)
        {
            if (spitOut) { return; }

            spitOut = true;

            BumpUpwards();
            CreateItem();
        }

        void CreateItem()
        {
            ModAPI.Notify("spit out item");

            GameObject powerUp = Instantiate(ModAPI.FindSpawnable(itemName).Prefab,
                new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable(itemName), powerUp);

            GetComponent<SpriteRenderer>().sprite = information.empty;

            StartCoroutine(powerUp.GetComponent<Item>().OnStart(
                new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z)));
        }
    }
}