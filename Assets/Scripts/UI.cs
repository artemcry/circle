using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text timeAndScore;
    [SerializeField]
    private GameController gController;
    [SerializeField]
    private GameObject btn_Start;
    [SerializeField]
    private GameObject btn_Stop;
    [SerializeField]
    private GameObject nextLevel;
    [SerializeField]
    private SpriteRenderer background;

    private Coroutine timerCoroutine;
    private System.DateTime startTime;

    public void Start()
    {
        gController.backgrounds.Add(background.sprite);
    }
    public void UpdateScore()
    {
        var t = System.DateTime.Now - startTime;
        timeAndScore.text = $"{gController.Score}/{gController.scoreForNextLevel}\n" +
            $"{t.ToString(@"mm\:ss")}";
    }
    private IEnumerator StartTimer()
    {
        while (true)
        {
            UpdateScore();
            yield return new WaitForSeconds(1f);
        }
    }
    public void ResetTimer()
    {
        startTime = System.DateTime.Now;
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(StartTimer());
    }
    public void StartGame(int lvl = 1)
    {
        var r = new System.Random();
        background.sprite = gController.backgrounds[r.Next(gController.backgrounds.Count)];              
        StartCoroutine(StartGameInTime(1, lvl));
    }
    public void StopGame()
    {
        btn_Start.SetActive(true);
        btn_Stop.SetActive(false);
        timeAndScore.gameObject.SetActive(false);
        gController.StopGame();
    }
    public void ResetGame()
    {
        StopGame();
        gController.ResetLevel();
    }
    IEnumerator StartGameInTime(float time, int lvl)
    {
        StopGame();
        btn_Start.SetActive(false);
        nextLevel.SetActive(true);
        nextLevel.GetComponent<Text>().text = $"Level {lvl}";
        yield return new WaitForSeconds(time);

        nextLevel.SetActive(false);
        btn_Stop.SetActive(true);
        timeAndScore.gameObject.SetActive(true);
        gController.StartGame();
    }
}
