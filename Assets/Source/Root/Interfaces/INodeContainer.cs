namespace EVI
{
    public interface INodeContainer
    {
        void AddEvent(LogicNode node);
        void AddRandomEvent(LogicNode node);
        void Popup(LogicNode node);
        void TriggerNextEvent();
        void ClearEvents();
        void ShuffleRandomEvents();
        LogicNode GetRandomEvent();
        void ResetRandomPool();
    }
}