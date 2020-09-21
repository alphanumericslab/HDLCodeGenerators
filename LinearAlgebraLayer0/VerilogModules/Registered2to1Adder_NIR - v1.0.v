`timescale 1ns / 1ps

module Registered2to1Adder_NIR
#(
parameter IN_WIDTH = 10
)(
input clk, enable, enAd,
input signed [IN_WIDTH-1:0] I0, I1, 
output reg signed [IN_WIDTH:0] Out
);

always @(posedge clk) begin
	if(enable) begin
		if(enAd) begin
			Out <= I0+I1;
		end
	end
end

endmodule
