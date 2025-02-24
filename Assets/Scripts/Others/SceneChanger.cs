using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] Animator anim;
    public static SceneChanger Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    bool changing = false;
    string changingName = null;
    public void SwitchScene(string sceneName)
    {
        if (changing) return;
        changing = true;
        changingName = sceneName;
        anim.Play("SceneChange");
    }
    public void Switch()
    {
        SceneManager.LoadScene(changingName);
        anim.Play("SceneChangeEnd");
        changing = false;
    }
}
