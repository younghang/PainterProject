/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2015/5/1
 * Time: 11:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
 

namespace CalculatorDll.Calculator.Calculator
{
	/// <summary>
	/// Description of Data.
	/// 数据保存形式
	/// </summary>
	public class MetaData
	{
		#region implemented abstract members of CalData

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		#endregion

		public MetaData()
		{
		}
		private Option option = null;
		private Double num;
		private FuncNames fun = null;
		public void SetNum(double a)
		{
			this.num = a;
			TypeN = 1;
		}
		public void SetFun(FuncNames a)
		{
			this.fun = a;
			TypeN = 3;
		}
		public void SetOption(Option op)
		{
			this.option = op;
			TypeN = 2;
			
		}
		int TypeN = 0;
		public int GetTypeN()
		{
			return TypeN;
		}
		public double GetNum()
		{
			return num;
		}
		public Option GetOption()
		{
			return option;
		}
		public FuncNames GetFunc()
		{
			return fun;
		}
		
	}
}
