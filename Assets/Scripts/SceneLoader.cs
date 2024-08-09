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
        //StopCoroutine(loadScreenTime());
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
        operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
        if (operation == null)
        {
            Debug.Log("no opeartion found");
            yield break;
        }
        while (!operation.isDone)
        {
            // Optionally, you can update a loading UI here
            yield return null;
        }
    }

}
