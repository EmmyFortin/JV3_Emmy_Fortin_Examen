using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Loading Canvas")]
    [SerializeField] private GameObject _loaderCanvas;

    [Header("Progress bar")]
    [SerializeField] private Image _progressBar;


    private float _target;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNewGame(){
        SceneManager.LoadScene(Scene.Level01.ToString());
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

    public void QuitGame(){
        Application.Quit();
    }

    public async void LoadAsyncScene(Scene sceneName){
        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName.ToString());
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do {
            //await Task.Delay(100);
            await Task.Delay(TimeSpan.FromSeconds(.2));
            //_progressBar.fillAmount = scene.progress;
            _target = scene.progress;
        } while(scene.progress < 0.9f);

        scene.allowSceneActivation = true;

        _loaderCanvas.SetActive(false);

    }

    void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    public void LoadSceneTest(string levelToLoad){
        _loaderCanvas.SetActive(true);

        StartCoroutine(LoadLevelAsych(levelToLoad));

    }

    IEnumerator LoadLevelAsych(string levelToLoad){
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _progressBar.fillAmount = progressValue;
            yield return null;
        } 
        _loaderCanvas.SetActive(false);

    }

    public enum Scene{
        MainMenu,
        Level01,
        Level02
    }
}


