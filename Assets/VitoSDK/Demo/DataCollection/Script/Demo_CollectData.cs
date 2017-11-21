using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Demo_CollectData : MonoBehaviour {
    public Text txtResult;
    public Text txtTips;
    // Use this for initialization
    float waitTime = 0;
     float realWaitTime;
    private bool isOver = false;
	void Start () {
        isOver = false;
        waitTime = Random.Range(20,40);
        realWaitTime = waitTime;
        txtTips.text = string.Format("{0}秒后随机生成本关卡的星级数据和三个阶段的得分数据以及总分数,控制台数据不会发送", Mathf.RoundToInt( waitTime));
        txtResult.text = string.Format("星级：{0}\n总分：{1}\n阶段1分数：{2}\n阶段2分数：{3}\n阶段3分数{4}\n",0,0,0,0,0);
        if (DataCollectionManager.instance != null)
        {
            CDStartLevel data = new CDStartLevel() { dataType = CDType.StartLevel, level = 1, levelName = "测试场景1", startTime = System.DateTime.Now.ToShortTimeString() };
            DataCollectionManager.instance.SendCollectedData(data);
        }
    }
    	
	// Update is called once per frame
	void Update () {
        if(!isOver)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                isOver = true;
                
                CDScore score = new CDScore() { dataType = CDType.Score, level = 1, levelName = "测试场景1", score1 = Random.Range(1, 10), score2 = Random.Range(1, 10), score3 = Random.Range(1, 10), sumScore = Random.Range(10, 20), sumTime = Mathf.RoundToInt(realWaitTime) };
                CDStar star = new CDStar() { dataType = CDType.StarGrade, level = 1, levelName = "测试场景1", starGrade=Random.Range(1,5), sumTime = Mathf.RoundToInt(realWaitTime) };
                txtResult.text = string.Format("星级：{0}\n总分：{1}\n阶段1分数：{2}\n阶段2分数：{3}\n阶段3分数{4}\n", star.starGrade, score.sumScore , score.score1, score.score2, score.score3);
                if(DataCollectionManager.instance!=null)
                {
                    DataCollectionManager.instance.SendCollectedData(score);
                    DataCollectionManager.instance.SendCollectedData(star);

                        CDEndLevel data = new CDEndLevel() { dataType = CDType.EndLevel, level = 1, levelName = "测试场景1", endTime = System.DateTime.Now.ToShortTimeString() };
                        DataCollectionManager.instance.SendCollectedData(data);

                }
                
            }
        }
	    
	}
}
