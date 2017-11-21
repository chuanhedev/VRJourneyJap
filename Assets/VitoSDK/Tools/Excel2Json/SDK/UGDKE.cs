using UnityEngine;
using System.Collections;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib;
using System.Text;
public class VirtualFile
{
    public int offset;
    public int lenght;
    public string name;
}
public class ZipUtility
{
    private static string rootPath = Application.dataPath+"/Excel2Json";

    private static byte[] memZip(byte[] src)
    {
        MemoryStream ms = new MemoryStream();
        GZipOutputStream gzipOutStream = new GZipOutputStream(ms);
        gzipOutStream.Write(src, 0, src.Length);
        gzipOutStream.Close();
        ms.Close();
        return ms.ToArray();
    }

    public static Dictionary<string, string> readAllVF()
    {
        Dictionary<string, string> vfMap = new Dictionary<string, string>();
        List<VirtualFile> vfList;
        //Debug.Log("datapath"+Application.persistentDataPath);
        FileStream fsContent = new FileStream(Application.persistentDataPath + "/res.db", FileMode.Open, FileAccess.Read);
        byte[] contentBytes = new byte[fsContent.Length];
        fsContent.Read(contentBytes, 0, (int)fsContent.Length);
        //Debug.Log(contentBytes.Length);
        fsContent.Close();
        FileStream fsDesc = new FileStream(Application.persistentDataPath + "/resl.db", FileMode.Open, FileAccess.Read);
        byte[] descBytes = new byte[fsDesc.Length];
        fsDesc.Read(descBytes, 0, (int)fsDesc.Length);
        fsDesc.Close();
        //Debug.Log(descBytes.Length);
        string descEncoded = Encoding.UTF8.GetString(descBytes);
        vfList = JsonConvert.DeserializeObject<List<VirtualFile>>(descEncoded);


        foreach (VirtualFile vf in vfList)
        {
            //Debug.Log(vf.name+":"+vf.offset+":"+vf.lenght);
            byte[] bytes = new byte[vf.lenght];
            Array.Copy(contentBytes, vf.offset, bytes, 0, vf.lenght);
            string unzipedStr = unZipBytes(bytes);
            vfMap.Add(vf.name, unzipedStr);
        }
        return vfMap;
    }

    public static string unZipBytes(byte[] src)
    {
        GZipInputStream gzi = new GZipInputStream(new MemoryStream(src));
        MemoryStream ms = new MemoryStream();
        int count = 0;
        byte[] data = new byte[4096];
        while ((count = gzi.Read(data, 0, data.Length)) != 0)
        {
            ms.Write(data, 0, count);
        }
        ms.Close();
        return System.Text.Encoding.UTF8.GetString(ms.ToArray());
    }

    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="srcPath">Source path.</param>
    public static void zipData(string srcPath = "")
    {
        if (string.IsNullOrEmpty(srcPath))
        {
            srcPath = rootPath + "/ExportData/";
        }
        FileStream filesContent = new FileStream(Application.streamingAssetsPath + "/res.db", FileMode.Create, FileAccess.ReadWrite);
        FileStream filesDesc = new FileStream(Application.streamingAssetsPath + "/resl.db", FileMode.Create, FileAccess.ReadWrite);
        DirectoryInfo dirInfo = new DirectoryInfo(srcPath);
        FileInfo[] files =dirInfo.GetFiles();
        List<VirtualFile> vfList = new List<VirtualFile>();
        int offset = 0;
        //char[] splitChar = new char[1] { '\\' };
        foreach (FileInfo fi in files)
        {
            if (fi.Name.EndsWith(".meta"))
            {
                continue;
            }
            byte[] zipContent = memZip(FileUtility.getFileByte(fi.FullName));

            VirtualFile vf = new VirtualFile();
            vf.name = fi.Name;
            vf.lenght = zipContent.Length;
            vf.offset = offset;
            offset += vf.lenght;
            //Debug.Log(vf.name+":"+vf.lenght+":"+vf.offset);
            vfList.Add(vf);
            //Debug.Log(zipContent.Length);
            filesContent.Write(zipContent, 0, zipContent.Length);
        }
        string vfListStr = JsonConvert.SerializeObject(vfList);
        byte[] vfListBytes = Encoding.UTF8.GetBytes(vfListStr);
        filesDesc.Write(vfListBytes, 0, vfListBytes.Length);
        filesDesc.Flush();
        filesDesc.Close();
        filesContent.Flush();
        filesContent.Close();
    }
}
public partial class SDK
{
    public static void Log(string log)
    {
#if DEBUG
        Debug.Log(log);
#endif
    }
}

public class Toolsa
{
    public static List<int> convertStringToArray(string source)
    {
        List<int> listArray = new List<int>();
        try
        {
            string[] subStrings = source.Split(new char[] { ',', '，' });
           // List<string> strItems = new List<string>();

            for (int i = 0; i < subStrings.Length; i++)
            {
                if (subStrings[i].Contains("-") || subStrings[i].Contains("-"))
                {
                    string[] subsubStr = subStrings[i].Split(new char[] { '-', '-' });
                    int start = Int32.Parse(subsubStr[0]);
                    int end = Int32.Parse(subsubStr[1]);
                    for (int j = start; j <= end; j++)
                    {
                        listArray.Add(j);
                    }
                }
                else if (subStrings[i].Contains("*") || subStrings[i].Contains("×"))
                {
                    string[] subsubStr = subStrings[i].Split(new char[] { '*', '×' });
                    int value = Int32.Parse(subsubStr[0]);
                    int times = Int32.Parse(subsubStr[1]);
                    for (int k = 0; k <= times; k++)
                    {
                        listArray.Add(value);
                    }
                }
                else {
                    listArray.Add(Int32.Parse(subStrings[i]));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
        return listArray;
    }


}

public class FileUtility
{
    public static void save2File(string fileName, string content, string path)
    {
        string destPath = path + fileName;
        FileInfo fileInfo = new FileInfo(destPath);
        if (File.Exists(destPath))
        {
            File.Delete(destPath);
        }
        StreamWriter sw = fileInfo.CreateText();
        sw.Write(content);
        sw.Flush();
        sw.Close();
        //sw.Dispose();
    }
    public static string getFileString(string path)
    {
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string content = sr.ReadToEnd();
            return content;
        }
        else {
            return "";
        }


    }

    public static byte[] getFileByte(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fs.Length];
        //fs.ReadByte(bytes,0,fs.Length);
        fs.Read(bytes, 0, (int)fs.Length);
        fs.Close();
        return bytes;
    }

}
