using EVI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEditor;
using NUnit.Framework;
using System;

public class Test : MonoBehaviour
{
    [SerializeField, ValueDropdown("@EVI.TypeSelector.GetAllTypeIDs()")] public List<string> _cardID;
    public CultistView CultistView;

    private string _id = "help";

    private void Start()
    {
        Vector2 vector2 = new Vector2(2f, 1.1f);
        JSONObject temp = new JSONObject();
        temp.Add("hell", vector2);

        Vector2 test = temp["hell"];

        /*Cultist model = new Cultist();
        model.Init();
        CultistView.Init(model);
        model.SetName("Vikulik!!!");
        StartCoroutine(Moving(model));*/
    }

    IEnumerator Moving(BaseModel model)
    {

        yield return null;
    }
}
