namespace EVI
{
    public interface IRegystryRoot
    {
        public void Registry(IUpdatable updatable);
        public void Unregisrty(IUpdatable updatable);
    }
}