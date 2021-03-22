//-----------------------------------------------------------------------------
//
// Title       : control_rightMst
// Design      : median2013
// Author      : eesa
// Company     : nik
//
//-----------------------------------------------------------------------------
//
// File        : control_leftMst.v
// Generated   : Thu Apr  2 09:59:59 2009
// From        : interface description file
// By          : Itf2Vhdl ver. 1.21
//
//-----------------------------------------------------------------------------
//
// Description : 
//
//-----------------------------------------------------------------------------
`timescale 1 ns / 1 ps

//{{ Section below this comment is automatically maintained
//   and may be overwritten
//{module {control_leftMst}}
module control_leftMst (Ti, Ti_R, Yi, s);

	output  [1:0] s;
	reg [1:0] s;
	input Ti, Ti_R, Yi;
	
	always @ (Ti, Ti_R, Yi)
	begin	
		case ({Ti, Ti_R, Yi})						
			3'b000, 3'b010:
			begin
				s[1:0] <= 2'b00;												
			end
						
			3'b110, 3'b011, 3'b111:
			begin
				s[1:0] <= 2'b01;												
			end
													
			3'b001:
			begin
				s[1:0] <= 2'b11;												
			end
							   
			default:
			begin
				s[1:0] <= 2'b00;
			end
		endcase	
	end 


endmodule
