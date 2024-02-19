using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    bool ingame;

    public static GameManager Instance;

    public GameModes gamemode;

    public GameObject baseController;
    public GameObject baseUIManager;

    public List<List<GameObject>> teams;

    int charaterDisplayed;

    public List<PjBase> pjList = new List<PjBase>();

    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask playerWallLayer;

    public GameObject damageText;

    public Color32 iceColor;
    public Color32 fireColor;
    public Color32 waterColor;
    public Color32 desertColor;
    public Color32 natureColor;
    public Color32 windColor;
    public Color32 lightningColor;
    public Color32 bloodColor;


    public enum GameModes
    {
        singleplayer, multiplayer
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartGame()
    {
        ingame = true;

        Instantiate(baseController);
        Instantiate(baseUIManager);

        yield return null;

    }

    public void OpenSelector()
    {
        SceneManager.LoadSceneAsync("CharacterSelector", LoadSceneMode.Additive);
    }
    public void CloseSelector()
    {
        SceneManager.UnloadSceneAsync("CharacterSelector");
    }
}
