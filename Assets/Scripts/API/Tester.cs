using System;
using SekaiLib;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public AccountResponseModel AccountResponse;
    
    void Start()
    {
        // MainApi.Instance.Register("SVKMOODNPI11","dsa1@fid.com", "asdlelv", "asdlelv", "male", "mr", "asd", "asd", "1234567890", (bool success, RegisterResponseModel result) => {
        //     Debug.Log(result);
        // });

        MainApi.Instance.Login("dsa@fid.com", "asdlelv", Roles.student, (bool success, AccountResponseModel result) => {
            Debug.Log(result);
            AccountResponse = result;
        });

        MainApi.Instance.GetMood(DateTime.Today, DateTime.Today, AccountResponse.student_id, (result, req) => {
            Debug.Log(result);
        });
    }

    void Update()
    {
        
    }
}
