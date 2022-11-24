using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int actualscore;//variabile
    public int nextindex;
    public int[] scoresaved = new int[10];

    //public void SaveScore(){GameManager.ScoreDataSave(actualscore,scoresaved);}

    public void IncrementScore(int value){actualscore+=value;}

    public void ScoreArrayInsert()
    {
        scoresaved[nextindex] = actualscore;
        if(nextindex > scoresaved.Length)nextindex=scoresaved.Length;
        else nextindex+=1;
    }

    public void ArrayOrder()
    {
        for(int i=0;i<scoresaved.Length;i++)
        {
            for(int j=i+1;j<scoresaved.Length;j++)
            {
                if(scoresaved[i] < scoresaved[j])
                {
                    int tmp = scoresaved[i];
                    scoresaved[i] = scoresaved[j];
                    scoresaved[j] = tmp;
                }
            }
        }
    }


}
