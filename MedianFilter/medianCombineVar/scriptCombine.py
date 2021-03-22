#Eesa Nikahd 12/01/1393
#script program to create a txt file for my topModule
import math
print("median filter\nby Eesa Nikahd\n")
W = input ("enter maximum size of the window:")
dataLen = input ("enter data length (per bit):")

macrofile = open("macro.vh","w")
macrofile.write("`ifndef _my_include_vh_\n`define _my_include_vh_\n\n")
macrofile.write("`define W_MAX %d\n`define LOG_WMAX %d\n`define DATA_LENGTH  %d\n`define HIGH_Z  %d'bz" %(W ,math.ceil(math.log(W,2)),dataLen,dataLen))
macrofile.write("\n`endif\n")
macrofile.close()

myfile = open ("top.v" ,"w")
myfile.write("`timescale 1ns / 1ps\n`include \"macro.vh\"\n///////////////////////\n")
myfile.write("module top ( clk ,reset ,X , W, median );\n\n")
myfile.write("output [`DATA_LENGTH-1:0] median ;\ninput [`DATA_LENGTH-1:0] X;\ninput [`LOG_WMAX-1:0] W;\ninput clk ,reset ;\n")

myfile.write("\nwire 	[`DATA_LENGTH-1:0] R_old")
for i in range(1,W+1):
    myfile.write(" ,R1_%d ,R2_%d" % (i,i))
myfile.write(";")

myfile.write("\nwire 	T1")
for i in range(2,W+1):
    myfile.write(" ,T%d" % i)
myfile.write(";")

myfile.write("\nwire 	 Z1")
for i in range(2,W):
    myfile.write(" ,Z%d" %i)
myfile.write(";\n")

								 #m1(.X(X), .clk(clk), .reset(reset), .W(W), .isMedian(isLast1|isLast2), .R1_R(R1_2), .R_old_in(R_old), .T_R(T2), .R1(R1_1), .R2(R2_1), .R_median(median), .R_old_out(R_old), .Z(Z1), .T(T1), .isLast(isLast1));				   
myfile.write("medianCell_leftMst  m1(.X(X), .clk(clk), .reset(reset), .W(W), .isMedian(isLast1|isLast2), .R1_R(R1_2), .R_old_in(R_old), .T_R(T2), .R1(R1_1), .R2(R2_1), .R_median(median), .R_old_out(R_old), .Z(Z1), .T(T1), .isLast(isLast1));\n")
for i in range(2,W/2+1):                                                                       
    myfile.write("medianFilterCell  m%d(.X(X) ,.clk(clk), .reset(reset) , .W(W), .cellNo(`LOG_WMAX'd%d), .isMedian(isLast%d|isLast%d), .R1_L(R1_%d), .R1_R(R1_%d), .R2_L(R2_%d), .R_old_in(R_old), .Z_L(Z%d), .T_L(T%d), .T_R(T%d), .R1(R1_%d), .R2(R2_%d), .R_median(median), .R_old_out(R_old), .Z(Z%d), .T(T%d), .isLast(isLast%d));\n"%(i,i,2*i-1,2*i,i-1,i+1,i-1,i-1,i-1,i+1,i,i,i,i,i))
if ( W%2==1):
	i=W/2+1
	myfile.write("medianFilterCell  m%d(.X(X) ,.clk(clk), .reset(reset) , .W(W), .cellNo(`LOG_WMAX'd%d), .isMedian(isLast%d), .R1_L(R1_%d), .R1_R(R1_%d), .R2_L(R2_%d), .R_old_in(R_old), .Z_L(Z%d), .T_L(T%d), .T_R(T%d), .R1(R1_%d), .R2(R2_%d), .R_median(median), .R_old_out(R_old), .Z(Z%d), .T(T%d), .isLast(isLast%d));\n"%(i,i,2*i-1,i-1,i+1,i-1,i-1,i-1,i+1,i,i,i,i,i))
	k = i+1
else:
	k = W/2+1
for i in range (k,W):
	myfile.write("medianFilterCell  m%d(.X(X) ,.clk(clk), .reset(reset) , .W(W), .cellNo(`LOG_WMAX'd%d), .isMedian(1'b0), .R1_L(R1_%d), .R1_R(R1_%d), .R2_L(R2_%d), .R_old_in(R_old), .Z_L(Z%d), .T_L(T%d), .T_R(T%d), .R1(R1_%d), .R2(R2_%d), .R_median(median), .R_old_out(R_old), .Z(Z%d), .T(T%d), .isLast(isLast%d));\n"%(i,i,i-1,i+1,i-1,i-1,i-1,i+1,i,i,i,i,i))
myfile.write("medianCell_rightMst  m%d(.X(X), .clk(clk), .reset(reset), .W(W), .cellNo(`LOG_WMAX'd%d), .R1_L(R1_%d), .R2_L(R2_%d), .R_old_in(R_old), .Z_L(Z%d), .T_L(T%d), .R1(R1_%d), .R_old_out(R_old), .T(T%d), .isLast(isLast%d));\n"%(W,W,W-1,W-1,W-1,W-1,W,W,W))

myfile.write("\n\nendmodule")     
                 
myfile.close()                 
