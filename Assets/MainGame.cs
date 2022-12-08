using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class UserData
{
    public string[] achievements;
    public int currentLife;

    public UserData(string[] achievements, int currentLife)
    {
        this.achievements = achievements;
        this.currentLife = currentLife;
    }
}

public class MainGame : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("Start func in webgl");

        IEnumerator result = FbInstant.instance.SendRequest(RequestType.CONTEXT, RequestMethodName.CHOOSE_ASYNC);
        yield return result;
        RequestItem resultData = JsonUtility.FromJson<RequestItem>(result.Current.ToString());
        Debug.Log($"Send request done {JsonConvert.SerializeObject(resultData)}");

    }
}
