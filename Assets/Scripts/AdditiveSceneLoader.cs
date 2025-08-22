using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ����Э��

public class AdditiveSceneLoader : MonoBehaviour
{
    void Start()
    {
        //TODO: ���UI����

        int i = 0;
        while (true)
        {
            i++;
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // ȷ������·����Ч
            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogError("Scene path is empty or invalid for index " + i + " .");
                break; // ���·����Ч���˳�ѭ��
            }
            StartCoroutine(LoadAdditiveSceneAsync(scenePath));
        }
    }

    IEnumerator LoadAdditiveSceneAsync(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
        {
            Debug.LogError("Scene '" + sceneName + "' is not in Build Settings!");
            yield break;
        }

        // ��ʼ�첽���Ӽ���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // �ȴ��������
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading " + sceneName + " progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // �ȴ���һ֡
        }

        Debug.Log("Scene '" + sceneName + "' loaded additively.");

        // ��ѡ�����¼��صĳ�������Ϊ��������Ա���Hierarchy�и����׹���
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    // ж��һ�����Ӽ��صĳ���
    public void UnloadAdditiveScene(string sceneName)
    {
        // ��鳡���Ƿ��Ѽ���
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log("Scene '" + sceneName + "' unloaded.");
        }
        else
        {
            Debug.LogWarning("Scene '" + sceneName + "' is not currently loaded.");
        }
    }
}
