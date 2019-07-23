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
<?xml version="1.0" encoding="UTF-8" ?>
<config>
	<iggid>123456</iggid>
	<language>CN</language>
  <serverList>
    <Server name = "服务器1" ip ="10.15.1.5:5088"/>
    <Server name = "服务器2" ip ="10.15.1.6:5088"/>
  </serverList>
</config>

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
    "iggid": 123456,
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
- 可读性对比XML较差

| 空         | JsonUtility | Newtonsoft.json |
| ---------- | ----------- | --------------- |
| array      | 支持        | 支持            |
| list       | 支持        | 支持            |
| dictionary | 不支持      | 支持            |
| hashtable  | 不支持      | 支持            |

1. 一定要把需要序列化的数据结构 [Serializable], 因为 如果不加这个, `int, string`等单一的类型可以解析,  但是`array, list, dictionary ,hashtable`等复杂的数据结构不能被序列化

1. 一定要记住: `json`文件里的字段名字,一定要和自己定义的需要序列化的数据结构里的名字"一模一样", 否则不会解析成功.
   

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

在业务层面的选择，如果没有配合脚本做热更的需求，传输协议比较稳定。推荐使用`Google Protocol Buffer`。如果有热更新需求，用`Json`会更简单点，也可以用`Protocol Buffer`。对于`Lua`或者`Js`这样的语言，没有特定的数值类型,`Json`可以非常方便的访问，没有繁琐的解析步骤。

## Asset Bundle

### 概述

`AssetBundles`系统提供了一种Unity可以索引的打包格式。这是为了提供了一个可以和Unity序列化系统兼容的数据传递（交付）方法。`AssetBundles`是Unity用来在安装后交付和更新非代码内容的主要工具。它使得开发者可以减少安装包中的资源大小、减少运行时的内存压力，还可以为终端设备做优化：**选择性的加载内容。**

了解`AssetBundles`的工作方式是构建成功Unity项目**（移动设备上）**的必要因素。

### Asset Bundle中有什么

一个`AssetBundle`中包含两个部分：**文件头**和**数据段。**
文件头是在构建`AssetBundle`时由Unity生成的。它包含了`AssetBundle`的信息，比如：`AssetBundle`的标识，`AssetBundle`是否被压缩，还有一个清单。这个清单是一个*查询表*，以**对象**的名字为键。每一项提供一个字节的索引来标识在`AssetBundle`的数据段中给定对象的位置。在大多数平台，这个查询表使用`STL`的`std::multimap`实现的。虽然具体算法依赖不同平台`STL`实现方式，但大多数都是平衡树的一种。Windows和OSX衍生系统（包括IOS）使用的是**红黑树**。所以，构建清单的时间和`AssetBundle`s中资源数量，***不是线性关系那么乐观*。**

**数据段**包含本`AssetBundle`中序列化的`Assets`原始数据。如果选择数据段被压缩，其中**LZMA**算法将会被应用到所有序列化的字节，也就是先序列化所有的`assets`，然后对整个字节数组进行压缩。

Unity5.3之前，`AssetBundle`中的`Objects`不会被单独压缩。因此，5.3之前的Unity若是想从压缩的`AssetBundle`中读取一个或多个Objects就需要将整个`AssetBundle`解压缩。一般来说，为了提高后续在同一个`AssetBundle`加载的性能，Unity会缓存一个`AssetBundle`解压后的副本。

Unity5.3添加了**LZ4**压缩选项。由**LZ4**压缩选项构建的`AssetBundles`将会**单独**压缩其中的`Objects`，来让Unity在硬盘上存储压缩的`AssetBundles`。这也允许Unity单独解压Objects，**而无须解压整个`AssetBundle`。**

### 加载Asset Bundles

在Unity5中，`AssetBundles`可以通过四个不同的API加载。这四个API根据下面两个条件情况将有不同的行为：

1. `AssetBundle`是否被压缩，是使用LZMA压缩还是LZ4压缩

