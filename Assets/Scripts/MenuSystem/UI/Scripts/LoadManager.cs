using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;

public class LoadManager : MonoBehaviour
{
   [SerializeField] GameObject newMenuObject, privMenuObject;

    void Start()
    {
        Addressables.InitializeAsync().Completed += AddressablesManager_Done;
    }
    private void AddressablesManager_Done(AsyncOperationHandle<IResourceLocator> obj)
    {
        //Debug.Log("Initializing");
    }
    public void InstantiateMenuGameObjects(AssetReference menu,Transform transform)
    {
       // string loadingManu = ProjectConstants.UI.GetAddressUi(menu);
        //Debug.Log(loadingManu);
        privMenuObject = newMenuObject;
        
        Addressables.InstantiateAsync(menu, transform).Completed += (c) => 
        {
            newMenuObject = c.Result.gameObject;
            newMenuObject.transform.DOScale(1f, 0.5f);
            
            if (privMenuObject != null)
            {
                privMenuObject.transform.DOScale(0, 0.1f);
                DestroyMenuGameObjects(privMenuObject);
            }
        };
    }
    public void DestroyMenuGameObjects(GameObject menu)
    {
        Addressables.ReleaseInstance(menu);
        privMenuObject = newMenuObject;
        //menu.SetActive(false);
    }
    public void LoadScene()
    {
        Addressables.LoadSceneAsync("Assets/Scenes/SampleScene.unity", UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += (s) =>
       {

       };
    }
}
