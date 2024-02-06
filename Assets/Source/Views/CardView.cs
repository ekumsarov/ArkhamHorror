using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class CardView : InterectableView
    {
        private void Awake()
        {
            OnClick += Clicked;
            StopDrag += OnEndDrag;
        }

        private void Clicked()
        {
            Debug.LogError("Clicked");
        }

        private void OnEndDrag(InterectableView temp)
        {
            Debug.LogError("Stop drag " + temp.name);
        }
    }
}

