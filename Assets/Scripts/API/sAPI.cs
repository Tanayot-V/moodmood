using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace SekaiLib
{
    public class sAPIOption
    {
        public int retryLimit = 3;
        public bool dontConvertToSprite = false;
        public object attachment = null;
    }



    public class sAPI
    {
        public static IEnumerator GET<T>(string apiPath, Dictionary<string, string> headers, Action<sWebResponse<T>, UnityWebRequest> callback = null, sAPIOption option = null)
        {
            yield return ProcessRequest<T>(apiPath, headers, option == null ? new sAPIOption() : option, callback, (url) => UnityWebRequest.Get(url));
        }


        public static IEnumerator POST<T>(string apiPath, object jsonData, Dictionary<string, string> headers, Action<sWebResponse<T>, UnityWebRequest> callback = null, sAPIOption option = null)
        {
            yield return ProcessRequest<T>(apiPath, headers, option == null ? new sAPIOption() : option, callback, (url) => {
                string json = JsonConvert.SerializeObject(jsonData);
                UnityWebRequest uwr = UnityWebRequest.PostWwwForm(url, json);
                uwr.uploadHandler   = (UploadHandler)   new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                return uwr;
            });
        }


        public static IEnumerator POST<T>(string apiPath, WWWForm form, Dictionary<string, string> headers, Action<sWebResponse<T>, UnityWebRequest> callback = null, sAPIOption option = null)
        {
            yield return ProcessRequest<T>(apiPath, headers, option == null ? new sAPIOption() : option, callback, (url) => {
                UnityWebRequest uwr = UnityWebRequest.Post(url, form);
                uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                return uwr;
            });
        }


        public static IEnumerator DELETE<T>(string apiPath, Dictionary<string, string> headers, Action<sWebResponse<T>, UnityWebRequest> callback = null, sAPIOption option = null)
        {
            yield return ProcessRequest<T>(apiPath, headers, option == null ? new sAPIOption() : option, callback, (url) => {
                UnityWebRequest uwr = UnityWebRequest.Delete(url);
                uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                return uwr;
            });
        }


        public static IEnumerator PUT<T>(string apiPath, object jsonData, Dictionary<string, string> headers, Action<sWebResponse<T>, UnityWebRequest> callback = null, sAPIOption option = null)
        {
            yield return ProcessRequest<T>(apiPath, headers, option == null ? new sAPIOption() : option, callback, (url) => {
                string json = JsonConvert.SerializeObject(jsonData);
                UnityWebRequest uwr = UnityWebRequest.Put(url, json);
                uwr.uploadHandler   = (UploadHandler)   new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                return uwr;
            });
        }
        private static IEnumerator ProcessRequest<T>(string apiPath, Dictionary<string, string> headers, sAPIOption option, Action<sWebResponse<T>, UnityWebRequest> callback, Func<string, UnityWebRequest> requester)
        {
            // Clear slash
            while(apiPath.StartsWith("/"))
            {
                apiPath = apiPath.Remove(0, 1);
            }

            //apiPath = Regex.Replace(apiPath, @"api/v\d+/(.*)", "$1");

            sAPIInfo config = sAPIInfo.Default;
            if (config == null)
            {
                Debug.LogWarning("[Error.Config]: No api config found.");
                yield break;
            }

            string url = string.Format("{0}/{2}", config.endPoint, config.version, apiPath);
            if (apiPath.StartsWith("http"))
            {
                url = apiPath;
            }
            
            Debug.Log("[Info.Endpoint]: " + url);

            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            // Process with retry
            int retry = 0;
            int retryLimit = option.retryLimit;

            DateTime retryOn = DateTime.Now;

            UnityWebRequest req;
            while(true) 
            {
                req = requester(url);
                foreach(var kv in headers)
                {
                    req.SetRequestHeader(kv.Key, kv.Value);
                }

                req.certificateHandler = null;
                
                yield return req.SendWebRequest();
                if (req.result == UnityWebRequest.Result.ConnectionError)
                {
                    retry++;
                    if (retry <= retryLimit)
                    {
                        Debug.LogWarning($"[Error.Request]: {req.url} method: {req.method} due to error: {req.error}... retry {retry} / {retryLimit}");
                    }
                    else
                    {
                        Debug.LogWarning($"[Error.Request]: {req.url} method: {req.method} due to error: {req.error}... after retried for {retryLimit} times");
                        break;
                    }

                    var diff = DateTime.Now.Subtract(retryOn);
                    if (diff.TotalSeconds < 1)
                    {
                        yield return new WaitForSeconds(1f - (float)diff.TotalSeconds);
                    }
                    continue;
                }
                break;
            }

            //Debug.Log("response J-Son : " + req.result);
            Debug.Log("response J-Son : " + req.downloadHandler.text);
            sWebResponse<T> result = JsonConvert.DeserializeObject<sWebResponse<T>>(req.downloadHandler.text);
            //result.attachment = option.attachment;
            callback?.Invoke(result, req);
        }
    }

    [System.Serializable]
    public class sAPIInfo
    {
        public int    port;
        public string host;
        public int    version; 

        public string endPoint
        {
            get
            {
                if (host.StartsWith("https"))
                {
                    return host;
                }
                else
                {
                    return $"{host}:{port}";
                }
            }
        }
        public static sAPIInfo Default
        {
            get
            {
                if (_default != null)
                {
                    return _default;
                }
                return null;
            }
        }
        public static sAPIInfo _default;
        public static sAPIInfo Create(string host, int port, int version = 1)
        {
            while (host.EndsWith("/"))
            {
                host = host.Remove(host.Length - 1);
            }

            sAPIInfo info  = new sAPIInfo();
            info.host      = host;
            info.port      = port;
            info.version   = (version == 0) ? 1 : version;
            
            _default = info;
            return info;
        }
    }

    [System.Serializable]
    public class sWebResponse<T>
    {
        //public bool success;
        //public int responseType;
        public int code;
        public string message;
        public T data;
        public SWebResponseError errors;
        //public object attachment;

        public string ToJson(bool format = false)
        {
            return JsonConvert.SerializeObject(this, (format ? Formatting.Indented : Formatting.None));
        }

        public string ToDataJson(bool format = false)
        {
            return JsonConvert.SerializeObject(data, (format ? Formatting.Indented : Formatting.None));
        }
    }


    [System.Serializable]
    public class MNull { }

    [System.Serializable]
    public class SWebResponseError
    {
        public string[] invalid;
    }

}