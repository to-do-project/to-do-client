using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    //오디오 재생할 오디오 소스
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    //오디오 클립 캐싱용
    Dictionary<string, AudioClip> _audioClip = new Dictionary<string, AudioClip>();

    public void Init()
    {
        //게임이 종료될 때 까지 audioSources를 물고 있음
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);
        }

        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"AudioCLip Missing {path}");
                return;
            }
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();

        }
        else
        {
            //부하가 큰 방법 -> 캐싱 필요
            //AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            AudioClip audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log($"AudioCLip Missing {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    //오디오클립 캐싱 함수
    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (_audioClip.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            _audioClip.Add(path, audioClip);
        }

        return audioClip;
    }

    //메모리 부하 방지 위해 캐싱해둔 오디오 클립이랑 소스 날리기
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClip.Clear();
    }
}
