using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static bool isFinished = false;

    SplashManager splashManager;
    CameraController cameraController;

    [SerializeField] Image img_cutScene;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        splashManager = FindObjectOfType<SplashManager>();
    }

    public bool CheckCutScene()
    {
        return img_cutScene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneCoroutine(string cutSceneName, bool isShow)
    {
        SplashManager.isFinished = false;
        StartCoroutine(splashManager.FadeOut(true, false));
        yield return new WaitUntil(() => SplashManager.isFinished);

        if (isShow)
        {
            Sprite t_Sprite = Resources.Load<Sprite>("CutScenes/" + cutSceneName);
            if (t_Sprite != null)
            {
                img_cutScene.gameObject.SetActive(true);
                img_cutScene.sprite = t_Sprite;
                cameraController.CameraTargetting(null, 0.1f, true, false);
            }
            else
                Debug.LogError("잘못된 컷신 cg 파일 이름입니다");
        }
        else
            img_cutScene.gameObject.SetActive(false);

        SplashManager.isFinished = false;
        StartCoroutine(splashManager.FadeIn(true, false));
        yield return new WaitUntil(() => SplashManager.isFinished);

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}
