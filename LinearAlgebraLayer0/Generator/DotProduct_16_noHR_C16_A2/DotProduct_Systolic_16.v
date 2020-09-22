`timescale 1ns / 1ps

module DotProduct_Systolic_16
#(
parameter IN_WIDTH = 10,
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

wire signed [(2*IN_WIDTH)-1:0] M0;
RegisteredMultiplier #(.IN_WIDTH(IN_WIDTH),
.INPUT_REG_DEPTH(INPUT_REG_DEPTH),
.MULT_PIPE_DEPTH(MULT_PIPE_DEPTH))
Mult0(clk, reset, enable,
inReady,
A0, B0,
MOR0, //not used
M0,
eMOR0); //not used

wire signed [2*IN_WIDTH:0] psl1 [0:0];
wire signed [(2*IN_WIDTH)+1:0] psl2 [0:1];
wire signed [(2*IN_WIDTH)+2:0] psl3 [0:3];
wire signed [(2*IN_WIDTH)+3:0] psl4 [0:7];

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH(2*IN_WIDTH), .OUT_WIDTH((2*IN_WIDTH)+1), .INPUT_REG_DEPTH(INPUT_REG_DEPTH), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA1(clk, reset, enable,
inReady,
A1, B1,
M0,
OR1, //not used
psl1[0],
eOR1); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+1), .OUT_WIDTH((2*IN_WIDTH)+2), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+1), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA2(clk, reset, enable,
inReady,
A2, B2,
psl1[0],
OR2, //not used
psl2[0],
eOR2); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+2), .OUT_WIDTH((2*IN_WIDTH)+2), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+2), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA3(clk, reset, enable,
inReady,
A3, B3,
psl2[0],
OR3, //not used
psl2[1],
eOR3); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+2), .OUT_WIDTH((2*IN_WIDTH)+3), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+3), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA4(clk, reset, enable,
inReady,
A4, B4,
psl2[1],
OR4, //not used
psl3[0],
eOR4); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+3), .OUT_WIDTH((2*IN_WIDTH)+3), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+4), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA5(clk, reset, enable,
inReady,
A5, B5,
psl3[0],
OR5, //not used
psl3[1],
eOR5); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+3), .OUT_WIDTH((2*IN_WIDTH)+3), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+5), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA6(clk, reset, enable,
inReady,
A6, B6,
psl3[1],
OR6, //not used
psl3[2],
eOR6); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+3), .OUT_WIDTH((2*IN_WIDTH)+3), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+6), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA7(clk, reset, enable,
inReady,
A7, B7,
psl3[2],
OR7, //not used
psl3[3],
eOR7); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+3), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+7), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA8(clk, reset, enable,
inReady,
A8, B8,
psl3[3],
OR8, //not used
psl4[0],
eOR8); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+8), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA9(clk, reset, enable,
inReady,
A9, B9,
psl4[0],
OR9, //not used
psl4[1],
eOR9); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+9), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA10(clk, reset, enable,
inReady,
A10, B10,
psl4[1],
OR10, //not used
psl4[2],
eOR10); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+10), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA11(clk, reset, enable,
inReady,
A11, B11,
psl4[2],
OR11, //not used
psl4[3],
eOR11); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+11), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA12(clk, reset, enable,
inReady,
A12, B12,
psl4[3],
OR12, //not used
psl4[4],
eOR12); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+12), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA13(clk, reset, enable,
inReady,
A13, B13,
psl4[4],
OR13, //not used
psl4[5],
eOR13); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+13), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA14(clk, reset, enable,
inReady,
A14, B14,
psl4[5],
OR14, //not used
psl4[6],
eOR14); //not used

MultiplyAdd #( .IN_M_WIDTH(IN_WIDTH), .IN_A_WIDTH((2*IN_WIDTH)+4), .OUT_WIDTH((2*IN_WIDTH)+4), .INPUT_REG_DEPTH(INPUT_REG_DEPTH+14), .MULT_PIPE_DEPTH(MULT_PIPE_DEPTH) )
MA15(clk, reset, enable,
inReady,
A15, B15,
psl4[6],
outReady,
psl4[7],
earlyOutReady);

assign DP = psl4[7];

endmodule
