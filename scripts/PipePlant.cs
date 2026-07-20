using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace PiranhaPlant
{
    class PipePlant : MonoBehaviour
    {
        public Sprite[] TwoSprites;

        Vector2 curSpot, realSpot, offset;

        bool isPlaced = false;

        int keyFrame;

        float frameTime;

        public SpriteRenderer rend;

        void Update()
        {
            if (!isPlaced)
            {
                HandlePlacing();
            }
            else
            {
                transform.position = realSpot;
            }

            Animate();
        }

        void HandlePlacing()
        {
            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                realSpot = new Vector2(Mathf.RoundToInt(Global.main.MousePosition.x),
                Mathf.RoundToInt(Global.main.MousePosition.y) + 0.5f);
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

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) { PlaceBlock(); }
        }

        void PlaceBlock()
        {
            isPlaced = true;

            GetComponent<BoxCollider2D>().enabled = true;
        }

        void Animate()
        {
            frameTime -= Time.deltaTime;

            if (frameTime <= 0)
            {
                keyFrame++;
                frameTime = 1;
            }

            if (keyFrame > 1)
            {
                keyFrame = 0;
            }

            rend.sprite = TwoSprites[keyFrame];
            GetComponent<PhysicalBehaviour>().RefreshOutline();
        }
    }
}
