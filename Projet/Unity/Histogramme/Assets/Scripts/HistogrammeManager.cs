using CP.ProChart;



using System;
using System.Collections;
using UnityEngine;

using System.Collections.Generic;
using System.Collections.ObjectModel;

///<summary>
/// Demo for Mesh Chart rendering
///</summary>
///

public class HistogrammeManager : MonoBehaviour
{
    int[] myArray;
    public string url = "http://192.168.43.160:3000";

    ///<summary>
    /// Reference for 3D bar chart
    ///</summary>
    public BarChartMesh bar3d;


    ///<summary>
    /// default thickness
    ///</summary>
    float thickness = 0.00004f;

    ///<summary>
    /// delta value to change thickness
    ///</summary>
    float delta = 0.0003f;

    ///<summary>
    /// 2D data set
    ///</summary>
    ChartData2D dataSet;

    //private 

    public float updateTime = 2.0f;
    private string results;
    //private Coroutine _coroutine;
    private int irequest = 0;

    private void Start()
    {
        /*_coroutine = */
        StartCoroutine(CoUpdate());
    }

    private void CollectResult()
    {
        //Debug.Log(results);
        char[] delimiterChars = { ',', '[', ']', '{', '}', '\t',':' };


        string[] words = results.Split(delimiterChars);
        int n = 0;
        bool test = true;

        dataSet = new ChartData2D();
        int i = 0;
        foreach (var word in words)
        {
            long number = 0;
            // verification si c'est un nombre 
            bool canConvert = long.TryParse(word, out number);
            if (canConvert == true)
            {
                if (test) {
                    n = Int32.Parse(word);
                    test = false;
                }
                else { 
                     dataSet[i, 0]= Int32.Parse(word);
                     i++;
                }
            }
        }
        bar3d.SetValues(ref dataSet);

    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {

            results = "";
            WebQuery();
            yield return new WaitForSeconds(updateTime);

        }
    }

    private void WebQuery()
    {

        //Worked
        irequest++;
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, CollectResult));
    }

    private IEnumerator WaitForRequest(WWW www, Action onComplete)
    {
        yield return www;
        if (www.error == null)
        {
            results = www.text;
            Debug.Log("results: " + results);
            onComplete();
            www.Dispose();
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    

    ///<summary>
    /// Use this for initialization
    ///</summary>

    ///<summary>
    /// Update orientation and params of chart
    ///</summary>
    void Update()
    {


        thickness += delta;
        if (thickness > 0.03f || thickness <= Mathf.Abs(delta))
        {
            delta = -delta;
        }
       
      //  bar3d.transform.Rotate(0, 40 * Time.deltaTime, 0);
    }
}
