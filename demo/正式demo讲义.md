[TOC]

# 整体框架

## 本地文件系统

​	游戏中除了从服务器发来一些数据之外，有些数据是保存在本地的。比如描述一个物品信息，服务器端只会发送过来一个物品ID。物品内部的具体信息（物品的名字，图标路径）是存在本地的，减少网络通信的流量。

本地文件格式类型有非常多种。常见的文件格式有

**XML** 

```xml
<?xml version="1.0" encoding="utf-8"?>
<WwiseSettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <CopySoundBanksAsPreBuildStep>true</CopySoundBanksAsPreBuildStep>
  <CreatedPicker>true</CreatedPicker>
  <CreateWwiseGlobal>true</CreateWwiseGlobal>
  <CreateWwiseListener>false</CreateWwiseListener>
  <GenerateSoundBanksAsPreBuildStep>false</GenerateSoundBanksAsPreBuildStep>
  <ShowMissingRigidBodyWarning>true</ShowMissingRigidBodyWarning>
  <SoundbankPath>Audio/GeneratedSoundBanks</SoundbankPath>
  <WwiseInstallationPathMac />
  <WwiseInstallationPathWindows>C:\Program Files (x86)\Audiokinetic\Wwise 2018.1.5.6835</WwiseInstallationPathWindows>
  <WwiseProjectPath>../../../../common/WwiseProj/princess_WwiseProject/princess_WwiseProject.wproj</WwiseProjectPath>
</WwiseSettings>
```

优点

* 格式统一，配置灵活。
* 可读性高

缺点

* 文件庞大，格式复杂。
* 解析耗时较大

**JSON**

```json
{
    "iggid": 787546,
	"language":"CN",
    "ServerList": [
		
        {
            "name": "主服务器",
            "ip": "10.15.1.10:7088"
        },
		{
            "name":"外网测试服务器",
            "ip":"54.70.167.134:7025"
        },
		{
        "name": "DEV-Test",
        "ip": "10.15.1.10:8000"
      },
      {
        "name": "jibao",
        "ip": "10.15.119.46:7000"
      },
      {
        "name": "剑伟",
        "ip": "10.15.119.43:7000"
      }
           
    ]
}
```

优点

* 格式灵活
* 文件较小
* 解析速度快

缺点

- 无法注释
- 可读性差

**CSV**

```
int,int,int,IntArray,IntArray,IntArray,IntArray
ID,Type,SubType,value1,value2,value3,achievement_id
1,4,0,1,80,,1001
2,9,0,101,50,,1002
3,9,0,102,50,,1003
4,5,0,101,,,1004
5,5,0,102,,,1005
6,23,0,1,2101,,1006
7,23,0,1,2102,,1007
8,23,0,1,2103,,1008
```

优点

- 解析速度快
- 文件体积小
- 跟Excel可以转换

缺点

- 基本无法手写
- 格式固定
- 可读性差



 Unity本身也提供了 **ScriptableObject** 文件格式。

```c#
   [CreateAssetMenu(fileName = "AssetBundleConfige", menuName = "AssetBundle/AssetBundleConfige", order = 200)]
    public class AssetBundleConfigData : ScriptableObject
    {
        public List<ResPackItem> datas = new List<ResPackItem>();
    }
```



```c#
    [Serializable]
    public class ResPackItem
    {
        [Serializable]
        public enum PackStrategy
        {
            /// <summary>
            /// 目录下面每个文件单独打包一个AssetBundle
            /// </summary>
            OneByOne,
            /// <summary>
            /// 目录下面所有的文件打成一个AssetBundle
            /// </summary>
            AllIn,
            /// <summary>
            /// 不递归，把当前目录下面的每个文件夹打包成一个AssetBundle
            /// </summary>
            DirOneNoRec,
            /// <summary>
            /// 递归，把目录下面的每个文件夹打包一个AssetBundle
            /// </summary>
            DirOne,
            /// <summary>
            /// 目录下面所有的文件打成一个AssetBundle
            /// </summary>
            DirAllin,
            ERROR,
        }
        public string path;
        public PackStrategy _strategy;
    }
```

优点

- 跟代码无缝衔接
- 解析非常快
- 格式灵活

缺点

- 无法大规模配置
- 需要写代码配合



在一个项目中，可能存在不同的本地配置数据格式。我们必须要一个本地数据管理模块，在业务层面使用统一的接口访问数据。在业务层面数据都是抽象的。类似的*Key-Value* 格式，这个*key*可以是纯数字的*ID*,也可以是字符串 *key*。



## 网络通信

## 框架层的完成体

# Demo分析

## 功能开发



## 数据驱动的逻辑