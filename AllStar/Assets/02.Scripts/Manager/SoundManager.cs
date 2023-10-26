using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public List<AudioClip> bgmList = new List<AudioClip>();
    public static SoundManager Instance { get; private set; }
    public AudioSource bgm_Sound;
    public AudioMixer mixer;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            SceneManager.sceneLoaded += SceneLoadedSetting;
        }
        else
        {
            Destroy(this);
        }
    }
    public void SFX_Sound(AudioClip clip)
    {
        GameObject go = new GameObject();
        AudioSource temp = go.AddComponent<AudioSource>();
        //temp.outputAudioMixerGroup = mixer.FindMatchingGroups("그룹 안에 있는 이름")[0];
        temp.clip = clip;
        temp.Play();
        Destroy(go, temp.clip.length);
    }
    public void BGM_Sound(Scene arg)
    {
        for (int i = 0; i < bgmList.Count; i++)
        {
            if(bgmList[i].name == arg.name)
            {
                bgm_Sound.clip = bgmList[i];
                bgm_Sound.loop = true;
                bgm_Sound.outputAudioMixerGroup = mixer.FindMatchingGroups("그룹 안에 있는 이름")[0];
                bgm_Sound.Play();
            }
        }
    }
    public void SFX_Sound_Volume(float val)
    {
        if (val > 0)
        {
            mixer.SetFloat("파라미터", Mathf.Log10(val) * 20);
        }
        else
            mixer.SetFloat("파라미터", -80);

    }
    public void BGM_Sound_Volume(float val)
    {
        if (val > 0)
        {
            mixer.SetFloat("파라미터", Mathf.Log10(val) * 20);
        }
        else
            mixer.SetFloat("파라미터", -80);
    }
    public void SceneLoadedSetting(Scene arg, LoadSceneMode arg1)
    {
        BGM_Sound(arg);
    }
}
