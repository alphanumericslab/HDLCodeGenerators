																		  //				macros:
//					"DATA_LENGTH" , "LOG_WMAX" and "WMAX" are  macros(global constant) that is defined in "macro.vh" 
//					and you can run "script.py" and create "top.v" and "macro.vh" and set  their values .		
//					"DATA_LENGTH" determine length of each eleman of input signal for example 32 bits
//					"WMAX" is maximum size of median window that you can set it when run "script.py" and henceforth you just set input W less than WMAX.
//					"LOG_WMAX" is logarithm of WMAX that use for  determine number of bits that has needed for counter. 
//-----------------------------------------------------------------------------
`timescale 1 ns / 1 ps	  
`include "macro.vh"

//R1 is registar that store the input data in descending order.
//R2 is registar that store input data in order which data is entered.
module medianCell_leftMst(X, clk, reset, W, isMedian, R1_R, R_old_in, T_R, R1, R2, R_median, R_old_out, Z, T, isLast);
	
	input [`DATA_LENGTH-1:0]  X, R1_R, R_old_in;
	input [`LOG_WMAX-1:0] W;
	input 	clk, reset, isMedian;
	input	T_R;
	
	
	output 	Z, T, isLast;	  
	output 	[`DATA_LENGTH -1:0] R1, R2, R_median, R_old_out;
		
	wire [`DATA_LENGTH-1:0] muxData_R; 	
		
	wire [1:0] S;
		
	reg  [`DATA_LENGTH-1:0] R1, R2;
	//reg  isLast;
		
	data_comparator dataCompOld(R_old_in, R1, Z);											 
	//assign Z = (Z_L | Y);
	
	mux4_1_data muxData( .in1(R1), .in2(X), .in3(`DATA_LENGTH'b0), .in4(R1_R), .out(muxData_R), .sel1(S[1:1]), .sel2(S[0:0]));
		
	control_leftMst ctr(.Ti(T), .Ti_R(T_R), .Yi(Z), .s(S));
	
	data_comparator dataComp(X, R1, T);
	
	///////
	assign isLast = (W==`LOG_WMAX'd1)?1:0;
	assign R_median = (isMedian)?R1: `HIGH_Z;
	assign R_old_out = (isLast)?R2: `HIGH_Z;
	
	always @(posedge clk)
	begin
		if(reset)
		begin
			R1 <= 0;		
			R2 <= 0;   
			//isLast <= (W==`LOG_WMAX'd1)?1:0;
		end
		else
		begin			
			R1 <= muxData_R;				
			R2 <= X;
		end
	end

endmodule		
							
			
			
			
			
			
			
				
							
						
					
					