using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MenuSystem
{
    public class MainMenu : MonoBehaviour
    {
        public Button play, playAi, exit;
        void Start()
        {
            play.onClick.AddListener(OnPlay);
            playAi.onClick.AddListener(OnPlayAi);
            exit.onClick.AddListener(OnExit);
        }

        private void OnPlayAi()
        {
            // GameEvents.Instance.StartGame();
            OnBuyMeTea();
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void OnPlay()
        {
            GameEvents.Instance.StartGame();
        }
       // [ContextMenu("Buy")]
        public async void OnBuyMeTea()
        {

            ///SendWithCapa("https://chapa.link/donation/view/DN-FXg4dUp4A1Lo",
            // (string error) => { Debug.Log(error); }, 
            //  (string sucess) => { Debug.Log(sucess); });


            UserData data = new UserData
            {
                amount = "10",
                currency = "ETB",
                email = "yayele88@gmail.com",
                first_name = "Yared",
                last_name = "Ayele",
                tx_ref = "tx-myecommerce12345"
            };

            var dataJson = JsonUtility.ToJson(data);
            Application.OpenURL("https://chapa.link/donation/view/DN-FXg4dUp4A1Lo"+data);
        }
        void SendWithCapa(string url, Action<string> onError, Action<string> onSucess)
        {
            StartCoroutine(SendPayment(url, onError, onSucess));
        }

        private IEnumerator SendPayment(string url, Action<string> onError, Action<string> onSucess)
        {

            UserData data = new UserData
            {
                amount = "10",
                currency = "ETB",
                email = "yayele88@gmail.com",
                first_name = "Yared",
                last_name = "Ayele",
                tx_ref = "tx-myecommerce12345"
            };

            var dataJson = JsonUtility.ToJson(data);

            using(UnityEngine.Networking.UnityWebRequest unityWebRequest = UnityEngine.Networking.UnityWebRequest.Post(url, dataJson))
            {
               // unityWebRequest.SetRequestHeader("Authorization", "Bearer CHASECK-AAAAAAAAAAAAAAAAAAA");
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
                    onError(unityWebRequest.error);
                else
                    onSucess(unityWebRequest.downloadHandler.text);
            }
        }
    }
}
struct UserData
{
    public string amount;
    public string currency;
    public string email;
    public string first_name;
    public string last_name;
    public string tx_ref;
}