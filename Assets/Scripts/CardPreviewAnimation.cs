﻿using UnityEngine;
using System.Collections;

public class CardPreviewAnimation : MonoBehaviour {

    public float cardSpawnPosition;

    private GameObject expandedCardObject;
    private CardController card;

    void Start() {
        card = gameObject.GetComponent<CardController>();
    }


    void OnMouseEnter() {
		if (!card.usable) return;
        if (!card.selected) {
            ShowExpandedCard();
        }
    }

    void OnMouseExit() {
        ShowNormalCard();
		card.HideSelect();
    }

    void OnMouseDown() {
    	if (!card.usable) return;
        ShowNormalCard();
		if (card.CanPlay()) card.ShowSelect();
    }

    private void ShowExpandedCard() {
        expandedCardObject = GenerateExpandedCard();
        renderer.enabled = false;
        foreach (Renderer r in GetComponentsInChildren(typeof(Renderer))) r.enabled = false;
        transform.Find("OutlineParticle").particleSystem.renderer.enabled = false;
    }

    private void ShowNormalCard() {
        Destroy(expandedCardObject);
        expandedCardObject = null;
        renderer.enabled = true;
        foreach (Renderer r in GetComponentsInChildren(typeof(Renderer))) r.enabled = true;
        transform.Find("OutlineParticle").particleSystem.renderer.enabled = true;
    }

    private GameObject GenerateExpandedCard() {
        Vector3 position = new Vector3(transform.position.x, Util.CardHeight, cardSpawnPosition);
        GameObject cardObject = Instantiate(gameObject, position, Util.CardRotation) as GameObject;
        Destroy(cardObject.collider);
        cardObject.transform.localScale = 1.2f * (new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
        cardObject.GetComponent<MoveToTransform>().Move(
	        new Vector3(transform.position.x, Util.CardHeight, cardSpawnPosition + 2),
	        cardObject.transform.rotation,
	        0.5f);
        return cardObject;
    }
}
