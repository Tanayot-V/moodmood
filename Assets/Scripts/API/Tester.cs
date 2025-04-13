using System;
using SekaiLib;
using UnityEngine;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{

    public AccountResponseModel AccountResponse;

    public CreateMoodRequest testCreateMood;
    
    void Start()
    {
        // MainApi.Instance.Register("SVKMOODNPI11","dsa1@fid.com", "asdlelv", "asdlelv", "male", "mr", "asd", "asd", "1234567890", (bool success, sWebResponse<AccountResponseModel> result) => {
        //     Debug.Log(result);
        // });

        // MainApi.Instance.Login("qweqwe@gmail.com", "qweqweqwe", Roles.student, (success, result) => {
        //     if (!success)
        //     {
        //         Debug.LogError(result.errors.invalid[0]);
        //         return;
        //     }

        //     //Debug.Log(result);
        //     AccountResponse = result.data;
        //     testCreateMood.data.student_id = AccountResponse.student_id;

            // var today = DateTime.Today;
            // var month = new DateTime(today.Year, today.Month, 1);

            // MainApi.Instance.GetMood(month, today, AccountResponse.student_id, (success, result) => {
                
            //     foreach(KeyValuePair<string,MoodData> item in result.data)
            //     {
            //         Debug.Log(item.Value.mood_choice_txt);
            //     }
            // });

            // MainApi.Instance.CreateMood(testCreateMood, (success, result) => {
            //     if (!success)
            //     {
            //         Debug.LogError(result.message);
            //         return;
            //     }
            // });
            
            // MainApi.Instance.CreateSOS(AccountResponse.student_id.ToString(), "Test SOS", (success, result) => {
            //     if (!success)
            //     {
            //         Debug.LogError(result.message);
            //         return;
            //     }
            //     Debug.Log(result);
            // });

            // MainApi.Instance.GetOveraLL(AccountResponse.student_id, getDataType.self, period.month, (success, result) => {
            //     if (!success)
            //     {
            //         Debug.LogError(result.message);
            //         return;
            //     }
            //     Debug.Log(result);
            // });

            // MainApi.Instance.GetOveraLLDetail(AccountResponse.student_id, getDataType.self, period.month,3 , (success, result) => {
            //     if (!success)
            //     {
            //         Debug.LogError(result.message);
            //         return;
            //     }
            //     Debug.Log(result);
            // });
            
        //});

        // MainApi.Instance.ForgotPassword("qweqwe@gmail.com", (success, result) => {
        //     if (!success)
        //     {
        //         Debug.LogError(result.errors.invalid[0]);
        //         return;
        //     }
        //     Debug.Log(result);
        // });

        // MainApi.Instance.ResetPassword("BNuMUY","1q2w3e4r", "1q2w3e4r", (success, result) => {
        //     if (!success)
        //     {
        //         Debug.LogError(result.message);
        //         return;
        //     }
        //     Debug.Log(result);
        // });
    }

    void Update()
    {
        
    }
}
