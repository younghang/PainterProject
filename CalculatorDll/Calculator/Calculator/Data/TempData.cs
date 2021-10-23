using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorDll.Calculator.Calculator.Data
{
	public class TempData
	{
		public TempData(string name, CalData value)
		{
			this.Name = name;
			this.Value = value;
		}


		private string Name = "";
		private CalData Value = null;

		public string GetName()
		{
			return Name;
		}
		public CalData GetCalData()
		{
			return Value;
		}
		public void SetValue(CalData value)
		{
			this.Value = value;
		}
		public void SetName(string name)
		{
			this.Name = name;
		}
		public override string ToString()
		{
			return string.Format("[Data Name={0}, Value=( {1} )]", Name, Value.ToString());
		}

		public Function.FuncString ConvertTOFunString()
		{
			FuncData fd = (FuncData)Value;
			return new Function.FuncString(Name, fd.GetExpValue(), fd.GetParamers());
		}

	}
}
