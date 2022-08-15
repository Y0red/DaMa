using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectConstants 
{
    public const string _LoginRememberKey = "Login_Remember";
    public const string _emailKey = "EMAIL_KEY";
    public const string _passwordKey = "PASSWORD_KEY";
    public const string _nameKey = "NAME_KEY";
    public const string _phoneKey = "PHONE_KEY";
    public const string _revenuKey = "REV_KEY";
    public const string _pointsKey = "P_KEY";
    public const string _watchedAdsKey = "WAD_KEY";
    public const string _idKey = "ID_KEY";

    #region RemoteCongigs
    public class RemoteConfig
    {
        public static string TermsOfServiceLink;
        public static string PrivacyPolicyLink;
    }
    #endregion

    #region PlayerPrefs
    /// <summary>
    /// Remember the user next time they log in
    /// This is used for Auto-Login purpose.
    /// </summary>
    public static bool RememberMe
    {
        get
        {
            return PlayerPrefs.GetInt(_LoginRememberKey, 0) == 0 ? false : true;
        }
        set
        {
            PlayerPrefs.SetInt(_LoginRememberKey, value ? 1 : 0);
        }
    }
    public static string GetData(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    public static void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        Debug.Log("saved");
    }
    public static void ClearRememberMe(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    #endregion

    #region UI_Menu
    public static class UI
    {
        public static readonly Dictionary<Menu, string> MenuAddresses = new Dictionary<Menu, string> 
        {
            {Menu.Loading, "Assets/Prifab/LoadingMenu.prefab" },
            {Menu.LogIn, "Assets/Prifab/LogInPage.prefab" },
            {Menu.AdMan, "Assets/Prifab/AdManagerMenu.prefab" },
            {Menu.Regestration, "Assets/Prifab/RigisterPage.prefab" },
        };
        public static  string GetAddressUi(Menu mm)
        {
            bool ok = MenuAddresses.TryGetValue(mm, out string value);
            if (ok)
            {
                return value;
             
            }
            return "Empty"; 
        }

        public enum Menu
        {
            Loading,
            LogIn,
            Regestration,
            AdMan,
            NONE,
        }
    }
    #endregion

    [System.Serializable]
    public enum SigningType
    {
        SignedIn,
        SignedOut,
        LogedIn
    }
    public class CashUserData
    {
        public static string id;
        public static string name;
        public static string email;
        public static string phone;
        public static int watchedAd;
        public static float revenu;
        public static float point;
    }

}
