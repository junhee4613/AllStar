using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
[System.Serializable]
public class SoundManager
{
    public List<AudioClip> bgmList = new List<AudioClip>();
    public AudioSource bgm_Sound;
    public float BGMVolumeSave = 0.5f;
    public AudioSource sfx_Sound;
    public float SFXVolumeSave = 0.5f;
    [SerializeField]public AudioMixer mixer;
    public void SFX_Sound(AudioClip clip)
    {
        if (sfx_Sound == null)
        {
            GameObject tempOBJ = new GameObject() { name = "SoundEffect" };
            sfx_Sound = tempOBJ.AddComponent<AudioSource>();
        }
        //temp.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[1];
        sfx_Sound.volume = SFXVolumeSave;
        sfx_Sound.clip = clip;
        sfx_Sound.Play();
    }
    public void BGM_Sound(string SceneName)
    {
        for (int i = 0; i < bgmList.Count; i++)
        {
            if(bgmList[i].name == SceneName)
            {
                if (mixer == null)
                {
                    mixer = Resources.Load<AudioMixer>("SoundMixer");
                }
                if (bgm_Sound == null)
                {
                    bgm_Sound = new GameObject() { name = "BGMDJ" }.AddComponent<AudioSource>();
                }
                bgm_Sound.volume = BGMVolumeSave;
                bgm_Sound.clip = bgmList[i];
                bgm_Sound.loop = true;
                bgm_Sound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
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
/*    public void SceneLoadedSetting(Scene arg, LoadSceneMode arg1)
    {
        BGM_Sound(arg);
    }*/
}
