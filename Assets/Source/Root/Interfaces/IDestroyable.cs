using System;

namespace EVI
{
    public interface IDestroyable
    {
        public Action OnDead { set; }
        public Action OnDestroy { set; }
        public void Dead();
        public void Destroy();
    }
}