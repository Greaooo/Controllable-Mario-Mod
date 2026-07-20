using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    class CharacterCreatorLibrary : MonoBehaviour
    {
        public static RaycastHit2D CanJump(float range, Vector2 position)
        {
            return Physics2D.Raycast(position, Vector2.down, range);
        }

        public static Sprite[] GetAnimationSprites(string filePath, int spriteAmount)
        {
            Sprite[] sprites = new Sprite[spriteAmount];

            for (int i = 0; i < spriteAmount; i++)
            {
                sprites[i] = ModAPI.LoadSprite(filePath + i + ".png");
            }

            return sprites;
        }
    }
}
