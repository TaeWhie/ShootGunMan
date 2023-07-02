using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UniRx;

public class DataManager : MonoBehaviour
{
    public ChartInfo[] chartInfos = new ChartInfo[(int)ChartName.Count];
    private ReactiveProperty<int> counter = new ReactiveProperty<int>();//counter -2 �� ��Ʈ �ε� �Ϸ����
    public ReactiveProperty<bool> allComplete = new ReactiveProperty<bool>();
    public List<Sprite[]> allSprites;//����!

    int index = 0;
    public int SpirteChartMax = 2;
    //Sprite address�� ������ 1�� �࿡�ۼ�
    public enum ChartName
    {
        MonsterChart, GunChart, BulletChart , WaveChart , Count
    }

    private void OneTimeLoad(ReactiveProperty<int> i)//i��ȣ ���� �ϳ��� �Ϸ�Ǹ� �������� �ε��ϴ½�
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
            //sprite�� chart�� address�� ���� �ε� �ϴ°�
            counter.Value = -1;
            allSprites = new List<Sprite[]>();
            LoadSprite(counter);
        }).AddTo(gameObject);
    
        /*�ذ��
           1. �ε� �����Ҷ� ��ȣ�� ���� ������ ������ �׹�ȣ�� �޴´�.=>�Ҽ������� �̰� ����Ʈ
           2. ����� ���� ������� ���� �� �ٽ� name�� ���� ��迭�Ѵ�.*/
    }
    /*�ڷ�ƾ ����

    �ڷ�ƾ������ ����ó���� �����
        -return ���� ����

    �ڷ�ƾ�� GC�� �����!

    unitask�� ���� return���� Ȯ���� �� �ִ�!
    unitask�� GC�� ������ �ʴ´�.
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
        //name.damage.level.hp ��
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
