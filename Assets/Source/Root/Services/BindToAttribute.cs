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

public class BindEventAttribute : Attribute
{
    public string Name = null;

    public BindEventAttribute()
    {

    }
}

[AttributeUsage(AttributeTargets.Method)]
public class BindProcessAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class BindableProcessAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class BindablePropertyAttribute : Attribute
{
    public string Name;

    public BindablePropertyAttribute(string memberName = null)
    {
        Name = memberName;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class BindableEventAttribute : Attribute
{
    public string Name;

    public BindableEventAttribute(string memberName = null)
    {
        Name = memberName;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JSONConvertAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class JSONSerializableAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class AutoFindComponentAttribute : Attribute
{
    public bool SearchInChildren { get; set; }

    public AutoFindComponentAttribute(bool searchInChildren = false)
    {
        SearchInChildren = searchInChildren;
    }
}