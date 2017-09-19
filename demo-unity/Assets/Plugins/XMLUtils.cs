using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using RapidXml;
using System.Reflection;

public class XMLUtils
{
    #region rapidXMl 操作相关接口
    /// <summary>
    /// 解析xml配置并放入list中
    /// </summary>
    /// <param name="xmlContent">xml 文本内容</param>
    /// <param name="rootNodeName">根节点名字</param>
    /// <returns></returns>
    public static List<T> RapidParse<T>(string xmlContent, String rootNodeName)
    {
        //Profiler.BeginSample("RapidParse List<T>");
        List<String> attributesNameList = new List<String>();
        List<T> list = new List<T>();
        RapidXmlParser xmlRapid = new RapidXmlParser();
        xmlRapid.Load(xmlContent);
        NodeElement RootNode = xmlRapid.FirstNode(rootNodeName);
        NodeElement firstNode = RootNode.FirstNode();
        int attribCount = firstNode.GetAttributeCount();
        int nodeCount = RootNode.GetChildNodeCount();
        var attribFormat = firstNode.FirstAttribute();
        string str = attribFormat.GetName();
        int num = 0;
        string name;
        while (num < attribCount)
        {
            name = attribFormat.GetName();
            attributesNameList.Add(name);
            attribFormat = attribFormat.NextAttribute();
            num++;
        }
#if UNITY_EDITOR
        try
        {
#endif
        num = 0;
        while (num < nodeCount)
        {
            T s = System.Activator.CreateInstance<T>();

            var attribute = firstNode.FirstAttribute();
            for (int i = 0; i < attribCount; i++)
            {
                SetObjectValue<T>(s, attributesNameList[i], attribute.GetValue());
                attribute = attribute.NextAttribute();
            }
            list.Add(s);
            firstNode = firstNode.NextSibling();
            num++;
        }
#if UNITY_EDITOR
        }
        catch (Exception e)
        {
            Debug.LogError(typeof(T).Name.ToString().Replace("Po", "") + ".xml表第" + (num + 3) + "行配的有问题，这个错误很严重一定要改，不改游戏会崩溃" + @"
啦啦啦德玛西亚，啦啦啦德玛西亚
配置表又配错啦，配置表又配错啦");
        }
#endif
        //Profiler.EndSample();
        return list;
    }

    /// <summary>
    /// 解析xml配置并放入Dictionary中
    /// </summary>
    /// <param name="xmlContent">xml文本内容</param>
    /// <param name="rootNodeName">根节点名字</param>
    /// <param name="fieldname">做完key保存使用的字段名</param>
    /// <returns></returns>
    public static Dictionary<int, T> RapidParse<T>(String xmlContent, String rootNodeName, String fieldname)
    {
        Dictionary<int, T> dict = new Dictionary<int, T>();
        List<String> attributesNameList = new List<String>();
        RapidXmlParser xmlRapid = new RapidXmlParser();
        xmlRapid.Load(xmlContent);
        NodeElement RootNode = xmlRapid.FirstNode(rootNodeName);
        NodeElement firstNode = RootNode.FirstNode();
        int attribCount = firstNode.GetAttributeCount();
        int nodeCount = RootNode.GetChildNodeCount();
        var attribFormat = firstNode.FirstAttribute();
        string str = attribFormat.GetName();
        int num = 0;
        int keyIndex = 0;
        string name;
        while (num < attribCount)
        {
            name = attribFormat.GetName();
            if (name == fieldname)
                keyIndex = num;
            attributesNameList.Add(name);
            attribFormat = attribFormat.NextAttribute();
            num++;
        }
        while (firstNode.IsValid())
        {
            T s = System.Activator.CreateInstance<T>();
            string nodename = firstNode.GetName();
            var attribute = firstNode.FirstAttribute();

            int key = 0;
            for (int i = 0; i < attribCount; i++)
            {
                if (i == keyIndex)
                {
                    key = attribute.GetInt();
                }
                SetObjectValue<T>(s, attributesNameList[i], attribute.GetValue());

                attribute = attribute.NextAttribute();
            }
            try
            {
                dict.Add(key, s);
            }
            catch
            {
                throw new ArgumentException("找不到key");
            }
            firstNode = firstNode.NextSibling();
        }
        //Profiler.EndSample();
        return dict;
    }

    /// <summary>
    /// 根据字段名称设置某个对象内部的值
    /// </summary>
    /// <param name="obj">需要赋值的对象</param>
    /// <param name="fieldname">字段名称</param>
    /// <param name="value"></param>
    /// <returns></returns>
    static void SetObjectValue<T>(T obj, String fieldname, object value)
    {
        Type type = obj.GetType();
        FieldInfo info = type.GetField(fieldname);
        if (info != null)
        {
            if (info.FieldType.Equals(typeof(Int32)) &&
                value.GetType().Equals(typeof(String)))
            {
                //如果字段的类型是int，而值的类型是string 则强转
                try
                {
                    info.SetValue(obj, int.Parse((String)value));
                }
                catch (Exception)
                {
                    Debug.LogError("配置字段类型有错误 " + type.ToString() + " " + fieldname);
                    throw new Exception("配置字段类型有错误 "+ type.ToString() + " " +fieldname);
                }
            }
            else if (info.FieldType.Equals(typeof(Int64)) &&
                            value.GetType().Equals(typeof(String)))
            {
                //如果字段的类型是int64，而值的类型是string 则强转
                info.SetValue(obj, Convert.ToInt64((String)value));
            }
            else if (info.FieldType.Equals(typeof(Single)) &&
                     value.GetType().Equals(typeof(String)))
            {
                //如果字段的类型是float，而值的类型是string 则强转
                info.SetValue(obj, float.Parse((String)value));
            }
            else if (info.FieldType.Equals(typeof(String)) &&
                     value.GetType().Equals(typeof(String)))
            {
                if (value.ToString() == "")
                {
                    info.SetValue(obj, null);
                }
                else
                {
                    info.SetValue(obj, value);
                }
            }
            else if (info.FieldType.Equals(typeof(float)) &&
                    value.GetType().Equals(typeof(String)))
            {
                //如果字段的类型是int，而值的类型是string 则强转
                info.SetValue(obj, float.Parse((String)value));
            }
            else
            {
                //如果字段的类型是int，而值的类型是string 则强转
                info.SetValue(obj, int.Parse((String)value));
            }
        }
    }
    #endregion

    #region XElement操作接口相关

    /// <summary>
    /// 保存XML文件到本地
    /// </summary>
    /// <param name="path"></param>
    /// <param name="root"></param>
    public static void SaveXElementToFile(string path, XElement root)
    {
        using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
        {
            sw.Write(root.ToString());
        }
    }
    /// <summary>
    /// 获取指定的xml节点的节点路径
    /// </summary>
    /// <param name="element"></param>
    public static string GetXElementNodePath(XElement element)
    {
        if (element == null)
            return null;
        try
        {
            string path = element.Name.ToString();
            element = element.Parent;
            while (null != element)
            {
                path = element.Name.ToString() + "/" + path;
                element = element.Parent;
            }

            return path;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return null;
    }

    /// <summary>
    /// 获取XElement文件树节点段XElement
    /// </summary>
    /// <param name="XElement">XElement文件载体</param>
    /// <param name="newroot">要查找的独立节点</param>
    /// <returns>独立节点XElement</returns>
    public static XElement GetXElement(XElement XElement, string newroot)
    {
        if (XElement != null)
        {
            IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
            if (xmlItems == null)
                return null;
            foreach (var xmlItem in xmlItems)
            {
                return xmlItem;
            }

            return null;
        }

        return null;
    }

    public static List<int> GetXElementAttributeIntList(XElement xElement, string attributeName, string newroot = "*")
    {
        List<int> result = new List<int>();

        if (xElement != null)
        {
            IEnumerable<XElement> xmlItems = null;
            if ("*" == newroot)
            {
                xmlItems = xElement.Elements();
            }
            else
            {
                xmlItems = xElement.DescendantsAndSelf(newroot);
            }
            if (xmlItems != null)
            {
                foreach (var xmlItem in xmlItems)
                {
                    result.Add(GetXElementAttributeInt(xmlItem, attributeName));
                }
            }
        }
        else
        {
            //throw new Exception(string.Format("GetXElement异常, 读取: {0} 失败, xml节点名: {1}", newroot, GetXElementNodePath(xElement))));
            //Debug.LogException(e);
        }

        return result;
    }
    public static int[] GetXElementAttributeIntArray(XElement xElement, string attributeName, string newroot = "*")
    {
        return GetXElementAttributeIntList(xElement, attributeName, newroot).ToArray();
    }
    /// <summary>
    /// 获取XElement文件树节点段XElement的列表
    /// </summary>
    /// <param name="XElement">XElement文件载体</param>
    /// <param name="newroot">要查找的独立节点</param>
    /// <returns>XElement列表</returns>
    public static List<XElement> GetXElementList(XElement xElement, string newroot)
    {
        List<XElement> xmlItemList = new List<XElement>();

        if (xElement != null)
        {
            IEnumerable<XElement> xmlItems = null;
            if ("*" == newroot)
            {
                xmlItems = xElement.Elements();
            }
            else
            {
                xmlItems = xElement.DescendantsAndSelf(newroot);
            }
            if (xmlItems != null)
            {
                foreach (var xmlItem in xmlItems)
                {
                    xmlItemList.Insert(xmlItemList.Count, xmlItem);
                }
            }
        }
        else
        {
            //throw new Exception(string.Format("GetXElement异常, 读取: {0} 失败, xml节点名: {1}", newroot, GetXElementNodePath(xElement))));
            //Debug.LogException(e);
        }

        return xmlItemList;
    }

    /// <summary>
    /// 获取XElement文件树节点段XElement
    /// </summary>
    /// <param name="xml">XElement文件载体</param>
    /// <param name="mainnode">要查找的主节点</param>
    /// <param name="attribute">主节点条件属性名</param>
    /// <param name="value">主节点条件属性值</param>
    /// <returns>以该主节点为根的XElement</returns>
    public static XElement GetXElement(XElement XElement, string newroot, string attribute, string value)
    {
        //FuncStat funcStat = new FuncStat(new System.Diagnostics.StackTrace(), string.Format("{0}/{1}=>{2}", newroot, attribute, value));


        if (XElement != null)
        {
            IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
            if (xmlItems != null)
            {
                foreach (var xmlItem in xmlItems)
                {
                    XAttribute attrib = null;
                    if (xmlItem != null)
                        attrib = xmlItem.Attribute(attribute);
                    if (null != attrib && attrib.Value == value)
                    {
                        return xmlItem;
                    }
                }
            }
            return null;
        }
        else
        {
            //throw new Exception(string.Format("GetXElement异常, 读取: {0}/{1}={2} 失败, xml节点名: {3}", newroot, attribute, value, GetXElementNodePath(XElement))));
            // Debug.LogException(e); 
            return null;
        }

    }

    /// <summary>
    /// 获取XElement文件树节点段XElement
    /// </summary>
    /// <param name="xml">XElement文件载体</param>
    /// <param name="mainnode">要查找的主节点</param>
    /// <param name="attribute1">主节点条件属性名1</param>
    /// <param name="value1">主节点条件属性值1</param>
    /// <param name="attribute2">主节点条件属性名2</param>
    /// <param name="value2">主节点条件属性值2</param>
    /// <returns>以该主节点为根的XElement</returns>
    public static XElement GetXElement(XElement XElement, string newroot, string attribute1, string value1, string attribute2, string value2)
    {
        //FuncStat funcStat = new FuncStat(new System.Diagnostics.StackTrace(), string.Format("{0}/{1}=>{2}", newroot, attribute, value));


        if (XElement != null)
        {
            IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
            foreach (var xmlItem in xmlItems)
            {
                XAttribute attrib1 = xmlItem.Attribute(attribute1);
                XAttribute attrib2 = xmlItem.Attribute(attribute2);
                if (null != attrib1 && attrib1.Value == value1)
                {
                    if (null != attrib2 && attrib2.Value == value2)
                    {
                        return xmlItem;
                    }
                }
            }

            return null;
        }
        else
        {
            // throw new Exception(string.Format("GetXElement异常, 读取: {0}/{1}={2}/{3}={4} 失败, xml节点名: {5}", newroot, attribute1, value1, attribute2, value2, GetXElementNodePath(XElement))));
            //  Debug.LogException(e); 
            return null;
        }


    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static XAttribute GetAttribute(XElement XElement, string attribute)
    {
        if (null == XElement) return null;

        //FuncStat funcStat = new FuncStat(new System.Diagnostics.StackTrace(), string.Format("{0}/{1}", XElement.Name, attribute));

        try
        {
            try
            {
                XAttribute attrib = XElement.Attribute(attribute);
                if (null == attrib)
                {
                    throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attribute, GetXElementNodePath(XElement)));
                    return null;
                }

                return attrib;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attribute, GetXElementNodePath(XElement)));
                Debug.LogException(e);
                return null;
            }
        }
        finally
        {
            //funcStat.PrintFuncStat();
            //funcStat = null;
        }
    }

    /// <summary>
    /// 安全获取xml文本字符串
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static string GetXElementAttributeStr(XElement XElement, string attributeName)
    {
        XAttribute attrib = GetAttribute(XElement, attributeName);
        if (null == attrib) return "";
        return (string)attrib;
    }

    /// <summary>
    /// 安全获取xml整型数值
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static int GetXElementAttributeInt(XElement XElement, string attributeName)
    {
        XAttribute attrib = GetAttribute(XElement, attributeName);
        if (null == attrib) return -1;
        string str = (string)attrib;
        if (null == str || str == "") return -1;
        int nReturn = 0;
        if (int.TryParse(str, out nReturn))
        {
            return nReturn;
        }
        else
        {
            //throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            //Debug.LogException(e); 
            //Debug.Log(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
            return -1;
        }
        /*
        try
        {
            return (int)Convert.ToDouble(str);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            Debug.LogException(e); 
            return -1;
        }
        */
    }

    /// <summary>
    /// 安全获取xml长整型数值
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static long GetXElementAttributeLong(XElement XElement, string attributeName)
    {
        XAttribute attrib = GetAttribute(XElement, attributeName);
        if (null == attrib) return -1;
        string str = (string)attrib;
        if (null == str || str == "") return -1;
        long nReturn = 0;
        if (long.TryParse(str, out nReturn))
        {
            return nReturn;
        }
        else
        {
            Debug.Log(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
            //throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            //Debug.LogException(e); 
            return -1;
        }
        /*
        try
        {
            return (long)Convert.ToInt64(str);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            Debug.LogException(e); 
            return -1;
        }
        */
    }

    /// <summary>
    /// 安全获取xml整型数值
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static double GetXElementAttributeDouble(XElement XElement, string attributeName)
    {
        XAttribute attrib = GetAttribute(XElement, attributeName);
        if (null == attrib) return -1;
        string str = (string)attrib;
        if (null == str || str == "") return -1;
        double nReturn = 0;
        if (double.TryParse(str, out nReturn))
        {
            return nReturn;
        }
        else
        {
            Debug.Log(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
            //throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            //Debug.LogException(e); 
            return -1;
        }

        /*
        try
        {
            return Convert.ToDouble(str);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            Debug.LogException(e); 
            return -1;
        }
        */
    }
    /// <summary>
    /// 安全获取xml整型数值
    /// </summary>
    /// <param name="XElement"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static float GetXElementAttributeFloat(XElement XElement, string attributeName)
    {
        XAttribute attrib = GetAttribute(XElement, attributeName);
        if (null == attrib) return -1.0f;
        string str = (string)attrib;
        if (null == str || str == "") return -1.0f;
        float nReturn = 0.0f;
        if (float.TryParse(str, out nReturn))
        {
            return nReturn;
        }
        else
        {
            Debug.Log(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
            //throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            //Debug.LogException(e); 
            return -1.0f;
        }

        /*
        try
        {
            return Convert.ToDouble(str);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement))));
            Debug.LogException(e); 
            return -1;
        }
        */
    }

    #endregion //XElement操作接口相关

    #region UTF8字节字符串转换
    private static string UTF8ByteArrayToString(byte[] b)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string s = encoding.GetString(b);
        return (s);
    }


    private static byte[] StringToUTF8ByteArray(string s)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] b = encoding.GetBytes(s);
        return b;
    }
    #endregion

    #region xml serialize
    public static object DeserializeXML(string content, Type t)
    {
        MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(content));

        XmlSerializer xs = new XmlSerializer(t);
        object obj = xs.Deserialize(ms);
        ms.Close();
        return obj;
    }


    public static void SerializeXML(object mObject, Type t, string XMLPath)
    {
        XmlWriterSettings xws = new XmlWriterSettings();
        xws.Encoding = Encoding.UTF8;

        XmlWriter xw = XmlWriter.Create(XMLPath, xws);

        XmlSerializer xs = new XmlSerializer(t);
        xs.Serialize(xw, mObject);
        xw.Close();
    }

    public static string SerializeObject(object mObject, Type t)
    {
        MemoryStream ms = new MemoryStream();
        XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);

        XmlSerializer xs = new XmlSerializer(t);
        xs.Serialize(xtw, mObject);

        ms = (MemoryStream)xtw.BaseStream;
        string xmlString = UTF8ByteArrayToString(ms.ToArray());
        Debug.Log("" + xmlString);
        return xmlString;
    }
    #endregion

}