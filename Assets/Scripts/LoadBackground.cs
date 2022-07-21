using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadBackground : MonoBehaviour
{
    // Start is called before the first frame update
    string winURL = "https://drive.google.com/uc?export=download&id=1wnmtx8YTZdWWBIxFkOOELkQtrvUTb9Sq";
    string androidURL = "https://drive.google.com/uc?export=download&id=1s6q9ayti9xq0FdCHaXU6OykFvPNsKZ20";
    string iosURL = "https://drive.google.com/uc?export=download&id=1r70RouKJ9lsxiWcbf0pTkvKmLM77YX2Q";
    string url;
    GameController gameController;
    public void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    public void StartDownload()
    {
        url = winURL;
        if (Application.platform == RuntimePlatform.Android)
            url = androidURL;
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            url = iosURL;

        StartCoroutine(Download());
    }
    IEnumerator Download()
    {
        while (!Caching.ready)
            yield return null;
               
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.Log(uwr.error);            
            else
            {
                AssetBundle abn = DownloadHandlerAssetBundle.GetContent(uwr);                
                var bks = abn.GetAllAssetNames();
                foreach (var sprite in bks)
                {
                    var req = abn.LoadAssetAsync(sprite, typeof(Sprite));
                    yield return req;
                    gameController.backgrounds.Add(req.asset as Sprite);
                }
            }
        }
    }
}
