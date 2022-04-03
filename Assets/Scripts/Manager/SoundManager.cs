using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    //����� ����� ����� �ҽ�
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    //����� Ŭ�� ĳ�̿�
    Dictionary<string, AudioClip> _audioClip = new Dictionary<string, AudioClip>();

    public void Init()
    {
        //������ ����� �� ���� audioSources�� ���� ����
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
            //���ϰ� ū ��� -> ĳ�� �ʿ�
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

    //�����Ŭ�� ĳ�� �Լ�
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

    //�޸� ���� ���� ���� ĳ���ص� ����� Ŭ���̶� �ҽ� ������
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
