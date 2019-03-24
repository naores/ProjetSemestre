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
	///<summary>
	/// 1D Data set
	///</summary>
	public class ChartData1D : ChartData
	{
		///<summary>
		/// Get or set data with operator[column]
		///</summary>
		public float this[int column]
		{
			get
			{
				return GetInternal(0, column);
			}
			set
			{
				SetInternal(0, column, value);
			}
		}
	}

} //namespace


