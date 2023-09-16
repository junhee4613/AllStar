using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Extensions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public static class FSMExtension
{
    public static void SetDefault(this BaseState Target,Animator anim,ref Dictionary<string,BaseState> list,string Key)
    {
        Target.animator = anim;
        list.Add(Key,Target);
    }
}