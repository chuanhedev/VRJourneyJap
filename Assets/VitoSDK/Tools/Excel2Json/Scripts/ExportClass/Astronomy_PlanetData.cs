using System.Collections; 

using System; 

[Serializable] 

public class Astronomy_PlanetData {

/// <summary> 
///ID
/// </summary>
public int id;

/// <summary> 
///类型
/// </summary>
public int type;

/// <summary> 
///中文名称
/// </summary>
public string name_cn;

/// <summary> 
///英文名称
/// </summary>
public string name_en;

/// <summary> 
///自身半径/赤道面
/// </summary>
public float selfRadius;

/// <summary> 
///自传周期
/// </summary>
public float selfPeriod;

/// <summary> 
///公转半径/黄道面
/// </summary>
public float orbitRadius;

/// <summary> 
///公转周期
/// </summary>
public float orbitPeriod;

/// <summary> 
///公转离心率
/// </summary>
public float orbitEccentricity;

/// <summary> 
///自转倾角/黄赤交角
/// </summary>
public float selfRotateOffset;

/// <summary> 
///公转倾角
/// </summary>
public float orbitRotateOffset;

/// <summary> 
///初始公转角度
/// </summary>
public float initOrbitRotate;

/// <summary> 
///初始自转角度
/// </summary>
public float initSelfRotate;

}

