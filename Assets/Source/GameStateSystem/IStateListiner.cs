using UnityEngine;

namespace EVI
{
    public interface IStateListiner
    {
        public void RegistryAsListiner();

        public void StateChanged(GameStateChange data);
    }
}
