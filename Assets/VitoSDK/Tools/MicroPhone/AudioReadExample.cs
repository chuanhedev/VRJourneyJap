using UnityEngine;
using System.Collections;

public class AudioReadExample : MonoBehaviour
{
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;
    void Start()
    {
        AudioClip myClip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }
    void OnAudioRead(float[] data)
    {
        int count = 0;
        string str = "";
        while (count < data.Length)
        {
            data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate));
            str += data[count].ToString();
            position++;
            count++;
        }
        Debug.Log(str);
    }
    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}