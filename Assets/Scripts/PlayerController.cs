﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerState {
    public List<CardController> hand;
    public List<CardState> deck;
    public int health;
    public int gold;
}

public class PlayerController : MonoBehaviour {

    const int maxGold = 10;

    public PlayerState playerState;
    
    public GameObject cardModel;
    public Vector3 cardSpawnPosition;
    public float handAngle;
    public Vector3 pivot;
    public float length;
    public float spacing;
    
    private List<CardState>.Enumerator deckEnumerator;
    private int turn;
    
    private PlayerDamageAnimation damageAnimation;
    private PlayerDeathAnimation deathAnimation;

    public void PlayPiece(PieceController piece, CardController card) {
        playerState.gold -= card.cardState.cost;
        piece.PlayCard(card.cardState);
        transform.Find("Gold").GetComponent<TextMesh>().text = playerState.gold.ToString();
    }

    public void RemoveCardFromHand(CardController card) {
        playerState.hand.Remove(card);
        UpdateHandPosition();
    }

    public void ReceiveDamage(int damage) {
        playerState.health -= damage;
        gameObject.GetComponent<PlayerDamageAnimation>().Animate(damage);
        transform.Find("Health").GetComponent<TextMesh>().text = playerState.health.ToString();
        if (playerState.health <= 0) {
            gameObject.GetComponent<PlayerDeathAnimation>().Animate();
            // TODO end game here
        }
        
    }

    public void DrawCard() {
        // animation here
        if (!deckEnumerator.MoveNext()) {
            // TODO fatigue here?
            return;
        }
        cardModel.GetComponent<CardController>().cardState = deckEnumerator.Current;
        GameObject cardObject = Instantiate(cardModel, cardSpawnPosition, Util.CardRotation) as GameObject;
        AddCardToHand(cardObject.GetComponent<CardController>());
    }

    public void NewTurn() {
        turn += 1;
        ResetGold();
        DrawCard();
    }

    void Awake() {
        Util.Shuffle(playerState.deck);
        this.deckEnumerator = playerState.deck.GetEnumerator();
        transform.Find("Gold").GetComponent<TextMesh>().text = playerState.gold.ToString();
        transform.Find("Health").GetComponent<TextMesh>().text = playerState.health.ToString();
        damageAnimation = gameObject.GetComponent<PlayerDamageAnimation>();
        deathAnimation = gameObject.GetComponent<PlayerDeathAnimation>();
    }

    void AddCardToHand(CardController card) {
        playerState.hand.Add(card);
        card.SetPlayerController(this);
        UpdateHandPosition();
    }
    
    void ResetGold() {
        playerState.gold = Mathf.Min(maxGold, turn);
        transform.Find("Gold").GetComponent<TextMesh>().text = playerState.gold.ToString();
    }
    
    void UpdateHandPosition() {
        // reposition all cards
        int numCards = playerState.hand.Count;
        float angle = spacing / 2 * (numCards - 1);
        if (angle > handAngle) angle = handAngle;
        float[] linspace = Util.Linspace(-angle, angle, numCards);
        float y = 10;
        for (int i = 0; i < numCards; i++) {
            Quaternion rotation = Quaternion.Euler(270, 180 + linspace[i], 0);
            Vector3 position = (Quaternion.Euler(0, linspace[i], 0) * new Vector3(0, 0, length)) + pivot;
            position.y = y + i;
            CardController card = playerState.hand[i];
            if (!card.selected) card.MoveInHand(position, rotation);
            else card.MoveInHandOnDrop(position, rotation);
        }
    }
}
