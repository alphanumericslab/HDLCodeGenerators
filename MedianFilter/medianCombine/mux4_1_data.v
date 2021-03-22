//-----------------------------------------------------------------------------
//
// Title       : mux2_1
// Design      : median
// Author      : eesa
// Company     : nik
//
//-----------------------------------------------------------------------------
//
// File        : mux2_1.v
// Generated   : Sat Jan  3 16:13:59 2009
// From        : interface description file
// By          : Itf2Vhdl ver. 1.21
//
//-----------------------------------------------------------------------------
//
// Description : 
//
//-----------------------------------------------------------------------------
`timescale 1 ns / 1 ps	  
`include "macro.vh"

//{{ Section below this comment is automatically maintained
//   and may be overwritten
//{module {mux2_1}}
module mux4_1_data ( in1 ,in2 ,in3, in4, out, sel1, sel2 );

	output [`DATA_LENGTH-1:0] out ;
	input [`DATA_LENGTH-1:0] in1 ;
	input [`DATA_LENGTH-1:0] in2 ; 
	input [`DATA_LENGTH-1:0] in3 ;
	input [`DATA_LENGTH-1:0] in4 ; 
	
	input sel1;
	input sel2;
	reg [`DATA_LENGTH-1:0] out;

	always @(in1, in2, in3, in4, sel1, sel2)
	begin 
		case ({sel1,sel2})						
			2'b00:
			begin
				out <= in1;												
			end
			
			2'b01:
			begin 	
				out <= in2;							
			end
			2'b10:
			begin
				out <= in3;
			end	
			2'b11:
			begin
				out <= in4;
			end						
		endcase							
	end

//}} End of automatically maintained section

// -- Enter your statements here -- //

endmodule
