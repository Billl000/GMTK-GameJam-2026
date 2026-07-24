using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator transitionAnim;
    public float transitionTime = 1f;


    // Load a scene by its exact name
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transitionAnim.SetTrigger("Start");
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
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
