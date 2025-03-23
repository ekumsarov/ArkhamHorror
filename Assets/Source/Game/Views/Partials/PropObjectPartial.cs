using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PropObject : MonoBehaviour
{
    public IEnumerable<string> GetStateIDs()
    {
        return _states.Select(s => s.ID);
    }
}
