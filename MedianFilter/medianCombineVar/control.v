
`timescale 1 ns / 1 ps
 `include "macro.vh"
module control ( Ti, Ti_L, Ti_R, Yi, Z_L, s );

	output  [1:0] s;
	reg [1:0] s;
	input Ti, Ti_L, Ti_R, Yi, Z_L;
	
	always @ (Ti, Ti_L, Ti_R, Yi, Z_L)
	begin	
		case ({Ti, Ti_L, Ti_R, Yi, Z_L})						
			5'b00000, 5'b00100, 5'b11101, 5'b10101:
			begin
				s[1:0] <= 2'b00;												
			end
						
			5'b00101, 5'b10100, 5'b00110, 5'b10110:
			begin
				s[1:0] <= 2'b01;												
			end
													
			5'b11100, 5'b11110:
			begin
				s[1:0] <= 2'b10;												
			end
			
			
			5'b00001, 5'b00010:
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
