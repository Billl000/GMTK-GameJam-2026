using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public Animator transitionAnim;
    public float transitionTime = .8f;
    [SerializeField] public GameObject changerPrefab;

    private bool isTransitioning = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Load a scene by its exact name
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        changerPrefab.gameObject.SetActive(true);
        transitionAnim.SetTrigger("Start");
        //FindAnyObjectByType<AudioManager>().Play("Begin Background Music");
        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Load a scene by its index number in Build Settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneWithTransitionByIndex(sceneIndex));
    }

    IEnumerator LoadSceneWithTransitionByIndex(int sceneIndex)
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        changerPrefab.gameObject.SetActive(true);
        transitionAnim.SetTrigger("Start");
        //FindAnyObjectByType<AudioManager>().Play("Begin Background Music");
        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneWithTransitionByIndex(nextSceneIndex));
        }
        else
        {
            Debug.LogWarning("No more scenes to load. You are at the last scene.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
