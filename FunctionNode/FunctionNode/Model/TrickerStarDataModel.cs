using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Data.Json;
using System.Runtime.Serialization;

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
    }
    [DataContract]
    public enum TrickerStarSlotSide
    {
        INPUT,
        OUTPUT
    }
    [DataContract]
    public class TrickerStarNodeSolt {
        [DataMember]
        public String SlotType;
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
        public String SlotType;
        [DataMember]
        public String SlotName;
        [DataMember]
        public TrickerStarSlotSide SlotSide;
        [DataMember]
        public int SlotIndex;
        [DataMember]
        public String LineName;

    }
    [DataContract]
    public class TrickerStarLineModel
    {
        [DataMember]
        public String LineName;
        [DataMember]
        public TrickerStarNodeSoltDetail From, To;

    }
    [DataContract]
    public class TrickerStarFunctionNodeModel
    {
        [DataMember]
        public String NodeName;
        [DataMember]
        public Point Pos;
        [DataMember]
        public List<TrickerStarNodeSoltDetail> InputSlot = new List<TrickerStarNodeSoltDetail>();
        [DataMember]
        public List<TrickerStarNodeSoltDetail> OutputSlot = new List<TrickerStarNodeSoltDetail>();
    }



}
