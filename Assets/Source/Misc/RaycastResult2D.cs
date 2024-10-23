
using UnityEngine;

public class RaycastResult2D<T> 
{
    public T Object {get; set;}
    public RaycastHit2D Hit {get; set;}

    public static RaycastResult2D<T> Create(T val, RaycastHit2D hit)
    {
        RaycastResult2D<T> temp = new RaycastResult2D<T>();

        temp.Object = val;
        temp.Hit = hit;

        return temp;
    }
}