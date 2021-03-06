`timescale 1ns / 1ps

module VectorAdd_2_noHR_NIR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, 
input signed [IN_WIDTH-1:0] B0, B1, 
output outReady,
output signed [IN_WIDTH:0] S0, S1, 
output earlyOutReady
);

//(* use_dsp48 = "yes" *) //yes, no, AutoMax, Auto
Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add0 (clk, reset, enable,
inReady,
A0, B0,
OutReady,
S0,
earlyOutReady);

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add1 (clk, reset, enable,
inReady,
A1, B1,
aor1, //not used
S1,
aeor1); //not used

endmodule
