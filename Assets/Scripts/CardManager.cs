using DG.Tweening;
using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Card
{
    public class CardManager : MonoBehaviour
    {
       
        private Card[] cards;

        public Sprite[] cardSheet;

        private Card previousCard;

        public CanvasGroup clearCanvas; 
        public bool isClear; 

        public Text scoreText; 
        public Text leftText;

        public Text[] endText; 

        void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                PlayerPrefs.DeleteKey("TempScore");
            }
            scoreText.text = "<size=40>점수</size>\n" + (PlayerPrefs.GetInt("TempScore", 0));
            
            cards = transform.GetChild(0).GetComponentsInChildren<Card>();
            cards = cards.OrderBy(x => new System.Random().Next()).ToArray();
            for (int i = 0; i < cards.Length; i += 2)
            {
                int num = new System.Random().Next(52);
                Card card = cards[i];
                card.SetData((Card.CardType)(num / 13), (byte)(num % 13), cardSheet[num], Click);
                card = cards[i + 1];
                card.SetData((Card.CardType)(num / 13), (byte)(num % 13), cardSheet[num], Click);
            }
        }

        void Click(Card card)
        {
            if(previousCard != null)
            {
                if(ReferenceEquals(previousCard, card))
                {
                    previousCard = null;
                    return;
                }
                if(previousCard.Equals(card))
                {
                    AudioPlayer.GetInstance().OnClick();
                    
                    card.Clear();
                    previousCard.Clear();
                    
                    previousCard = null;

                    int score = int.Parse(scoreText.text.Split(new string[] { "\n" }, System.StringSplitOptions.None)[1]);
                    scoreText.text = "<size=40>점수</size>\n" + (score + 100);
                    PlayerPrefs.SetInt("TempScore", score + 100);
                    if(PlayerPrefs.GetInt("HighScore") < score + 100)
                    {
                        PlayerPrefs.SetInt("HighScore", score + 100);
                    }
                    leftText.text = "<size=40>남은 타일</size>\n" + cards.Count(x => !x.isDeleted);

                    endText[0].text = "!    C l e a r    !\n<size=60>최종 스코어</size>\n<size=80>" + (score + 100) + "</size>\n<size=60>최고 스코어</size>\n<size=80>" + PlayerPrefs.GetInt("HighScore", 0) + "</size>";
                    endText[1].text = "F a i l e d\n<size=60>최종 스코어</size>\n<size=80>" + (score + 100) + "</size>";
                    ClearCheck(); 
                    return;
                }
                card.Flip();
                previousCard.Flip();
                previousCard = null;
                return;
            }
            previousCard = card;
        }

        void ClearCheck()
        {
            isClear = true;
            for (int i = 0; i < cards.Length; i++)
            {
                if(!cards[i].isDeleted)
                {
                    isClear = false;
                    break;
                }
            }

            if (isClear)
            {
                clearCanvas.DOFade(1, 1f).OnComplete(delegate
                {

                    StartCoroutine(FadeCoroutine());
                    

                    int score = PlayerPrefs.GetInt("HighScore", 0);
                    if(PlayerPrefs.GetInt("TempScore", 0) > score)
                    {
                        PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("TempScore", 0));
                        PlayerPrefs.Save();
                    }
                });
            }
        }

        IEnumerator FadeCoroutine()
        {
            yield return new WaitForSeconds(2f);
            if(SceneManager.GetActiveScene().buildIndex + 1 == 5)
            {
                PlayerPrefs.DeleteKey("TempScore");
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 == 4 ? 0 : SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
