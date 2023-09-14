using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculatorDll.Calculator.UIController.Data;
using CalculatorDll.Calculator.Function;
using System.Threading;
using CalculatorDll.Calculator.Calculator;

namespace CalculatorDll.Calculator.UIController
{
	public  delegate void ShowMessage(string info,string type);
	public delegate string GetMessage(); 
	public class UIController
	{
		public static event ShowMessage ShowOutput;
		public event GetMessage GetInput; 
		string InputData = "";
		static TempDataCollection tdc = new TempDataCollection();
		Function.FuncString funstring = new Function.FuncString();

		public UIController()
		{			
		}
		public void GetDataToInput()
		{
			//Console.Write(">>");
			//InputData = Console.ReadLine();
			ShowOutput(">>",">>");			
		}
		//   2*3e
		void AnalyseInputG(int blacket)
		{
			if (blacket == 4)
			{//有括号的
				if (IsOrder())
				{
					//					PushToShow("进入命令程序");
					Command cm = new Command();
					var funs = funstring.GetFuncExpressTemp();
					switch (funstring.GetFuncName())
					{

						case "plot":
							string[] paramers = funstring.GetParamersData();
							FuncString funcs = FindFuncInTDC(paramers[0]);
							double left = 0;
							double right = 1;
							try
							{
								left = double.Parse(paramers[1]);
								right = double.Parse(paramers[2]);
							}
							catch(Exception)
							{
								if (tdc.CheckName(paramers[1]))
								{
									BlockData bd = (BlockData)tdc.GetData(paramers[1]).GetCalData();
									if (bd != null)
									{
										left = bd.GetXStart();
										right = bd.GetXEnd();
									}

								}

							}
							if (funcs != null)
							{
								new Thread(new ThreadStart(() => cm.PlotFunc(funcs, 6, 1000, left, right))).Start();
								//								new Thread(new ThreadStart(()=>cm.PlotFuncParallel(funcs, 6, 1000, -10, 10))).Start();
							}
							else
								PushToShow("Wrong!", "error");
							break;
						case "list":

							#region 处理list
							bool needJudge = true;
							DataType dt = DataType.NONE;
							switch (funs)
							{
								case "exp":
									dt = DataType.EXP;
									break;
								case "matrix":
									dt = DataType.MATRIX;
									break;
								case "block":
									dt = DataType.BLOCK;
									break;
								case "func":
									dt = DataType.FUNC;
									break;
								default:
									needJudge = false;
									break;
							}
							TempData td;
							for (int i = 1; i < tdc.GetLength(); i++)
							{
								td = tdc.GetData(i);
								bool show = false;
								if (td != null)
								{
									CalData cd = td.GetCalData();
									if (cd.CalType == dt)
									{
										show = true;
									}



									if (!needJudge || show)
									{
										PushToShow(td.ToString());
									}
								}

							}
							#endregion

							break;
						case "beep":
							int times = 10;
							int.TryParse(funs, out times);
							if (times == 0)
							{
								times = 10;
							}
							for (int i = 0; i < times; i++)
							{
								PushToShow("倒计时" + (times - i - 1) + "s");
								Console.Beep(1000 * 7, 500);
								Console.Beep(500, 500);
							}
							Console.Beep(30000, 1000);

							break;
						case "matrix":
							 
							break;

						case "clc":

							if (string.IsNullOrEmpty(funs))
							{
								tdc.RemoveAll();
								PushToShow("已清空存储数据");
							}
							else
							{
								if (tdc.CheckName(funs))
								{
									tdc.Remove(tdc.FindPosition(funs));
									PushToShow("clear " + funs);
								}
								else
									PushToShow("fail to clear.Can't find " + funs, "error");

							}
							break;
						case "exit":
							Environment.Exit(0);
							break;
						case "clear":
							Console.Clear();
							break;//----------------------这个GetFuncExpressTemp最多返回""不会返回null
						case "help":
							cm.Help(funstring.GetFuncExpressTemp());
							break;
					}
					return;
				}
				if (tdc.CheckName(funstring.GetFuncName()))
				{
					PushToShow("该函数存在数据中");

					if (funstring.GetParamers() != null)
					{
						//PushToShow("进入函数求值过程");
						FuncString funs = FindFuncInTDC(funstring.GetFuncName());
						double result = GetFuncResult(funs, new FuncString(InputData).GetParamersData());
						PushToShow("结果:" + result,"result");
					}
					else
						PushToShow("没有输入参数");
					return;
					//					PushToShow("这是啥：" + tdc.GetData(funstring.GetFuncName()).GetValue().ToString());
				}
				{
					var str = SimpleCal(InputData);
					if (str == null)
					{
						PushToShow("没有此函数");
					}
					else
						ShowOutput(str+'\n',"result");

					return;
				}

			}


			if (blacket == 1)
			{//没有括号 Name即是计算的表达式
				var Name = funstring.GetFuncName();
				CheckDataType(Name);

			}
		}

