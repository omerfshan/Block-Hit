using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public GameObject[] bloks;
    public List<GameObject> levels;
    private GameObject level1;
    private GameObject level2;
    private Vector2 level1Pos;
    private Vector2 level2Pos;
    public int shootCount;
    private ShotCountText shotCountText;
    public int BallsCount;
    private GameObject BallContainer;
    public GameObject gameOver;
    public int score;
   public Text ballsCountText;
   private bool firstShot;
    private void Awake() {
        shotCountText=GameObject.Find("ShotCountText").GetComponent<ShotCountText>();
        ballsCountText=GameObject.Find("BallCountText").GetComponent<Text>();
        BallContainer=GameObject.Find("ballContainer");
    }
    void Start()
    {   
        Physics2D.gravity=new Vector2(0,-17);
        BallsCount=PlayerPrefs.GetInt("BallCount",5);
        ballsCountText.text=BallsCount.ToString();
        SpawnLevel();
        GameObject.Find("Conan").GetComponent<Animator>().SetBool("MoveIn",true);
    }

 
   private void Update() {
    if(BallContainer.transform.childCount==0&& shootCount==4){
        gameOver.SetActive(true);
        GameObject.Find("Conan").GetComponent<Animator>().SetBool("MoveIn",false);
    }
    if(shootCount>2){
        firstShot=false;
    }
    else{
        firstShot=true;
    }
    
    CheckBlocks();
    // print(shootCount);
    print("firtShot"+firstShot);
    print("score="+score);
   }

    void SpawnNewLevel(int numberLevel1,int numberLevel2,int min,int max){
        if(shootCount>1)
            Camera.main.GetComponent<CameraTransitions>().RotateCameraToFront();
        
        shootCount=1;

        level1Pos=new Vector2(3.5f,1);
        level2Pos=new Vector2(3.5f,-3.4f);

        level1=levels[numberLevel1];
        level2=levels[numberLevel2];

        Instantiate(level1,level1Pos,Quaternion.identity);
        Instantiate(level2,level2Pos,Quaternion.identity);
        SetBlocksCount(min,max);
    }
   void  SpawnLevel(){
        if(PlayerPrefs.GetInt("Level",0)==0){
        SpawnNewLevel(0,17,3,5);
        }
        if(PlayerPrefs.GetInt("Level")==1){
        SpawnNewLevel(0,17,3,5);
        }
        if(PlayerPrefs.GetInt("Level")==2){
         SpawnNewLevel(0,17,3,5);
        }
         if(PlayerPrefs.GetInt("Level")==3){
        SpawnNewLevel(0,17,3,5);
        }
         if(PlayerPrefs.GetInt("Level")==4){
        SpawnNewLevel(0,17,3,5);
        }
         if(PlayerPrefs.GetInt("Level")==5){
        SpawnNewLevel(0,17,3,5);
        }
         if(PlayerPrefs.GetInt("Level")==6){
         SpawnNewLevel(0,17,3,5);
        }
         if(PlayerPrefs.GetInt("Level")==7){
         SpawnNewLevel(0,17,3,5);
        }
    }
    void SetBlocksCount(int min,int max){
        bloks=GameObject.FindGameObjectsWithTag("Block");
        for(int i=0; i<bloks.Length; i++){
            int count=Random.Range(min,max);
            bloks[i].GetComponent<Block>().SetStartingCount(count);
        }
    }
    void CheckBlocks(){
        GameObject[] Block=GameObject.FindGameObjectsWithTag("Block");
        // print(Block.Length);
        if(Block.Length<1)
        {
            PlayerPrefs.SetInt("Level",PlayerPrefs.GetInt("Level")+1);
            ballDestroy(); 
            SpawnLevel();
            if (BallsCount >= PlayerPrefs.GetInt("BallsCount", 5))
                PlayerPrefs.SetInt("BallsCount", BallsCount);

            if (firstShot) {
    Debug.Log("First Shot Bonus Applied");
    score += 5;
} else {
    Debug.Log("Regular Score Applied");
    score += 3;
}

        }
        
    }
    void ballDestroy(){
        GameObject[] balls=GameObject.FindGameObjectsWithTag("ball");
        for (int i = 0; i < balls.Length; i++)
        {
            Destroy(balls[i]);
        }
    }
    public void CheckShotCount()
    {
      if(shootCount==1)
      {
       shotCountText.SetTopText("SHOT");
       shotCountText.SetBottomText("1/3");
       shotCountText.Flash();
      }
       if(shootCount==2)
      {
       shotCountText.SetTopText("SHOT");
       shotCountText.SetBottomText("2/3");
        shotCountText.Flash();
      }
      if(shootCount==3)
      {
       shotCountText.SetTopText("FINAL");
       shotCountText.SetBottomText("SHOT");
        shotCountText.Flash();
      }
    }
}
