///<summary>
/// Pro Chart is a graph and chart system for Unity3D. It has been designed to 
/// support 2D rendering into Unity Canvas System and 2D/3D rendering as Meshes.
/// The chart system supports multiple type of charts, curves and data formats.
///</summary>
///<version>
/// 1.3, 2015.02.10 by Attila Odry, Tamas Barsony, Laszlo Papp
///</version>
///<remarks>
/// Copyright (beer) 2015, Creative Pudding
/// All rights reserved.
/// 
/// Limitation of redistribution:
/// - Redistribution of the code or part of the code in any form is not allowed,
///   but only by written permission from CreativePudding.
///
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
/// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
/// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS
/// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
/// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
/// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
/// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
///</remarks>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CP.ProChart
{

	public class BarChartMesh : ChartMesh
	{
		///<summary>
		/// 2D data set associated with this chart
		///</summary>
		private ChartData2D values = null;
		
		///<summary>
		/// Distance between data groups, float in range of 0.1f to 1f, where 0.1f is minimum space
		///</summary>
		[SerializeField][Range(0.1f, 1.0f)]private float spacing = 1;
		public float Spacing {
			get { return spacing; }
			set { if (spacing != value && value >= 0.1f && value <= 1.0f) { spacing = value; Dirty = true; } }
		}

		///<summary>
		/// Thickness of bars, float in range of 0.1f to 1f, where 1f is the thickest bar
		///</summary>
		[SerializeField][Range(0.1f, 1.0f)] private float thickness = 1;
		public float Thickness {
			get { return thickness; }
			set { if (thickness != value && value >= 0.1f && value <= 1.0f) { thickness = value; Dirty = true; } }
		}

		/// <summary>
		/// Enable/disable stacked version
		/// </summary>
		[SerializeField] private bool m_stacked = false;
		public bool stacked
		{
			get { return m_stacked; }
			set { if (m_stacked != value) { m_stacked = value; Dirty = true; } }
		}

		///<summary>
		/// Enable the chart and create test data to give visuals in editor
		///</summary>
		void OnEnable()
		{
			//only in editor
			if (!Application.isPlaying)
			{
				ChartData2D testValues = new ChartData2D();
				testValues[0, 0] = 2;
				testValues[0, 1] = 1;
				testValues[0, 2] = 3;
				testValues[1, 0] = 1;
				testValues[1, 1] = 5;
				testValues[1, 2] = 1;
				SetValues(ref testValues);
			}
			Dirty = true;
		}

		///<summary>
		/// Set data set using reference
		///</summary>
		public void SetValues(ref ChartData2D values)
		{
			if (this.values != null)
			{
				this.values.onDataChangeDelegate -= OnDataChangeDelegate;
			}
			this.values = values;
			this.values.onDataChangeDelegate += OnDataChangeDelegate;

			Dirty = true;
		}

		///<summary>
		/// Generate the chart
		///</summary>
		protected override void Create()
		{
			Vertices = new List<Vector3>();
			VertexColors = new List<Color32>();
			Triangles = new List<int>();

			if (values == null || values.isEmpty)
			{
				return;
			}

			float w;
			if (stacked)
			{
				w = size.x / (float)(values.Columns + (values.Columns - 1));
			}
			else
			{
				w = size.x / (float)(values.Columns * values.Rows + (values.Columns - 1));
			}
			float gap = w * spacing;

			float max = float.MinValue;
			float min = float.MaxValue;

			for (int j = 0; j < values.Columns; j++)
			{
				if (stacked)
				{
					float valMax = 0;
					float valMin = 0;
					for (int i = 0; i < values.Rows; i++)
					{
						float val = values[i, j];
						if (val < 0)
						{
							valMin += val;
						}
						else
						{
							valMax += val;
						}

					}
					max = valMax > max ? valMax : max;
					min = valMin < min ? valMin : min;
				}
				else
				{
					for (int i = 0; i < values.Rows; i++)
					{
						float val = values[i, j];
						max = val > max ? val : max;
						min = val < min ? val : min;
					}
				}
			}

			float d = (max != min) ? max - min : 1;
			float total = Mathf.Abs(max) + Mathf.Abs(min);
			float x0 = -size.x / 2;

			float z = mode_3d ? -size.z / 2 : 0.0f;

			if (values.Columns > 0)
			{
				for (int i = 0; i < values.Columns; i++)
				{
					Vector2 p1 = Vector2.zero;
					Vector2 p2 = Vector2.zero;

					float y0 = -size.y / 2;

					float top = y0 + Mathf.Abs(min) / total * size.y;
					float bottom = top;


					for (int j = 0; j < values.Rows; j++)
					{
						float val = values[j, i];

						Color32 colorTop = colors[j % colorCount, 0];
						Color32 colorBottom = colors[j % colorCount, 1];

						if (stacked)
						{
							p1.x = x0 + (float)(i * w + (i * gap)) + w * (1 - thickness) / 2;
						}
						else
						{
							p1.x = x0 + (float)((j + i * values.Rows) * w + (i * gap)) + w * (1 - thickness) / 2;
						}

						p2.x = p1.x + w * thickness;
						if (stacked)
						{
							if (val < 0)
							{
								p1.y = bottom;
								p2.y = bottom + val / total * size.y;

								bottom = p2.y;
							}
							else
							{
								p1.y = top + val / total * size.y;
								p2.y = top;

								top = p1.y;
							}
						}
						else
						{
							if (max < 0)
							{
								p1.y = size.y / 2;
								p2.y = p1.y - val / min * size.y;
							}
							else if (min < 0)
							{
								p1.y = y0 + ((Mathf.Abs(min) + val) / d) * size.y;
								p2.y = y0 + (Mathf.Abs(min) / d) * size.y;
							}
							else
							{
								p1.y = y0 + (val / max) * size.y;
								p2.y = y0;
							}
						}


						if(min < 0 && max > 0 && values[j,i] < 0)
						{
							float x = p1.x;
							p1.x = p2.x;
							p2.x = x;						
							AddQuad(new Vector3(p1.x, p1.y, z), new Vector3(p2.x, p1.y, z), new Vector3(p1.x, p2.y, z), new Vector3(p2.x, p2.y, z), colorBottom, colorTop);
						}
						else
						{
							AddQuad(new Vector3(p1.x, p1.y, z), new Vector3(p2.x, p1.y, z), new Vector3(p1.x, p2.y, z), new Vector3(p2.x, p2.y, z), colorTop, colorBottom);
						}

						if (mode_3d)
						{
							float z1 = size.z + z;
							if(min < 0 && max > 0 && values[j,i] < 0)
							{
								AddQuad(new Vector3(p2.x, p1.y, z1), new Vector3(p1.x, p1.y, z1), new Vector3(p2.x, p2.y, z1), new Vector3(p1.x, p2.y, z1), colorBottom, colorTop);

								AddQuad(new Vector3(p2.x, p1.y, z), new Vector3(p2.x, p1.y, z1), new Vector3(p2.x, p2.y, z), new Vector3(p2.x, p2.y, z1), colorBottom, colorTop);

								AddQuad(new Vector3(p1.x, p1.y, z1), new Vector3(p1.x, p1.y, z), new Vector3(p1.x, p2.y, z1), new Vector3(p1.x, p2.y, z), colorBottom, colorTop);

								AddQuad(new Vector3(p1.x, p1.y, z1), new Vector3(p2.x, p1.y, z1), new Vector3(p1.x, p1.y, z), new Vector3(p2.x, p1.y, z), colorBottom, colorBottom);

								AddQuad(new Vector3(p2.x, p2.y, z1), new Vector3(p1.x, p2.y, z1), new Vector3(p2.x, p2.y, z), new Vector3(p1.x, p2.y, z), colorTop, colorTop);
							}
							else
							{
								AddQuad(new Vector3(p2.x, p1.y, z1), new Vector3(p1.x, p1.y, z1), new Vector3(p2.x, p2.y, z1), new Vector3(p1.x, p2.y, z1), colorTop, colorBottom);

								AddQuad(new Vector3(p2.x, p1.y, z), new Vector3(p2.x, p1.y, z1), new Vector3(p2.x, p2.y, z), new Vector3(p2.x, p2.y, z1), colorTop, colorBottom);

								AddQuad(new Vector3(p1.x, p1.y, z1),new Vector3(p1.x, p1.y, z), new Vector3(p1.x, p2.y, z1),new Vector3(p1.x, p2.y, z),colorTop, colorBottom);

								AddQuad(new Vector3(p1.x, p1.y, z1), new Vector3(p2.x, p1.y, z1), new Vector3(p1.x, p1.y, z), new Vector3(p2.x, p1.y, z),colorTop, colorTop);

								AddQuad(new Vector3(p2.x, p2.y, z1), new Vector3(p1.x, p2.y, z1), new Vector3(p2.x, p2.y, z),new Vector3(p1.x, p2.y, z),colorBottom, colorBottom);
							}
						}
					}
				}
			}

			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (meshFilter.sharedMesh == null)
			{
				meshFilter.sharedMesh = new Mesh();
			}
			meshFilter.sharedMesh.Clear();
			meshFilter.sharedMesh.name = "Bar chart";
			meshFilter.sharedMesh.vertices = Vertices.ToArray();
			meshFilter.sharedMesh.triangles = Triangles.ToArray();
			meshFilter.sharedMesh.colors32 = VertexColors.ToArray();

			MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
			Shader shader = Shader.Find("CreativePudding/VertexColor");
			if (renderer.sharedMaterial == null)
			{
				renderer.sharedMaterial = new Material(shader);
			}
			else
			{
				renderer.sharedMaterial.shader = shader;
			}
		}

		///<summary>
		/// Add a quad
		///</summary>
		void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color32 color1, Color32 color2)
		{
			Vertices.Add(a);
			Vertices.Add(b);
			Vertices.Add(c);
			Vertices.Add(d);

			VertexColors.Add(color1);
			VertexColors.Add(color1);
			VertexColors.Add(color2);
			VertexColors.Add(color2);

			Triangles.Add(Vertices.Count - 4);
			Triangles.Add(Vertices.Count - 3);
			Triangles.Add(Vertices.Count - 2);

			Triangles.Add(Vertices.Count - 3);
			Triangles.Add(Vertices.Count - 1);
			Triangles.Add(Vertices.Count - 2);
		}
	}

} //namespace


