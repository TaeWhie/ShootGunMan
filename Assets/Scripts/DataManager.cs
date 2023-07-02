using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UniRx;

public class DataManager : MonoBehaviour
{
    public ChartInfo[] chartInfos = new ChartInfo[(int)ChartName.Count];
    private ReactiveProperty<int> counter = new ReactiveProperty<int>();//counter -2 는 차트 로드 완료상태
    public ReactiveProperty<bool> allComplete = new ReactiveProperty<bool>();
    public List<Sprite[]> allSprites;//문제!

    int index = 0;
    public int SpirteChartMax = 2;
    //Sprite address는 무조건 1번 행에작성
    public enum ChartName
    {
        MonsterChart, GunChart, BulletChart , WaveChart , Count
    }

    private void OneTimeLoad(ReactiveProperty<int> i)//i번호 부터 하나가 완료되면 다음꺼를 로딩하는식
    {
        i.Value++;
        if (i.Value < (int)ChartName.Count)
        {
            chartInfos[i.Value] = new ChartInfo();
            chartInfos[i.Value].chartName = ((ChartName)i.Value).ToString();
            AsyncOperationHandle<TextAsset> obj = new AsyncOperationHandle<TextAsset>();
 
            Addressables.LoadAssetAsync<TextAsset>(chartInfos[i.Value].chartName).Completed +=
            (AsyncOperationHandle<TextAsset> obj) =>
            {
                chartInfos[i.Value].chart = obj.Result;
                OneTimeLoad(i);
            };
        } 
        else
        {
            i.Value = -2;
        }
    }

    public void Init()
    {
        counter.Value = -1;
        OneTimeLoad(counter);

        counter.Where(x => x==-2).Subscribe(data =>
        { 
            for (int i = 0; i < chartInfos.Length; i++)
            {
                Analyzetxt( chartInfos[i]);
            }
            //sprite를 chart의 address에 따라 로드 하는곳
            counter.Value = -1;
            allSprites = new List<Sprite[]>();
            LoadSprite(counter);
        }).AddTo(gameObject);
    
        /*해결법
           1. 로딩 시작할때 번호를 같이 던져서 받을때 그번호로 받는다.=>할수있으면 이게 베스트
           2. 결과를 순서 상관없이 받은 후 다시 name을 통해 재배열한다.*/
    }
    /*코루틴 단점

    코루틴에서는 예외처리가 힘들다
        -return 값이 없다

    코루틴은 GC가 생긴다!

    unitask를 쓰면 return값을 확인할 수 있다!
    unitask는 GC가 생기지 않는다.
    */
    public void LoadSprite(ReactiveProperty<int> i )
    {
        i.Value++;

        if (i.Value == 0)
        {
            allSprites.Add(new Sprite[chartInfos[index].lineSize]);
        }

        if (i.Value < chartInfos[index].lineSize-1)
        {
            Addressables.LoadAssetAsync<Sprite>(chartInfos[index].stringchart[i.Value + 1, 1]).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                allSprites[index][i.Value] = obj.Result;
                LoadSprite(i);
            };
        }
        else
        {
            if(index<SpirteChartMax)
            {
                index++;
                i.Value = -1;
                LoadSprite(i);
            }
            else
            {
                allComplete.Value = true;    
            }
        }
    }

    public string[] ReadRow(int id, string[] word,int chartInfoNum)
    {
        //name.damage.level.hp 순
        word = new string[chartInfos[chartInfoNum].rowSize];
        for (int i=0 ;i<chartInfos[chartInfoNum].rowSize ;i++)
        {
            word[i] = chartInfos[chartInfoNum].stringchart[id, i];          
        }

        return word;
    }
    public int ReturnlineSize(ChartInfo chart)
    {
        return chart.lineSize;
    }
    private void Analyzetxt(ChartInfo chartInfo)
    {
        string[] word=null;
        string currentText = chartInfo.chart.text.Substring(0, chartInfo.chart.text.Length - 1);
        chartInfo.line = currentText.Split('\n');
        chartInfo.lineSize = chartInfo.line.Length;
        chartInfo.rowSize = chartInfo.line[0].Split(',').Length;
        chartInfo.stringchart = new string[chartInfo.lineSize, chartInfo.rowSize];

        for (int i = 0; i < chartInfo.lineSize; i++)
        {
            string[] sentence = chartInfo.line[i].Split(',');
            word = sentence;

            for (int j = 0; j < chartInfo.rowSize; j++)
            {
                chartInfo.stringchart[i, j] = word[j];
            }
        }
    }
}
