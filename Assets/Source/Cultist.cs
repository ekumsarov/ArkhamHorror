using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVI;

public class Cultist : BaseModel
{
    [BindableProperty(Name = nameof(MyName))]
    public string MyName { get; private set; }

    public void SetName(string name)
    {
        MyName = name;
        InvokeChange(nameof(MyName), MyName);
    }
}
