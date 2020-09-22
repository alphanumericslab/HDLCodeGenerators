`timescale 1ns / 1ps

module VectorAdd_10_SI_HRx2_NIR_NIL
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
output readyForNewDataSeries,
output inSeries,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, 
input signed [IN_WIDTH-1:0] B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, 
output S0toS4outReady,
output SNoutReady, //not used
output outSeries,
output signed [IN_WIDTH:0] S0, S1, S2, S3, S4, 
output S0toS4earlyOutReady,
output SNearlyOutReady //not used
);

wire addInReady;
wire signed [IN_WIDTH-1:0] AS0, AS1, AS2, AS3, AS4;
SItoSoE_10_HRx2_NIL_NOR #( .IN_WIDTH(IN_WIDTH) )
SItSoEA(
clk, reset, enable,
readyForNewDataSeries,
inSeries,
inReady,
A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, 
addInReady,
AS2outReady, //not used
AoutSeries, //not used
AS0, AS1, AS2, AS3, AS4, 
AS1earlyOutReady, //not used
AS2earlyOutReady //not used
);

wire signed [IN_WIDTH-1:0] BS0, BS1, BS2, BS3, BS4;
SItoSoE_10_HRx2_NIL_NOR #( .IN_WIDTH(IN_WIDTH) )
SItSoEB(
clk, reset, enable,
NISSB, //not used
ISB, //not used
inReady,
B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, 
ORB, //not used
BS2outReady, //not used
BoutSeries, //not used
BS0, BS1, BS2, BS3, BS4, 
BS1earlyOutReady, //not used
BS2earlyOutReady //not used
);

VectorAdd_10_S5E_HRx2_NIR#( .IN_WIDTH(IN_WIDTH) )
VAE(
clk, reset, enable,
ARFNDS, //not used
AIS, //not used
addInReady,
AS0, AS1, AS2, AS3, AS4, 
BS0, BS1, BS2, BS3, BS4, 
S0toS4outReady,
SNoutReady, //not used
outSeries,
S0, S1, S2, S3, S4, 
S0toS4earlyOutReady,
SNearlyOutReady //not used
);

endmodule
