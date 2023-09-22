using Card;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    ScrollMechanic scrollMechanic;
    public int MaxTime;
    private int CurrentTime;

    public CardManager cardManager;

    public CanvasGroup failCanvas;

    public CanvasGroup[] countDown;


    void Start()
    {
        countDown[0].DOFade(1, 0.3f).OnComplete(delegate
        {
            countDown[0].DOFade(0, 0.3f).SetDelay(0.4f).OnComplete(delegate
            {
                Destroy(countDown[0].gameObject);
            });
            countDown[1].DOFade(1, 0.3f).SetDelay(0.7f).OnComplete(delegate
            {
                countDown[1].DOFade(0, 0.3f).SetDelay(0.4f).OnComplete(delegate
                {
                    Destroy(countDown[1].gameObject);
                });
                countDown[2].DOFade(1, 0.3f).SetDelay(0.7f).OnComplete(delegate
                {
                    countDown[2].DOFade(0, 0.3f).SetDelay(0.4f).OnComplete(delegate
                    {
                        Destroy(countDown[2].gameObject);
                        StartCoroutine(TimerCoroutine());
                    });
                });
            });
        });
        CurrentTime = MaxTime;
        scrollMechanic = GetComponent<ScrollMechanic>();
        List<string> list = new List<string>();
        for(int i = MaxTime; i >= 0; i--)
        {
            list.Add(i.ToString());
        }
        scrollMechanic.Initialize(list);
    }

    IEnumerator TimerCoroutine()
    {
        while (CurrentTime > 0 && !cardManager.isClear)
        {
            CurrentTime--;
            scrollMechanic.JumpToLerp(MaxTime - CurrentTime);
            yield return new WaitForSeconds(1);  
        }

        if(!cardManager.isClear && CurrentTime == 0)
        {
            failCanvas.DOFade(1, 1f);
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
    }
}
