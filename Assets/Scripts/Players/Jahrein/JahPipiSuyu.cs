﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahPipiSuyu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(15,5)*50f);

    }

    // Update is called once per frame
    void Update () {

    }
    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(this.gameObject);
    }
}
