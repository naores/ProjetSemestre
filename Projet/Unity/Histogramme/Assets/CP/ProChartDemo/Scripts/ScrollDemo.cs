using UnityEngine;
using System.Collections;

using CP.ProChart;

public class ScrollDemo : MonoBehaviour 
{
	///<summary>
	/// Reference for chart
	///</summary>
	public LineChart chart;

	///<summary>
	/// Test data set
	///</summary>
	private ChartData2D dataSet;

	private float n = 0;

	///<summary>
	/// Initialize chart
	///</summary>
	void Start()
	{
		dataSet = new ChartData2D();

		for (int i = 0; i < 250; i++)
		{
			dataSet[0, i] = Mathf.Sin(n) * 50.0f;
			dataSet[1, i] = Mathf.Cos(n / 2) * 30.0f;
			dataSet[2, i] = Mathf.PerlinNoise(n, 0) * 40.0f;
			n += 0.03f;
		}
		chart.SetValues(ref dataSet);
	}

	///<summary>
	/// Update dataset, so the chart will show the change
	///</summary>
	void Update ()
	{
		dataSet.KeepColumns(250);

		int i = dataSet.Columns;
		dataSet[0, i] = Mathf.Sin(n) * 50.0f;
		dataSet[1, i] = Mathf.Cos(n / 2) * 30.0f;
		dataSet[2, i] = Mathf.PerlinNoise(n, 0) * 40.0f;
		n += 0.03f;
	}

} //class
