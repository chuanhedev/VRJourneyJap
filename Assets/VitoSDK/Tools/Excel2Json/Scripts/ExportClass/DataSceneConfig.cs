using System.Collections; 

using System; 

[Serializable] 

public class DataSceneConfig {

/// <summary> 
///ID
/// </summary>
public int id;

/// <summary> 
///ui列表中的顺序
/// </summary>
public int order;

/// <summary> 
///是否显示
/// </summary>
public bool isVisiable;

/// <summary> 
///名称
/// </summary>
public string name;

/// <summary> 
///描述
/// </summary>
public string describe;

/// <summary> 
///图标
/// </summary>
public string icon;

/// <summary> 
///标签
/// </summary>
public string[] tags;

/// <summary> 
///场景文件路径
/// </summary>
public string path;

}

