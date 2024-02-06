﻿#if UNITY_POST_PROCESSING_STACK_V2
namespace UniTween.Data
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UniTween.Core;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [CreateAssetMenu(menuName = "Tween Data/Post-processing Stack v2/Auto Exposure")]
    public class PPAutoExposureTween : TweenData
    {
        [Space(15)]
        [Tooltip("If true, the post-processing effect you want to tween will be automatically activated.")]
        public bool automaticOverride = true;
        [Space]
        public AutoExposureCommand command;

        [HideIf("ShowFiltering")]
        public float to;
        [ShowIf("ShowFiltering")]
        public Vector2 filtering;

        /// <summary>
        /// Creates and returns a Tween for all components contained inside the UniTweenTarget.
        /// The Tween is configured based on the attribute values of this TweenData file.
        /// </summary>
        /// <param name="uniTweenTarget">Wrapper that contains a List of the component that this TweenData can tween.</param>
        /// <returns></returns>
        public override Tween GetTween(UniTweenObject.UniTweenTarget uniTweenTarget)
        {
            List<PostProcessVolume> volumes = (List<PostProcessVolume>)GetComponent(uniTweenTarget);
            Sequence tweens = DOTween.Sequence();
            if (customEase)
            {
                foreach (var t in volumes)
                {
                    if (t != null)
                        tweens.Join(GetTween(t).SetEase(curve));
                }
            }
            else
            {
                foreach (var t in volumes)
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
        public Tween GetTween(PostProcessVolume volume)
        {
            var setting = volume.profile.GetSetting<AutoExposure>();

            if (setting != null)
            {
                setting.active = automaticOverride;
                switch (command)
                {
                    case AutoExposureCommand.Filtering:
                        setting.filtering.overrideState = automaticOverride;
                        return DOTween.To(() => setting.filtering.value, x => setting.filtering.value = x, filtering, duration);
                    case AutoExposureCommand.MinimumEV:
                        setting.minLuminance.overrideState = automaticOverride;
                        return DOTween.To(() => setting.minLuminance.value, x => setting.minLuminance.value = x, to, duration);
                    case AutoExposureCommand.MaximumEV:
                        setting.maxLuminance.overrideState = automaticOverride;
                        return DOTween.To(() => setting.maxLuminance.value, x => setting.maxLuminance.value = x, to, duration);
                    case AutoExposureCommand.ExposureCompensation:
                        setting.keyValue.overrideState = automaticOverride;
                        return DOTween.To(() => setting.keyValue.value, x => setting.keyValue.value = x, to, duration);
                }
            }
            else
            {
                Debug.Log("UniTween could not find an Auto Exposure to tween. Be sure to add it on your Post Process Volume component");
            }

            return null;
        }

        public bool ShowFiltering()
        {
            return command == AutoExposureCommand.Filtering;
        }

        public enum AutoExposureCommand
        {
            Filtering,
            MinimumEV,
            MaximumEV,
            ExposureCompensation,
        }
    }
}
#endif