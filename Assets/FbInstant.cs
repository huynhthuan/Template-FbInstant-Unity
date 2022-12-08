using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;

public enum RequestStatus
{
    PENDING = 100,
    FULLFILLED = 200,
    REJECT = 500
}

public class RequestType
{
    public const string TOURNAMENT = "tournament";
    public const string PLAYER = "player";

    /// <summary>
    /// <para>context</para>
    /// Contains functions and properties related to the current game context.
    /// </summary>
    public const string CONTEXT = "context";
}

public class RequestMethodName
{
    /// <summary>
    /// A unique identifier for the player. A Facebook user's player ID will remain constant, and is scoped to a specific game. This means that different games will have different player IDs for the same user. This function should not be called until FBInstant.initializeAsync() has resolved.
    /// </summary>
    /// <returns>A string unique identifier for the player.</returns>
    public const string GET_ID = "getID";
    public const string GET_ASID_ASYNC = "getASIDAsync";
    public const string CAN_SUB_BOT_ASYNC = "canSubscribeBotAsync";
    public const string GET_NAME = "getName";
    public const string GET_DATA_ASYNC = "getDataAsync";
    public const string SET_DATA_ASYNC = "setDataAsync";
    public const string CHOOSE_ASYNC = "chooseAsync";
}

[Serializable]
public class RequestItem
{
    string id;
    public RequestStatus status;
    public string data;

    public RequestItem(string id, RequestStatus status, string data)
    {
        this.id = id;
        this.status = status;
        this.data = data;
    }
}

[Serializable]
public class UpdateData
{
    public string id;
    public RequestStatus status;

    [TextArea]
    public string data;
}

public class FbInstant : SingletonMonoBehaviour<FbInstant>
{
    [DllImport("__Internal")]
    private static extern void SendFBInstantRequestToJS(string id, string type, string methodName, string paramsRequest);

    private int requestNumber = 0;

    private RequestItem currentRequest;

    private void AddRequest(string id, RequestStatus status)
    {
        currentRequest = new RequestItem(id, status, null);
    }

    public void UpdateRequest(string updateData)
    {
        UpdateData newData = JsonUtility.FromJson<UpdateData>(updateData);
        currentRequest = new RequestItem(newData.id, newData.status, newData.data);
    }

    private void ResetRequest()
    {
        currentRequest = null;
    }

    public IEnumerator SendRequest(string type, string methodName, string paramsRequest = "null")
    {

        requestNumber++;
        string requestId = requestNumber + "_" + type + "_" + methodName;
        AddRequest(requestId, RequestStatus.PENDING);
        Debug.Log($"Start call request [{requestId}] - Status [{currentRequest.status}]");

        SendFBInstantRequestToJS(requestId, type, methodName, paramsRequest);

        yield return new WaitUntil(() => IsRequestDone());
        yield return JsonUtility.ToJson(currentRequest);
        ResetRequest();
    }

    private bool IsRequestDone()
    {
        return currentRequest.status == RequestStatus.REJECT || currentRequest.status == RequestStatus.FULLFILLED;
    }
}

