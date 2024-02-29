using Assets.Scripts.TileEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class AnimationManager : MonoBehaviour
{
    private List<ActiveAnimaiton> ActiveAnimationList;

    private bool IsAnimationCompleted;
    void Start()
    {
        ActiveAnimationList = new List<ActiveAnimaiton>();
    }

    void Update()
    {
       foreach (ActiveAnimaiton Animation in ActiveAnimationList) 
       {
            IsAnimationCompleted = true;
            foreach (SlideObject Object in Animation.Group)
            {
                Object.Timer += Time.deltaTime;
                Object.Percent = Mathf.Clamp01(Object.Timer / Object.Duration);
                switch(Object.EaseType)
                {
                    case EaseType.Lerp:
                        if(Object.AnimationType == AnimationType.Position)
                            Object.GameObject.transform.position = Vector3.Lerp(Object.StartPosition, Object.EndPosition, Object.Percent);
                        else
                            Object.GameObject.transform.localScale = Vector3.Lerp(Object.StartPosition, Object.EndPosition, Object.Percent);
                        break;
                }
                if (Object.Percent < 1)
                    IsAnimationCompleted = false;
            }
            if(IsAnimationCompleted)
            {
                Animation.OnCompleted.Invoke();
                ActiveAnimationList.Remove(Animation);
                break;
            }
       }
    }
    public void Slide(List<SlideObject> ObjectList,Action OnCompleted)
    {
        ActiveAnimaiton Anim = new ActiveAnimaiton();
        foreach (SlideObject obj in ObjectList)
        {
            Anim.Group.Add(obj);
        }
        Anim.OnCompleted = OnCompleted;
        ActiveAnimationList.Add(Anim);
    }
}

public enum AnimationType
{
    Position,
    Scale
}
public enum EaseType
{
    Lerp
}
public class SlideObject
{
    public AnimationType AnimationType;
    public GameObject GameObject;
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float Duration;
    public EaseType EaseType;
    public float Timer = 0.0f;
    public float Percent = 0;
}
public class ActiveAnimaiton
{
    public List<SlideObject> Group = new List<SlideObject>();
    public Action OnCompleted;
}