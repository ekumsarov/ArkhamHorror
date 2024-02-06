using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IBindable
{
    public IBindable BindableObject { get; }

    public bool IsBinded { get; }

    public List<string> GetBinds { get; }

    public void InvokeChange<T>(string memberName, T obj);

    public void InvokeChange<T>(string memberName, T obj, Action callback);
    public void AddListiner(string name, Delegate del);
    public void RemoveListener(string name, Delegate del);
}
