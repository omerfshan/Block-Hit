using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShoterScripts : MonoBehaviour
{
    public float Power=2f;
    private int dots=15;
    private Vector2 starPos;
    private bool shoot,isAiming;
    private GameObject Dots;
    private GameController gc;
    private List<GameObject> projectilesPath;
    private Rigidbody2D ballBody;
    public GameObject ballPrefab;
    public GameObject ballContainer;

    private void Awake() 
    {
     gc=GameObject.Find("GameController").GetComponent<GameController>();
     Dots=GameObject.Find("Dots");

    }
    void Start()
    {
       
        projectilesPath=Dots.transform.Cast<Transform>().ToList().ConvertAll(t=> t.gameObject);
      HideDots();
    
    
    }

    
    void Update()
    { 
        ballBody=ballPrefab.GetComponent<Rigidbody2D>();
        if(gc.shootCount<=3&&!IsMouseOverUI())
        {
            Aiming();
            Rotate();
        }
       
    }
    private bool IsMouseOverUI(){
       return EventSystem.current.IsPointerOverGameObject();
   }
    void Aiming(){
        if(shoot)
        {
            return;
        }
           
        if(Input.GetMouseButton(0))
        {
            if(!isAiming)
            {
                isAiming= true;
                starPos = Input.mousePosition;
                gc.CheckShotCount();
            }
            else
            {
             PathCalculation();

            }
           
        }
         
        else if(isAiming&&!shoot)
        {
            isAiming=false;
            HideDots();
            StartCoroutine(Shoot());
            if(gc.shootCount==1)
            Camera.main.GetComponent<CameraTransitions>().RotateCameraToSide();
            
        }
    }
    Vector2 ShootForce(Vector3 force)
    {
       return (new Vector2(starPos.x,starPos.y)-new Vector2(force.x,force.y))*Power;
    }

   Vector2 DotPath(Vector2 StartP,Vector2 startVel,float t){
    return StartP+startVel*t+0.5f*Physics2D.gravity*t*t;

   }
   void PathCalculation()
   {
    Vector2 vel =ShootForce(Input.mousePosition)*Time.fixedDeltaTime/ballBody.mass;
      for (int i = 0; i < projectilesPath.Count; i++)
      {    projectilesPath[i].GetComponent<Renderer>().enabled=true;

        float t=i/15f;
        Vector3 point=DotPath(transform.position,vel,t);
        point.z=1;
        projectilesPath[i].transform.position=point;
      }
      
   }

void ShowDots(){
for(int i=0;i<projectilesPath.Count;i++)
{
    projectilesPath[i].GetComponent<Renderer>().enabled=true;
}
}
void HideDots()
{
  for(int i=0; i<projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled=false;
        }
}
void Rotate(){
    Vector2 dir=GameObject.Find("dot (1)").transform.position-transform.position;
    float angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
    transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);
}

IEnumerator Shoot(){
    for(int i=0; i<gc.BallsCount; i++){
        yield return new WaitForSeconds(0.07f);
        GameObject ball=Instantiate(ballPrefab,transform.position,Quaternion.identity);
        ball.name="Ball";
        ball.transform.SetParent(ballContainer.transform);
        ballBody=ball.GetComponent<Rigidbody2D>();
        ballBody.AddForce(ShootForce(Input.mousePosition)); 
        int balls=gc.BallsCount-i;
        gc.ballsCountText.text=(gc.BallsCount-i-1).ToString();


    }
     yield return new WaitForSeconds(0.5f);
    gc.shootCount++;
    gc.ballsCountText.text=gc.BallsCount.ToString();
}
}
