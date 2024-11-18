using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 jumpForce = new Vector2(0, 20);
    [SerializeField] private float timeJumping;
    [SerializeField] private int health = 1;

    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private GameObject losePanel;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        textScore.text = $"Score: {Mathf.Round(PlayerInfo.score).ToString()}";
        textCoin.text = $"Coin: {PlayerInfo.coin.ToString()}";
        losePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            TurnOnLosePanel();
            PlayerInfo.coin = 0;
            PlayerInfo.score = 0;
            return;
        }

        if (this.transform.position.y < -6f)
        {
            Debug.Log("Player die");
            AudioManager.audioInstance.PlaySfx("Dead");
            health--;
        }

        PlayerInfo.score += Time.deltaTime;
        textScore.text = $"Score: {Mathf.Round(PlayerInfo.score).ToString()}";

        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal < 0f)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isMoving", true);
        }
        if (horizontal > 0f)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("isMoving", true);
        }
        if (horizontal == 0f)
        {
            animator.SetBool("isMoving", false);
        }

        rigidBody2D.velocity = new Vector3(speed * horizontal , 0, 0);

        if (animator.GetBool("isJumping"))
        {
            timeJumping += Time.deltaTime;

            if (timeJumping > 0.05f)
            {
                animator.SetBool("isJumping", false);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody2D.AddForce(jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
    }

    private void TurnOnLosePanel()
    {
        losePanel.SetActive(true);
        StartCoroutine(TurnOffLosePanel());
    }

    IEnumerator TurnOffLosePanel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        { 
            PlayerInfo.coin++;
            AudioManager.audioInstance.PlaySfx("Coin");
            textCoin.text = $"Coin: {PlayerInfo.coin.ToString()}";
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Crate"))
        {
            Debug.Log("You lose");
            health--;
            AudioManager.audioInstance.PlaySfx("Dead");
        }
    }
}
