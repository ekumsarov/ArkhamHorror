using EVI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistView : BaseView
{
    [BindTo]
    private void MyNameChanged(string name)
    {
        Debug.LogError(name);
    }
}
