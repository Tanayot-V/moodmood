using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SekaiLib;
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
    public AccountResponseModel AccountResponse;
    public Dictionary<string, MoodData> Moods = new Dictionary<string, MoodData>();
    public GetOveraLLResponse overallResponse;
    public GetOveraLLDetailResponse overallDetailResponse;

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
        Action<bool, sWebResponse<AccountResponseModel>> callback = null
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
        Action<bool, sWebResponse<AccountResponseModel>> callback = null
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

        yield return sAPI.POST<AccountResponseModel>("/api/student/register",form , null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"Register response: {result?.message}");
                callback?.Invoke(true,result);
                AccountResponse = result.data;
            }
            else
            {
                Debug.LogError($"Register failed: No response from server");
                callback?.Invoke(false,result);
            }
        });    
    }

    public void Login(
        string email,
        string password,
        Roles roles,
        Action<bool, sWebResponse<AccountResponseModel>> callback = null
    )
    {
        StartCoroutine(LoginCoroutine(email, password, roles, callback));
    }

    private IEnumerator LoginCoroutine(
        string email,
        string password,
        Roles roles,
        Action<bool, sWebResponse<AccountResponseModel>> callback = null
    )
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("type", roles.ToString());

        yield return sAPI.POST<AccountResponseModel>("/api/login", form, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"Login response: {result?.message}");
                callback?.Invoke(true,result);
                AccountResponse = result.data;
            }
            else
            {
                Debug.LogError($"Login failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void ForgotPassword(
        string email,
        Action<bool, sWebResponse<JObject>> callback = null
    )
    {
        StartCoroutine(ForgotPasswordCoroutine(email, callback));
    }
    private IEnumerator ForgotPasswordCoroutine(
        string email,
        Action<bool, sWebResponse<JObject>> callback = null
    )
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        yield return sAPI.POST<JObject>("/api/student/forgot-password", form, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"ForgotPassword response: {result?.message}");
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"ForgotPassword failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void ResetPassword(string token, string password, string passwordConfirm, Action<bool, sWebResponse<JObject>> callback = null)
    {
        StartCoroutine(ResetPasswordCoroutine(token, password, passwordConfirm, callback));
    }
    private IEnumerator ResetPasswordCoroutine(string token, string password, string passwordConfirm, Action<bool, sWebResponse<JObject>> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("password", password);
        form.AddField("password_confirmation", passwordConfirm);

        yield return sAPI.POST<JObject>("/api/student/reset-password", form, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"ResetPassword response: {result?.message}");
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"ResetPassword failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void GetMood(DateTime start,DateTime end, int student_id, Action<bool, sWebResponse<Dictionary<string,MoodData>>> callback = null)
    {
        StartCoroutine(GetMoodCoroutine(start, end, student_id, callback));
    }
    private IEnumerator GetMoodCoroutine(DateTime start, DateTime end, int student_id, Action<bool, sWebResponse<Dictionary<string,MoodData>>> callback = null)
    {
        string path = $"/api/student/get-moods?date_start={start.ToString("yyyy-MM-dd")}&date_end={end.ToString("yyyy-MM-dd")}&student_id={student_id}";
        yield return sAPI.GET<Dictionary<string,MoodData>>(path, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"GetMood response: {result?.message}");
                callback?.Invoke(true,result);
                Moods = result.data;
            }
            else
            {
                Debug.LogError($"GetMood failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void CreateMood(CreateMoodRequest request, Action<bool, sWebResponse<MoodData>> callback = null)
    {
        StartCoroutine(CreateMoodCoroutine(request, callback));
    }
    private IEnumerator CreateMoodCoroutine(CreateMoodRequest request, Action<bool, sWebResponse<MoodData>> callback = null)
    {
        yield return sAPI.POST<MoodData>("/api/student/create-mood", request, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"CreateMood response: {result?.message}");
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"CreateMood failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void CreateSOS(string studentId, string message, Action<bool, sWebResponse<JObject>> callback = null)
    {
        StartCoroutine(CreateSOSCoroutine(studentId, message, callback));
    }
    private IEnumerator CreateSOSCoroutine(string studentId, string message, Action<bool, sWebResponse<JObject>> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("student_id", studentId);
        form.AddField("message", message);

        yield return sAPI.POST<JObject>("/api/student/create-sos", form, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"CreateSOS response: {result?.message}");
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"CreateSOS failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }

    public void GetOveraLL(int studentId, getDataType type, period period, Action<bool, sWebResponse<GetOveraLLResponse>> callback = null)
    {
        StartCoroutine(GetOverallCoroutine(studentId, type, period, callback));
    }
    private IEnumerator GetOverallCoroutine(int studentId, getDataType type, period period, Action<bool, sWebResponse<GetOveraLLResponse>> callback = null)
    {
        string path = $"/api/student/get-overall?student_id={studentId}&type={type.ToString()}&period={period.ToString()}";
        yield return sAPI.GET<GetOveraLLResponse>(path, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"GetOverall response: {result?.message}");
                overallResponse = result.data;
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"GetOverall failed: {result?.message}");
                callback?.Invoke(false,result);
            }
        });
    }
    public void GetOveraLLDetail(int studentId, getDataType type, period period,int mood, Action<bool, sWebResponse<GetOveraLLDetailResponse>> callback = null)
    {
        StartCoroutine(GetOverallDetailCoroutine(studentId, type, period, mood, callback));
    }
    private IEnumerator GetOverallDetailCoroutine(int studentId, getDataType type, period period, int mood, Action<bool, sWebResponse<GetOveraLLDetailResponse>> callback = null)
    {
        string path = $"/api/student/get-overall-detail?student_id={studentId}&type={type.ToString()}&period={period.ToString()}&mood={mood}";
        yield return sAPI.GET<GetOveraLLDetailResponse>(path, null, (result, req) =>
        {
            if (result.code == 200)
            {
                Debug.Log($"GetOverallDetail response: {result?.message}");
                overallDetailResponse = result.data;
                callback?.Invoke(true,result);
            }
            else
            {
                Debug.LogError($"GetOverallDetail failed: {result?.message}");
                callback?.Invoke(false,result);
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

public enum getDataType
{
    self,
    friend,
}
public enum period
{
    current_week,
    previous_week,
    month
}
