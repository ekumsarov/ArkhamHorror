namespace EVI
{
    public class MyTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public MyTuple(T1 value1, T2 value2)
        {
            Item1 = value1;
            Item2 = value2;
        }
    }
}