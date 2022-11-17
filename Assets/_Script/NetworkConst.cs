using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConst : MonoBehaviour
{
    [Serializable]
    public struct postLogin
    {
        public string email;
        public string password;
    }


    [Serializable]
    public struct postRegister
    {
        public string name;
        public string email;
        public string password;
        public int mobile;
        public string companyName;
        public string city;
        public string country;
        public int user_type;
    }

    public struct postRevover
    {
        public string name;
        public string email;
    }
    public struct postAddress
    {
        public string address;
    }

}
