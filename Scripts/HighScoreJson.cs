using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class HighScoreJson 
{


    //{"result":[
    //        {"id":"73","name":"http:\/\/www.roundsapp.com\/post","value":"0"},
    //        {"id":"107","name":"test1324","value":"0"},
    //        {"id":"106","name":"test1324","value":"0"},
    //        {"id":"75","name":"http:\/\/www.roundsapp.com\/post","value":"0"},
    //        {"id":"76","name":"testInterview","value":"0"}],
    //"error":null
    //}

    public string error;
    public Result[] result;

}

[Serializable]
public class Result {

    public int id;
    public string name;
    public int value;

}
