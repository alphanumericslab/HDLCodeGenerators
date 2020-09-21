`timescale 1ns / 1ps

module Registered2to1Adder_IR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable, inReady,
input signed [IN_WIDTH-1:0] I0, I1,
output reg outReady = 0,
output reg signed [IN_WIDTH:0] out,
output reg earlyOutReady = 0
);

reg signed [IN_WIDTH-1:0] I0R, I1R;
always @(posedge clk) begin
	if(reset) begin
		earlyOutReady <= 0;
		outReady <= 0;
		//out <= 0;
	end
	else if(enable) begin
		earlyOutReady <= inReady;
		outReady <= earlyOutReady;
		if(inReady) begin
			I0R <= I0;
			I1R <= I1;
		end
		if(earlyOutReady) begin
			out <= I0R+I1R;
		end
	end
end

endmodule
