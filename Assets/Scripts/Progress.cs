using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public RectTransform extraBall;
    private GameController gameController;
    private float currentWidth,addWidth,totalWidth;
    private void Awake() {
        gameController=GameObject.Find("GameController").GetComponent<GameController>();
        
    }
    void Start()
    {
        extraBall.sizeDelta=new Vector2(31,117);
        currentWidth=31;
        totalWidth=600;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWidth>=totalWidth)
        {
            gameController.BallsCount++;
            gameController.ballsCountText.text=gameController.BallsCount.ToString();
            currentWidth=31;
        }
        if(currentWidth>=addWidth)
        {
          addWidth+=5;
          extraBall.sizeDelta=new Vector2(addWidth,117);

        }
        else
        {
           addWidth=currentWidth;

        }
    }
    public void IncreaseCurrentWidth()
    {
      int addRandom=Random.Range(80,120);
      currentWidth=addRandom+31+currentWidth%576;
    }
}
