using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

public class BindToAttribute : Attribute
{
    public string Name = null;

    public BindToAttribute()
    {

    }
}


[AttributeUsage(AttributeTargets.Property)]
public class BindablePropertyAttribute : Attribute
{
    public string Name;

    public BindablePropertyAttribute(string memberName = null)
    {
        Name = memberName;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JSONConvertAttribute : Attribute
{

}