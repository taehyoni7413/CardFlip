using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    public class Card : MonoBehaviour
    {

        public override bool Equals(object op1)
        {
            return cardType == ((Card)op1).cardType && cardNumber == ((Card)op1).cardNumber;
        }
        public override int GetHashCode()
        {
            return cardNumber;
        }

        public enum CardType : byte
        {
            Spade,
            Diamond,
            Heart,
            Clover
        }

        private CardType cardType;
        private byte cardNumber;
        private Image image;
        private RectTransform rectTransform;

        private Sprite backSprite;
        private Sprite forwardSprite;
        private Action<Card> callback;

        public bool isDeleted;

        private void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            backSprite = image.sprite;
        }

        public void SetData(CardType cardType, byte number, Sprite sprite, Action<Card> clickCallback)
        {
            this.cardType = cardType;
            this.cardNumber = number;
            forwardSprite = sprite;
            this.callback = clickCallback;
        }

        public void Flip()
        {
           
            StartCoroutine(FlipCoroutine(false));
        }

        private IEnumerator FlipCoroutine(bool b)
        {
            image.raycastTarget = false;
            rectTransform.DOScaleX(image.sprite == backSprite ? -1 : 1, 0.25f);
            rectTransform.DORotate(new Vector3(0, image.sprite == backSprite ? 180 : 0), 0.25f); 
            yield return new WaitForSeconds(.125f);
            image.sprite = image.sprite == backSprite ? forwardSprite : backSprite;
            yield return new WaitForSeconds(.5f);
            if(b)
            {
                callback?.Invoke(this);
            }
            image.raycastTarget = true;
        }

        public void OnClick()
        {
            StartCoroutine(FlipCoroutine(true));
        }

        public void Clear()
        {
            isDeleted = true;
            image.raycastTarget = false;
            image.DOFade(0, .25f);
        }
    }
}

