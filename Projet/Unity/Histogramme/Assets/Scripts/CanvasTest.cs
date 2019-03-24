using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using CP.ProChart;

///<summary>
/// Demo for canvas based bar and line chart using 2D data set
///</summary>
public class CanvasTest : MonoBehaviour 
{
	//bar chart datas
	public BarChart barChart;
    //public BarChartMesh bar3d;
    public Slider spacing;
	public Slider barThickness;
	public InputField distanceInput;
	public InputField marginInput;
	public Toggle barLabelToggle;
	public Slider barLabelPos;

	//line chart datas
	//data value change items
	public GameObject dataPanel;
	public Text infoData;
	public Slider data;
	public InputField dataInput;
	public Text info;

	//tooltip items
	public RectTransform tooltip;
	public Text tooltipText;

	//labels
	public GameObject labelBar;
	public GameObject axisXLabel;
	public GameObject axisYLabel;

	//2D Data set
	private ChartData2D dataSet;

	//selection of data
	private int row = -1;
	private int column = -1;
	private int overRow = -1;
	private int overColumn = -1;

	private List<Text> barLabels = new List<Text>();
	private List<Text> barXLabels = new List<Text>();
	private List<Text> barYLabels = new List<Text>();

	///<summary>
	/// Manage selection of data to be able to change it
	///</summary>
	public void OnSelectDelegate(int row, int column)
	{
		this.row = row;
		this.column = column;
		infoData.gameObject.SetActive(false);
		dataPanel.SetActive(true);
		data.value = dataSet[row, column];
		info.text = string.Format("Data[{0},{1}]", row, column);
		dataInput.text = dataSet[row, column].ToString();
	}

	///<summary>
	/// Manage over state of chart
	///</summary>
	public void OnOverDelegate(int row, int column)
	{
		overRow = row;
		overColumn = column;
	}

	///<summary>
	/// Initialize data set and charts
	///</summary>
	void OnEnable() 
	{
		spacing.value = barChart.Spacing;
		barThickness.value = barChart.Thickness;
        

		dataSet = new ChartData2D();
		dataSet[0, 0] = 50;
		dataSet[0, 1] = 30;
		dataSet[0, 2] = 70;
		dataSet[0, 3] = 10;
		dataSet[0, 4] = 90;
		dataSet[1, 0] = 40;
		dataSet[1, 1] = 25;
		dataSet[1, 2] = 53;
		dataSet[1, 3] = 12;
		dataSet[1, 4] = 37;
		dataSet[2, 0] = 68;
		dataSet[2, 1] = 91;
		dataSet[2, 2] = 30;
		dataSet[2, 3] = 44;
		dataSet[2, 4] = 63;

		barChart.SetValues(ref dataSet);

		barChart.onSelectDelegate += OnSelectDelegate;
		barChart.onOverDelegate += OnOverDelegate;
	
		distanceInput.text = barChart.Spacing.ToString("0.00");
		marginInput.text = barThickness.value.ToString("0.00");

		labelBar.SetActive(false);
		axisXLabel.SetActive(false);
		axisYLabel.SetActive(false);

		barLabels.Clear();

		for (int i = 0; i < dataSet.Rows; i++)
		{
			for (int j = 0; j < dataSet.Columns; j++)
			{
				GameObject obj = (GameObject)Instantiate(labelBar);
				obj.SetActive(barLabelToggle.isOn);
				obj.name = "Label" + i + "_" + j;
				obj.transform.SetParent(barChart.transform, false);
				Text t = obj.GetComponentInChildren<Text>();
				barLabels.Add(t);

		
			}
		}

		barXLabels.Clear();

		for (int i = 0; i < dataSet.Columns; i++)
		{
			GameObject obj = (GameObject)Instantiate(axisXLabel);
			obj.SetActive(barLabelToggle.isOn);
			obj.name = "Label" + i;
			obj.transform.SetParent(barChart.transform, false);
			Text t = obj.GetComponent<Text>();
			t.text = t.gameObject.name;
			barXLabels.Add(t);

			
		}

		barYLabels.Clear();

		for (int i = 0; i < dataSet.Columns; i++)
		{
			GameObject obj = (GameObject)Instantiate(axisYLabel);
			obj.SetActive(barLabelToggle.isOn);
			obj.name = "Label" + i;
			obj.transform.SetParent(barChart.transform, false);
			Text t = obj.GetComponent<Text>();
			t.text = t.gameObject.name;
			barYLabels.Add(t);
            
		}
	}

