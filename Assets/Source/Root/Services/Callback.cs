using EVI;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callback 
{
    private string _name;
    private Type[] _arguments;
    private Delegate _delegate;
    private Delegate _back;

    private List<Delegate> _delegates;

    public Callback(string name)
    {
        _name = name;
        _delegates = new List<Delegate>();
    }

    public void AddListener(Delegate del)
    {
        _delegates.Add(del);
    }

    public void RemoveListener(Delegate del)
    {
        if (_delegates.Contains(del))
            _delegates.Remove(del);
    }

    public void TryInvoke(object obj)
    {
        foreach(var del in _delegates)
        {
            try
            {
                del?.DynamicInvoke(obj);
            }
            catch
            {
                del?.DynamicInvoke();
            }
        }
        
    }

    public void TryInvoke(object obj, Action callback)
    {
        foreach (var del in _delegates)
        {
            try
            {
                del?.DynamicInvoke(obj, callback);
            }
            catch
            {
                del?.DynamicInvoke();
            }
        }
    }

    public bool IsName(string name)
    {
        return _name.Equals(name);
    }
}
