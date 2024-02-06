using System;
using UnityEngine;

namespace EVI
{
    public interface ITransformable
    {
        public void ResetRotation(float rotation);
        public void Rotate(float rotation);
        public void ResetPoisition(Vector2 position);
        public void MoveTo(Vector2 position);
        public void ResetScale(float sacle);
        public void ScaleTo(float scale);
    }
}