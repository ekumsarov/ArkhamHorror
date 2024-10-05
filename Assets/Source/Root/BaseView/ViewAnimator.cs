using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using Sirenix.Utilities;

namespace EVI
{
    [RequireComponent(typeof(Animator))]
    public class ViewAnimator : MonoBehaviour
    {
        [SerializeField, OnInspectorInit("UpdateComponent")] private Animator _animator;
        protected List<string> _avaliableAnimation;
        private string _currentStage = string.Empty;

        public bool IsAvaliable
        {
            get
            {
                if (_animator == null)
                    Debug.LogError("No AnimatorController on object : " + gameObject.name);

                return _animator != null && _animator.runtimeAnimatorController != null;
            }
        }

        private bool HasAnimation(string animName)
        {
            return _avaliableAnimation.Any(anim => string.Equals(anim, animName));
        }

        public void Initialize()
        {
            if (IsAvaliable == false)
                return;

            _avaliableAnimation = new List<string>();
            foreach(var anim in _animator.runtimeAnimatorController.animationClips)
            {
                _avaliableAnimation.Add(anim.name);
            }
        }

        public void ChangeState(string animName)
        {
            if (CheckAnimationToStart(animName) == false)
                return;

            _currentStage = animName;
            PlayAnimation();
        }

        public void ChangeState(string animName, Action callback)
        {
            if (CheckAnimationToStart(animName) == false)
            {
                callback?.Invoke();
                return;
            }
                
            
            _currentStage = animName;
            PlayAnimation();
            StartCoroutine(CheckEnded(animName, callback));
        }

        private bool _breakForce = false;
        public void ForcePlayAnimation(string animName, Action callback)
        {
            if (HasAnimation(animName) == false)
            {
                Debug.LogError("No animation: " + animName);
                callback?.Invoke();
                return;
            }

            if(string.IsNullOrEmpty(_currentStage))
            {
                _currentStage = _avaliableAnimation.First(anim => anim.Contains("Idle"));
            }
            
            _breakForce = false;
            StartCoroutine(ForcePlayCheckEnded(animName, callback, _currentStage));
        }

        public void BreakForceAnim()
        {
            _breakForce = true;
            StopAllCoroutines();
        }

        private IEnumerator ForcePlayCheckEnded(string animToPlay, Action OnComplete, string previousAnim)
        {
            float timer = _animator.runtimeAnimatorController.animationClips.First(anim => anim.name == animToPlay).length;
            _animator.Play(animToPlay);

            while (_animator.GetCurrentAnimatorStateInfo(0).IsName(animToPlay) == false)
                yield return null;

            while (timer > 0)
            {
                if(_breakForce == true)
                {
                    if(string.IsNullOrEmpty(previousAnim) == false)
                    {
                        _animator.Play(previousAnim);
                        yield break;
                    }
                }

                timer -= Time.deltaTime;
                yield return null;
            }
                

            if(string.IsNullOrEmpty(previousAnim) == false)
            {
                _animator.Play(previousAnim);
                while (_animator.GetCurrentAnimatorStateInfo(0).IsName(previousAnim) == false)
                    yield return null;
            }

            OnComplete?.Invoke();
        }

        private IEnumerator CheckEnded(string CurrentAnim, Action Oncomplete, string previousAnimation = null)
        {
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim) == false)
                yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                yield return null;

            if(string.IsNullOrEmpty(previousAnimation) == false)
            {
                ChangeState(previousAnimation);
            }
            
            if (Oncomplete != null)
                Oncomplete();
        }

        private bool CheckAnimationToStart(string animName)
        {
            if (IsAvaliable == false)
                return false;

            if (HasAnimation(animName) == false)
            {
                Debug.LogError("No animation: " + animName);
                return false;
            }

            if (animName == _currentStage)
                return false;

            return true;
        }

        private void PlayAnimation()
        {
            _animator.PlayInFixedTime(_currentStage);
        }

#if UNITY_EDITOR
        [Button("Update Component")]
        public void UpdateComponent()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                    _animator = gameObject.AddComponent<Animator>();
            }
                
        }
#endif
    }
}
