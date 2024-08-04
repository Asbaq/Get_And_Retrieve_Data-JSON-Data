using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using static LoginManager;
using Newtonsoft.Json.Linq;

public class LoginManager : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public LoginResponse response;
    public ScoreRespone scoreResponse;
    [SerializeField]
    private InputField obtainedScoreInput;
    [SerializeField]
    private InputField totalScoreInput;
    public Text errorText;

    private string loginURL = "https://aiedu.datavivservers.in/account/login/";
    private string scoreAPIURL = "https://aiedu.datavivservers.in/score/";

    [Serializable]
    public class LoginResponse
    {
        public int status;
        public Data data;

        [Serializable]
        public class Data
        {
            public string refresh;
            public string access;
            public bool is_admin;
            public int folder_name;
            public int folder_id;
            public Permissions permissions;
            public List<ViewModule> view_modules;
        }


        [Serializable]
        public class Permissions
        {
            public bool create;
            public bool view;
            public bool remove;
            public bool edit;
        }

        [Serializable]
        public class ViewModule
        {
            public int id;
            public GroupInfo group_info;
            public DateTime created_at;
            public DateTime updated_at;
            public string updatedAt;
            public string name;
            public int group;

            [Serializable]
            public class GroupInfo
            {
                public string name;
                public int id;
            }

        }

    }

    [Serializable]
    public class ScorePostData
    {
        public int obtained_score;
        public int total_score;
        public int module;
    }

    [Serializable]
    public class ScoreRespone
    {
        public int id;
        public int student;
        public string student_info;
        public List<ModuleInfo> module_info;
        public DateTime created_at;
        public DateTime updated_at;
        public int obtained_score;
        public int total_score;
        public int module;

        [Serializable]
        public class ModuleInfo
        {
            public int id;
            public GroupInfo group_info;
            public DateTime created_at;
            public DateTime updated_at;
            public string name;
            public int group;

            [Serializable]
            public class GroupInfo
            {
                public string name;
                public int id;
            }
        }
    }


    public void OnLoginButtonClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        StartCoroutine(SendLoginRequest(email, password));
    }


    IEnumerator SendLoginRequest(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error: " + www.error);
                errorText.text = "Login failed. Please try again.";
            }
            else
            {
                string responseText = www.downloadHandler.text;

                try
                {
                    // Deserialize JSON response using Json.NET
                    response = JsonConvert.DeserializeObject<LoginResponse>(responseText);
                    Debug.Log(responseText);

                    // Access the data
                    Debug.Log($"Status: {response.status}");
                    Debug.Log($"Refresh: {response.data.refresh}");
                    Debug.Log($"Access: {response.data.access}");
                    Debug.Log($"Is Admin: {response.data.is_admin}");
                    Debug.Log($"Folder Name: {response.data.folder_name}");
                    Debug.Log($"Folder ID: {response.data.folder_id}");
                    Debug.Log($"Create Permission: {response.data.permissions.create}");
                    Debug.Log($"View Permission: {response.data.permissions.view}");
                    // Access ViewModules as needed
                    foreach (var viewModule in response.data.view_modules)
                    {
                        Debug.Log($"View Module Name: {viewModule.name}");
                        Debug.Log($"View Module ID: {viewModule.id}");

                        Debug.Log($"GroupInfo Name: {viewModule.group_info.name}");
                        Debug.Log($"GroupInfo ID: {viewModule.group_info.id}");
                    }

                    PostScore(15,6,28);

                }
                catch (Exception e)
                {
                    Debug.Log("Error parsing JSON: " + e.Message);
                    errorText.text = "An error occurred while processing the response.";
                }
            }
        }
    }

    public void PostScore(int obtainedScore = 15, int totalScore = 6, int module = 28)
    {
                ScorePostData scoreData = new ScorePostData
                {
                    obtained_score = obtainedScore,
                    total_score = totalScore,
                    module = module
                };

                string scoreDataJson = JsonConvert.SerializeObject(scoreData);
                Debug.Log(scoreDataJson);
                StartCoroutine(SendScore(scoreDataJson, response.data.access));
    }

    IEnumerator SendScore(string scoreDataJson, string jwtToken)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(scoreDataJson);
        using (UnityWebRequest www = new UnityWebRequest(scoreAPIURL, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error posting score: " + www.error);
            }
            else
            {
                string ScoreText = www.downloadHandler.text;
                // Handle the response from the server, if needed
 
            try
                {
                    // Deserialize JSON response using Json.NET
                    scoreResponse = JsonConvert.DeserializeObject<ScoreRespone>(ScoreText);
                    Debug.Log(ScoreText);
                    // Log each field of the response
                    Debug.Log($"ID: {scoreResponse.id}");
                    Debug.Log($"Student: {scoreResponse.student}");
                    Debug.Log($"Student Info: {scoreResponse.student_info}");

                    foreach (var moduleInfo in scoreResponse.module_info)
                    {
                        Debug.Log($"Module ID: {moduleInfo.id}");
                        Debug.Log($"Module Name: {moduleInfo.name}");
                        Debug.Log($"Module Group Name: {moduleInfo.group_info.name}");
                    }

                    Debug.Log($"Created At: {scoreResponse.created_at}");
                    Debug.Log($"Updated At: {scoreResponse.updated_at}");
                    Debug.Log($"Obtained Score: {scoreResponse.obtained_score}");
                    Debug.Log($"Total Score: {scoreResponse.total_score}");
                    Debug.Log($"Module: {scoreResponse.module}");
                }
                catch (Exception e)
                {
                    Debug.Log("Error parsing JSON: " + e.Message);
                    errorText.text = "An error occurred while processing the response.";
                }
                Debug.Log("Score posted successfully.");
            }
        }
    }
}