		void CheckDataType(string Name)
		{
			if (tdc.CheckName(Name))
			{//简单输出显示 变量的值
				CalData datatemp;
				try
				{
					datatemp = tdc.GetData(Name).GetCalData();
				}
				catch (AssignedError e)
				{
					PushToShow(e.Message, "error");
					return;
				}

				switch (datatemp.CalType)
				{
					case DataType.BLOCK:
						{
							BlockData bd = (BlockData)datatemp;
							PushToShow(bd.ToString());
						}
						break;
					case DataType.EXP:
						{
							ExpData ed = (ExpData)datatemp;
							try
							{
								PushToShow(ed.ToString(),"result");
							}
							catch (ExpError e)
							{
								PushToShow(e.Message);
							}
							break;
						}
					case DataType.MATRIX:
						{
							Matrix matrix = (Matrix)datatemp;
							PushToShow(matrix.ToString());
						}
						break;
					case DataType.FUNC:
						{
							FuncData fd = (FuncData)datatemp;
							PushToShow(fd.ToString());
						}
						break;
				}
			}
			else
			{
				var str = SimpleCal(InputData);
				if (str == null)
				{
					PushToShow("No result");
				}
				else
					ShowOutput(str+'\n',"result");
			}


		}
		internal static string SimpleCal(string inputexpress)
		{
			bool wrong = false;
			string sf = ReplaceParamers(inputexpress);
			if (sf != null)
			{
				StringCalculatorProvided sc = new StringCalculatorProvided();
				sc.SetCalString(sf);

				double result = sc.GetResult();
				if (sc.IsError())
				{
					//PushToShow("Wrong Input");
					PushToShow(sc.GetErrorMessage(), "error");
					wrong = true;
				}
				else
					return result.ToString();
			}


			PushToShow("表达式错误", "error");


			if (wrong)
			{
				return null;
			}
			return null;
		}
		/// <summary>
		/// 给定函数的名字 返回修改Funcexpresstemp后的FuncString对象 连同变量一同替换 一步到位  
		/// </summary>
		/// <param name="funname">函数名</param>
		/// <returns>FuncString 或者 null</returns>
		public FuncString FindFuncInTDC(string funname)
		{
			FuncString fs = null;
			if (tdc.CheckName(funname))
			{
				FuncString funs = tdc.GetData(funname).ConvertTOFunString();
				string funcexpress = ReplaceParamers(funs.GetFuncExpress(), funs.GetParamers());
				if (funcexpress != null)
				{
					funs.SetFuncExpress(funcexpress);
				}
				fs = funs;
			}
			return fs;
		}

		//f(x,y)=x+y+t

		/// <summary>
		/// 替换已知变量，fstr是一个字符串表达式:x+y 不是f=x+y ，
		/// 没有变量是会原串返回的
		/// 里面找未知数时会调用底层 CalString 的函数，里面已经自动排除sin，cos，avg等 函数名了
		/// </summary>
		/// <param name="fstr">含参表达式</param>
		/// <param name="ps">排除变量</param>
		/// <returns>返回FuncExpressTemp为替换后的string
		/// 或含有未知变量返回null</returns>
		public static string ReplaceParamers(string fstr, string[] ps = null)
		{
			string[] strarr = null;
			FuncString fs = new FuncString(fstr);
			//			fs.SetFuncExpressTemp(fstr);
			fs.SetFuncExpress(fstr);//为向下兼容不出错


			FuncCal fc = new FuncCal();
			if (ps == null)
			{
				strarr = fs.FindUnKnown();
			}
			else
				strarr = fs.FindUnKnown(ps);

			fs.SetParamers(strarr);//为向下兼容不出错,不明觉厉
			if (strarr.Length != 0)
				PushToShow("含有未知数 尝试替换已知变量");

			string[] paramersdata = new string[strarr.Length];
			fc.SetFuncString(fs);
			for (int i = 0; i < strarr.Length; i++)
			{
				if (tdc.CheckName(strarr[i]))
				{
					CalData datatemp = tdc.GetData(strarr[i]).GetCalData();

					switch (datatemp.CalType)
					{
						case DataType.BLOCK:
							{
								PushToShow("Type not match!", "error");
							}
							break;
						case DataType.EXP:
							{
								ExpData ed = (ExpData)datatemp;
								paramersdata[i] = ed.GetValueEx();
								//PushToShow("提取" + strarr[i] + "的值：" + paramersdata[i]);
								break;
							}
						case DataType.MATRIX:
							{
								Matrix matrix = (Matrix)datatemp;
								PushToShow(matrix.ToString());
							}
							break;
					}

					//					if (datatemp.CalType == DataType.EXP) {
					//						ExpData ed = (ExpData)datatemp;
					//						paramersdata[i] = ed.GetValueEx();
					//						PushToShow("提取" + strarr[i] + "的值：" + paramersdata[i]);
					//					}
					//
					//					else
					//						PushToShow("Type not match!");
					continue;

				}

				PushToShow("未发现变量" + strarr[i] + "还未定义其值\n退出","error");
				return null;


			}
			if (paramersdata.Length != 0)
				fc.DealFuncstring(paramersdata);
			else
				fs.SetFuncExpressTemp(fstr);
			return fc.GetFuncString().GetFuncExpressTemp();



		}

