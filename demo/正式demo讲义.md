[TOC]

# 整体框架

## 本地文件系统

​	游戏中除了从服务器发来一些数据之外，有些数据是保存在本地的。比如描述一个物品信息，服务器端只会发送过来一个物品ID。物品内部的具体信息（物品的名字，图标路径）是存在本地的，减少网络通信的流量。

本地文件格式类型有非常多种。常见的文件格式有

### 常用格式

#### XML

> [XML - Wikipedia](https://en.wikipedia.org/wiki/XML)
>
> [C# XML](https://docs.microsoft.com/zh-cn/previous-versions/gg145036(v=vs.110))

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

#### JSON

> [LitJson](https://github.com/LitJSON/litjson) 可以解析复杂格式
>
> Unity系统自带[JsonUtility](https://docs.unity3d.com/Manual/JSONSerialization.html) (不能解析复杂格式)
>
> [Newtonsoft.Json](https://github.com/SaladLab/Json.Net.Unity3D/releases) 可以解析复杂格式

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
        "name": "abc",
        "ip": "10.15.119.46:7000"
      },
      {
        "name": "xx",
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

#### CSV

> [Csv Wikipedia ](https://en.wikipedia.org/wiki/Comma-separated_values)
>
> [网络上 简单解析](https://www.jianshu.com/p/55b12878c926)

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
- 格式僵硬，要整体修改，不能修改局部
- 可读性差



####  Scriptable Object

> [Unity文档](https://docs.unity3d.com/ScriptReference/ScriptableObject.html)
>
> [网络上一些使用说明](https://www.jianshu.com/p/d2bf2b9436b4)



```c#
   [CreateAssetMenu(fileName = "AssetBundleConfige", menuName = 		        "AssetBundle/AssetBundleConfige", order = 200)]
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
            ERROR,
        }
        public string path;
        public PackStrategy _strategy;
    }
```

![](pic\scriptObject.jpg)

优点

- 跟代码无缝衔接
- 解析非常快
- 格式灵活

缺点

- 无法大规模配置
- 需要写代码配合



在一个项目中，可能存在不同的本地配置数据格式。我们必须要一个本地数据管理模块，在业务层面使用统一的接口访问数据。在业务层面数据都是抽象的。类似的`Key-Value` 格式，这个`key`可以是纯数字的*ID*,也可以是字符串 `key`。

### 项目的选择

#### 配置文件

一般小型的配置文件，我们推荐用`JSON`格式，灵活解析速度快。而且跟服务端通信复杂格式我们也推荐使用`JSON`

比如配置更新文件

```json
{
    "versions": [
        {
            "version": "0.0.1",
            "forceupdate": false,
            "path": "update-0-3-12"
        }
    ]
}
```

因为这样的配置，一般量不会太大，会手动修改。随时会添加新的字段。可读性也不错。对比`XML `优势，主要在通用性考量。服务端或者Web的 我们用Http协议获得都是`Json`居多。 如果系统自带的 `JsonUtility` 库还是不错的选择，如果`Json`格式比较复杂，使用`LitJson` 或者`Newtonsoft.Json` 都是可以的。

#### 常规配置

常规的配置，如果规模比较大。上面说的这些文件格式都不是特别合适。

* 没有合适的编辑器工具，手写容易出错
* 对于策划，非程序员不好理解

所以这部分常规数据配置工作，一般项目组推荐使用`Excel` 编辑。在实际项目的使用过程中转成`csv`格式读取。

如果在项目组有一些是代码生成的配置，比如有节课我们的说的界面的配置信息。还有`assetbundle` 映射信息，这些都是通过程序生成的。这些配置推荐使用`scriptableObject`。因为写入的时候都是通过代码序列化，读取反序列的过程代码都是复用的，比较好操作。

### 工程实现

```C#
public class Table
{
     private Dictionary<string, TableRecord> m_map = new Dictionary<string, TableRecord>();
    public bool LoadTable(string contextstr,string filename,int mode = 0)
    {
        //TODO:
    }
    public int count()
    {
         return m_map.Count;
    }
    public TableRecord query(string key)
    {
         //TODO:
    }
    public  TableRecord query(int nid)
    {
          
    }
    public  TableRecord queryIndex(int nindex)
    {
          
    }
}
```





```
public class TableRecord
    {
        private Dictionary<string, TableData> m_datas = new Dictionary<string, TableData>();

        public string MainKey { set; get; }

        public TableData Get(string key)
        {
            if (m_datas.ContainsKey(key))
            {
                return m_datas[key];
            }
            return null;
        }

        public bool HasValue(string key)
        {
            return m_datas.ContainsKey(key);
        }

        public void AddValue(string key, TableData value)
        {
            if (m_datas.ContainsKey(key))
            {
                m_datas[key] = value;
            }
            else
            {
                m_datas.Add(key, value);
            }
        }
    }
```



```c#

    public class LocalData : BaseComponentTemplate<LocalData>
    {
        Dictionary<string, Table> datas = new Dictionary<string, Table>();
        public override bool Init(AppConfig config)
        {
            DebugLog.Log("LocalData");
            return base.Init(config);
        }

        public Table GetTable(string key)
        {
            Table table;
            if(datas.TryGetValue(key, out table))
            {
                return table;
            }
            return null;
        }

        public void AddTable(string key, Table table)
        {
            datas[key] = table;
        }

    }
```

## 消息中心

```c#
namespace FrameWork
{
    public delegate void MsgDelegate(IMessage message);

    public interface IEventSet
    {
        void RegisterMsg(string name, MsgDelegate cb);

        void ExecuteMsg(IMessage message);

        void RemoveMsg(string name, MsgDelegate cb);

        bool HasMsg(string name, MsgDelegate cb);

        void RemoveAll();

    }
}

```



```c#
using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class EventSet : IEventSet
    {
        private Dictionary<string, Delegate> _MsgMap = null;

        public EventSet()
        {
            _MsgMap = new Dictionary<string, Delegate>();
        }
        public virtual void RegisterMsg(string name, MsgDelegate cb)
        {
            Delegate del;
            if (_MsgMap.TryGetValue(name,out del))
            {
                _MsgMap[name] = Delegate.Combine(del, cb);
            }
            else
            {
                _MsgMap[name] = cb;

            }
        }

        public virtual void ExecuteMsg(IMessage message)
        {
            Delegate del;
            if (_MsgMap.TryGetValue(message.Name, out del))
            {
                MsgDelegate cb = del as MsgDelegate;
                if (null != cb)
                {
                    cb(message);
                }
            }
        }

        public virtual void RemoveMsg(string name, MsgDelegate cb)
        {
            Delegate dMulti;
            if (_MsgMap.TryGetValue(name, out dMulti))
            {
                Delegate currentDel = Delegate.Remove(dMulti, cb);

                if (currentDel == null)
                {
                    _MsgMap.Remove(name);
                }
                else
                {
                    _MsgMap[name] = currentDel;
                }
            }
        }

        public virtual bool HasMsg(string name, MsgDelegate cb)
        {
            if (!_MsgMap.ContainsKey(name)) return false;


            Delegate[] dels = _MsgMap[name].GetInvocationList();
            for (int i = 0; i < dels.Length; i++)
            {
                if (dels[i].Equals(cb))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual  void RemoveAll()
        {
            _MsgMap.Clear();
        }
    }
}

```



```C#

using System;
public class Message : IMessage
{
    public Message(string name)
        : this(name, null, null)
	{ }

    public Message(string name, object body)
        : this(name, body, null)
	{ }

    public Message(string name, object body, string type)
	{
		m_name = name;
		m_body = body;
		m_type = type;
	}

	/// <summary>
	/// Get the string representation of the <c>Notification instance</c>
	/// </summary>
	/// <returns>The string representation of the <c>Notification</c> instance</returns>
	public override string ToString()
	{
		string msg = "Notification Name: " + Name;
		msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
		msg += "\nType:" + ((Type == null) ? "null" : Type);
		return msg;
	}

	/// <summary>
    /// The name of the <c>Notification</c> instance
    /// </summary>
	public virtual string Name
	{
		get { return m_name; }
	}
		
    /// <summary>
    /// The body of the <c>Notification</c> instance
    /// </summary>
	/// <remarks>This accessor is thread safe</remarks>
	public virtual object Body
	{
		get
		{
			// Setting and getting of reference types is atomic, no need to lock here
			return m_body;
		}
		set
		{
			// Setting and getting of reference types is atomic, no need to lock here
			m_body = value;
		}
	}
		
	/// <summary>
	/// The type of the <c>Notification</c> instance
	/// </summary>
	/// <remarks>This accessor is thread safe</remarks>
	public virtual string Type
    {
		get
		{
			// Setting and getting of reference types is atomic, no need to lock here
			return m_type;
		}
		set
		{
			// Setting and getting of reference types is atomic, no need to lock here
			m_type = value;
		}
	}

	/// <summary>
    /// The name of the notification instance 
    /// </summary>
	private string m_name;

    /// <summary>
    /// The type of the notification instance
    /// </summary>
	private string m_type;

    /// <summary>
    /// The body of the notification instance
    /// </summary>
	private object m_body;
}


```



## 网络通信

现在网络游戏中，常用的数据传输协议一般有三种 `TCP` `UDP` `HTTP`,前两者都是长链接协议，大部分的网络游戏都是基于这两个。`Http`这样的短链接的协议在很多卡牌游戏或者网络操作游戏的应用比较多。

`TCP` `UDP` 都是在OSI中的应用层，`Http`实在引用层中。`Http`的本质还是基于`Tcp`的，其中的原理大家看一些文档说明。我们今天说的是，常见的`TCP` 和`UDP`。

### TCP

`TCP`全称是`Transmission Control Protocol`，中文名为传输控制协议。

它可以提供可靠的、面向连接的网络数据传递服务。传输控制协议主要包含下列任务和功能

- 确保IP数据报的成功传递。

- 对程序发送的大块数据进行分段和重组。

- 确保正确排序及按顺序传递分段的数据。

- 通过计算校验和，进行传输数据的完整性检查。

原理：

`TCP`的连接建立过程又称为TCP三次握手：

首先发送方主机向接收方主机发起一个建立连接的同步（`SYN`）请求；

接收方主机在收到这个请求后向发送方主机回复一个同步/确认（`SYN/ACK`）应答；

发送方主机收到此包后再向接收方主机发送一个确认（`ACK`），此时`TCP`连接成功建立.

一旦初始的三次握手完成，在发送和接收主机之间将按顺序发送和确认段。关闭连接之前，`TCP`使用类似的握手过程验证两个主机是否都完成发送和接收全部数据。

### UDP

`UDP`全称是`User Datagram Protocol`，中文名为用户数据报协议。

`UDP` 提供无连接的网络服务，该服务对消息中传输的数据提供不可靠的、最大努力传送。这意味着它不保证数据报的到达，也不保证所传送数据包的顺序是否正确。

特点

- 性能消耗低
- 速度快

在项目实际使用上，`Tcp`相对简单，`UDP`相对复杂。如果项目对于游戏的延迟非常敏感推荐使用`UDP`。但是要实现数据的可靠的控制。

> `Github` 上面有几个不错的基于UDP的库 [LiteNetLib](https://github.com/RevenantX/LiteNetLib)

### 业务通信协议

业务的协议跟我们这边提的概念上有点不同，是我们数据的承载体。不管是`Tcp` 还是`UDP` 从服务端到客户端之间的传输数据都是二进制数据的。要转换成我们实际用的数据。我列举了几种常用的。

#### JSON

就是上面提过的JSON文件格式，很多对网络传输延迟不高的游戏。会比较常用`Json`文件格式，灵活。文件格式不需要代码配合解析，可以在业务层直接解析。配合上一些脚本语言`Lua`之类的写业务非常方便。缺点就是解析需要耗时，而且传输的数据量很大。

#### 自定义流

自定义流数据格式，通过自己写的结构数据序列化反序列化工具读取。传输量小，解析速度最快。

#### Protocol buffer

> [Protocol buffers](https://developers.google.com/protocol-buffers/)

Google 推出的一套跨平台传输协议，非常推荐使用。支持常用的语言`C++ C# DART GO JAVA Python`，基于pro描述文件生成对应语言的序列化和反序列代码，传输量非常小(内部有压缩),解析速度较快，基本影响不大。

### 总结

对于网络的部分的选择，现在游戏中会根据不同的项目要求使用不同的策略。一搬种类游戏，对于实时性要求不高。可以选择`TCP`。对于延迟要求较高的游戏类型如`FPS` `MOBA` `格斗` 游戏可以推荐使用`UDP`。如果游戏类型是弱联网的`TCP`也是个不错的选择。

在业务层面的选择，如果没有配合脚本做热更的需求，传输协议比较稳定。推荐使用`Google Protocol Buffer`,如果有热更新需求，也可以用`Protocol Buffer`。用`Json`会更简单点，对于`Lua`或者`Js`这样的语言，没有特定的数值类型,`Json`可以非常方便的访问，没有繁琐的解析步骤。

## Asset Bundle

## 需求分析



## 功能开发



## 数据驱动的逻辑