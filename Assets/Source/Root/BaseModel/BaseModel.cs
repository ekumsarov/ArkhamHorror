using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EVI
{
    public class BaseModel : Bindable
    {
        [BindableProperty(nameof(Position))]
        public Vector2 Position { get; private set; }
        [BindableProperty(nameof(Rotation))]
        public float Rotation { get; private set; }
        [BindableProperty(nameof(CurrentAnimation))]
        public string CurrentAnimation { get; private set; }
        [BindableProperty(nameof(Active))]
        public bool Active { get; private set; }
        [BindableProperty(nameof(Scale))]
        public Vector3 Scale { get; private set; }
        [BindableProperty(nameof(CurrentDirection))]
        public Direction CurrentDirection { get; private set; }

        public void ActiveUnit(bool val)
        {
            Active = val;
            InvokeChange(nameof(Active), Active);
        }
        

        public void MoveTo(Vector2 position)
        {
            Position = position;
            InvokeChange(nameof(Position), Position);
        }

        public void PlayAnimation(string animation)
        {
            if (CurrentAnimation == animation)
                return;

            CurrentAnimation = animation;
            InvokeChange(nameof(CurrentAnimation), CurrentAnimation);
        }

        public void PlayAnimation(string animation, Action callback)
        {
            if (CurrentAnimation == animation)
                return;

            CurrentAnimation = animation;
            InvokeChange(nameof(CurrentAnimation), CurrentAnimation, callback);
        }

        public void ResetPoisition(Vector2 position)
        {
            Position = position;
        }

        public virtual void PositionCollision(Vector2 position)
        {
            ResetPoisition(position);
        }

        public void ResetRotation(float rotation)
        {
            Rotation = rotation;
            CurrentDirection.SetParentAngle(rotation);
        }

        public void Rotate(float rotation)
        {
            Rotation = rotation;
            CurrentDirection.SetParentAngle(rotation);
            InvokeChange(nameof(Rotation), Rotation);
        }

        public void SetDirection(Vector2 target)
        {
            CurrentDirection.SetDirection(Position, target);
            InvokeChange(nameof(CurrentDirection), CurrentDirection);
        }

        public void SetDirection(Direction.DirectionXAxis xAxis, Direction.DirectionIntensivity intense)
        {
            CurrentDirection.SetDirection(xAxis, intense);
            InvokeChange(nameof(CurrentDirection), CurrentDirection);
        }

        public void SetupDirection(float startangle, Vector2 target)
        {
            Rotation = startangle;
            CurrentDirection.SetParentAngle(startangle);
            CurrentDirection.SetDirection(Position, target);
            InvokeChange(nameof(CurrentDirection), CurrentDirection);
        }

        public void ScaleTo(float scale)
        {
            Scale = new Vector3(scale, scale, scale);
            InvokeChange(nameof(Scale), Scale);
        }

        public void ResetScale(float scale)
        {
            Scale = new Vector3(scale, scale, scale);
        }

        

        public void ResetScale(Vector3 scale)
        {
            Scale = scale;
        }
    }
}
