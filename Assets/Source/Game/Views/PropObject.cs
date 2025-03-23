using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PropState
{
    [HorizontalGroup("Split"), LabelWidth(60)]
    public string ID;

    [HorizontalGroup("Split"), LabelWidth(60)]
    public GameObject StateObject;
}

public partial class PropObject : MonoBehaviour
{
    [Title("Состояния пропса")]
    [SerializeField, ListDrawerSettings(Expanded = true)]
    private List<PropState> _states = new();

    [ShowInInspector, ReadOnly]
    private string _currentID;

    [Button("Включить состояние по ID")]
    public void SetState(string id)
    {
        bool found = false;

        foreach (var state in _states)
        {
            bool isTarget = state.ID == id;
            if (state.StateObject != null)
                state.StateObject.SetActive(isTarget);

            if (isTarget)
            {
                _currentID = id;
                found = true;
            }
        }

        if (!found)
        {
            Debug.LogWarning($"[PropObject] Нет состояния с ID: {id}");
        }
    }

    [Button("Следующее состояние")]
    public void NextState()
    {
        if (_states.Count == 0) return;

        int currentIndex = _states.FindIndex(s => s.ID == _currentID);
        int nextIndex = (currentIndex + 1) % _states.Count;

        SetState(_states[nextIndex].ID);
    }

    private void OnEnable()
    {
        if (_states.Count > 0)
        {
            SetState(_states[0].ID); // Стартовое состояние
        }
    }

    public string CurrentState => _currentID;
}
