using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public abstract class ModalPopUpElement : MonoBehaviour
{
    #region Variables
    [Header("Holders")]
    [SerializeField] Transform headerTransform;
    [SerializeField] Transform contentTransform;
    [SerializeField] Transform footerTransform;

    [Header("Text fields")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI contentText;

    [Header("Buttons")]
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Button closeButton;

    [Header("Button text fields")]
    [SerializeField] TextMeshProUGUI yesButtonText;
    [SerializeField] TextMeshProUGUI noButtonText;
    [SerializeField] TextMeshProUGUI closeButtonText;

    [Header("Actions for Button Callback")]
    Action OnYesButtonAction;
    Action OnNoButtonAction;
    #endregion

    #region PopUp System
    public void CreatePopUp(string message, string title = null, Action yes=null, string yesText=null, Action no=null, string noText = null, bool isTimed = false)
    {
        //set title
        if(title != null)
        {
            headerTransform.gameObject.SetActive(true);
            titleText.text = title;
        }
        else
        {
            headerTransform.gameObject.SetActive(false);
        }
        //set message
        contentText.text = message;
        //set yes buttton
        bool hasYesAction = (yes != null);
        yesButton.gameObject.SetActive(hasYesAction);
        if (yesText != null)yesButtonText.text = yesText;
        OnYesButtonAction = yes;
        //set no button
        bool hasNoAction = (no != null);
        noButton.gameObject.SetActive(hasNoAction);
        if (noText != null) noButtonText.text = noText;
        OnNoButtonAction = no;
        //show modal
        TooglePopUpWindow(1f);
        //set timer
        if (isTimed)
        {
            StartCoroutine(AutoClose());
        }
    }
    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(2);
        TooglePopUpWindow(0f);
    }
    void TooglePopUpWindow(float toggle) => transform.DOScale(toggle, 0.1f);
    #endregion

    #region Button Callbacks
    public void OnYesClicked()
    {
        OnYesButtonAction?.Invoke();
        TooglePopUpWindow(0f);
    }
    public void OnNoClicked()
    {
        OnNoButtonAction?.Invoke();
        TooglePopUpWindow(0f);
    }
    public void OnCloseClicked() => TooglePopUpWindow(0f);
    
    #endregion
}
