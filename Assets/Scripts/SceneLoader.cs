using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI tipToolText;


    private float minWaitTime;
    private float animationTime = 1f;
    private AsyncOperation operation;
    [SerializeField] Animator anim; 
    private void Start()
    {
        minWaitTime = 5;
        StartCoroutine(loadScreenTime());
    }

    

    IEnumerator loadScreenTime()
    {
        yield return new WaitForSecondsRealtime(minWaitTime);
        anim.SetTrigger("Start");
        StartCoroutine(StartCrossFadeAnimation());
    }

    IEnumerator StartCrossFadeAnimation()
    {
        yield return new WaitForSecondsRealtime(animationTime);
        SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
    }

}
