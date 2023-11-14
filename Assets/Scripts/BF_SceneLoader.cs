using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class BF_SceneLoader : MonoBehaviour
{
    public static BF_SceneLoader Instance { get; private set; }

    [SerializeField] private BF_Fader fader;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        if (fader != null)
        {
            fader.FadeOut();
            StartCoroutine(LoadSceneAfterFade(sceneName));
        }
        else
        {
            Debug.LogError("Fader reference is not set. Loading scene without fading.");
            SceneManager.LoadScene(sceneName);
        }
    }

    private IEnumerator LoadSceneAfterFade(string sceneName)
    {
        yield return new WaitForSeconds(fader.FadeDuration);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