		/// <summary>
		/// 检查是否重名
		/// </summary>
		/// <returns></returns>
		private bool CheckDuplicationName(string strName)
		{
			if (tdc.CheckName(strName))
			{
				//PushToShow("变量重复,就不更新掉了");

				return true;
			}
			return false;
		}
		private bool IsOrder()
		{
			bool IsOrde = false;
			if (Command.cl.Contains(funstring.GetFuncName()))
			{
				IsOrde = true;
			}
			return IsOrde;
		}

		/// <summary>
		/// 给定函数FuncString类型 求值
		/// </summary>
		/// <param name="fs">必须得有参数 名字 表达式</param>
		/// <param name="nums">替换的参数</param>
		/// <returns>返回计算结果</returns>
		public double GetFuncResult(FuncString fs, params string[] nums)
		{//这里的fs应该时是完整的?
			double result = 0;
			if (fs != null)
			{//改一下

				FuncCal fc = new FuncCal();
				//				FuncString funs=tdc.GetData(fs).ConvertTOFunString();
				fc.SetFuncString(fs);
				fc.DealFuncstring(nums);
				result = fc.GetResult();

				if (fc.IsWrong())
				{
					PushToShow(fc.ErrorMessage(),"error");
				}
			}
			return result;
		}
		public void Run(string str="")
		{
			if (str == "")
			{
				InputData = GetInput();

			}
			else {
				InputData = str;
			}
			//				PushToShow("进入run程序");
			funstring.SetString(InputData);
			int Extype = funstring.AnalyseEquation();
			if (Extype == 1 || Extype == 4)
			{//没有等号 判断为命令或求值之类
				//					PushToShow("不是存值");
				AnalyseInputG(Extype);
				return;
			}
			//保存起来
			string funs = funstring.GetFuncName();

			CalData caldata = null;
			switch (Extype)
			{

				case 2:

					DealAssignment da = new DealAssignment(funstring.GetFuncExpress());
					if (tdc.CheckName(funs))
					{
						DataType getdt = new DataType();
						try
						{
							getdt = da.GetDataType();
						}
						catch (ExpError e)
						{
							PushToShow(e.Message, "error");
							return;
						}
						if (getdt == DataType.EXP)
						{
							try
							{
								((ExpData)(da.GetCalData())).GetValueFinal();//防止循环调用SimpleCal--> ReplaceParamers--> GetValueEx-->SimpleCal
							}
							catch (AssignedError e)
							{
								PushToShow(e.Message,"error");
								return;
							}

						}

					}
					try
					{
						caldata = da.GetCalData();
					}
					catch (AssignedError e)
					{
						PushToShow(e.Message, "error");
						return;
					}

					break;
				case 3:
					caldata = new FuncData(funstring.GetFuncExpress(), funstring.GetParamers());
					break;
				default:
					break;

			}
			if (CheckDuplicationName(funs))
			{
				tdc.Remove(tdc.FindPosition(funs));
			}
			if (funstring.CheckFunName(funs))
			{
				PushToShow("Variable Name not acceptable!", "error");
				return;
			}
			if (tdc.CreateData(funstring.GetFuncName(), caldata))
				PushToShow("数据已保存");
			else
				PushToShow("Not accept input!","error");
			#region 之前写的不好就改了


			//				int a = funstring.FindOp(0, '=');
			//				int b = funstring.FindOp(0, '(');
			//				if (a != -1) {
			//
			//
			//					if (b > a) {
			//						funstring.AnalyseEquation();
			////						funstring.ShowParamers();
			//						if(CheckDuplicationName()) continue;
			//
			//						tdc.CreateData(funstring.GetFuncName(), funstring.GetFuncExpress(), funstring.GetParamers());
			//					} else {
			//						funstring.AnalyseVariable();
			////						funstring.ShowParamers();
			//						if(CheckDuplicationName()) continue;
			//						tdc.CreateData(funstring.GetFuncName(), funstring.GetFuncExpress());
			//					}
			//				} else {
			//
			//				}


			#endregion
		 
		}

		public static void PushToShow(string toshow,string type="")
		{
			//有界面再用到
			//Console.WriteLine(toshow);
			ShowOutput(toshow+"\n", type);
		}
		#region  废弃


		public double FuncResult(string fs, TempDataCollection tdcl, params string[] nums)
		{
			double result = 0;
			if (fs != null)
			{
				//PushToShow("有括号求值");
				FuncCal fc = new FuncCal();
				FuncString funs = tdcl.GetData(fs).ConvertTOFunString();
				fc.SetFuncString(funs);
				fc.DealFuncstring(nums);
				result = fc.GetResult();
				PushToShow( fc.GetResult().ToString(),"result");
				if (fc.IsWrong())
				{
					PushToShow("出错了！" + fc.ErrorMessage(), "error");
				}
			}
			return result;
		}
		#endregion
	}
}
