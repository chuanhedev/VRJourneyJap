using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MicUtils {

    public static void Save(AudioClip clip, string path)
    {
        string filePath = Path.GetDirectoryName(path);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        using (FileStream fileStream = CreateEmpty(path))
        {
            ConvertAndWrite(fileStream, clip);
            WriteHeader(fileStream, clip);
        }
    }

    private static FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //preparing the header  
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    private static void WriteHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);

    }

    private static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; //to convert float to Int16  

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        fileStream.Write(bytesData, 0, bytesData.Length);
    }

	public static string error;
	public static string text;

	public static IEnumerator Upload(string fromPath, string toPath) {
		Debug.Log ("uploading from " + fromPath + " to " + toPath);
		MicUtils.error = "";
		MicUtils.text = "";
		WWW www = new WWW ("file:///"+ fromPath);
		yield return www;
		if(!String.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("load complete!");
		}
		byte[] myData = www.bytes;
		Debug.Log("myData " + myData.Length + " " + toPath + "?filename=" + fromPath.Substring(fromPath.LastIndexOf('/')+1));

		using (UnityWebRequest upload = UnityWebRequest.Put (toPath + "?filename=" + fromPath.Substring (fromPath.LastIndexOf ('/') + 1), myData)) {
			//upload.timeout = 5;
            
			yield return upload.Send ();
			//Debug.Log("~~~~~" + upload.downloadHandler.text);
			if (upload.isError) {
				Debug.Log (upload.error);
				MicUtils.error = upload.error;
			} else {
				Debug.Log ("Upload complete! " + upload.downloadHandler.text);
				if(upload.downloadHandler.text == "done")
					MicUtils.text = MicStatus.UploadSuccss;
				else
					MicUtils.text = MicStatus.ErrorUnknown;
			}
		}
    }


    public static IEnumerator UploadFileToServer(byte[]data, string url, string filename, string foldername)
    {
        Debug.Log(data.Length);
        Debug.Log(url + "?filename=" + filename + "&folder=" + foldername);
        using (UnityWebRequest upload = UnityWebRequest.Put(url + "?filename=" + filename + "&folder=" + foldername, data))
        {
            yield return upload.Send();
            //Debug.Log("~~~~~" + upload.downloadHandler.text);
            if (upload.isError)
            {
                Debug.Log(upload.error);
                MicUtils.error = upload.error;
            }
            else
            {
                Debug.Log("Upload complete! " + upload.downloadHandler.text);
                if (upload.downloadHandler.text == "done")
                    MicUtils.text = MicStatus.UploadSuccss;
                else
                    MicUtils.text = MicStatus.ErrorUnknown;
            }
        }
    }
}

public static class MicStatus{
	public static string Uploading = "uploading";
	public static string UploadSuccss = "uploadsuccess";
	public static string ErrorUnknown = "errorunknown";
}
