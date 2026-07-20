using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using CharactersScript;

namespace CharacterAnimator
{
    public struct FrameInformation
    {
        public float _length;
        public Sprite _sprite;
    }

    public class CharacterAnimation
    {
        public float speed { get; private set; }
        public string name { get; private set; }

        public List<FrameInformation> frames { get; private set; } = new List<FrameInformation>();

        public bool canBeOverriden { get; private set; } = true;

        public bool loopAnimation { get; private set; } = false;

        public bool loopCertainAmount { get; private set; } = false;

        public int maxLoopIterations { get; private set; }

        public void AddFrame(FrameInformation info)
        {
            frames.Add(info);
        }
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetCanBeOverriden(bool value)
        {
            canBeOverriden = value;
        }
        public void SetLoopAnimation(bool does_loop)
        {
            loopAnimation = does_loop;
        }
        public void SetLoopAmountOfTimes(int iterations)
        {
            loopAnimation = true;
            loopCertainAmount = true;

            maxLoopIterations = iterations;
        }

        public void RemoveFrame(FrameInformation info)
        {
            frames.Remove(info);
        }

        public void ClearFrames()
        {
            frames.Clear();
        }
    }

    public class CharacterAnimatorInstance : MonoBehaviour
    {
        private Character accessingCharacter;

        public Dictionary<string, CharacterAnimation> AllAnimations = new Dictionary<string, CharacterAnimation>();

        private CharacterAnimation currentAnimation;

        private CharacterAnimation queuedAnimation;

        public bool stopAllAnimations { get; private set; } = false;

        public int currentAnimationFrame { get; private set; } = 0;
        private float currentAnimationFrameLength;

        private float currentTimer;

        private float animatorSpeed = 1;

        private int loopIterationCount = 0;

        public UnityAction<CharacterAnimation> onAnimationFinished;
        public UnityAction<CharacterAnimation> onAnimationChanged;
        public UnityAction<int, FrameInformation> onFrameStep;

        private void Update()
        {
            AnimationUpdate();
        }

        public void SetAnimatorSpeed(float speed)
        {
            animatorSpeed = speed;
        }

        public void SetCharacter(Character character)
        {
            accessingCharacter = character;
        }

        public void CreateNewAnimation(string animationName, out CharacterAnimation madeAnimation)
        {
            CharacterAnimation newAnim = new CharacterAnimation();

            newAnim.SetName(animationName);

            AllAnimations.Add(animationName, newAnim);

            madeAnimation = newAnim;
        }

        // create new frame for a specific animation held in this animator instance.
        public void AddFrameToAnimation(string animName, FrameInformation frame)
        {
            if (AllAnimations[animName] == null) { return; }

            AllAnimations[animName].AddFrame(frame);
        }


        public void StartAnimation(string animationName)
        {
            AllAnimations.TryGetValue(animationName, out CharacterAnimation anim);

            if (anim.name == null) 
            { 
                ModAPI.Notify("Animation not found, cant play..."); 
                return; 
            }

            if (anim == currentAnimation) 
            { 
                return; 
            }

            if (currentAnimation != null && currentAnimation.canBeOverriden == false)
            {
                queuedAnimation = anim;

                Debug.Log("Queued animation: " + anim);

                return;
            }

            currentAnimationFrame = 0;

            loopIterationCount = 0;

            currentAnimation = anim;

            onAnimationChanged?.Invoke(currentAnimation);
        }

        private void AnimationUpdate()
        {
            currentTimer -= Time.deltaTime * animatorSpeed;

            if (currentTimer <= 0)
            {
                //iterate current frame in animation.
                currentAnimationFrame += 1;

                //if finished with animation, handle.
                if (currentAnimationFrame > currentAnimation.frames.Count - 1)
                {
                    //if the animation is over and the animation doesnt loop, finish.
                    if(!currentAnimation.loopAnimation) 
                    { FinishedAnimation(); return; }

                    //if the animation loops but has a finite amount and has exceded cap,
                    //finish.
                    if(loopIterationCount > currentAnimation.maxLoopIterations && currentAnimation.loopCertainAmount) 
                    { FinishedAnimation(); return; }

                    //if all other restrictions are not met, reset animation frame.
                    currentAnimationFrame = 0;

                    loopIterationCount += 1;
                }

                FrameInformation info = currentAnimation.frames[currentAnimationFrame];

                accessingCharacter.SetRendererSprite(info._sprite);
                currentTimer = info._length;

                accessingCharacter.ReloadOutline();
            }
        }

        //handle when animation is done.
        private void FinishedAnimation()
        {
            loopIterationCount = 0;

            onAnimationFinished?.Invoke(currentAnimation);

            currentAnimation = queuedAnimation;

            queuedAnimation = null;
        }
    }
}
