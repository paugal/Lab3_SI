﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]

public class enemyMovement : MonoBehaviour
{
    
    public Transform[] points;
    public Transform[] points2;
    private int destPoint = 0;
    private int destPoint2 = 0;

    private NavMeshAgent agent;
    private LineRenderer myLineRender;

    public GameObject enemy;

    public GameObject textController;
    private textControler cs;
    private int level;

    public bool pathShown = false;
    public float shownTime = 5f;
    private float globalTime = 0.0f;

    public void Start () {
        
        cs = textController.GetComponent<textControler>();
        level = cs.level;

        destPoint = 0;
        destPoint2 = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        //ENEMY's PATH
        myLineRender = GetComponent<LineRenderer>();
        myLineRender.startWidth = 0.15f;
        myLineRender.endWidth = 0.15f;
        myLineRender.positionCount = 0;

        GotoNextPoint();
    }

    void GotoNextPoint() {
        if (points.Length == 0)
            return;
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void GotoNextPoint2()
    {
        if (points2.Length == 0) return;
        agent.destination = points2[destPoint2].position;
        destPoint2 = (destPoint2 + 1) % points2.Length;
    }


    void Update () {
        
        level = cs.level;

        if (!agent.pathPending && agent.remainingDistance < 0.5f){
            if(level == 0){
                GotoNextPoint();  
            }else{
                GotoNextPoint2(); 
            }  
        }

        globalTime += Time.deltaTime;
        if(pathShown == true){
            shownTime -= Time.deltaTime;
            DrawPath();
        }
    }

    void DrawPath(){

        if(shownTime>=0){
            myLineRender.enabled = true;
            myLineRender.positionCount = agent.path.corners.Length;
            
            Vector3 agentPosition = new Vector3(transform.position.x, transform.position.y+50, transform.position.z);
            myLineRender.SetPosition(0, agentPosition);

            if(agent.path.corners.Length < 2){
                return;
            }

            for(int i=1; i < agent.path.corners.Length; i++){
                Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y+50, agent.path.corners[i].z);
                myLineRender.SetPosition(i, pointPosition);
                myLineRender.SetWidth(1f, 1f);
            }

        }else{
            myLineRender.enabled = false;
            shownTime = 5;
            pathShown = false;
        }
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player2")){
            if(this.CompareTag("enemy")){
                pathShown = true;
                DrawPath();
            }
        }
    }
}
