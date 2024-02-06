using EVI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Test : MonoBehaviour
{
    public CultistView CultistView;

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
        model.MoveTo(new Vector2(2f, 1f));
        yield return new WaitForSeconds(2f);
        model.Rotate(70f);
        model.MoveTo(new Vector2(3f, 0f));
        yield return new WaitForSeconds(2f);
        model.Rotate(0f);
        model.MoveTo(new Vector2(0f, 0f));
        yield return new WaitForSeconds(2f);
        model.Rotate(-37f);
        model.MoveTo(new Vector2(1f, 1f));

        yield return null;
    }
}
