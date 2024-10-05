namespace EVI
{
    public interface IUpdatable
    {
        public void Tick(float deltaTime);
    }

    public interface IFixedUpdatable
    {
        public void FixedTick();
    }
}