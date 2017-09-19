//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RapidXml
{
    // Attribute
    public struct NodeAttribute
    {
        public RapidXmlParser Document;
        public IntPtr NativeAttrPtr;

        [Conditional("UNITY_EDITOR")]
        public static void EditorAssert(bool bInCondition)
        {
            if (!bInCondition)
            {
                UnityEngine.Debug.DebugBreak();
            }
        }

        public bool IsValid()
        {
            return Document != null && NativeAttrPtr != IntPtr.Zero;
        }

        public string GetName()
        {
            EditorAssert(IsValid());

            IntPtr Result = RapidXmlParser.GetAttributeNamePtr(Document.NativeDocumentPtr, NativeAttrPtr);
            return Result != IntPtr.Zero ? Marshal.PtrToStringAnsi(Result) : "";
        }

        public string GetValue()
        {
            EditorAssert(IsValid());

            IntPtr Result = RapidXmlParser.GetAttributeValuePtr(Document.NativeDocumentPtr, NativeAttrPtr);

            return Result != IntPtr.Zero ? Marshal.PtrToStringAnsi(Result) : "";
        }

        public bool GetBool()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueBool(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public int GetInt()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueInt(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public uint GetUInt()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueUInt(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public Int64 GetInt64()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueInt64(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public UInt64 GetUInt64()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueUInt(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public float GetFloat()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueFloat(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public double GetDouble()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeValueDouble(Document.NativeDocumentPtr, NativeAttrPtr);
        }

        public NodeAttribute NextAttribute(string InName = null)
        {
            EditorAssert(IsValid());

            NodeAttribute Attr = new NodeAttribute();
            Attr.Document = this.Document;
            Attr.NativeAttrPtr = string.IsNullOrEmpty(InName) ?
                RapidXmlParser.NextAttributePtr(Document.NativeDocumentPtr, NativeAttrPtr) :
                RapidXmlParser.NextAttributePtrWithName(Document.NativeDocumentPtr, NativeAttrPtr, InName);

            return Attr;
        }
    }

    public struct NodeElement
    {
        public RapidXmlParser Document;
        public IntPtr NativeNodePtr;

        [Conditional("UNITY_EDITOR")]
        public static void EditorAssert(bool bInCondition)
        {
            if (!bInCondition)
            {
                UnityEngine.Debug.DebugBreak();
            }
        }

        public bool IsValid()
        {
            return Document != null && NativeNodePtr != IntPtr.Zero;
        }
        
        public NodeElement FirstNode(string InName = null)
        {
            EditorAssert(IsValid());

            NodeElement Element = new NodeElement();
            Element.Document = Document;
            Element.NativeNodePtr =
                string.IsNullOrEmpty(InName) ?
                RapidXmlParser.FirstNodePtr(Document.NativeDocumentPtr, NativeNodePtr) :
                RapidXmlParser.FirstNodePtrWithName(Document.NativeDocumentPtr, NativeNodePtr, InName);

            return Element;
        }

        public NodeElement NextSibling(string InName = null)
        {
            EditorAssert(IsValid());

            NodeElement Element = new NodeElement();
            Element.Document = Document;
            Element.NativeNodePtr = 
                string.IsNullOrEmpty(InName) ?
                RapidXmlParser.NextSiblingPtr(Document.NativeDocumentPtr, NativeNodePtr) :
                RapidXmlParser.NextSiblingPtrWithName(Document.NativeDocumentPtr, NativeNodePtr, InName);

            return Element;
        }

        public NodeAttribute FirstAttribute(string InName= null)
        {
            EditorAssert(IsValid());

            NodeAttribute Attr = new NodeAttribute();
            Attr.Document = this.Document;
            Attr.NativeAttrPtr =
                string.IsNullOrEmpty(InName) ?
                RapidXmlParser.FirstAttributePtr(Document.NativeDocumentPtr, NativeNodePtr) :
                RapidXmlParser.FirstAttributePtrWithName(Document.NativeDocumentPtr, NativeNodePtr, InName);

            return Attr;
        }

        public bool HasAttribute(String InName)
        {
            EditorAssert(IsValid());

            return RapidXmlParser.HasAttribute(Document.NativeDocumentPtr, NativeNodePtr, InName);
        }

        public bool AttributeBool(String InName)
        {
            EditorAssert(IsValid());

            return RapidXmlParser.AttributeBool(Document.NativeDocumentPtr, NativeNodePtr, InName);
        }

        public int AttributeInt(string InName)
        {
            EditorAssert(IsValid());

            return RapidXmlParser.AttributeInt(Document.NativeDocumentPtr, NativeNodePtr, InName);
        }

        public uint AttributeUInt(string InName)
        {
            EditorAssert(IsValid());

            return RapidXmlParser.AttributeUInt(Document.NativeDocumentPtr, NativeNodePtr, InName);
        }

        public float AttributeFloat(String InName)
        {
            EditorAssert(IsValid());

            return RapidXmlParser.AttributeFloat(Document.NativeDocumentPtr, NativeNodePtr, InName);
        }

        public string AttributeString(string InName)
        {
            EditorAssert(IsValid());

            IntPtr Result = RapidXmlParser.AttributeStringPtr(Document.NativeDocumentPtr, NativeNodePtr, InName);
            return Result != IntPtr.Zero ? Marshal.PtrToStringAnsi(Result) : "";
        }

        // the same with AttributeString
        // created for compatible
        public string Attribute(string InName)
        {
            EditorAssert(IsValid());

            IntPtr Result = RapidXmlParser.AttributeStringPtr(Document.NativeDocumentPtr, NativeNodePtr, InName);
            return Result != IntPtr.Zero ? Marshal.PtrToStringAnsi(Result) : "";
        }

        public string GetName()
        {
            EditorAssert(IsValid());

            IntPtr Result = RapidXmlParser.GetNodeTagPtr(Document.NativeDocumentPtr, NativeNodePtr);
            return Result != IntPtr.Zero ? Marshal.PtrToStringAnsi(Result) : "";
        }

        public int GetChildNodeCount()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetChildNodeCount(Document.NativeDocumentPtr, NativeNodePtr);
        }

        public int GetAttributeCount()
        {
            EditorAssert(IsValid());

            return RapidXmlParser.GetAttributeCount(Document.NativeDocumentPtr, NativeNodePtr);
        }
    }

    public class RapidXmlParser : IDisposable
    {
        public const string PluginName = "RapidXml";

        public IntPtr NativeDocumentPtr = IntPtr.Zero;

        public void Load(string InContent)
        {
            NativeDocumentPtr = LoadFromString(InContent);

            string ErrorMessage = Marshal.PtrToStringAnsi(GetLastErrorMessage(NativeDocumentPtr));

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                throw new Exception(ErrorMessage);
            }
        }

        public void Dispose()
        {
            if (NativeDocumentPtr != IntPtr.Zero)
            {
                DisposeThis(NativeDocumentPtr);
                NativeDocumentPtr = IntPtr.Zero;
            }
        }

        public NodeElement FirstNode(string InName = null)
        {
            NodeElement Element = new NodeElement();
            Element.Document = this;
            Element.NativeNodePtr =
                string.IsNullOrEmpty(InName) ?
                FirstNodePtr(NativeDocumentPtr, IntPtr.Zero) :
                FirstNodePtrWithName(NativeDocumentPtr, IntPtr.Zero, InName);

            return Element;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // internal use
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        private static extern IntPtr LoadFromString([MarshalAs(UnmanagedType.LPStr)]string InContent);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        private static extern IntPtr GetLastErrorMessage(IntPtr InDocumentNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        private static extern void DisposeThis(IntPtr InDocumentNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr FirstAttributePtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr FirstAttributePtrWithName(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr NextAttributePtr(IntPtr InDocumentNativePtr, IntPtr InAttrPtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr NextAttributePtrWithName(IntPtr InDocumentNativePtr, IntPtr InAttrPtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern bool HasAttribute(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern bool AttributeBool(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern int AttributeInt(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern uint AttributeUInt(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern Int64 AttributeInt64(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern UInt64 AttributeUInt64(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern float AttributeFloat(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern double AttributeDouble(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr AttributeStringPtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr FirstNodePtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr FirstNodePtrWithName(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr NextSiblingPtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr NextSiblingPtrWithName(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr, [MarshalAs(UnmanagedType.LPStr)]String InName);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr GetNodeTagPtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern int GetChildNodeCount(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern int GetAttributeCount(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr GetAttributeNamePtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern IntPtr GetAttributeValuePtr(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern bool GetAttributeValueBool(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern int GetAttributeValueInt(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern uint GetAttributeValueUInt(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern int GetAttributeValueInt64(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern uint GetAttributeValueUInt64(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern float GetAttributeValueFloat(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);


#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#else
        [DllImport(PluginName, CallingConvention = CallingConvention.Cdecl)]
#endif
        internal static extern double GetAttributeValueDouble(IntPtr InDocumentNativePtr, IntPtr InNodeNativePtr);
    }
}

