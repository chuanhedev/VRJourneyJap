using UnityEngine;
using System.Collections;

using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
using Excel;
using System.Data;
//using System.Data;
#endif
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib;
using System.Text;
//using System.Data;
#if UNITY_EDITOR
public class UGDK : Editor {
	[MenuItem("VitoSDK/Excel/导出表类")]
	public static void ExportExcelClass(){
		ExcelUtility.init ();
		ExcelUtility.ConvertExcel2Class();
        AssetDatabase.Refresh();
		SDK.Log("导出类成功");
	}



    [MenuItem("VitoSDK/Font/生成字库")]
    public static void FontMaker()
    {
        if (Selection.objects.Length != 2)
        {
            return;
        }
        string path1 = null;
        string path2 = null;

        if (AssetDatabase.GetAssetPath(Selection.objects[0]).Contains(".fontsettings"))
        {
            path1 = AssetDatabase.GetAssetPath(Selection.objects[0]);
        }
        if (AssetDatabase.GetAssetPath(Selection.objects[1]).Contains(".fontsettings"))
        {
            path1 = AssetDatabase.GetAssetPath(Selection.objects[1]);
        }
        if (AssetDatabase.GetAssetPath(Selection.objects[0]).Contains(".fnt"))
        {
            path2 = AssetDatabase.GetAssetPath(Selection.objects[0]);
        }
        if (AssetDatabase.GetAssetPath(Selection.objects[1]).Contains(".fnt"))
        {
            path2 = AssetDatabase.GetAssetPath(Selection.objects[1]);
        }
        if (path1 == null || path2 == null)
        {
            return;
        }

        Font m_myFont =
            AssetDatabase.LoadAssetAtPath(path1, typeof(Font)) as Font;
        TextAsset m_data =
            AssetDatabase.LoadAssetAtPath(path2, typeof(TextAsset)) as TextAsset;

        ZBMFont mbFont = new ZBMFont();

        ZBMFontReader.Load(mbFont, m_data.name, m_data.bytes);  // 借用NGUI封装的读取类
        CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphCount];
        Dictionary<int, ZBMGlyph> glyphs = mbFont.GetGlyphs();
        int index = 0;
        foreach (KeyValuePair<int, ZBMGlyph> pair in glyphs)
        {
            ZBMGlyph bmInfo = pair.Value;
            CharacterInfo info = new CharacterInfo();
            info.index = bmInfo.index;
            info.uv.x = (float)bmInfo.x / (float)mbFont.texWidth;
            info.uv.y = 1 - (float)bmInfo.y / (float)mbFont.texHeight;
            info.uv.width = (float)bmInfo.width / (float)mbFont.texWidth;
            info.uv.height = -1f * (float)bmInfo.height / (float)mbFont.texHeight;
            info.vert.x = (float)bmInfo.offsetX;
            //info.vert.y = (float)bmInfo.offsetY;
            info.vert.y = 0f - (float)bmInfo.height / 2;//自定义字库UV从下往上，所以这里不需要偏移，填0即可。(为了设置锚点在0.5，需要减去高度的一半)
            info.vert.width = (float)bmInfo.width;
            info.vert.height = (float)bmInfo.height;
            info.width = (float)bmInfo.advance;
            characterInfo[index] = info;
            index++;
        }
        m_myFont.characterInfo = characterInfo;
        //string ss = m_myFont.name;
        //m_myFont.name = "";
        //m_myFont.name = ss;
        AssetDatabase.Refresh();
        Debug.Log("Font make over");
    }


    [MenuItem ("VitoSDK/Excel/导出表数据")]
	public static void ExportExcelData(){
		ExcelUtility.init ();
		ExcelUtility.ConvertExcel2Json();
        //ZipUtility.zipData();
        AssetDatabase.Refresh();
        SDK.Log("导出数据成功");
	}

