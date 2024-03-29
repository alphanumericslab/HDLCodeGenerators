`timescale 1ns / 1ps

module VectorAdd_12_SI_HRx1_NIR_NIL
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
output readyForNewDataSeries,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, 
input signed [IN_WIDTH-1:0] B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, 
output S0toS11outReady,
output SNoutReady, //not used
output signed [IN_WIDTH:0] S0, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, 
output S0toS11earlyOutReady,
output SNearlyOutReady //not used
);

wire addInReady;
wire signed [IN_WIDTH-1:0] AS0, AS1, AS2, AS3, AS4, AS5, AS6, AS7, AS8, AS9, AS10, AS11;
SItoSoE_12_HRx1_NIL_NOR #( .IN_WIDTH(IN_WIDTH) )
SItSoEA(
clk, reset, enable,
readyForNewDataSeries,
inSeries,
inReady,
A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, 
addInReady,
AS2outReady, //not used
AoutSeries, //not used
AS0, AS1, AS2, AS3, AS4, AS5, AS6, AS7, AS8, AS9, AS10, AS11, 
AS1earlyOutReady, //not used
AS2earlyOutReady //not used
);

wire signed [IN_WIDTH-1:0] BS0, BS1, BS2, BS3, BS4, BS5, BS6, BS7, BS8, BS9, BS10, BS11;
SItoSoE_12_HRx1_NIL_NOR #( .IN_WIDTH(IN_WIDTH) )
SItSoEB(
clk, reset, enable,
NISSB, //not used
ISB, //not used
inReady,
B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, 
ORB, //not used
BS2outReady, //not used
BoutSeries, //not used
BS0, BS1, BS2, BS3, BS4, BS5, BS6, BS7, BS8, BS9, BS10, BS11, 
BS1earlyOutReady, //not used
BS2earlyOutReady //not used
);

VectorAdd_12_S12E_HRx1_NIR#( .IN_WIDTH(IN_WIDTH) )
VAE(
clk, reset, enable,
ARFNDS, //not used
AIS, //not used
addInReady,
AS0, AS1, AS2, AS3, AS4, AS5, AS6, AS7, AS8, AS9, AS10, AS11, 
BS0, BS1, BS2, BS3, BS4, BS5, BS6, BS7, BS8, BS9, BS10, BS11, 
S0toS11outReady,
SNoutReady, //not used
outSeries,
S0, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, 
S0toS11earlyOutReady,
SNearlyOutReady //not used
);

endmodule
