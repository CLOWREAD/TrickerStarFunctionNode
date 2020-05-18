using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

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
    public enum TrickerStarSlotSide
    {
        INPUT,
        OUTPUT
    }
    public class TrickerStarNodeSolt {
        public String SlotType;
        public String SlotName;
        public int SlotIndex;
        public TrickerStarSlotSide SlotSide;
    }
    public class TrickerStarNodeSoltDetail
    {

        public String NodeName;
        public String SlotType;
        public String SlotName;
        public TrickerStarSlotSide SlotSide;
        public int SlotIndex;
        public String LineName;

    }
    public class TrickerStarLine
    {
        public String LineName;
        public TrickerStarNodeSoltDetail From, To;

    }
    public class TrickerStarFunctionNodeModel
    {
        public String NodeName;
        public Point Pos;
        public List<TrickerStarNodeSoltDetail> InputSlot = new List<TrickerStarNodeSoltDetail>();
        public List<TrickerStarNodeSoltDetail> OutputSlot = new List<TrickerStarNodeSoltDetail>();
    }



}
