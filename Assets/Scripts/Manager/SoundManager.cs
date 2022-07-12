using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    /*    //����� ����� ����� �ҽ�
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
        }*/

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;
    public bool onBGM = true;
    public bool onSFX = true;

    
    private AudioClip mainBgmAudioClip; //main BGM


    Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();

    public void Init()
    {
        //bgmPlayer�� sfxPlayer ����
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);
        }

        GameObject bgm = new GameObject { name = "bgmPlayer" };
        bgmPlayer = bgm.AddComponent<AudioSource>();
        bgm.transform.parent = root.transform;

        bgmPlayer.loop = true;

        GameObject sfx = new GameObject { name = "sfxPlayer" };
        sfxPlayer = sfx.AddComponent<AudioSource>();
        sfx.transform.parent = root.transform;

        mainBgmAudioClip = Managers.Resource.Load<AudioClip>("Sound/BGM/5 Stages of You - Kit Wheston _ Unminus");
        if (mainBgmAudioClip == null)
        {
            Debug.Log("mainBgmAudioClip is null");
        }

        if (PlayerPrefs.HasKey("volumeBGM")) masterVolumeBGM = Managers.Player.GetFloat("volumeBGM");
        else masterVolumeBGM = 1f;

        if (PlayerPrefs.HasKey("volumeSFX")) masterVolumeSFX = Managers.Player.GetFloat("volumeSFX");
        else masterVolumeSFX = 1f;

        if (PlayerPrefs.HasKey("onBGM")) onBGM = Managers.Player.GetInt("onBGM") == 1;
        else onBGM = true;

        if (PlayerPrefs.HasKey("onSFX")) onSFX = Managers.Player.GetInt("onSFX") == 1;
        else onSFX = true;
    }

    //�̸����� ȿ���� ���
    //��� ���� Ŭ�� �߰��ؼ� ���
    public void PlaySFXSound(string name, float volume = 1f, string path = null)
    {
        if (onSFX == false) return;

        if (audioClipDic.ContainsKey(name) == false)
        {
            if (path.Contains("Sound/") == false)
            {
                path = $"Sound/{path}/{name}";
            }

            AudioClip audioClip = GetOrAddAudioClip(name, path);
            if (audioClip == null)
            {
                Debug.Log($"AudioCLip Missing {path}");
                return;
            }
        }
        sfxPlayer.PlayOneShot(audioClipDic[name], volume * masterVolumeSFX);
    }

    //init���� �⺻���� ������ bgm ���
    //��� ���� Ŭ�� �����ؼ� ���
    public void PlayBGMSound(float volume = 1f, string path = null)
    {
        if (onBGM == false) return;

        AudioClip audioClip;
        if (path==null)
        {
            audioClip = mainBgmAudioClip;
        }
        else
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"AudioCLip Missing {path}");
                return;
            }
        }

        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
        }

        bgmPlayer.clip = audioClip;
        bgmPlayer.volume = volume * masterVolumeBGM;
        bgmPlayer.Play();
    }

    // BGM�� ������ �����ϴ� �ڵ�
    // <param name="volume"></param> ���� ��
    public void BGMSoundChange(float volume = 1f)
    {
        if (volume > 1 || volume < 0)
        {
            Debug.Log("���� >> ������ 0 ~ 1 ������ ���� �ƴմϴ�!");
        }
        bgmPlayer.volume = volume * masterVolumeBGM;
    }

    public void StopOrPlayBGM(bool play)
    {
        if(play && bgmPlayer.isPlaying == false) {
            bgmPlayer.Play();
        }
        else {
            bgmPlayer.Stop();
        }
        onBGM = play;
    }

    //sfx Ŭ�� ��Ͽ� �� Ŭ�� �߰�
    AudioClip GetOrAddAudioClip(string name, string path)
    {
        AudioClip audioclip = null;
        if (audioClipDic.TryGetValue(name, out audioclip) == false)
        {
            audioclip = Managers.Resource.Load<AudioClip>(path);
            audioClipDic.Add(name, audioclip);
        }
        return audioclip;
    }

    public void Clear()
    {
        bgmPlayer.clip = null;
        bgmPlayer.Stop();

        sfxPlayer.clip = null;
        sfxPlayer.Stop();

        audioClipDic.Clear();
    }

    public void PlayNormalButtonClickSound()
    {
        PlaySFXSound("��ư��",1f,"SFX");
    }

    public void PlayPopupSound()
    {
        PlaySFXSound("�˾�â", 1f, "SFX");
    }
}
