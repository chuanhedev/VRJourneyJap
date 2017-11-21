using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;

public class VideoManager : SingleInstance<VideoManager> {
    public override void OnAwkae()
    {
        base.OnAwkae();
    }
    public MediaPlayer _mediaPlayer;
    private MediaPlayer.FileLocation _nextVideoLocation;
    private string _nextVideoPath;
    public bool _useFading = false;
    public string androidPath = "";
    public string pcPath = "E:/vitovideo/";
    string vitovideo = "";
    // Use this for initialization
    void  Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            vitovideo = androidPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            vitovideo = pcPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            vitovideo = pcPath;
        }
        //test code
        //Play("show1.mp4");
    }

    public void Play(string src)
    {
        LoadVideo(vitovideo+src, true);
    }

    public void PlayByPath(string path)
    {
        LoadVideo(path, true);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.F1))
        //{
        //    Play("show1.mp4");
        //}
        //if (Input.GetKeyUp(KeyCode.F2))
        //{
        //    Play("show2.mp4");
        //}
    }

    private void LoadVideo(string filePath, bool url = false)
    {
        // Set the video file name and to load. 
        if (!url)
            _nextVideoLocation = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
        else
            _nextVideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;
        _nextVideoPath = filePath;

        // IF we're not using fading then load the video immediately
        if (!_useFading)
        {
            // Load the video
            if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart))
            {
                Debug.LogError("Failed to open video!");
            }

        }
        else
        {
            StartCoroutine("LoadVideoWithFading");
        }
    }

    private IEnumerator LoadVideoWithFading()
    {
        const float FadeDuration = 0.25f;
        float fade = FadeDuration;

        // Fade down
        while (fade > 0f && Application.isPlaying)
        {
            fade -= Time.deltaTime;
            fade = Mathf.Clamp(fade, 0f, FadeDuration);

            Color _color = new Color(1f, 1f, 1f, fade / FadeDuration);
            _mediaPlayer.Control.SetVolume(fade / FadeDuration);

            yield return null;
        }

        // Wait 3 frames for display object to update
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Load the video
        if (Application.isPlaying)
        {
            if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart))
            {
                Debug.LogError("Failed to open video!");
            }
            else
            {
                // Wait for the first frame to come through (could also use events for this)
                while (Application.isPlaying && !_mediaPlayer.Control.IsPlaying() && _mediaPlayer.TextureProducer.GetTextureFrameCount() <= 0)
                {
                    yield return null;
                }

                // Wait 3 frames for display object to update
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
        }

        // Fade up
        while (fade < FadeDuration && Application.isPlaying)
        {
            fade += Time.deltaTime;
            fade = Mathf.Clamp(fade, 0f, FadeDuration);

            Color c = new Color(1f, 1f, 1f, fade / FadeDuration);
            _mediaPlayer.Control.SetVolume(fade / FadeDuration);

            yield return null;
        }
    }
}
