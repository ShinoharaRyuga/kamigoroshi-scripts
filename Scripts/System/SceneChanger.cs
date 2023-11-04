using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
    }
    public AsyncOperation ReturnAsyncOperation(string name)
    {
        var async = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

        return async;
    }
}
