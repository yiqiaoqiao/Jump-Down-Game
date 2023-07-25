using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int Hp;
    [SerializeField] GameObject HpBar;
    [SerializeField] Text scoreText;
    int score;
    float scoreTime;
    Animator anim;
    SpriteRenderer render;
    AudioSource deathsound;
    [SerializeField] GameObject restartButton;
    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathsound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D)){
            transform.Translate(moveSpeed*Time.deltaTime, 0,0);
            render.flipX = true;
            anim.SetBool("run", true);
        }
        else if(Input.GetKey(KeyCode.A)){
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            render.flipX = false;
            anim.SetBool("run", true);
        }
        else{
            anim.SetBool("run", false);
        }
        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Normal"){
            if(other.contacts[0].normal == new Vector2(0f, 1f)){
                //Debug.Log("Hit Normal");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(other.gameObject.tag == "Nails"){
            if(other.contacts[0].normal == new Vector2(0f, 1f)){
                //Debug.Log("Hit Nails");
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                if(Hp > 0){
                    other.gameObject.GetComponent<AudioSource>().Play();
                }
                
            }
        }
        else if(other.gameObject.tag == "Ceiling"){
            //Debug.Log("Hit Ceiling");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            if(Hp > 0){
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "DeathLine"){
            //Debug.Log("Game Lost");
            Die();
        }
    }

    void ModifyHp(int num){
        Hp += num;
        if(Hp > 10){
            Hp = 10;
        }
        if(Hp < 0){
            Hp = 0;
            Die();
        }
        UpdateHpBar();
    }

    void UpdateHpBar(){
        for(int i = 0; i < HpBar.transform.childCount; i++){
            if(Hp>i){
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else{
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore(){
        scoreTime += Time.deltaTime;
        if(scoreTime > 2f){
            score ++;
            scoreTime = 0f;
            scoreText.text = "Ground Level " + score.ToString();
        }
    }

    void Die(){
        deathsound.Play();
        Time.timeScale = 0f;
        restartButton.SetActive(true);
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
