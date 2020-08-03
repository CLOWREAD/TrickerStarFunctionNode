

class D:
    def NODEFFF6B24D4E4DA2943BF1A9842104DB36 (self, param):
        GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
        V=  "HELLO WORLD";
        d= dict();
        d["FUNCTIONNAMETOINVOKE"]="NODE8503E3AD414BB00770803860C3F5AEBE";
        d["VALUE"]=V;
        return d;

    def NODE8503E3AD414BB00770803860C3F5AEBE (self, param):
        GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
        V=GLOBAL_OUTPUT["NODEFFF6B24D4E4DA2943BF1A9842104DB36"]["VALUE"];
        d= dict();
        print(V);
        d["FUNCTIONNAMETOINVOKE"]="null";
        return d;

	


    def MAIN (self, param):
        result = dict();
        method = self.NODEFFF6B24D4E4DA2943BF1A9842104DB36;            
        F_Name_invoking = "NODEFFF6B24D4E4DA2943BF1A9842104DB36";          
        while (F_Name_invoking != None and F_Name_invoking != "null" ):            
            d = dict();
            d["GLOBAL_OUTPUT"] = result;
            invoke_res =method(d);
            result[F_Name_invoking] = invoke_res;
            if(invoke_res["FUNCTIONNAMETOINVOKE"]==None):        
                break;         
            if(invoke_res["FUNCTIONNAMETOINVOKE"]=="null"):        
                break;
            F_Name_invoking = invoke_res["FUNCTIONNAMETOINVOKE"]
            method =  getattr(self ,F_Name_invoking);


            


dtest=D();
dtest.MAIN(None);
