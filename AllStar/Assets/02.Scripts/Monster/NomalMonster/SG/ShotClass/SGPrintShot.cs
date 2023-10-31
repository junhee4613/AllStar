using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGPrintShot : SGBaseShot
{
    private static readonly string[] SPLIT = { "\n", "\n", "\n\n" };        //�޸����� �о ������ �����͸� ���� 3���� ��

    public TextAsset paintDataText;                                         //�޸��� �ؽ�Ʈ�� ���� �� ����

    public float paintCenterAngle = 180f;
    public float betweenAngle = 3f;
    public float nextLineDelay = 0.1f;

    private int nowIndex;
    private float delayTimer;

    private List<List<int>> paintData;
    private float paintStartAngle;


    public override void Shot()
    {
        if(projectileSpeed <= 0f || paintDataText == null || string.IsNullOrEmpty(paintDataText.text))
        {//���� �ؽ�Ʈ �˻�, �ؽ�Ʈ ���� �ȿ� string�� �ִ����� �˻�
            return;
        }

        if (_shooting)
        {
            return;
        }

        if(paintData != null)                   //List �ʱ�ȭ
        {
            for (int i = 0; i < paintData.Count; i++)
            {
                paintData[i].Clear();
                paintData[i] = null;
            }
            paintData.Clear();
            paintData = null;
        }

        paintData = LoadPaintData();

        if(paintData == null || paintData.Count <= 0)
        {
            return;
        }

        paintStartAngle = paintCenterAngle - (paintData[0].Count % 2 == 0 ?
            (betweenAngle * paintData[0].Count / 2f) + (betweenAngle / 2f) :
            betweenAngle * Mathf.Floor(paintData[0].Count / 2f));

        _shooting = true;
        nowIndex = 0;
        delayTimer = 0f;

    }

    private List<List<int>> LoadPaintData()
    {
        if (paintDataText == null || string.IsNullOrEmpty(paintDataText.text))
        {
            return null;
        }

        string[] lines = paintDataText.text.Split(SPLIT, System.StringSplitOptions.RemoveEmptyEntries);
        
        var paintData = new List<List<int>>(lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#"))
            {
                continue;
            }
            paintData.Add(new List<int>(lines[i].Length));

            for (int j = 0; j < lines[i].Length; j++)
            {
                paintData[paintData.Count - 1].Add(lines[i][j] == 'O' ? 1 : 0);     //���ĺ� O(��)

            }
        }

        paintData.Reverse();

        return paintData;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_shooting == false)
        {
            return;
        }
        delayTimer -= SGTimer.Instance.deltaTime;
        while (delayTimer <= 0)
        {
            List<int> lineData = paintData[nowIndex];
            for (int i = 0; i < lineData.Count; i++)
            {
                if(lineData[i] == 1)
                {
                    SGProjectile projectile = GetProjectile(transform.position);
                    if(projectile == null)
                    {
                        break;
                    }
                    float angle = paintStartAngle + (betweenAngle * i);
                    ShotProjectile(projectile, projectileSpeed, angle);
                    projectile.UpdateMove(-delayTimer);
                }
            }
            nowIndex++;
            FiredShot();
            if(nowIndex >= paintData.Count)
            {
                FinishedShot();
                return;
            }
            delayTimer += nextLineDelay;
        }
    }
}
