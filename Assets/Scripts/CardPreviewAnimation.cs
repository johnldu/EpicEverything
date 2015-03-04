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
        if (!card.selected) {
            ShowExpandedCard();
        }
    }

    void OnMouseExit() {
        ShowNormalCard();
		transform.Find("OutlineParticle").gameObject.active = false;
    }

    void OnMouseDown() {
        ShowNormalCard();
		transform.Find("OutlineParticle").gameObject.active = true;
    }

	void OnMouseUp() {
		transform.Find ("OutlineParticle").gameObject.active = false;
	}

    private void ShowExpandedCard() {
        expandedCardObject = GenerateExpandedCard();
        renderer.enabled = false;
        foreach (Renderer r in GetComponentsInChildren(typeof(Renderer))) r.enabled = false;
		transform.Find("OutlineParticle").particleSystem.renderer.enabled = true;
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
		cardObject.transform.Find("OutlineParticle").gameObject.active = true;
        cardObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		cardObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        return cardObject;
    }
}
