﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour
{
    
    Animator anim;
    public GameObject vidanjor, pipiSuyu, vidanjorSpawn, pipiSuyuSpawn;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;
    public Transform rayStart, rayEnd;


    public AudioClip[] skillSounds;

    //PlayerController _playerController;
    PhotonView _photonView;
    Controller2D _controller;
    Player _player;

    bool jahAtt = false;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    public float[] skillCoolDowns = { 4, 7, 10, 25 };
    public float[] skillDurations = { 1, 1, 1, 1 };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];

    int usedSkill;
    //Abilities q, w, e, r;

    private void Awake()
    {

    }


    void setSkills()
    {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for (int i = 0; i < 4; i++)
        {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillACD[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillACD[i].abilityButtonAxisName = skillKeyMaps[i];
            skillACD[i].coolDownDuration = skillCoolDowns[i];
            skillACD[i].durationTime = skillDurations[i];
            //Debug.Log(skillCoolDownCheck[i].abilityButtonAxisName);
        }
    }

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _player = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        if(photonView.isMine) {

            setSkills();

        }
    }

    public void Fear()
    {
        Debug.Log("Korktum :("); // Bunu yazıyor
        _controller.enabled = false; // Bu amına kodumun yeri çalışmıyor bilgisayarı parçalıyacam.
        _player.enabled = false;
        StartCoroutine("FearCounter");
    }

    IEnumerator FearCounter()
    {
        yield return new WaitForSeconds(3); //Burayada giriyor
        _controller.enabled = true;
        _player.enabled = true;
    }

    void Update()
    {
        if (photonView.isMine)
        {
            Raycasting();
            if (GetComponent<Stats>().isAlive)
            {
                #region animationInput
                if(_controller.canMove)
                {
                    if (Input.GetKey(KeyCode.RightArrow))//set can jump!!!!!
                    {
                        anim.SetInteger("State", 4);
                        //anim.Play("jahreinRunning");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahreinRunning");
                    }
                    if (Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        anim.SetInteger("State", 0);
                        //anim.Play("jahIdle");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
                    }

                    if (Input.GetKey(KeyCode.LeftArrow))//set can jump!!!!!
                    {
                        anim.SetInteger("State", 4);
                        //anim.Play("jahreinRunning");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahreinRunning");
                    }
                    if (Input.GetKeyUp(KeyCode.LeftArrow))
                    {
                        anim.SetInteger("State", 0);
                        //anim.Play("jahIdle");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
                    }
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        anim.SetInteger("State", 5);
                        //anim.Play("jahJump");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahJump");
                    }

                }
                #endregion

                if(Input.GetButtonDown("Attack")) // Basic Attack
                {
                    anim.SetInteger("State", 7);
                    _photonView.RPC("AnimTrigger", PhotonTargets.All, "BasicAttackv2");
                    _photonView.RPC("basicAttack", PhotonTargets.All);
                }
                else
                {
                    jahAtt = false;
                }


                if (_player.canUseSkill)
                {

                    if (Input.GetButtonDown("SkillQ") && skillACD[0].itsReady && _controller.collisions.below)
                    {
                        _player.canUseSkill = false;
                        skillACD[0].use();
                        usedSkill = 0;
                        _player.canMove = false;
                        anim.Play("jahRagev2");
                        //anim.SetInteger("State", 1);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "jahRagev2");
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillW") && skillACD[1].itsReady)
                    {
                        _player.canUseSkill = false;
                        skillACD[1].use();
                        usedSkill = 1;

                        _controller.canMove = false;

                        anim.Play("kutsamav2");
                        //anim.SetInteger("State", 2);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "kutsamav2");
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillE") && skillACD[2].itsReady)
                    {

                        _player.canUseSkill = false;
                        skillACD[2].use();
                        usedSkill = 2;

                        anim.Play("PipiSuyu");
                        //anim.SetInteger("State", 3);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "PipiSuyu");
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);

                    }

                    if (Input.GetButtonDown("SkillR") && skillACD[3].itsReady)
                    {
                        _player.canUseSkill = false;
                        skillACD[3].use();
                        usedSkill = 3;
                        
                        _photonView.RPC("jahUlti", PhotonTargets.All);
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);

                    }
                }

                else
                {
                    //if player cant use skill
                    if (skillACD[usedSkill].durationEnd)
                    {
                        //if duration ends player can use skill again
                        _player.canUseSkill = true;
                    }
                }
                //if game started 
                _player.canMove = skillACD[0].durationEnd;
            }
        }
    }

    #region PunRPC's
    [PunRPC]
    private void basicAttack()
    {
        jahAtt = true;
    }

    [PunRPC]
    void jahRageSkill()
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(6, 0), ForceMode2D.Impulse);
        _player.velocity = Vector2.zero;
        _player.velocity.x = 22f * (_player.isfacingRight ? 1 : -1);
        
    }

    /*[PunRPC]//animtrigger a taşı
    void PipiSuyu()
    {
        JahPipiSuyu objPipiSuyu = Instantiate(pipiSuyu, new Vector3(pipiSuyuSpawn.transform.position.x, pipiSuyuSpawn.transform.position.y, 0), this.transform.rotation).GetComponent<JahPipiSuyu>();
        objPipiSuyu.pvID = _photonView.viewID;
        if (_player.isfacingRight)
            objPipiSuyu.fDir = 1;
        else
            objPipiSuyu.fDir = -1;
    }*/

    [PunRPC]
    void jahUlti()
    {
        Instantiate(vidanjor, new Vector3(-18f,-2f,0f), this.transform.rotation);
        //this.transform.position.x, this.transform.position.y/2,0
    }
    [PunRPC]
    public void playSound(int skillID) {
        AudioSource.PlayClipAtPoint(skillSounds[skillID], transform.position, 2f);

    }

    #endregion

    #region Animation RPC
    [PunRPC]
    void AnimTrigger(string animName) {
        anim.Play(animName);
    }
    /*
    [PunRPC]
    void JahRageAnimTrigger()
    {
        anim.Play("jahRagev2");
    }

    [PunRPC]
    void KutsamaAnimTrigger()
    {
        anim.Play("kutsamav2");
    }

    [PunRPC]
    void PipisuyuAnimTrigger()
    {
        anim.Play("PipiSuyu");
    }

    [PunRPC]
    void RunningAnimTrigger()
    {
        anim.Play("jahreinRunning");
    }

    [PunRPC]
    void IdleAnimTrigger()
    {
        anim.Play("jahIdle");
    }

    [PunRPC]
    void JumpAnimTrigger()
    {
        anim.Play("jahJump");
    }

    [PunRPC]
    void AttackAnimTrigger()
    {
        anim.Play("BasicAttackv2");
    }*/
    #endregion

   /* [PunRPC]
    void giveDamage() {
        _player.target.GetComponent<Stats>().TakeDamage(_player.damage);
    }*/

    #region Animation events

    private bool Raycasting() {
        Debug.DrawLine(rayStart.position, rayEnd.position, Color.green);
        bool rayHit = Physics2D.Linecast(rayStart.position, rayEnd.position, 1 << LayerMask.NameToLayer("enemy"));
        return rayHit;
    }

    void basicAttTrigger() {
        
        Debug.Log("hit enemy:" + Raycasting());
        if(Raycasting()) {
            _player.target.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, _player.damage);

        }
    }


    void JahRageTrigger()
    {
        _photonView.RPC("AnimTrigger", PhotonTargets.All, "jahRagev2");
    }

    void ChangeToIdle()
    {
        _photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
        anim.SetInteger("State", 0);
    }

    void CanMove()
    {
        _controller.canMove = true;
    }

    void ChangeVelocity()
    {
        _player.velocity.x = 0;
    }
    #endregion

}
