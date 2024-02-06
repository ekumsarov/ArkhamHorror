﻿namespace UniTween.Data
{
    using DG.Tweening;
    using System.Collections.Generic;
    using UniTween.Core;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Tween Data/Canvas/Canvas Group")]
    public class CanvasGroupTween : TweenData
    {

        [Space(15)]
        public CanvasGroupCommand command;
        public float value;

        /// <summary>
        /// Creates and returns a Tween for all components contained inside the UniTweenTarget.
        /// The Tween is configured based on the attribute values of this TweenData file.
        /// </summary>
        /// <param name="uniTweenTarget">Wrapper that contains a List of the component that this TweenData can tween.</param>
        /// <returns></returns>
        public override Tween GetTween(UniTweenObject.UniTweenTarget uniTweenTarget)
        {
            List<CanvasGroup> groups = (List<CanvasGroup>)GetComponent(uniTweenTarget);
            Sequence tweens = DOTween.Sequence();
            if (customEase)
            {
                foreach (var t in groups)
                {
                    if (t != null)
                        tweens.Join(GetTween(t).SetEase(curve));
                }
            }
            else
            {
                foreach (var t in groups)
                {
                    if (t != null)
                        tweens.Join(GetTween(t).SetEase(ease));
                }
            }
            return tweens;
        }

        /// <summary>
        /// Creates and returns a Tween for the informed component.
        /// The Tween is configured based on the attribute values of this TweenData file.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Tween GetTween(CanvasGroup canvasGroup)
        {
            switch (command)
            {
                case CanvasGroupCommand.Fade:
                    return canvasGroup.DOFade(value, duration);
            }
            return null;
        }

        public enum CanvasGroupCommand
        {
            Fade
        }
    }
}