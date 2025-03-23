using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;
using Zenject;

namespace EVI
{
    [JSONSerializable]
    public class BaseModel : SOBindable, IIdentifiable, INodeContainer
    {
        [SerializeField, ReadOnly, JSONConvert, OnInspectorInit("BaseModelComponents")]
        private string _id;

        public string ID => _id;

        [Inject] protected readonly NodeSystem _nodeSystem;

        private void BaseModelComponents()
        {
            _id = this.name;
        }

        public Action<BaseModel> OnDestroyed;
        public void DestroyModel()
        {
            OnDestroyed?.Invoke(this);
            CleanUp();
            OnDestroyed = null;
        }

        protected virtual void CleanUp()
        {

        }

        #region  NodeContainer Logic

        [SerializeField] protected List<LogicNode> eventQueue = new();
        [SerializeField] protected List<LogicNode> randomPool = new(); 
        private List<LogicNode> discardPile = new();

        public void AddEvent(LogicNode node)
        {
            if (node == null) return;
            eventQueue.Add(node);
        }

        public void AddRandomEvent(LogicNode node)
        {
            if (node == null) return;
            randomPool.Add(node);
            ShuffleRandomEvents();
        }

        public void Popup(LogicNode node)
        {
            if (node == null) return;
            eventQueue.Insert(0, node);
        }

        public void TriggerNextEvent()
        {
            // 1️⃣ Если есть запланированные события → выполняем их по порядку
            if (eventQueue.Count > 0)
            {
                LogicNode nextEvent = eventQueue[0];
                eventQueue.RemoveAt(0);
                Debug.Log($"🔥 Вызвано запланированное событие: {nextEvent.name}");
                nextEvent.Initialize(this, _nodeSystem);
                _nodeSystem.NextNode(nextEvent);
                return;
            }

            // 2️⃣ Если нет запланированных, но есть случайные → выбираем случайное
            if (randomPool.Count > 0)
            {
                LogicNode randomEvent = GetRandomEvent();
                Debug.Log($"🎲 Вызвано случайное событие: {randomEvent.name}");
                randomEvent.Initialize(this, _nodeSystem);
                _nodeSystem.NextNode(randomEvent);
                return;
            }

            // 3️⃣ Если и randomPool пуст, но есть discardPile → перетасовываем
            if (discardPile.Count > 0)
            {
                ResetRandomPool();
                LogicNode randomEvent = GetRandomEvent();
                if (randomEvent != null)
                {
                    Debug.Log($"♻️ Перемешали сброс! Вызвано случайное событие: {randomEvent.name}");
                    randomEvent.Initialize(this, _nodeSystem);
                    _nodeSystem.NextNode(randomEvent);
                    return;
                }
            }

            Debug.Log("❌ Нет доступных событий.");
        }

        public void ClearEvents()
        {
            eventQueue.Clear();
            randomPool.Clear();
            discardPile.Clear();
        }

        public void ShuffleRandomEvents()
        {
            for (int i = 0; i < randomPool.Count; i++)
            {
                int randIndex = Random.Range(i, randomPool.Count);
                (randomPool[i], randomPool[randIndex]) = (randomPool[randIndex], randomPool[i]); // Swap
            }
        }

        public LogicNode GetRandomEvent()
        {
            if (randomPool.Count == 0) return null;

            int index = Random.Range(0, randomPool.Count);
            LogicNode selected = randomPool[index];

            // Перемещаем событие в discardPile
            discardPile.Add(selected);
            randomPool.RemoveAt(index);

            return selected;
        }

        public void ResetRandomPool()
        {
            if (discardPile.Count == 0) return;

            Debug.Log("🔄 Перемешиваем сброс и заполняем случайные события заново!");

            randomPool.AddRange(discardPile);
            discardPile.Clear();
            ShuffleRandomEvents();
        }

        #endregion
    }
}
