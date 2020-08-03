using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Data.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace FunctionNode.Model
{
    class TrickerStarDataModel
    {
        public static String RandToken()
        {
            Random r = new Random();
            String res = "";
            for (int i = 0; i < 16; i++)
            {
                int ri = r.Next() % 256;
                res += ri.ToString("X2").Replace("0x", "");
            }
            return res;
        }
        public static Model.TrickerStarSlotType GetSlotTypeFromName(String name)
        {
            switch (name)
            {
                case "STRING":
                    return TrickerStarSlotType.STRING;
                    break;
                case "string":
                    return TrickerStarSlotType.STRING;
                    break;
                case "BOOL":
                    return TrickerStarSlotType.BOOL;
                    break;
                case "bool":
                    return TrickerStarSlotType.BOOL;
                    break;
                case "INT":
                    return TrickerStarSlotType.INT;
                    break;
                case "int":
                    return TrickerStarSlotType.INT;
                    break;
                case "DOUBLE":
                    return TrickerStarSlotType.DOUBLE;
                    break;
                case "double":
                    return TrickerStarSlotType.DOUBLE;
                    break;
                case "STRUCTURE":
                    return TrickerStarSlotType.STRUCTURE;
                    break;
                case "structure":
                    return TrickerStarSlotType.STRUCTURE;
                    break;
                case "EXECUTE":
                    return TrickerStarSlotType.EXECUTE;
                    break;
                case "PLACEHOLDER":
                    return TrickerStarSlotType.PLACEHOLDER;
                    break;
                case "INSTANCE_VALUE":
                    return TrickerStarSlotType.INSTANCE_VALUE;
                    break;
                default:      
                    return TrickerStarSlotType.UNDEFINED;
                    break;
            }

        }
    }
    [DataContract]
    public enum TrickerStarSlotSide
    {
        INPUT,
        OUTPUT
    }
    [DataContract]
    public enum TrickerStarSlotType
    {
        DOUBLE,
        BOOL,
        INT,
        STRING,
        PLACEHOLDER,
        EXECUTE,
        INSTANCE_VALUE,
        STRUCTURE,
        UNDEFINED
    }

    


    [DataContract]
    public class TrickerStarNodeSolt {
        [DataMember]
        public TrickerStarSlotType SlotType;
        [DataMember]
        public String SlotName;
        [DataMember]
        public int SlotIndex;
        [DataMember]
        public TrickerStarSlotSide SlotSide;
    }
    [DataContract]
    public class TrickerStarNodeSoltDetail
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public TrickerStarSlotType SlotType;
        [DataMember]
        public String SlotName;
        [DataMember]
        public TrickerStarSlotSide SlotSide;
        [DataMember]
        public int SlotIndex;
        [DataMember]
        public String LineName;
        [DataMember]
        public String SlotValue;

    }
    [DataContract]
    public class TrickerStarLineModel
    {
        [DataMember]
        public String LineName;
        [DataMember]
        public TrickerStarNodeSoltDetail From =new TrickerStarNodeSoltDetail(), To=new TrickerStarNodeSoltDetail();

    }
    [DataContract]
    public class TrickerStarFunctionNodeModel
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public String NodeTitle;
        [DataMember]
        public Point Pos;
        [DataMember]
        public List<TrickerStarNodeSoltDetail> InputSlot = new List<TrickerStarNodeSoltDetail>();
        [DataMember]
        public List<TrickerStarNodeSoltDetail> OutputSlot = new List<TrickerStarNodeSoltDetail>();
    }

    [DataContract]
    public class TrickerStarFunctionNodeCodeModel
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public String Code;
      
      
    }
    [DataContract]
    public class TrickerStarPresetCodeModel
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public String OriginalCode;

    }
    [DataContract]
    public class TrickerStarGroupModel
    {
        [DataMember]
        public String GroupName;
        [DataMember]
        public String GroupTitle;
    }
    [DataContract]
    public class TrickerStarNodeGroupModel
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public String GroupName;
        [DataMember]
        public String GroupTitle;
    }

    [DataContract]
    public class TrickerStarNodeViewSerialize
    {
        [DataMember]
        public List<TrickerStarFunctionNodeModel> Nodes = new List<TrickerStarFunctionNodeModel>();
        [DataMember]
        public List<TrickerStarLineModel> Lines = new List<TrickerStarLineModel>();
        [DataMember]
        public List<TrickerStarFunctionNodeCodeModel> Codes = new List<TrickerStarFunctionNodeCodeModel>();
        [DataMember]
        public List<TrickerStarGroupModel> Groups = new List<TrickerStarGroupModel>();
        [DataMember]
        public List<TrickerStarNodeGroupModel> NodeGroups = new List<TrickerStarNodeGroupModel>();
    }
    [DataContract]
    public class TrickerStarPresetSerialize
    {
        [DataMember]
        public List<TrickerStarPresetCodeModel> PresetCodes = new List<TrickerStarPresetCodeModel>();
    }

    public class JsonHelper

    {

        public static string ToJson(Object obj, Type type)

        {



            MemoryStream ms = new MemoryStream();



            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);





            seralizer.WriteObject(ms, obj);

            ms.Seek(0, SeekOrigin.Begin);



            StreamReader sr = new StreamReader(ms);

            string jsonstr = sr.ReadToEnd();



            //jsonstr = jsonstr.Replace("\"", "\\\"");



            sr.Close();

            ms.Close();

            return jsonstr;

        }

        public static Object FromJson(String jsonstr, Type type)

        {



            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonstr));



            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);



            ms.Seek(0, SeekOrigin.Begin);



        Object res = seralizer.ReadObject(ms);





            ms.Close();

            return res;

        }



    }

}


