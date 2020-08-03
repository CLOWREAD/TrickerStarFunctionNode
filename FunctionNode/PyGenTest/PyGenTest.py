

class D:
    a=0
    GlobalD=dict();
    def fn_test(self):
        GlobalD[0]=fn_test;
        while (a==0 & a>1):
            a+=1;
        getattr(self ,"")
        return
    def MAIN (self, param):
        result = dict();
        method = null;            
        F_Name_invoking = "null";          
        while (F_Name_invoking != None &   F_Name_invoking != "" ):
            
            d = dict;
            d.GLOBAL_OUTPUT = result;
            invoke_res =method(d);
            result[F_Name_invoking] = invoke_res;
            if(invoke_res.FUNCTIONNAMETOINVOKE==None):
        
                break;
         
            F_Name_invoking = invoke_res.FUNCTIONNAMETOINVOKE;
            method =  getattr(self ,F_Name_invoking);
            

    
    
    
@MAIN
INPUT API_NAME : INSTANCE_VALUE
OUTPUT INVOKE : EXECUTE 
-------------------
def TRICKER_STAR_INSTANCE_VALUE(API_NAME) (self, param):
    result = dict();
    method = self.TRICKER_STAR_INVOKE(INVOKE);            
    F_Name_invoking = "TRICKER_STAR_INVOKE(INVOKE)";          
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
            

@COMMON
INPUT RUN : EXECUTE
INPUT V : STRING
OUTPUT INVOKE : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    V=GLOBAL_OUTPUT[TRICKER_STAR_LINKED_NODE(V)].TRICKER_STAR_LINKED_SLOT(V);
    d= dict();
    print("%s",d);
    d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(INVOKE)";
    return d;

@PRINT
INPUT RUN : EXECUTE
INPUT V : STRING
OUTPUT INVOKE : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    V=GLOBAL_OUTPUT["TRICKER_STAR_LINKED_NODE(V)"]["TRICKER_STAR_LINKED_SLOT(V)"];
    d= dict();
    print(V);
    d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(INVOKE)";
    return d;





@INSTANCEVALUE_STRING
INPUT RUN : EXECUTE
INPUT V : INSTANCE_VALUE
OUTPUT INVOKE : EXECUTE 
OUTPUT VALUE : STRING 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    V=  "TRICKER_STAR_INSTANCE_VALUE(V)";
    d= dict();
    d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(INVOKE)";
    d["VALUE"]=V;
    return d;





@INSTANCEVALUE_INT
INPUT RUN : EXECUTE
INPUT V : INSTANCE_VALUE
OUTPUT VALUE : INT 
OUTPUT INVOKE : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    V=  TRICKER_STAR_INSTANCE_VALUE(V);
    d= dict();
    d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(INVOKE)";
    d["VALUE"]=V;
    return d;





@IF
INPUT RUN : EXECUTE
INPUT COND : BOOL
OUTPUT TRUE : EXECUTE 
OUTPUT FALSE : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    COND=GLOBAL_OUTPUT[TRICKER_STAR_LINKED_NODE(V)].TRICKER_STAR_LINKED_SLOT(V);
    d= dict();
    if COND:
        d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(TRUE)";
    else 
        d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(FALSE)";
    return d;

@COMPARE
INPUT RUN : EXECUTE
INPUT A : INT
INPUT B : INT
OUTPUT Greater : EXECUTE 
OUTPUT Lower : EXECUTE 
OUTPUT Equals : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):
    GLOBAL_OUTPUT = param["GLOBAL_OUTPUT"];
    COND=GLOBAL_OUTPUT["TRICKER_STAR_LINKED_NODE(V)"]["TRICKER_STAR_LINKED_SLOT(V)"];
    d= dict();
    if A>B:
        d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(Greater)";
    if A==B:
        d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(Equals)";
    if A<B:
        d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(Lower)";
    
    return d;

@MERGE
INPUT RUN0 : EXECUTE
INPUT RUN1 : EXECUTE
OUTPUT INVOKE : EXECUTE 
-------------------
def @TRICKER_STAR_FUNCTIONNAME (self, param):   
    d= dict();
    d["FUNCTIONNAMETOINVOKE"]="TRICKER_STAR_INVOKE(INVOKE)";
    return d;