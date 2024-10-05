using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace EVI
{
    public class GameStateSystem : IInitializable
    {
        private GameState _gameState = GameState.ManualControl;
        public GameState GameState => _gameState;

        private GamePhase _gamePhase = GamePhase.Morning;
        public GamePhase GamePhase => _gamePhase;

        private List<IStateListiner> _listeners;

        public void Initialize()
        {
            if(_listeners == null)
                _listeners = new List<IStateListiner>();
        }

        public void AddListiner(IStateListiner listener)
        {
            if(_listeners.Contains(listener) == false)
                return;

            _listeners.Add(listener);
        }

        public void RemoveListener(IStateListiner listener)
        {
            if(_listeners.Contains(listener) == false)
                return;

            _listeners.Remove(listener);
        }

        public void SetGameState(GameState state)
        {
            _gameState = state;
            GameStateChange status = new GameStateChange();
            status._previousState = _gameState;
            foreach(var listner in _listeners)
            {
                //listner.StateChanged();
            }
        }
    }

    public class GameStateChange
    {
        public GameState _previousState;
        public GameState _nextState;

        public GamePhase _previousPhase;
        public GamePhase _nextPhase;
    }
}
