using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MenuSystem {
    public class UiManager : Manager<UiManager>
    {
        [SerializeField] Transform menuRegion;
        [SerializeField] public ModalPopUpManager _modal;

        GameObject currentMenu, privMenu;

        [SerializeField] private List<Menu> menus;
        private Dictionary<string, string> menuDictionary = new Dictionary<string, string>();

        private void Start()
        {
            // Addressables.InitializeAsync().Completed += AddressablesManager_Done;
            Initialize();
            LoadMenu("loading");
            GameEvents.Instance.OnLoadMainMenu += LoadMainMenuAtStart;
        }

        private void LoadMainMenuAtStart()
        {
            LoadMenu("Main_Menu");
        }
        private void AddressablesManager_Done(AsyncOperationHandle<IResourceLocator> obj)
        {
            Debug.Log("Initializing");
        }
        private void Initialize()
        {
            foreach (Menu menu in menus)
            {
                menuDictionary.Add(menu.menuName, menu.menuAddress.AssetGUID);
            }
        }
        public void LoadMenu(string menu)
        {
            bool ok = menuDictionary.TryGetValue(menu, out string value);
            if (ok)
            {
                InstantiateMenuGameObjects(value, menuRegion);
            }
            else
            {
                Debug.LogError("Menu with " + tag + "not found");
            }
        }
        public void InstantiateMenuGameObjects(string menu, Transform transform)
        {
            Addressables.InstantiateAsync(menu, transform).Completed += (c) =>
            {
                if (currentMenu != null) privMenu = currentMenu;

                currentMenu = c.Result.gameObject;
                currentMenu.transform.DOScale(1f, 1f);

                if (privMenu != null)
                {
                    RemoveMenuGameObject(privMenu);
                }
            };
        }
        void RemoveMenuGameObject(GameObject privMenu)
        {
            Addressables.ReleaseInstance(privMenu);
            //privMenuObject = newMenuObject;
            //privMenu.SetActive(false);
        }
    }

    [System.Serializable]
    public class Menu
    {
        public string menuName;
        public AssetReferenceGameObject menuAddress;
    }
}
