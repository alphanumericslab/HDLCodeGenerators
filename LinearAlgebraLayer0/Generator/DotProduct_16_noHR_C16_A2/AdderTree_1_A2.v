`timescale 1ns / 1ps

module AdderTree_1_A2
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A0, 
output outReady,
output signed [IN_WIDTH+-1:0] out,
output earlyOutReady
);

endmodule
