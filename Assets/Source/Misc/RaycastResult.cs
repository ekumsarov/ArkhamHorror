
using UnityEngine;

public class RaycastResult<T> 
{
    public T Object {get; set;}
    public RaycastHit Hit {get; set;}

    public static RaycastResult<T> Create(T val, RaycastHit hit)
    {
        RaycastResult<T> temp = new RaycastResult<T>();

        temp.Object = val;
        temp.Hit = hit;

        return temp;
    }
}