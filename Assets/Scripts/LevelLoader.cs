using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] string targetScene;
    private float animationTime = 1f;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    public void LoadScene()
    {
        LoadingData.sceneToLoad = targetScene;
        anim.SetTrigger("Start");
        StartCoroutine(StartFadeAnimation());

    }

    IEnumerator StartFadeAnimation()
    {
        yield return new WaitForSecondsRealtime(animationTime);
        SceneManager.LoadScene("Loading");
    }

}
