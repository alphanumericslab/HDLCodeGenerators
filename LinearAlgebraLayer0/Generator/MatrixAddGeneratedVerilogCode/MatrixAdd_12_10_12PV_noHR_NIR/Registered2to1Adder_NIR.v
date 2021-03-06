`timescale 1ns / 1ps

module Registered2to1Adder_NIR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] I0, I1, 
output reg outReady = 0,
output reg signed [IN_WIDTH:0] out,
output earlyOutReady
);

always @(posedge clk) begin
	if(reset) begin
		outReady <= 0;
	end
	else if(enable) begin
		outReady <= inReady;
		if(inReady) begin
			out <= I0+I1;
		end
	end
end

assign earlyOutReady = inReady;

endmodule
