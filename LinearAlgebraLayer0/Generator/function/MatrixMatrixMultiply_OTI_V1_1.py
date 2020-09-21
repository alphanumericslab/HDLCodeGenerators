#uses MatrixMatrixMultiply_#M_#N_#prPR_S#noepsE_HRx#hrf_C#mamcs_A#baseAdderSize_(NCL, CL)_(NRL, RL) - v1.1
#uses OTItoSeries_#N_HRx#hrr_S#noepsE_(NOR, OR)_(NIL, IL) - v1.0x

import sys
import math

def MatrixMatrixMultiply_OTI(parameters, __Print_To_File):

	M = parameters["M"]
	N = parameters["N"]
	Q = parameters["Q"]
	PR = parameters["PR"]
	PC = parameters["PC"]
	HRR = parameters["HRR"]
	MAMCS = parameters["MAMCS"]
	AdderSize = parameters["ADDER_SIZE"]
	Enable_Input_Latch = parameters["ENABLE_INPUT_LATCH"]
	Enable_Column_Latch = parameters["ENABLE_COLUMN_LATCH"]
	Enable_Row_Latch = parameters["ENABLE_ROW_LATCH"]
	Enable_OTI_Output_Registers = parameters["ENABLE_OTI_OUTPUT_REGISTERS"]
	
	#IN_WIDTH>=1
	IN_WIDTH = 10
	#INPUT_REG_DEPTH>=0
	INPUT_REG_DEPTH = 1
	#MULT_PIPE_DEPTH>=0
	MULT_PIPE_DEPTH = 1

	NMult = math.ceil(N/HRR) #chooses best value of NMult automatically
	RS = math.ceil(M/PR)
	CS = math.ceil(Q/PC)
	#NumOfComponents = math.ceil(NMult/MAMCS)

	ModuleName = "MatrixMatrixMultiply_{}_{}_{}_{}PR_{}PC_OTI".format(M, N, Q, PR, PC)
	if Enable_OTI_Output_Registers<=0:
		ModuleName += "N"
	ModuleName += "R_HRx{}_S{}E_C{}_A{}_".format(HRR, NMult, MAMCS, AdderSize)
	if Enable_Input_Latch<=0:
		ModuleName += "N"
	ModuleName += "IL_"
	if Enable_Column_Latch<=0:
		ModuleName += "N"
	ModuleName += "CL_"
	if Enable_Row_Latch<=0:
		ModuleName += "N"        
	ModuleName += "RL"

	if __Print_To_File<=0:
		of = sys.stdout
	else:
		of = open("./"+ModuleName+".v", 'w+')

	print("`timescale 1ns / 1ps\n", file=of)
	print("module "+ModuleName, file=of)
	print("#(parameter", file=of)
	print("IN_WIDTH = {},".format(IN_WIDTH), file=of)
	print("INPUT_REG_DEPTH = {},".format(INPUT_REG_DEPTH), file=of)
	print("MULT_PIPE_DEPTH = {}".format(MULT_PIPE_DEPTH), file=of)
	print(")(", file=of)
	print("input clk, reset, enable,", file=of)
	print("output newInSeriesStart,", file=of)
	lghrr=math.ceil(math.log2(HRR))
	print("output ", end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print("reg ", end='', file=of)
	if lghrr==1:
		print("inSeries", end='', file=of)
	elif lghrr>1:
		print("[{}:0] inSeries".format(lghrr-1), end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print(" = 0,", file=of)
	else:
		print(",", file=of)
	lgRS=math.ceil(math.log2(RS))
	print("output ", end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print("reg ", end='', file=of)
	if lgRS<=1:
		print("rowSetInNo", end='', file=of)
	else:
		print("[{}:0] rowSetInNo".format(lgRS-1), end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print(" = 0,", file=of)
	else:
		print(",", file=of)
	lgCS=math.ceil(math.log2(CS))
	print("output ", end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print("reg ", end='', file=of)
	if lgCS<=1:
		print("columnSetInNo", end='', file=of)
	else:
		print("[{}:0] columnSetInNo".format(lgCS-1), end='', file=of)
	if Enable_OTI_Output_Registers>0:
		print(" = 0,", file=of)
	else:
		print(",", file=of)
	print("input inReady,", file=of)
	print("input signed [IN_WIDTH-1:0] ", end='', file=of)
	for i in range(PR):
		for j in range(N):
			print("AE{}R{}, ".format(j, i), end='', file=of)
		print(file=of)
	print("input signed [IN_WIDTH-1:0] ", end='', file=of)
	for i in range(PC):
		for j in range(N):
			print("BE{}C{}, ".format(j, i), end='', file=of)
		print(file=of)
	ac=CS*PC-Q
	QE = PC-ac-1
	if QE==0:
		stc1="C0"
	else:
		stc1="C0toC{}".format(QE)
	if QE==(PC-2):
		stc2="C{}".format(PC-1)
	else:
		stc2="C{}toC{}".format(QE+1, PC-1)
	ar=RS*PR-M
	ME = PR-ar-1
	if ME==0:
		str1="R0"
	else:
		str1="R0toR{}".format(ME)
	if ME==(PR-2):
		str2="R{}".format(PR-1)
	else:
		str2="R{}toR{}".format(ME+1, PR-1)
	print("output "+stc1+str1+"EReady,", file=of)
	if ar!=0:
		print("output "+stc1+str2+"EReady,", file=of)
	else:
		print("output reg NUEReady1 = 0, //not used", file=of)
	if ac!=0:
		print("output "+stc2+str1+"EReady,", file=of)
		if ar!=0:
			print("output "+stc2+str2+"EReady,", file=of)
		else:
			print("output reg NUEReady2 = 0, //not used", file=of)
	else:
		print("output reg NUEReady2 = 0, //not used", file=of)
		print("output reg NUEReady3 = 0, //not used", file=of)
	if lgRS<=1:
		print("output rowSetOutNo,", file=of)
	else:
		print("output [{}:0] rowSetOutNo,".format(lgRS-1), file=of)
	if lgCS<=1:
		print("output columnSetOutNo,", file=of)
	else:
		print("output [{}:0] columnSetOutNo,".format(lgCS-1), file=of)
	lgN=math.ceil(math.log2(N))
	al=-1+lgN
	if al==0:
		print("output signed [(2*IN_WIDTH):0] ", end='',file=of)
	elif al<0:
		print("output signed [(2*IN_WIDTH){}:0] ".format(al), end='',file=of)
	else:
		print("output signed [(2*IN_WIDTH)+{}:0] ".format(al), end='',file=of)
	for i in range(PC):
		for j in range(PR):
			print("EC{}R{}, ".format(i, j), end='',file=of)
	print(file=of)
	print("output early"+stc1+str1+"EReady,", file=of)
	if ar!=0:
		print("output early"+stc1+str2+"EReady,", file=of)
	else:
		print("output reg NUeEReady1 = 0, //not used", file=of)
	if ac!=0:
		print("output early"+stc2+str1+"EReady,", file=of)
		if ar!=0:
			print("output early"+stc2+str2+"EReady", file=of)
		else:
			print("output reg NUeEReady2 = 0 //not used", file=of)
	else:
		print("output reg NUeEReady2 = 0, //not used", file=of)
		print("output reg NUeEReady3 = 0 //not used", file=of)
	print(");\n", file=of)

	#OTItoSeries(N, HRR, Enable_Input_Latch, Enable_OTI_Output_Registers, __Print_To_File);

	print("wire MMInReadyR;", file=of)
	if Enable_Row_Latch>0:
		print("wire inReadyCR = (columnSetInNo==0) & inReady;", file=of)
	for j in range(PR):
		print("wire signed [IN_WIDTH-1:0] ", end='', file=of)
		for i in range(NMult-1):
			print("ASE{}R{}, ".format(i, j), end='', file=of)
		print("ASE{}R{};".format(NMult-1, j), file=of)
		print("OTItoSeries_{}_HRx{}_S{}E_NOR_".format(N, HRR, NMult), end='', file=of)
		if Enable_Input_Latch<=0:
			print("N", end='', file=of)
		print("IL #( .IN_WIDTH(IN_WIDTH) )", file=of)
		print("OTItSA{} (".format(j), file=of)
		print("clk, reset, enable,", file=of)
		print("A{}NISS, //not used".format(j), file=of)
		print("A{}IS, //not used".format(j), file=of)
		if Enable_Row_Latch>0:
			print("inReadyCR,", file=of)
		else:
			print("inReady,", file=of)    
		for i in range(N):
			print("AE{}R{}, ".format(i, j), end='', file=of)
		print(file=of)
		if j==0:
			print("MMInReadyR,", file=of)
		else:
			print("A{}outReady, //not used".format(j), file=of)
		print("A{}S2outReady, //not used".format(j), file=of)
		print("A{}outSeries, //not used".format(j), file=of)
		for i in range(NMult):
			print("ASE{}R{}, ".format(i, j), end='', file=of)
		print(file=of)
		print("A{}S1earlyOutReady, //not used".format(j), file=of)
		print("A{}S2earlyOutReady //not used".format(j), file=of)
		print(");\n", file=of)

	print("wire MMInReadyC;", file=of)
	if Enable_Column_Latch>0:
		print("wire inReadyCC = (rowSetInNo==0) & inReady;", file=of)
	for j in range(PC):
		print("wire signed [IN_WIDTH-1:0] ", end='', file=of)
		for i in range(NMult-1):
			print("BSE{}C{}, ".format(i, j), end='', file=of)
		print("BSE{}C{};".format(NMult-1, j), file=of)
		print("OTItoSeries_{}_HRx{}_S{}E_NOR_".format(N, HRR, NMult), end='', file=of)
		if Enable_Input_Latch<=0:
			print("N", end='', file=of)
		print("IL #( .IN_WIDTH(IN_WIDTH) )", file=of)
		print("OTItSB{} (".format(j), file=of)
		print("clk, reset, enable,", file=of)
		print("B{}NISS, //not used".format(j), file=of)
		print("B{}IS, //not used".format(j), file=of)
		if Enable_Column_Latch>0:
			print("inReadyCC,", file=of)
		else:
			print("inReady,", file=of)    
		for i in range(N):
			print("BE{}C{}, ".format(i, j), end='', file=of)
		print(file=of)
		if j==0:
			print("MMInReadyC,", file=of)
		else:
			print("B{}outReady, //not used".format(j), file=of)
		print("B{}S2outReady, //not used".format(j), file=of)
		print("B{}outSeries, //not used".format(j), file=of)
		for i in range(NMult):
			print("BSE{}C{}, ".format(i, j), end='', file=of)
		print(file=of)
		print("B{}S1earlyOutReady, //not used".format(j), file=of)
		print("B{}S2earlyOutReady //not used".format(j), file=of)
		print(");\n", file=of)

	if Enable_OTI_Output_Registers>0:
		print("reg newInSeriesStartI = 0;", file=of)
		if Enable_Row_Latch<=0 or Enable_Column_Latch<=0:
			inrCC = False
			print("wire inReadyC = inReady;", file=of)
			print("assign newInSeriesStart = newInSeriesStartI;", file=of)
		else: #Enable_Row_Latch>0 and Enable_Column_Latch>0
			#inrCC: inReady Check Cancel!
			inrCC = True
			print("wire inrCC = (columnSetInNo!=0) & (rowSetInNo!=0);", file=of) # ~inrCC : (columnSetInNo==0) | (rowSetInNo==0)
			print("wire inReadyC = inReady | inrCC;", file=of)
			print("assign newInSeriesStart = newInSeriesStartI & (~inrCC);", file=of)

	if Enable_OTI_Output_Registers>0:
		print("always @(posedge clk) begin", file=of)
		print("\tif(reset) begin", file=of)
		print("\t\tinSeries <= 0;", file=of)
		print("\t\tnewInSeriesStartI <= 0;", file=of)
		print("\tend", file=of)
		print("\telse if(enable & ", end='', file=of)
		if Enable_Input_Latch<=0:
			print("inReadyC", end='', file=of)
		else:
			print("(inReadyC | (inSeries!=0))", end='', file=of)
		print(") begin", file=of)
		print("\t\tif(inSeries=={}) begin".format(HRR-1), file=of)
		print("\t\t\tnewInSeriesStartI <= 1;", file=of)
		print("\t\t\tinSeries <= 0;", file=of)
		print("\t\tend", file=of)
		print("\t\telse begin", file=of)
		print("\t\t\tnewInSeriesStartI <= 0;", file=of)
		if HRR==2:
			print("\t\t\tinSeries <= 1;", file=of)    
		else:
			print("\t\t\tinSeries <= inSeries+1;", file=of)
		print("\t\tend", file=of)
		print("\tend", file=of)
		print("end", file=of)
		print(file=of)

	if Enable_OTI_Output_Registers>0 and RS>1:
		print("always @(posedge clk) begin", file=of)
		print("\tif(reset) begin", file=of)
		print("\t\trowSetInNo <= 0;", file=of)
		print("\tend", file=of)
		print("\telse if(enable & (inSeries=={})".format(HRR-1), end='', file=of)
		if Enable_Input_Latch<=0:
			print(" & inReadyC", end='', file=of)
		print(") begin", file=of)
		print("\t\tif(rowSetInNo=={}) begin".format(RS-1), file=of)
		print("\t\t\trowSetInNo <= 0;", file=of)
		print("\t\tend", file=of)
		print("\t\telse begin", file=of)
		if RS==2:
			print("\t\t\trowSetInNo <= 1;", file=of)
		else:
			print("\t\t\trowSetInNo <= rowSetInNo+1;", file=of)
		print("\t\tend", file=of)
		print("\tend", file=of)
		print("end", file=of)
		print(file=of)

	if Enable_OTI_Output_Registers>0 and CS>1:
		print("always @(posedge clk) begin", file=of)
		print("\tif(reset) begin", file=of)
		print("\t\tcolumnSetInNo <= 0;", file=of)
		print("\tend", file=of)
		print("\telse if(enable & (inSeries=={}) & (rowSetInNo={})".format(HRR-1, RS-1), end='', file=of)
		if Enable_Input_Latch<=0:
			print(" & inReadyC", end='', file=of)
		print(") begin", file=of)
		print("\t\tif(columnSetInNo=={}) begin".format(RS-1), file=of)
		print("\t\t\tcolumnSetInNo <= 0;", file=of)
		print("\t\tend", file=of)
		print("\t\telse begin", file=of)
		if CS==2:
			print("\t\t\tcolumnSetInNo <= 1;", file=of)
		else:
			print("\t\t\tcolumnSetInNo <= columnSetInNo+1;", file=of)
		print("\t\tend", file=of)
		print("\tend", file=of)
		print("end", file=of)
		print(file=of)

	#MatrixMatrixMultiply_HR(M, N, Q, PR, PC, NMult, MAMCS, AdderSize, Enable_Column_Latch, Enable_Row_Latch, __Print_To_File)

	IModuleName = "MatrixMatrixMultiply_{}_{}_{}_{}PR_{}PC_S{}E_HRx{}_C{}_A{}_".format(M, N, Q, PR, PC, NMult, HRR, MAMCS, AdderSize)
	if Enable_Column_Latch<=0:
		IModuleName += "N"
	IModuleName += "CL_"
	if Enable_Row_Latch<=0:
		IModuleName += "N"        
	IModuleName += "RL"

	print("wire MMInReady = MMInReadyR | MMInReadyC;", file=of)
	print(IModuleName, file=of)
	print("#(IN_WIDTH, INPUT_REG_DEPTH, MULT_PIPE_DEPTH)", file=of)
	print("MMM(clk, reset, enable,".format(i), file=of)
	if Enable_OTI_Output_Registers<=0:
		print("newInSeriesStart,", file=of)
	else:
		print("MMNewInSeriesStart, //not used", file=of)    
	if Enable_OTI_Output_Registers<=0:
		print("inSeries,", file=of)
	else:
		print("MMInSeries, //not used", file=of)
	if Enable_OTI_Output_Registers<=0:
		print("rowSetInNo,", file=of)
	else:
		print("MMRowSetInNo, //not used", file=of)        
	if Enable_OTI_Output_Registers<=0:
		print("columnSetInNo,", file=of)
	else:
		print("MMColumnSetInNo, //not used", file=of)        
	print("MMInReady,", file=of)
	for j in range(PR):
		for k in range(NMult):
			print("ASE{}R{}, ".format(k, j), end='', file=of)
		print(file=of)
	for j in range(PC):
		for k in range(NMult):
			print("BSE{}C{}, ".format(k, j), end='', file=of)
		print(file=of)

	print(stc1, end='', file=of)
	print(str1, end='', file=of)
	print("EReady,", file=of)
	if ar!=0:
		print(stc1, end='', file=of)
		print(str2, end='', file=of)
		print("EReady,", file=of)
	else:
		print("NUmmEReady1, //not used", file=of)
	if ac!=0:
		print(stc2, end='', file=of)
		print(str1, end='', file=of)
		print("EReady,", file=of)
		if ar!=0:
			print(stc2, end='', file=of)
			print(str2, end='', file=of)
			print("EReady,", file=of)
		else:
			print("NUmmEReady2, //not used", file=of)
	else:
		print("NUmmEReady2, //not used", file=of)
		print("NUmmEReady3, //not used", file=of)

	print("rowSetOutNo,", file=of)
	print("columnSetOutNo,", file=of)
	for i in range(PC):
		for j in range(PR):
			print("EC{}R{}, ".format(i, j), end='',file=of)
	print(file=of)

	print("early", end='', file=of)
	print(stc1, end='', file=of)
	print(str1, end='', file=of)
	print("EReady,", file=of)
	if ar!=0:
		print("early", end='', file=of)
		print(stc1, end='', file=of)
		print(str2, end='', file=of)
		print("EReady,", file=of)
	else:
		print("NUmmeEReady1, //not used", file=of)
	if ac!=0:
		print("early", end='', file=of)
		print(stc2, end='', file=of)
		print(str1, end='', file=of)
		print("EReady,", file=of)
		if ar!=0:
			print("early", end='', file=of)
			print(stc2, end='', file=of)
			print(str2, end='', file=of)
			print("EReady", file=of)
		else:
			print("NUmmeEReady2 //not used", file=of)
	else:
		print("NUmmeEReady2, //not used", file=of)
		print("NUmmeEReady3 //not used", file=of)

	print(");", file=of)
	print(file=of)

	print("endmodule", file=of)

	if __Print_To_File>0:
		of.close()
