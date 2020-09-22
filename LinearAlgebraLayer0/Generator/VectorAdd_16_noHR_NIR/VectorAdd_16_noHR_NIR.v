`timescale 1ns / 1ps

module VectorAdd_16_noHR_NIR
#(
parameter IN_WIDTH = 14
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, A12, A13, A14, A15, 
input signed [IN_WIDTH-1:0] B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, 
output outReady,
output signed [IN_WIDTH:0] S0, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14, S15, 
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

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add2 (clk, reset, enable,
inReady,
A2, B2,
aor2, //not used
S2,
aeor2); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add3 (clk, reset, enable,
inReady,
A3, B3,
aor3, //not used
S3,
aeor3); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add4 (clk, reset, enable,
inReady,
A4, B4,
aor4, //not used
S4,
aeor4); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add5 (clk, reset, enable,
inReady,
A5, B5,
aor5, //not used
S5,
aeor5); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add6 (clk, reset, enable,
inReady,
A6, B6,
aor6, //not used
S6,
aeor6); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add7 (clk, reset, enable,
inReady,
A7, B7,
aor7, //not used
S7,
aeor7); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add8 (clk, reset, enable,
inReady,
A8, B8,
aor8, //not used
S8,
aeor8); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add9 (clk, reset, enable,
inReady,
A9, B9,
aor9, //not used
S9,
aeor9); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add10 (clk, reset, enable,
inReady,
A10, B10,
aor10, //not used
S10,
aeor10); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add11 (clk, reset, enable,
inReady,
A11, B11,
aor11, //not used
S11,
aeor11); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add12 (clk, reset, enable,
inReady,
A12, B12,
aor12, //not used
S12,
aeor12); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add13 (clk, reset, enable,
inReady,
A13, B13,
aor13, //not used
S13,
aeor13); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add14 (clk, reset, enable,
inReady,
A14, B14,
aor14, //not used
S14,
aeor14); //not used

Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add15 (clk, reset, enable,
inReady,
A15, B15,
aor15, //not used
S15,
aeor15); //not used

endmodule