2. `AssetBundle`在什么平台上加载

这四个API是：

- [AssetBundle.LoadFromMemoryAsync](http://docs.unity3d.com/ScriptReference/AssetBundle.LoadFromMemoryAsync.html?_ga=1.126585038.236528352.1476878759)

- [AssetBundle.LoadFromFile](http://docs.unity3d.com/ScriptReference/AssetBundle.LoadFromFile.html?_ga=1.126585038.236528352.1476878759)

- [WWW.LoadFromCacheOrDownload](http://docs.unity3d.com/ScriptReference/WWW.LoadFromCacheOrDownload.html?_ga=1.126575566.236528352.1476878759)

- [UnityWebRequest](http://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html?_ga=1.126575566.236528352.1476878759)的[DownloadHandlerAssetBundle](http://docs.unity3d.com/ScriptReference/Networking.DownloadHandlerAssetBundle.html?_ga=1.131703884.236528352.1476878759)(至少是Unity5.3)

####  `AssetBundle.LoadFromMemory(Async)`

​	**Unity建议不要使用这个函数**

[AssetBundle.LoadFromMemoryAsync](http://docs.unity3d.com/ScriptReference/AssetBundle.LoadFromMemoryAsync.html?_ga=1.159539326.236528352.1476878759)从托管代码的字节数组中加载`AssetBundle(C#中的byte[])`.它总是将源数据从托管代码的字节数组中拷贝到一个新分配连续的本地内存块中.如果`AssetBundle`是`LZMA`压缩格式，它将在拷贝时将`AssetBundle`解压。未压缩和`LZ4`压缩的`AssetBundles`将逐字拷贝。

这个API所消耗的内存峰值至少是`AssetBundle`**大小的两倍**：一份是API所创建的本地内存中的副本，一份是传递给API的托管字节数组。通过这个API从`AssetBundle`中加载的`Assets`，因此会在内存中出现*3次*：一次是从托管代码的字节数组，一次是`AssetBundle`的本地内存副本，还有一次是GPU或系统内存中。

```c#
IEnumerator LoadFromMemoryAsync(string path)
{
    AssetBundleCreateRequest createRequest = 	AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));

    yield return createRequest;

    AssetBundle bundle = createRequest.assetBundle;

    var prefab = bundle.LoadAsset.<GameObject>("MyObject");
    Instantiate(prefab);
}
```

####  `AssetBundle.LoadFromFile(Async)`

[ssetBundle.LoadFromFile](http://docs.unity3d.com/ScriptReference/AssetBundle.LoadFromFile.html?_ga=1.203148498.236528352.1476878759)是一个用来从本地（一块硬盘或一个SD卡）加载未压缩`AssetBundle`的高效API。如果`AssetBundles`是未压缩的或LZ4压缩，这个API的行为如下：

*移动设备：*API只会加载`AssetBundle`的文件头，不会从硬盘中加载剩余的数据。`AssetBundle`的`Objects`只有被加载方法调用(比如`AssetBundle.Load`)，或实例ID被间接引用时，才会被加载。在这个情境中没有额外的内存被消耗。

*Unity编辑器：*这个API将会把整个`AssetBundle`加载到内存中，就如`AssetBundle.LoadFromMemoryAsync`被调用一样，将所有字节从硬盘中读出。如果项目在Unity编辑器中进行评估，这个API在`AssetBundle`加载时，将会引起内存峰值。不过这不会影响实机上的表现，在实机上需要重新测试下，确定会遇到峰值问题再进行补救行为。

>  *备注：*对**LZMA**压缩的`AssetBundles`调用`AssetBundle.LoadFromFile`将会失败。
>
> ```c#
> public class LoadFromFileExample extends MonoBehaviour 
> {
>     function Start() {
>         var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myassetBundle"));
>         if (myLoadedAssetBundle == null) {
>             Debug.Log("Failed to load AssetBundle!");
>             return;
>         }
>         var prefab = myLoadedAssetBundle.LoadAsset.<GameObject>("MyObject");
>         Instantiate(prefab);
>     }
> }
> ```

#### `WWW.LoadFromCacheOrDownload`

[WWW.LoadFromCacheOrDownload](http://docs.unity3d.com/ScriptReference/WWW.LoadFromCacheOrDownload.html?_ga=1.224028504.236528352.1476878759)是一个从远端服务器或本地存储加载Objects的API。本地文件可以通过添加`file://` URL进行加载。如果`AssetBundle`已经在Unity缓存中了，这个API的处理方式和`AssetBundle.LoadFromFile`一模一样。

如果`AssetBundle`还未被缓存，`WWW.LoadFromCacheOrDownload`将会从他的源地址读取`AssetBundle`。如果`AssetBundle`被压缩了，将会在工作线程中解压并写入缓存。否则，他将通过工作线程被直接写入缓存。

一旦`AssetBundl`e被缓存，`WWW.LoadFromCacheOrDownload`将从缓存中加载头信息，解压`AssetBundle`。接着API的表现与通过`AssetBundle.LoadFromFile`加载`AssetBundle`一致。

>   备注：当数据被解压并通过一个固定大小的缓冲区写入缓存，WWW对象将在本地内存中保持一份完整的`AssetBundle`字节副本。保留这个额外的`AssetBundle`副本是为了支持[WWW.bytes](http://docs.unity3d.com/ScriptReference/WWW-bytes.html?_ga=1.122801224.236528352.1476878759)这个属性。

**由于在WWW对象中缓存一份`AssetBundle`内容字节的内存开销**，建议所有使用`WWW.LoadFromCacheOrDownload`的开发者确保他们的`AssetBundles`小一些：最多几兆。接下来还有个建议，对那些对内存有限制的平台（比如移动设备）的开发者：为了防止内存峰值，确保同一时间代码中只下载一个`AssetBundle`。

> 备注：每调用一次这个API将会创建一个新的工作线程。当多次调用这个API时，注意此时创建了过多的线程。如果超过5-10个`AssetBundles`需要下载，建议你的代码写成可以确保只有少量的`AssetBundle`在同时下载。



```c#
using UnityEngine;
using System.Collections;

public class LoadFromCacheOrDownloadExample : MonoBehaviour
{
    IEnumerator Start ()
    {
            while (!Caching.ready)
                    yield return null;

        var www = WWW.LoadFromCacheOrDownload("http://myserver.com/myassetBundle", 5);
        yield return www;
        if(!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield return;
        }
        var myLoadedAssetBundle = www.assetBundle;

        var asset = myLoadedAssetBundle.mainAsset;
    }
}
```



#### `AssetBundleDownloadHandler`

[UnityWebRequest](http://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html?_ga=1.236504258.236528352.1476878759)在Unity5.3中引入到移动平台上，相比于[WWW](http://docs.unity3d.com/ScriptReference/WWW.html?_ga=1.190498024.236528352.1476878759) 它提供了另一种更灵活的选择。`UnityWebRequest`允许开发者指定如何处理下载的数据，并且让开发者避免不必要的内存使用。通过`UnityWebRequest`下载一个`AssetBundle`最简单的方法就是调用[UnityWebRequest.GetAssetBundle](http://docs.unity3d.com/ScriptReference/Networking.DownloadHandlerAssetBundle.html?_ga=1.234988226.236528352.1476878759)。

根据这篇文章的主题，我们最关心的类就是[DownloadHandlerAssetBundle](http://docs.unity3d.com/ScriptReference/Networking.DownloadHandlerAssetBundle.html?_ga=1.226543582.236528352.1476878759)。使用时，这个`DownloadHandler`表现的和`WWW.LoadFromCacheOrDownload`一样。使用一个工作线程，他将下载的数据放入一个固定大小的缓冲，然后根据`DownloadHandler`的配置，将缓冲数据移至一个临时存储或`AssetBundle`缓存。**LZMA**压缩的`AssetBundles`在下载时将会被解压，然后缓存起来。

所有的这些操作都发生在本地代码，避免了扩大托管堆的风险。而且，这个`DownloadHandler`*没有*在本地代码中保存下载字节的副本，这样减少了下载`AssetBundl`e带来的内存占用。

当下载结束了，可以通过`DownloadHandler`的[assetBundle](http://docs.unity3d.com/ScriptReference/Networking.DownloadHandlerAssetBundle-assetBundle.html?_ga=1.204652755.236528352.1476878759)属性，访问下载好的`AssetBundle`，就像对下载好的`AssetBundle`调用`AssetBundle.LoadFromFile`一样。

`UnityWebRequest`也支持像`WWW.LoadFromCacheOrDownload`一样的缓存方式。如果提供给`UnityWebRequest`对象的缓存信息中，正在请求的`AssetBundle`已经在Unity缓存中了，那么`AssetBundle`将会立即可用，并且这个API将会进行和`AssetBundle.LoadFromFile`一样的操作。

>  备注：Unity的`AssetBundle`缓存在`WWW.LoadFromCacheOrDownload`和`UnityWebRequest`之间是共享的。在一个API中下载的任何`AssetBundle`在另一个API中也是有效的。

> *备注：*不像`WWW`，`UnityWebRequest`系统它内部维护一个工作线程池，还有一个内部任务系统来确保开发者不会同时启动过量的下载。线程池的大小目前无法配置。

```C#
IEnumerator InstantiateObject()
{
    string uri = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;        
UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);
    yield return request.Send();
    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
    GameObject cube = bundle.LoadAsset<GameObject>("Cube");
    GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
    Instantiate(cube);
    Instantiate(sprite);
}
```

#### 建议

一般来说，应该尽可能的采用[AssetBundle.LoadFromFile](https://unity3d.com/cn/learn/tutorials/topics/best-practices/assetbundle-fundamentals#AssetBundle.LoadFromFile)。这个API在速度，硬盘和运行内存的使用上是效率最高的。

对于那些必须下载和对`AssetBundles`进行打补丁的项目来说，强烈建议：在Unity5.3以及更新的版本上使用[UnityWebRequest](https://unity3d.com/cn/learn/tutorials/topics/best-practices/assetbundle-fundamentals#AssetBundleDownloadHandler)，而那些使用Unity5.2或更老的项目则使用[WWW.LoadFromCacheOrDownload](https://unity3d.com/cn/learn/tutorials/topics/best-practices/assetbundle-fundamentals#WWW.LoadFromCacheOrDownload)。在下一章的[发布](https://link.jianshu.com?t=https://unity3d.com/learn/tutorials/topics/best-practices/asset-bundle-usage-patterns#Distribution)这节中我们将详细描述，如何将`AssetBundle`打包在在项目的安装包中。

当使用`WWW.LoadFromCacheOrDownload`时，强烈建议你确保项目的`AssetBundles`小于项目最大内存预算的**2-3%**，来防止应用由于内存使用峰值被强行终止。对于大部分项目来说，`AssetBundles`的文件大小应该不超过5MB，并且同时下载的`AssetBundles`不超过1-2个。

不论是用`WWW.LoadFromCacheOrDownload`还是`UnityWebRequest`，确保下载部分的代码在加载完`AssetBundle`后适时调用***Dispose***。还有一种选择：C#的[using](https://link.jianshu.com?t=https://msdn.microsoft.com/en-us//library/yh598w02.aspx)声明是最方便的方法来确保一个`www`或`UnityWebRequest`*被安全Dispose掉。

对于那种大型团队和特殊缓存及下载需求的项目来说，定制一个Downloader是有必要的。自己写个`Downloader`是一项大工程，并且自己写的这些`Downloader`应该和`AssetBundle.LoadFromFile`兼容。详情请见下篇的[发布](https://link.jianshu.com?t=https://unity3d.com/learn/tutorials/topics/best-practices/asset-bundle-usage-patterns#Distribution)部分。

### 从Asset Bundle 加载 Asset

`UnityEngine.Objects`可以通过三个不同的API从`AssetBundles`中加载。

- [LoadAsset](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAsset.html) ([LoadAssetAsync](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAssetAsync.html))
- [LoadAllAssets](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAllAssets.html) ([LoadAllAssetsAsync](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAllAssetsAsync.html))
- [LoadAssetWithSubAssets](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAssetWithSubAssets.html) ([LoadAssetWithSubAssetsAsync](https://docs.unity3d.com/ScriptReference/AssetBundle.LoadAssetWithSubAssetsAsync.html))

这些API的同步版本将始终比其异步版本快至少一帧。

当加载多个独立的`UnityEngine.Object`时，可以使用`LoadAllAssets`。这个接口应该只在同一个`AssetBundle`中多数或全部`Objects`需要被加载时再去调用。相比于其他两个接口，**`LoadAllAssets`**比多次单独调用**`LoadAssets`**要快一点点。因此，如果需要同时加载大量`assets`，但是这些assets又不超过`AssetBundle`内容的三分之二，可以考虑将`AssetBundle`细分为多个小点的包，然后使用`LoadAllAssets`。

当去加载一个**复杂**的`Asset`（包含许多内嵌的`Object`，比如一个`FBX`模型内嵌动画，或一个图集，内嵌多个精灵）时，可以使用`LoadAssetWithSubAssets`。如果这些需要加载的`Objects`都来自同一个`Asset`，同时这个`AssetBundle`中还有很多其他不相关的Objects，那就用这个接口。

对于其他情况，就用`LoadAsset`或`LoadAssetAsync`吧。

###  `AssetBundle`依赖

在Unity5的`AssetBundle`系统中，`AssetBundles`间的依赖，根据不同的运行环境，可以通过两个不同的API来跟踪。在Unity编辑器中，`AssetBundle`依赖关系可以通过[AssetDatabase](http://docs.unity3d.com/ScriptReference/AssetDatabase.html?_ga=1.159540478.236528352.1476878759)API去查询。AssetBundle的分配和依赖可以通过[AssetImporter](http://docs.unity3d.com/ScriptReference/AssetImporter.html?_ga=1.223971544.236528352.1476878759)API访问和改变。运行时，Unity提供一个` ScriptableObject-based` [AssetBundleManifest](https://link.jianshu.com?t=http://docs.unity3d.com/ScriptReference/AssetBundleManifest.html) 去加载构建`AssetBundle`时生成的依赖信息。

当`AssetBundle`的父`AssetBundle`（直接引用）中Object有引用其他的`AssetBundle`中的Object，那么就存在了间接引用关系（这个关系并没有记录在清单中）。关于对象间引用的更多信息，请查看[Assets,Objects and Serialization](https://unity3d.com/assets-objects-and-serialization)一文中的[对象间的引用](https://unity3d.com/assets-objects-and-serialization#InterObject_References)章节。

如[Assets, Objects and Serialization](https://unity3d.com/assets-objects-and-serialization)一文里[序列化和实例](https://unity3d.com/assets-objects-and-serialization#Serialization_and_Instances)段落中描述的一样，Object数据被糅合在`AssetBundle`中，以`FileGUID`和`LocalID`作为唯一标识。

Object在他的实例ID被第一次引用时被加载；Object在其`AssetBundle`被加载时，被赋予一个有效的实例ID。由此看来，加载一个Object之前应该先把其依赖项所在的`AssetBundles`全都加载了。而并不是说拥有依赖关系的`AssetBundle`之间必须按顺序加载。Unity不会在一个父`AssetBundle`加载时，帮你把其子`AssetBundles`都自动加载的。

**示例：**
假设*材质A*引用*纹理B*。材质A被打包至AssetBundle1，而纹理B被打包到AssetBundle2。![img](https://unity3d.com/sites/default/files/learn/ab1.jpg)

在此用例中，从AssetBundle1中加载材质A之前，必须要先加载AssetBundle2。



这并不意味着AssetBundle2必须要先于AssetBundle1加载，或者纹理B必须显式的从AssetBundle2中加载。只有从AssetBundle1中加载材质A时，AssetBundle2才需要先加载。

在AssetBundle1加载时，Unity*不会*自动加载AssetBundle2。这必须通过脚本手动完成。上面提到的`AssetBundle`的API都可以任意用于加载AssetBundles1和AssetBundles2上，随意组合。通过`WWW.LoadFromCacheOrDownload`加载`AssetBundles`，可以任意和通过`AssetBundle.LoadFromFile`或`AssetBundle.LoadFromMemoryAsync`加载`AssetBundles`组合在一起。

#### `AssetBundle`清单(manifests)

当通过接口`BuildPipeline.BuildAssetBundles`执行`AssetBundle`的构建流程时，Unity会序列化一个对象，它包含每个`AssetBundle`的依赖信息。这个数据存储在一个单独的`AssetBundle`中，而这个`AssetBundle`只包含一个[AssetBundleManifest](http://docs.unity3d.com/ScriptReference/AssetBundleManifest.html?_ga=1.167928674.236528352.1476878759)类型的对象。

这个Asset所在的`AssetBundle`和构建的`AssetBundles`在同一个目录中，其（这个Asset所在的`AssetBundle`）名称就是这个目录名。如果项目将`AssetBundles`构建至*(工程目录)/build/Client*，那么包含清单的`AssetBundle`将会保存为`(工程目录)/build/Client/Client.manifest`。

包含清单的这个`AssetBundle`可以被加载，缓存，卸载。就像其他的`AssetBundle`一样。

`AssetBundleManifes`t对象本身提供[GetAllAssetBundles](http://docs.unity3d.com/ScriptReference/AssetBundleManifest.GetAllAssetBundles.html?_ga=1.197773292.236528352.1476878759)接口，用来罗列所有和清单一起构建的`AssetBundle`。还有两个方法，用来查询指定`AssetBundle`的依赖：

[AssetBundleManifest.GetAllDependencies](http://docs.unity3d.com/ScriptReference/AssetBundleManifest.GetAllDependencies.html?_ga=1.30984444.236528352.1476878759)返回`AssetBundle`所有的依赖，包括迭代依赖。

```C#
AssetBundle assetBundle = AssetBundle.LoadFromFile(manifestFilePath);
AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
```

### 管理已经加载Asset Bundle



在内存紧张的环境中，小心控制加载Objects的大小和数量尤为重要。Objects被移出激活的场景时，Unity不会自动卸载他们。Asset的清理会在特定的时间触发，当然也可以手动触发

> [Resources.UnloadUnusedAssets](http://docs.unity3d.com/ScriptReference/Resources.UnloadUnusedAssets.html?_ga=1.234006365.236528352.1476878759) 

必须小心的管理`AssetBundles`自身文件。一个`AssetBundle`在本地存储（不论是在`UnityCache`中，还是通过[AssetBundle.LoadFromFile](http://docs.unity3d.com/ScriptReference/AssetBundle.LoadFromFile.html?_ga=1.25769917.1251464125.1470796528)加载的文件）中以一个文件的形式存在时，其占用的内存开销很小，几乎不会超过10-40kb。但如果有大量的`AssetBundles`存在，这开销依旧不容忽视。

因为多数工程允许用户重复体验某内容（比如重玩一个关卡），所以知道什么时候去加载或卸载一个`AssetBundle`就尤为重要了。**如果一个`AssetBundle`被不恰当的卸载了，这可能会引起Object在内存中存重复存在。**不恰当的卸载`AssetBundle`在某些情况下也会导致与期望不符的表现，比如：引起纹理的缺失。想要知道为什么会发生这些，请参阅[Assets,Objects和序列化](https://unity3d.com/learn/tutorials/topics/best-practices/assets-objects-and-serialization)文章中的段落[Object之间的引用](https://unity3d.com/learn/tutorials/topics/best-practices/assets-objects-and-serialization#InterObject_References)。

管理`Assets`和`AssetBundles`时，最重要的事情莫过于清楚，调用`AssetBundle.Unload`时传入参数*true*或*false*，分别会发生什么情况，有何不同。

这个API在调用时会将对应`AssetBundle`的头信息卸载掉。其参数标记是否也去卸载掉那些从该`AssetBundle`实例化的Objects。如果参数是*true*，那么所有从这个`AssetBundle`创建的Objects，即使正在激活的场景中使用，也会被立即卸载掉。

举例来说，假设材质*M*从`AssetBundle AB`*中加载，并且假设*M当前正在激活的场景中。

![](https://unity3d.com/sites/default/files/ab2a.jpg)

如果`AB.Unload(true)`被调用，那么*M*将会从场景中被移除，销毁和卸载。如果`AB.Unload(false)`*被调用，那么*AB*的头信息会被卸载，但是*M*依然留在场景中，并且将会一直有效。调用*`AssetBundle.Unload(false)`*会打断*M*和*AB*之间的连接。如果*`AssetBundle`稍后再次加载，那`AssetBundle`中的Objects会以新的身份被重新加载进内存。
![img](https://unity3d.com/sites/default/files/ab2b.jpg)

如果`AssetBundle`稍后被再次加载，那么重新加载的是`AssetBundle`头信息的新副本。`M`并不是从`AssetBundle`新副本中加载的。Unity不会去在`AssetBundle`新副本和`M`之间建立任何连接。

![](https://unity3d.com/sites/default/files/ab2c.jpg)

如果调用`AssetBundle.LoadAsset()`去重新加载*M*，Unity不会将旧的*M*副本解释为`AssetBundle`中的实例数据。所以Unity会去加载一个新的*M*副本，因此这里会有两个完全一样的M副本存在在场景中。

![](https://unity3d.com/sites/default/files/ab2d.jpg)

对于大多数项目来说，这不是想要的行为。大多数项目应该使用`AssetBundle.Unload(true)`，并且用一些方法确保这些Objects不会重复出现。常见的两种方法：

1. 在应用生命周期中，一些明显的界限点（不同场景之间，或加载界面中）上，将那些短暂的（不是全局存在的基础包）`AssetBundles`卸载掉的。这是最简单和常见的选项。
2. 单独地为每个Objects维护引用计数，只有当`AssetBundle`中所有Objects都未被使用时，才去卸载掉它`AssetBundle`。这就允许应用去卸载和重载单独的Objects，而不会出现重复的内存。

如果一个应用必须使用`AssetBundle.Unload(false)`，那么只能通过下面两种方法将单独的Objects卸载：

1. 消除这个不想要的Object的所有引用，场景中和代码中的都要清除掉。都做好了，就可以调用[Resources.UnloadUnusedAssets](http://docs.unity3d.com/ScriptReference/Resources.UnloadUnusedAssets.html?_ga=1.234006365.236528352.1476878759)了。
2. 以非叠加的方式加载一个场景。这样会销毁当前场景中的所有Objects，然后自动调用[Resources.UnloadUnusedAssets](http://docs.unity3d.com/ScriptReference/Resources.UnloadUnusedAssets.html?_ga=1.223912536.236528352.1476878759)。

> [Asset Bundle 官方文档](https://unity3d.com/cn/learn/tutorials/topics/best-practices/assetbundle-usage-patterns)

## 需求分析

### 资源整体大小的评估

#### 贴图

#### 模型

#### 动画

### 制作流程设计

#### 工具链

#### 检查工具

### 性能

#### 



## 功能开发

Unity开发的中有些坑的点

在Unity中，文件的唯一标识符不是通过文件名。而是通过meta文件描述



## 数据驱动的逻辑