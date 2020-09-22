`timescale 1ns / 1ps

module DotProduct_16_noHR_C16_A2
#(
parameter IN_WIDTH = 14,
parameter INPUT_REG_DEPTH = 1,
parameter MULT_PIPE_DEPTH = 1
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, A12, A13, A14, A15, 
input signed [IN_WIDTH-1:0] B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, 
output outReady,
output signed [(2*IN_WIDTH)+3:0] DP,
output earlyOutReady
);

wire DPOutReady;
wire signed [(2*IN_WIDTH)+3:0] DPpart0;
DotProduct_Systolic_16#( .IN_WIDTH(IN_WIDTH), .INPUT_REG_DEPTH(INPUT_REG_DEPTH), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
	DPS16_0(clk, reset, enable,
	inReady,
	A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, A12, A13, A14, A15, 
	B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, 
	DPOutReady,
	DPpart0,
	DPEarlyOutReady0); //not used

AdderTree_1_A2#( .IN_WIDTH((2*IN_WIDTH)+4) )
	AT(clk, reset, enable, DPOutReady,
	DPpart0, 
	outReady, DP, earlyOutReady);

endmodule
