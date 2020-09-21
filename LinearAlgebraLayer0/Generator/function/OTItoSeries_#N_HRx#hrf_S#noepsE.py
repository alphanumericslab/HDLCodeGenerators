def OTItoSeries_N_HRxhrf_SnoepsE(N, HRR, NOEPS, Enable_Output_Registers, Enable_Input_Latch, IN_WIDTH, __Print_To_File):

	import sys
	import math
		
	if N<2:
		N=2
	
	if HRR<2:
		HRR=2
	elif HRR>N:
		HRR=N
	
	NOEPSMin = math.ceil(N/HRR)
	NOEPSMax = math.ceil(N/(HRR-1))-1
	NOEPSMM = (NOEPSMin!=NOEPSMax)	
	if NOEPSMM:
		if NOEPS<NOEPSMin:
			NOEPS=NOEPSMin
		elif NOEPS>NOEPSMax:
			NOEPS=NOEPSMax
	else:
		NOEPS=NOEPSMin
	
	if IN_WIDTH<1:
		IN_WIDTH=1

	ModuleName="OTItoSeries_{}_HRx{}_S{}E_".format(N, HRR, NOEPS)
	if Enable_Output_Registers<=0:
		ModuleName+="N"
	ModuleName+="OR_"
	if Enable_Input_Latch<=0:
		ModuleName+="N"
	ModuleName+="IL"
		
	if __Print_To_File<=0:
		of=sys.stdout
	else:
		of=open("./"+ModuleName+".v", 'w+')
	
	print("`timescale 1ns / 1ps\n", file=of)
	print("module "+ModuleName, file=of)
	print("#(", file=of)
	print("parameter IN_WIDTH = {}".format(IN_WIDTH), file=of)
	print(")(", file=of)
	print("input clk, reset, enable,", file=of)
	print("output reg readyForNewDataSeries = 1,", file=of)
	lghrr=math.ceil(math.log2(HRR))
	if lghrr==1:
		print("output reg inSeries = 0,", file=of)
	elif lghrr>1:
		print("output reg [{}:0] inSeries = 0,".format(lghrr-1), file=of)
	print("input inReady,", file=of)
	print("input signed [IN_WIDTH-1:0] ", end='', file=of)
	for i in range(N):
		print("A{}, ".format(i), end='', file=of)
	print(file=of)
	NE = N - NOEPS*(HRR-1)
	while NE<=0:
		NE += NOEPS
	if NE==1:
		print("output reg O0outReady,", file=of)
	else:
		print("output reg O0toO{}outReady,".format(NE-1), file=of)
	if NE==NOEPS:
		print("output reg ONoutReady=0, //not used", file=of)
	elif NE==NOEPS-1:
		print("output reg O{}outReady,".format(NE), file=of)
	else:
		print("output reg O{}toO{}outReady,".format(NE, NOEPS-1), file=of)
	print("output ", end='', file=of)
	if Enable_Output_Registers>0:
		print("reg ", end='', file=of)
	if lghrr==1:
		print("outSeries", end='', file=of)
	elif lghrr>1:
		print("[{}:0] outSeries".format(lghrr-1),  end='', file=of)
	if Enable_Output_Registers>0:
		print(" = {},".format(HRR-1), file=of)
	else:
		print(",", file=of)
	print("output reg signed [IN_WIDTH-1:0] ", end='', file=of)
	for i in range(NOEPS-1):
		print("O{}, ".format(i), end='', file=of)
	print("O{}".format(NOEPS-1), end='', file=of)
	if Enable_Output_Registers>0:
		print(",", file=of)
		if NE==1:
			print("output O0earlyOutReady,", file=of)
		else:
			print("output O0toO{}earlyOutReady,".format(NE-1), file=of)
		if NE==NOEPS:
			print("output reg ONearlyOutReady=0 //not used", file=of)
		elif NE==NOEPS-1:
			print("output O{}earlyOutReady".format(NE), file=of)
		else:
			print("output O{}toO{}earlyOutReady".format(NE, NOEPS-1), file=of)
	else:
		print(file=of)
	print(");\n", file=of)
	
	print("always @(posedge clk) begin", file=of)
	print("\tif(reset) begin", file=of)
	print("\t\treadyForNewDataSeries <= 1;", file=of)
	print("\t\tinSeries <= 0;", file=of)
	print("\tend", file=of)
	print("\telse if(enable) begin", file=of)
	print("\t\tif(inSeries==0) begin", file=of)
	print("\t\t\tif(inReady) begin", file=of)
	print("\t\t\t\treadyForNewDataSeries <= 0;", file=of)
	print("\t\t\t\tinSeries <= 1;", file=of)
	print("\t\t\tend", file=of)
	print("\t\tend", file=of)
	print("\t\telse", end='', file=of)
	if HRR>2:
		print(" if(inSeries=={})".format(HRR-1), end='', file=of)
	print(" begin", file=of)
	print("\t\t\treadyForNewDataSeries <= 1;", file=of)
	print("\t\t\tinSeries <= 0;", file=of)
	print("\t\tend", file=of)
	if HRR>2:
		print("\t\telse begin", file=of)
		if HRR==3:
			print("\t\t\tinSeries <= 2;", file=of)        
		else:
			print("\t\t\tinSeries <= inSeries+1;", file=of)
		print("\t\tend", file=of)
	print("\tend", file=of)
	print("end", file=of)
	print(file=of)
	
	if Enable_Input_Latch>0:
		print("reg signed [IN_WIDTH-1:0] ", end='', file=of)
		for i in range(NOEPS,N-1):
			print("A{}l, ".format(i), end='', file=of)
		print("A{}l;".format(N-1), file=of)
		print("always @(posedge clk) begin", file=of)
		print("\tif(enable) begin", file=of)
		print("\t\tif(inReady & (inSeries==0)) begin", file=of)
		print("\t\t\t", end='', file=of)
		for i in range(NOEPS,N):
			print("A{}l <= A{}; ".format(i, i), end='', file=of)
		print("\n\t\tend", file=of)
		print("\tend", file=of)
		print("end", file=of)
		print(file=of)
	
	#(inSeries==0) ? inReady : 1
	print("wire eOR = ((inSeries==0) & inReady) | (inSeries!=0);", file=of)
	if NE!=NOEPS:
		if HRR==2:
			print("wire eORC = (inSeries==0) & inReady;".format(HRR-1), file=of)
		elif HRR==3:
			print("wire eORC = ((inSeries==0) & inReady) | (inSeries==1);".format(HRR-1), file=of)
		else:
			print("wire eORC = ((inSeries==0) & inReady) | ((inSeries!=0) & (inSeries!={}));".format(HRR-1), file=of)
	
	if Enable_Output_Registers>0:
		print("always @(posedge clk) if(enable) begin", file=of)
	else:
		print("always @(*) begin", file=of)
	print("\tcase(outSeries)", file=of)
	for i in range(HRR):
		if i==0:
			print("\t\t0: ", end='', file=of)
			if Enable_Output_Registers>0:
				print("if(inReady) ", end='', file=of)
			print("begin", file=of) #currently more power efficient, can use "begin" for simpler hardware
		else:
			print("\t\t{}: begin".format(i), file=of)
		print("\t\t\t", end='', file=of)
		for j in range(NOEPS):
			print("O{} ".format(j), end='', file=of)
			if Enable_Output_Registers>0:
				print("<", end='', file=of)
			k=NOEPS*i+j
			if k >= N:
				print("= 0; ", end='', file=of)
			elif k<NOEPS:
				print("= A{}; ".format(k), end='', file=of)
			else:
				print("= A{}".format(k), end='', file=of)
				if Enable_Input_Latch>0:
					print("l", end='', file=of)
				print("; ", end='', file=of)
		print(file=of)
		print("\t\tend", file=of)
	print("\tendcase", file=of)
	
	if NE==1:
		print("\tO0outReady ", end='', file=of)
	else:
		print("\tO0toO{}outReady ".format(NE-1), end='', file=of)
	if Enable_Output_Registers>0:
		print("<", end='', file=of)
	print("= eOR;", file=of)
	
	if NE!=NOEPS:
		if NE==NOEPS-1:
			print("\tO{}outReady ".format(NE), end='', file=of)
		else:
			print("\tO{}toO{}outReady ".format(NE, NOEPS-1), end='', file=of)
		if Enable_Output_Registers>0:
			print("<", end='', file=of)
		print("= eORC;", file=of)
	print("end\n", file=of)
	
	if Enable_Output_Registers>0:
		if NE==1:
			print("assign O0earlyOutReady = eOR;", file=of)
		else:
			print("assign O0toO{}earlyOutReady = eOR;".format(NE-1), file=of)
		if NE==NOEPS-1:
			print("assign O{}earlyOutReady = eORC;".format(NE), file=of)
		elif NE!=NOEPS:
			print("assign O{}toO{}earlyOutReady = eORC;".format(NE, NOEPS-1), file=of)
	
	if Enable_Output_Registers<=0:
		print("assign outSeries = inSeries;", file=of)
	else:
		print("always @(posedge clk) begin", file=of)
		print("\tif(reset) begin", file=of)
		print("\t\toutSeries <= {};".format(HRR-1), file=of)
		print("\tend", file=of)
		print("\telse if(enable) begin", file=of)
		print("\t\tif((outSeries==0) & inReady) begin", file=of)
		print("\t\t\toutSeries <= 1;", file=of)
		print("\t\tend", file=of)
		print("\t\telse", end='', file=of)
		if HRR>2:
			print(" if(outSeries=={})".format(HRR-1), end='', file=of)
		print(" begin", file=of)
		print("\t\t\toutSeries <= 0;", file=of)
		print("\t\tend", file=of)
		if HRR>2:
			print("\t\telse begin", file=of)
			if HRR==3:
				print("\t\t\toutSeries <= 2;", file=of)        
			else:
				print("\t\t\toutSeries <= outSeries+1;", file=of)
			print("\t\tend", file=of)
		print("\tend", file=of)
		print("end", file=of)
		print(file=of)
	
	print("endmodule", file=of)
	
	if __Print_To_File>0:
		of.close()