//	[MenuItem ("工具/导出编辑器类")]
//	public static void ExportEditorClass(){
//		ExcelUtility.init ("config_","/Editor/ExcelData/");
//		ExcelUtility.ConvertExcel2Class("/Editor/ExportClass/");
//		SDK.Log("导出Editor类成功");
//	}
//	[MenuItem ("工具/导出编辑器数据")]
//	public static void ExportEditorData(){
//		ExcelUtility.init ("config_","/Editor/ExcelData/");
//		ExcelUtility.ConvertExcel2Json("/Editor/ExportData/");
//		SDK.Log("导出Editor数据成功");
//	}


    //[MenuItem("工具/生成字库")]
    //public static void FontMaker()
    //{
    //    if (Selection.objects.Length != 2)
    //    {
    //        return;
    //    }
    //    string path1 = null;
    //    string path2 = null;

    //    if (AssetDatabase.GetAssetPath(Selection.objects[0]).Contains(".fontsettings"))
    //    {
    //        path1 = AssetDatabase.GetAssetPath(Selection.objects[0]);
    //    }
    //    if (AssetDatabase.GetAssetPath(Selection.objects[1]).Contains(".fontsettings"))
    //    {
    //        path1 = AssetDatabase.GetAssetPath(Selection.objects[1]);
    //    }
    //    if (AssetDatabase.GetAssetPath(Selection.objects[0]).Contains(".fnt"))
    //    {
    //        path2 = AssetDatabase.GetAssetPath(Selection.objects[0]);
    //    }
    //    if (AssetDatabase.GetAssetPath(Selection.objects[1]).Contains(".fnt"))
    //    {
    //        path2 = AssetDatabase.GetAssetPath(Selection.objects[1]);
    //    }
    //    if (path1 == null || path2 == null)
    //    {
    //        return;
    //    }
    //    Font m_myFont =
    //        AssetDatabase.LoadAssetAtPath(path1, typeof(Font)) as Font;
    //    TextAsset m_data =
    //        AssetDatabase.LoadAssetAtPath(path2, typeof(TextAsset)) as TextAsset;

    //    BMFont mbFont = new BMFont();
    //    BMFontReader.Load(mbFont, m_data.name, m_data.bytes);  // 借用NGUI封装的读取类
    //    CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphCount];
    //    Dictionary<int, BMGlyph> glyphs = mbFont.GetGlyphs();
    //    int index = 0;
    //    foreach (KeyValuePair<int, BMGlyph> pair in glyphs)
    //    {
    //        BMGlyph bmInfo = pair.Value;
    //        CharacterInfo info = new CharacterInfo();
    //        info.index = bmInfo.index;
    //        info.uv.x = (float)bmInfo.x / (float)mbFont.texWidth;
    //        info.uv.y = 1 - (float)bmInfo.y / (float)mbFont.texHeight;
    //        info.uv.width = (float)bmInfo.width / (float)mbFont.texWidth;
    //        info.uv.height = -1f * (float)bmInfo.height / (float)mbFont.texHeight;
    //        info.vert.x = (float)bmInfo.offsetX;
    //        //info.vert.y = (float)bmInfo.offsetY;
    //        info.vert.y = 0f - (float)bmInfo.height / 2;//自定义字库UV从下往上，所以这里不需要偏移，填0即可。(为了设置锚点在0.5，需要减去高度的一半)
    //        info.vert.width = (float)bmInfo.width;
    //        info.vert.height = (float)bmInfo.height;
    //        info.width = (float)bmInfo.advance;
    //        characterInfo[index] = info;
    //        index++;
    //    }

    //    m_myFont.characterInfo = characterInfo;
    //    string ss = m_myFont.name;
    //    m_myFont.name = "";
    //    m_myFont.name = ss;
    //    //AssetDatabase.Refresh();  
    //    //AssetDatabase.SaveAssets();
    //    Debug.Log("Font make over!");
    //}


	[MenuItem("VitoSDK/Tools/清除存档")]
	public static void Clear(){
		DirectoryInfo dirInfo=new DirectoryInfo(Application.persistentDataPath);
		foreach(var file in dirInfo.GetFiles()){
			SDK.Log(file.Name);
			file.Delete();
		}
        AssetDatabase.Refresh();
        SDK.Log("清除成功");
	}


}
#endif

#if UNITY_EDITOR
public class ExcelUtility{

	private static List<string> fileList=new List<string>();
	private static string rootPath=Application.dataPath+"/VitoSDK/Tools/Excel2Json";

	public static  void init(string perFix="data",string midPath="/ExcelData/"){
		fileList.Clear();
		DirectoryInfo directInfo=new DirectoryInfo(rootPath+midPath);
		foreach (var file in directInfo.GetFiles()){
			if(file.Name.StartsWith(perFix)&&(file.Name.EndsWith(".xlsx")||file.Name.EndsWith(".xls"))){
				Debug.Log(file.FullName);
				fileList.Add(file.FullName);
			}
		}

	}

