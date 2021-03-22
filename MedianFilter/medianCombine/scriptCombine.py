#Eesa Nikahd 12/01/1393
#script program to create a txt file for my topModule
import math
print("median filter\nby Eesa Nikahd\n")
W = input ("enter size of  window :")
dataLen = input ("enter data length (per bit):")

macrofile = open("macro.vh","w")
macrofile.write("`ifndef _my_include_vh_\n`define _my_include_vh_\n\n")
macrofile.write("`define W %d\n`define LOG_W %d\n`define DATA_LENGTH  %d" %(W ,math.ceil(math.log(W,2)),dataLen))
macrofile.write("\n`endif\n")
macrofile.close()

fifo = open ("FIFO.v" ,"w")
fifo.write("`timescale 1ns / 1ps\n`include \"macro.vh\"\n///////////////////////\n")
fifo.write("module FIFO( in , out , clk ,reset);\n\n")
fifo.write("input  [`DATA_LENGTH-1:0] in;\noutput [`DATA_LENGTH-1:0] out;\ninput  clk ,reset;\n")			
fifo.write("reg	[`DATA_LENGTH-1:0] fifo [0:`W-1];\n\n")
fifo.write("assign  out =  fifo[`W-1] ;\n")
fifo.write("always @(posedge clk)\nbegin\n\tif(reset)\n\tbegin")
for i in range(W):
    fifo.write("\n\t\tfifo[%d] <= 0;"%i)
fifo.write("\n\tend\n\telse\n\tbegin\n\t\t\tfifo[0] <= in ;")
for i in range(1,W):
    fifo.write("\n\t\t\tfifo[%d] <= fifo[%d];" %(i,i-1))
fifo.write("\n\tend\nend\n\nendmodule")
fifo.close()

myfile = open ("top.v" ,"w")
myfile.write("`timescale 1ns / 1ps\n`include \"macro.vh\"\n///////////////////////\n")
myfile.write("module top ( clk ,reset ,X ,median );\n\n")
#myfile.write("parameter    WMAX = %d , LOG_WMAX = %d ,DATA_LENGTH = %d ;\n" % (Wmax,int(math.log(Wmax,2)+1),dataLen))
myfile.write("output [`DATA_LENGTH-1:0] median ;\ninput [`DATA_LENGTH-1:0] X ;\ninput clk ,reset ;\n")

myfile.write("\nwire 	[`DATA_LENGTH-1:0] R_old, R1")
for i in range(2,W+1):
    myfile.write(" ,R%d" % i)
myfile.write(";")

myfile.write("\nwire 	T1")
for i in range(2,W+1):
    myfile.write(" ,T%d" % i)
myfile.write(";")

myfile.write("\nwire 	 Z1")
for i in range(2,W):
    myfile.write(" ,Z%d" %i)
myfile.write(";\n")

if (W%2) == 0:
    myfile.write(" assign median = (R%d + R%d)>>1;\n\n" %((W/2),(W/2)+1))
else:
    myfile.write("assign median =  R%d;\n\n" %(int(math.ceil(W/2.0))))

myfile.write("FIFO myfifo(.in(X), .out(R_old), .clk(clk), .reset(reset));\n")

myfile.write("medianCell_leftMst  m1(.X(X) , .clk(clk), .reset(reset) , .R_R(R2), .T_R(T2), .R_old(R_old), .Z(Z1), .R(R1), .T(T1));\n")
for i in range(2,W):                                                                       
    myfile.write("medianFilterCell  m%d(.X(X) ,.clk(clk), .reset(reset) ,.Z_L(Z%d), .R_L(R%d),.R_R(R%d) ,.T_L(T%d), .T_R(T%d), .R_old(R_old), .Z(Z%d), .R(R%d), .T(T%d));\n"%(i,i-1,i-1,i+1,i-1,i+1,i,i,i))
                
myfile.write("medianCell_rightMst  m%d(.X(X) ,.clk(clk), .reset(reset), .Z_L(Z%d), .R_L(R%d), .T_L(T%d), .R_old(R_old), .R(R%d),.T(T%d));\n"%(W,W-1,W-1,W-1,W,W))

myfile.write("\n\nendmodule")     
                 
myfile.close()                 
