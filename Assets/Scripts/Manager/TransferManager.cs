using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    string locationName;

    SplashManager splashManager;
    InteractionController interactionController;

    public static bool isFinished = true;

    private void Start()
    {
        splashManager = FindObjectOfType<SplashManager>();
        interactionController = FindObjectOfType<InteractionController>();
    }
    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        isFinished = false;
        interactionController.SettingMouseUI(false);
        SplashManager.isFinished = false;
        StartCoroutine(splashManager.FadeOut(false, true));
        yield return new WaitUntil(() => SplashManager.isFinished);
        locationName = p_LocationName;
        TransferSpawnManager.canSpawn = true;
        SceneManager.LoadScene(p_SceneName);
    }

    public IEnumerator Done()
    {
        SplashManager.isFinished = false;
        StartCoroutine(splashManager.FadeIn(false, true));
        yield return new WaitUntil(() => SplashManager.isFinished);

        isFinished = true;
        yield return new WaitForSeconds(0.3f);
        if (!DialogueManager.isWaiting)
            interactionController.SettingMouseUI(true);
    }
    
    public string GetLocationName()
    {
        return locationName;
    }
}