	public static void  ConvertExcel2Class(string midPath="/Scripts/ExportClass/"){

		foreach( var srcFilePath in fileList){
			List<string > inlineStrList=new List<string>();
			FileStream excelFile =File.Open(srcFilePath,FileMode.Open,FileAccess.Read);
			Debug.Log("准备导出类："+excelFile.Name);
			IExcelDataReader excelReader=ExcelReaderFactory.CreateOpenXmlReader(excelFile);
            DataSet dataSet=excelReader.AsDataSet();

			var table0=dataSet.Tables[0];
			int columns=table0.Columns.Count;
			string className=table0.TableName;
			inlineStrList.Add("using System.Collections; \n");
            inlineStrList.Add("using System; \n");
            inlineStrList.Add("[Serializable] \n");
			inlineStrList.Add("public class "+className+" {\n");
			for(int c=0;c<columns;c++)
            {
				//if(c!=1)
                { //第二列用于注释
					string valueType=table0.Rows[2][c].ToString().Trim();
					if(string.IsNullOrEmpty(valueType))
                    {
                        Debug.Log("导出类失败");
                        return;
                    }
					string valueName=table0.Rows[1][c].ToString().Trim();
					string tips=table0.Rows[0][c].ToString().Trim();
					inlineStrList.Add("/// <summary> \n"+"///"+tips+"\n"+"/// </summary>");
					inlineStrList.Add("public "+valueType+" "+valueName+";\n");
				}
			}
			inlineStrList.Add("}\n");
			string destP=rootPath+midPath+className+".cs";
            Debug.Log("classPath:"+destP);
			FileInfo classFile=new FileInfo(destP);

			if( File.Exists(destP)){
				File.Delete(destP);
			}
			StreamWriter sw=classFile.CreateText();
			for(int i=0;i<inlineStrList.Count;i++){
				sw.WriteLine(inlineStrList[i]);
			}
			sw.Flush();
			sw.Close();
			sw.Dispose();
		}
	}
	public static void ConvertExcel2Json(string midPath="/ExportData/"){
		System.Reflection.Assembly assembly=System.Reflection.Assembly.Load("Assembly-CSharp");
		foreach( var srcFilePath in  fileList){
			FileStream excelFile=File.Open(srcFilePath,FileMode.Open,FileAccess.Read);
			IExcelDataReader excelReader=ExcelReaderFactory.CreateOpenXmlReader(excelFile);
			DataSet dataSet=excelReader.AsDataSet();
			for(int i=0;i<1;i++){
				DataTable table=dataSet.Tables[i];
				int colums =table.Columns.Count;
				int rows=table.Rows.Count;
				string classType=table.TableName.ToString();
				Debug.Log(classType);
				System.Type type =assembly.GetType(classType);
				IList<object> list=new List<object>();

				for(int r=3;r<rows;r++){

					object obj=Activator.CreateInstance(type);
					list.Add(obj);

					for(int c=0;c<colums;c++){                   
							string value=table.Rows[r][c].ToString().Trim();
                            if(c==0&&string.IsNullOrEmpty(value))
                            {
                                list.Remove(obj);
                                break;
                            }
							string field=table.Rows[1][c].ToString().Trim();
							if(string.IsNullOrEmpty(field)){continue;}
							FieldInfo fi=type.GetField(field);
							string filedType=table.Rows[2][c].ToString().Trim();
							filedType=filedType.ToLower();
							try{
								switch(filedType){
								case "int[]":
									if(string.IsNullOrEmpty(value)){
										fi.SetValue(obj, null);
									}else{
                                           
                                            List<int> array = new List<int>();
                                            if(value.Contains("|"))
                                            {
                                               value= value.Replace('|', ',');
                                            }
                                            if (value.Contains("(")||value.Contains(")"))
                                            {
                                                //char[] trimChar = new char[2] { '(', ')' };
                                                value=value.Replace('(', ' ');
                                                value = value.Replace(')',' ');
                                                value=value.Trim();
                                            }

                                          //  Debug.Log(""+value);
                                            {
                                                string[] items = value.Split(',');
                                                
                                                for (int itemI = 0; itemI < items.Length; itemI++)
                                                {
                                                    string item = items[itemI];
                                                    int valueII = 0;
                                                    if (item.Contains("-"))
                                                    {
                                                        string startS = item.Split('-')[0];
                                                        string endS = item.Split('-')[1];
                                                        int start = int.Parse(startS.Trim());
                                                        int end = int.Parse(endS.Trim());
                                                        for (int ll = start; ll <= end; ll++)
                                                        {
                                                            array.Add(ll);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (int.TryParse(item.Trim(), out valueII))
                                                        {
                                                            array.Add(valueII);
                                                        }
                                                    }
                                                   
                                                }
                                            }
                                            int[] intArray = new int[array.Count];
                                            intArray = array.ToArray();
                                            fi.SetValue(obj, intArray);

                                        }

									break;
								case "int":
									int valueI=0;
									int.TryParse(value, out valueI);
									fi.SetValue(obj,valueI);
									break;
								case "string":
									string valueS="";
									valueS=value;
									fi.SetValue(obj,valueS);
									break;
                                case "string[]":
                                        string[] valuess = value.Split(new char[] { '，', '，' });
                                        fi.SetValue(obj, valuess);
                                        break;
								case "bool":
								case "boolean":
									bool valueB=false;
									if(value.ToLower()=="true"){
										valueB=true;
									}
									fi.SetValue(obj,valueB);
									break;
                                case "float":
                                    float valueF = 0;
                                    float.TryParse(value,out valueF);
                                    fi.SetValue(obj, valueF);
                                    break;
								}
							}catch	(Exception e) {
                                Debug.Log("c:"+c+"r:"+r);
								Debug.LogError("fieldType:"+filedType+"field:"+field+"value:"+value);
								Debug.Log(e);
							}
					}
				}
				string str=JsonConvert.SerializeObject(list);
				string destPath=rootPath+midPath;
				FileUtility.save2File(classType+".txt",str,destPath);
			}
			excelFile.Close();
		}
	}

}

#endif
