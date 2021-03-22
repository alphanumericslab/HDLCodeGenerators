`timescale 1 ns / 1 ps	  
`include "macro.vh"

module data_comparator(data_in1, data_in2, T);
		
	input [`DATA_LENGTH-1:0] data_in1, data_in2;
	output T;									 
	assign T = ( data_in2 <= data_in1) ? 1'b1: 1'b0;	 	
	
endmodule

	