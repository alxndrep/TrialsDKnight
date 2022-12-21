using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public TextMeshProUGUI textProgress;
    [SerializeField] public TextMeshProUGUI title;
    
    public void LoadLevel (string sceneName)
    {
        title.text = "Loading";
        textProgress.text = "0%";
        StartCoroutine(LoadSceneCoroutine(sceneName));
    } 

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if(GameObject.FindGameObjectWithTag("Player")) PlayerMovimiento.Player.sePuedeMover = false;
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<Animator>().SetTrigger("Loading");
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            textProgress.text = ((int)(progress * 100)) + "%";
            if(progress >= 0.9f)
            {
                textProgress.text = "100%";
                title.text = "Loaded";
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
