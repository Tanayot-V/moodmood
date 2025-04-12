using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SekaiLib;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class MainApi : MonoBehaviour
{
    public static MainApi Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        sAPIInfo.Create(host, port, 1);
    }

    [Header("API SETTINGS")]
    public string host;
    public int port;


    [Space(20)]
    [Header("DATA")]
    public AccountResponseModel AccountResponse = new AccountResponseModel();

    public void Register(
        string invite_code,
        string email,
        string password,
        string password_confirmation,
        string sex,
        string prefix_name,
        string first_name,
        string last_name,
        string student_number,
        Action<bool, AccountResponseModel> callback = null
    )
    {
        StartCoroutine(RegisterCoroutine(invite_code, email, password, password_confirmation, sex, prefix_name, first_name, last_name, student_number, callback));
    }
    public IEnumerator RegisterCoroutine(
        string invite_code,
        string email,
        string password,
        string password_confirmation,
        string sex,
        string prefix_name,
        string first_name,
        string last_name,
        string student_number,
        Action<bool, AccountResponseModel> callback = null
    )
    {
        WWWForm form = new WWWForm();
        form.AddField("invite_code", invite_code);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("password_confirmation", password_confirmation);
        form.AddField("sex", sex);
        form.AddField("prefix_name", prefix_name);
        form.AddField("first_name", first_name);
        form.AddField("last_name", last_name);
        form.AddField("student_number", student_number);

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("invite_code", invite_code);
        header.Add("email", email);
        header.Add("password", password);
        header.Add("password_confirmation", password_confirmation);
        header.Add("sex", sex);
        header.Add("prefix_name", prefix_name);
        header.Add("first_name", first_name);
        header.Add("last_name", last_name);
        header.Add("student_number", student_number);

        yield return sAPI.POST<AccountResponseModel>("/api/student/register",form , null, (result, req) =>
        {
            if (result != null)
            {
                Debug.Log($"Register response: {result?.message}");
                callback?.Invoke(true,result.data);
                AccountResponse = result.data;
            }
            else
            {
                Debug.LogError($"Register failed: No response from server");
                callback?.Invoke(false,null);
            }
        });    
    }

    public void Login(
        string email,
        string password,
        Roles roles,
        Action<bool, AccountResponseModel> callback = null
    )
    {
        StartCoroutine(LoginCoroutine(email, password, roles, callback));
    }

    private IEnumerator LoginCoroutine(
        string email,
        string password,
        Roles roles,
        Action<bool, AccountResponseModel> callback = null
    )
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("type", roles.ToString());

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("email", email);
        header.Add("password", password);
        header.Add("type", roles.ToString());

        yield return sAPI.POST<AccountResponseModel>("/api/login", form, null, (result, req) =>
        {
            if (result != null)
            {
                Debug.Log($"Login response: {result?.message}");
                callback?.Invoke(true,result.data);
                AccountResponse = result.data;
            }
            else
            {
                Debug.LogError($"Login failed: No response from server");
                callback?.Invoke(false,null);
            }
        });
    }

    public void GetMood(DateTime start,DateTime end, int student_id, Action<bool, JsonObject> callback = null)
    {
        StartCoroutine(GetMoodCoroutine(start, end, student_id, callback));
    }
    private IEnumerator GetMoodCoroutine(DateTime start, DateTime end, int student_id, Action<bool, JsonObject> callback = null)
    {
        string path = $"/api/student/get-moods?date_start={start.Date}&date_end={end.Date}&student_id={student_id}";
        yield return sAPI.GET<JsonObject>(path, null, (result, req) =>
        {
            if (result != null)
            {
                Debug.Log($"GetMood response: {result?.message}");
                callback?.Invoke(true,result.data);
            }
            else
            {
                Debug.LogError($"GetMood failed: No response from server");
                callback?.Invoke(false,null);
            }
        });
    }
}

public enum Roles
{
    student = 0,
    teacher = 1,
    ADMIN = 9
}
