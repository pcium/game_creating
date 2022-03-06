using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Security.Cryptography;

public class XmlManager
{
    /// <summary>XML로 해당 path 경로에 classForSave 변수를 저장합니다.</summary>
    public static void XMLSerialize<T>(T classForSave, string path)
    {
        System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (System.IO.TextWriter tw = new System.IO.StreamWriter(path))
        {
            sr.Serialize(tw, classForSave);
            tw.Close();
        }
    }
    /// <summary>XML로 해당 path 경로에서 클래스를 불러옵니다.</summary>
    public static T XMLDeserialize<T>(string path)
    {
        System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open))
        {
            T t = (T)sr.Deserialize(fs);
            fs.Close();

            return t;
        }
    }

    /// <summary>해당 파일이 존재하는지 검사합니다.</summary>
    public static bool IsExist(string path)
    {
        if (System.IO.File.Exists(path))
            return true;
        else
            return false;
    }
    /// <summary>pattern(정규식)과 일치하는 파일이 있는지 검사합니다.</summary>
    public static bool IsMatchExist(string path, string pattern)
    {
        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
        string[] files =  System.IO.Directory.GetFiles(path);
        foreach(var file in files)
        {
            if (rgx.IsMatch(file))
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>pattern(정규식)과 일치하는 파일이 있는지 검사하고, 문자열 배열로 받아옵니다.
    /// 주로 해당 정규식의 파일 이름을 한번에 받아올 때 사용합니다.</summary>
    public static string[] GetMatchFileString(string path, string pattern)
    {
        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
        string[] files = System.IO.Directory.GetFiles(path);
        List<string> saveFiles = new List<string>();
        foreach (var file in files)
        {
            if (rgx.IsMatch(file))
                saveFiles.Add(file);
        }

        return saveFiles.ToArray();
    }
    /// <summary>pattern(정규식)과 일치하는 파일이 있는지 검사하고, 클래스의 배열로 받아옵니다.
    /// 주로 해당 정규식의 파일들을 한번에 받아올 때 사용합니다.</summary>
    public static T[] GetMatchFiles<T>(string path, string pattern)
    {
        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
        string[] files = System.IO.Directory.GetFiles(path);
        List<T> saveFiles = new List<T>();
        foreach (var file in files)
        {
            if (rgx.IsMatch(file))
            {
                T t = XMLDeserialize<T>(file);
                saveFiles.Add(t);
            }
        }

        return saveFiles.ToArray();
    }


    

    #region FOR_DEBUG
    /// <summary>
    /// Resources 폴더 내에 해당 클래스의 Xml 파일을 만듭니다. ex) ToXMLFileInResources(stat, "profile");
    /// 확장자나 경로는 입력하지 않아도 됩니다.
    public static T ToXMLFileInResources<T>(T classForCopy, string fileName)
    {
        XMLSerialize<T>(classForCopy, Application.dataPath + "/Resources/" + fileName + ".xml");
        return classForCopy;
    }
    /// <summary>
    /// Resources 폴더 내에 해당 클래스의 Xml 파일을 만듭니다. ex) ToXMLFileInResources(stat, "profile");
    /// 확장자나 경로는 입력하지 않아도 됩니다. 버튼을 인자로 받을때 버튼 클릭시 xml생성
    public static T ToXMLFileInResources<T>(T classForCopy, string fileName, UnityEngine.UI.Button button)
    {
        button.onClick.AddListener(delegate
        {
            XMLSerialize<T>(classForCopy, Application.dataPath + "/Resources/" + fileName + ".xml");
        });
        return classForCopy;
    }
    /// <summary>
    /// 기존에 있던 번역xml파일들의 경로를 PersistantDataPath로 옮깁니다.
    /// 이는 윈도우 환경에서만 읽히는 파일들을 android 또는 iOS 환경에서 파일을 읽기 위함입니다.</summary>
    public static string[] SaveToPersistantDataPath(string pathTextAsset)
    {
        List<string> ss = new List<string>();
        TextAsset[] ta = Resources.LoadAll<TextAsset>(pathTextAsset);
        foreach (var a in ta)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new System.Xml.Serialization.XmlSerializer(a.GetType());
            serializer.Serialize(stringwriter, a);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + a.name + ".xml", a.text);
            ss.Add(stringwriter.ToString());
        }
        
        return ss.ToArray();
    }
    /// <summary>
    /// 기존에 있던 번역xml파일들의 경로를 PersistantDataPath로 옮긴 것을 불러옵니다.
    /// 이는 윈도우 환경에서만 읽히는 파일들을 android 또는 iOS 환경에서 파일을 읽기 위함입니다.</summary>
    public static T LoadFromXMLString<T>(string xmlText)
    {
        var stringReader = new System.IO.StringReader(xmlText);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(stringReader);
    }
    #endregion
}

public class Binary
{
    /// <summary>바이너리화 하여 저장합니다</summary>
    public static void BinarySerialize<T>(T classForSave, string path)
    {
        var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        using (System.IO.FileStream file = System.IO.File.Create(path))
        {
            bf.Serialize(file, classForSave);

            file.Close();
        }   
    }
    /// <summary>바이너리로 해당 path 경로에서 암호화 된 classForCopy 함수를 불러옵니다.
    /// Stat.savePath + "/" + Stat.fileName을 경로로 추천합니다.</summary>
    public static T BinaryDeserialize<T>(string path)
    {
        T t = default(T);
        if (System.IO.File.Exists(path))
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (System.IO.FileStream file = System.IO.File.Open(path, System.IO.FileMode.Open))
            {
                t = (T)bf.Deserialize(file);
                file.Close();
            }
                
        }
        return t;
    }

}

public class EncryptDecrypt
{

    public static string EncryptData(string toEncrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("00000000000000000000000000000000");

        // 256-AES key 
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        RijndaelManaged rDel = new RijndaelManaged();

        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;

        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }


    public static string DecryptData(string toDecrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("00000000000000000000000000000000");

        // AES-256 key 
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;


        rDel.Padding = PaddingMode.PKCS7;

        // better lang support 
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}
