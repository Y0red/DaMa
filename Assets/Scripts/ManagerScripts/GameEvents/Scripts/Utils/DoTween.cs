using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class DoTween : MonoBehaviour
{
    [SerializeField] TweeningModes selectedMode;

    public float time;
    void Start()
    {
        switch (selectedMode)
        {
            case TweeningModes.Spinner:
                DoSpin();
                break;
            case TweeningModes.Scale:
                DoScale();
                break;
            case TweeningModes.Rotator:
                DoRotator();
                break;
            case TweeningModes.ScaleUpDown:
                DoScaleUpDown();
                break;
            case TweeningModes.Fade:
                Dofade();
                break;
        }  
        //text.DOFade(0, 2).OnComplete(myFunction);
    }

    private void Dofade()
    {
        //
    }

    private void DoScaleUpDown()
    {
        transform.DOShakeScale(time, .3f, 3, 90f).onComplete += delegate
        {
            DoScaleUpDown();
        };
    }

    private void DoRotator()
    {
       float angle = 90;
        transform.DORotate(new Vector3(0, 0, angle), time / 4f)
           .onComplete += delegate
           {
                 if (angle <= -360) angle = 0;

               DoRotator();
            };
    }

    private void DoScale()
    {
        transform.DOScale(Vector3.zero, time);
    }

    private void DoSpin()
    {
        float angle = 90;
        transform.DORotate(new Vector3(0, 0, angle), time / 4f)
           .onComplete += delegate
           {
               if (angle <= -360) angle = 0;

               DoRotator();
           };
    }
}
[System.Serializable]
public enum TweeningModes
{
    Spinner, Rotator, Scale, ScaleUpDown, Fade
}