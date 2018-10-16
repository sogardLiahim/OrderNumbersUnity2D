using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;  // for class Encoding
using System.IO;    // for StreamReader
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class HttpMethods : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetText());
        StartCoroutine(Upload());
        //otherUploadMethod();
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://development.m75.ro/test_mts/public/highscore/5"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                //Debug.Log(www.downloadHandler.text);

                HighScoreJson highscores = JsonUtility.FromJson<HighScoreJson>(www.downloadHandler.text);
                Debug.Log("User List of highscores is:");
                foreach (var hs in highscores.result)
                {
                    print("User id:" + hs.id + "   Name:  " + hs.name + " User score:" + hs.value);
                }

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }



    IEnumerator Upload()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        
        WWWForm form = new WWWForm();
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("name=mihai&value="+-299+""));
        form.AddField("name", "'Mihai'");
        form.AddField("value", 1202);
       
        UserHighScore user = new UserHighScore();
        user.name = "Mihai";
        user.value = -1202;
        string json = JsonUtility.ToJson(user);
        Debug.Log("Sending this json:"+json.ToString());


        UnityWebRequest www = UnityWebRequest.Post("https://development.m75.ro/test_mts/public/highscore/public/highscore/", form);
        ///www.ce=ServicePointManager.ServerCertificateValidationCallback;
        www.SetRequestHeader("Content-Type", "multipart/form-data");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.responseCode);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

    }

    public void otherUploadMethod()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;


        // this is what we are sending
        string post_data = "name=myName&value=-299";

        // this is where we will send it
        string uri = "https://development.m75.ro/test_mts/public/highscore/public/highscore/";

        // create a request

        HttpWebRequest request = (HttpWebRequest)
        WebRequest.Create(uri); request.KeepAlive = false;
        request.ProtocolVersion = HttpVersion.Version10;
        request.Method = "POST";

        // turn our request string into a byte stream
        byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

        // this is important - make sure you specify type this way
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = postBytes.Length;
        Stream requestStream = request.GetRequestStream();

        // now send it
        System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
        requestStream.Write(postBytes, 0, postBytes.Length);
        requestStream.Close();

        // grab te response and print it out to the console along with the status code
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    }

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
} 

public class MyPolicy:ICertificatePolicy
{
    public bool CheckValidationResult(ServicePoint srvPoint,
X509Certificate certificate, WebRequest request,
int certificateProblem)
    {
        //Return True to force the certificate to be accepted.
        return true;
    }
}

[Serializable]
public class UserHighScore
{
    public string name;
    public double value;
}





