using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Transform spinner;
    public TextMeshProUGUI message;

    public float spinDuration;

    private void Start()
    {
        RotateSpinner(-90);
    }

    void RotateSpinner(float angle)
    {
        spinner.DORotate(new Vector3(0, 0, angle), spinDuration / 4f)
            .onComplete += delegate
        {
            // if (angle <= -360) angle = 0;

            // RotateSpinner(angle - 90);
            GameEvents.Instance.LoadMainMenu();
        };
    }
}
