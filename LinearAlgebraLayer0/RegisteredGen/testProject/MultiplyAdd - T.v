`timescale 1ns / 1ps

module
MultiplyAddT
#(
parameter unsigned IN_M_WIDTH=10//comment  0
,parameter IN_A_WIDTH = 20,OUT_WIDTH /*comment1   */= 21,
parameter [3:0] INPUT_REG_DEPTH = 0, // comment2
parameter MULT_PIPE_DEPTH=0)(input clk, reset , enable,
input inReady,
input signed/*    comment3*/[IN_M_WIDTH-1:0] input_A,B,
input /**/signed[IN_A_WIDTH-1:0]_C,
output unsigned outReady,/*     comment4     ,,,,,,,,[salam:1]*/
output reg signed [OUT_WIDTH-1:0] signedRES=/*//comment5*/0,
output/**/earlyOutReady,
output reg unsigned [1:0] oa=0, ob=1, oc,/*//comm/*ent6//*/
output [2*(OUT_WIDTH+3):6] t
);

reg outReadyR = 0;
always @(posedge clk) begin
	if(reset) begin
		outReadyR <= 0;
	end
	else if(enable) begin
		if(inReady) begin
			signedRES <= _C + (input_A * B);	
		outReadyR <= inReady;
		end
	end
end
assign outReady = outReadyR;
assign earlyOutReady = inReady;

endmodule