	///<summary>
	/// Remove hanlders when object is disabled
	///</summary>
	void OnDisable()
	{
		barChart.onSelectDelegate -= OnSelectDelegate;
		barChart.onOverDelegate -= OnOverDelegate;
	}

	///<summary>
	/// manage tooltip
	///</summary>
	void Update ()
	{
		tooltip.gameObject.SetActive(overRow != -1);
		if (overRow != -1)
		{
			tooltip.anchoredPosition = (Vector2)Input.mousePosition + tooltip.sizeDelta * tooltip.localScale.x / 2;
			tooltipText.text = string.Format("Data[{0},{1}]\nValue: {2:F2}", overRow, overColumn, dataSet[overRow, overColumn]);
		}

		UpdateLabels();	
	}

	
	
	///<summary>
	/// Update values when input changed
	///</summary>
	public void OnValueChanged(string input)
	{
		if (input == "distance")
		{
			barChart.Spacing = spacing.value;
			distanceInput.text = barChart.Spacing.ToString("0.00");
		}
		else if (input == "distanceInput")
		{
			barChart.Spacing = float.Parse(distanceInput.text);
			spacing.value = barChart.Spacing;
			distanceInput.text = spacing.value.ToString("0.00");
		}		
		else if (input == "margin")
		{
			barChart.Thickness = barThickness.value;
			marginInput.text = barThickness.value.ToString("0.00");
		}
		else if (input == "marginInput")
		{
			barChart.Thickness = float.Parse(marginInput.text);
			barThickness.value = barChart.Thickness;
			marginInput.text = barThickness.value.ToString("0.00");
		}		
		else if (input == "data")
		{
			dataSet[row, column] = data.value;
			info.text = string.Format("Data[{0},{1}]", row, column);
			dataInput.text = dataSet[row, column].ToString("0.00");
		}
		else if (input == "dataInput")
		{
			dataSet[row, column] = Mathf.Clamp(float.Parse(dataInput.text), -100, 100);
			data.value = dataSet[row, column];
			dataInput.text = dataSet[row, column].ToString();
		}
		
	}


	///<summary>
	/// Update labels
	///</summary>
	public void UpdateLabels()
	{
		for (int i = 0; i < dataSet.Rows; i++)
		{
			for (int j = 0; j < dataSet.Columns; j++)
			{
				LabelPosition labelPos = barChart.GetLabelPosition(i, j, barLabelPos.value);
				if (labelPos != null)
				{
					barLabels[i * dataSet.Columns + j].transform.parent.gameObject.SetActive(barLabelToggle.isOn);
					barLabels[i * dataSet.Columns + j].text = labelPos.value.ToString("0.00");
					barLabels[i * dataSet.Columns + j].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = labelPos.position;
				}
			}
		}
		barLabelPos.interactable = barLabelToggle.isOn;
		
		LabelPosition[] positions = barChart.GetAxisXPositions();
		if (positions != null)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				barXLabels[i].gameObject.SetActive(barLabelToggle.isOn);
				barXLabels[i].GetComponent<RectTransform>().anchoredPosition = positions[i].position;
			}
		}

		positions = barChart.GetAxisYPositions();
		if (positions != null)
		{
			for (int i = 0; i < barYLabels.Count; i++)
			{
				if (positions.Length - 1 < i)
				{
					barYLabels[i].gameObject.SetActive(false);
				}
				else
				{
					barYLabels[i].gameObject.SetActive(barLabelToggle.isOn);
					barYLabels[i].text = positions[i].value.ToString("0.0");
					barYLabels[i].GetComponent<RectTransform>().anchoredPosition = positions[i].position;
				}
			}
		}
        

		
	}

} //class
