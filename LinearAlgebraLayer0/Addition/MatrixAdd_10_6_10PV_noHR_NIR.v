`timescale 1ns / 1ps

module MatrixAdd_10_6_10PV_noHR_NIR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
output reg vectorSetInNo = 0,
input inReady,
input signed [IN_WIDTH-1:0] A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, 
A0V1, A1V1, A2V1, A3V1, A4V1, A5V1, 
A0V2, A1V2, A2V2, A3V2, A4V2, A5V2, 
A0V3, A1V3, A2V3, A3V3, A4V3, A5V3, 
A0V4, A1V4, A2V4, A3V4, A4V4, A5V4, 
A0V5, A1V5, A2V5, A3V5, A4V5, A5V5, 
A0V6, A1V6, A2V6, A3V6, A4V6, A5V6, 
A0V7, A1V7, A2V7, A3V7, A4V7, A5V7, 
A0V8, A1V8, A2V8, A3V8, A4V8, A5V8, 
A0V9, A1V9, A2V9, A3V9, A4V9, A5V9, 
input signed [IN_WIDTH-1:0] B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, 
B0V1, B1V1, B2V1, B3V1, B4V1, B5V1, 
B0V2, B1V2, B2V2, B3V2, B4V2, B5V2, 
B0V3, B1V3, B2V3, B3V3, B4V3, B5V3, 
B0V4, B1V4, B2V4, B3V4, B4V4, B5V4, 
B0V5, B1V5, B2V5, B3V5, B4V5, B5V5, 
B0V6, B1V6, B2V6, B3V6, B4V6, B5V6, 
B0V7, B1V7, B2V7, B3V7, B4V7, B5V7, 
B0V8, B1V8, B2V8, B3V8, B4V8, B5V8, 
B0V9, B1V9, B2V9, B3V9, B4V9, B5V9, 
output V0toV9outReady,
output reg VNoutReady1 = 0, //not used
output reg VNoutReady2 = 0, //not used
output reg vectorSetOutNo = 1,
output signed [IN_WIDTH:0] S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, 
S0V1, S1V1, S2V1, S3V1, S4V1, S5V1, 
S0V2, S1V2, S2V2, S3V2, S4V2, S5V2, 
S0V3, S1V3, S2V3, S3V3, S4V3, S5V3, 
S0V4, S1V4, S2V4, S3V4, S4V4, S5V4, 
S0V5, S1V5, S2V5, S3V5, S4V5, S5V5, 
S0V6, S1V6, S2V6, S3V6, S4V6, S5V6, 
S0V7, S1V7, S2V7, S3V7, S4V7, S5V7, 
S0V8, S1V8, S2V8, S3V8, S4V8, S5V8, 
S0V9, S1V9, S2V9, S3V9, S4V9, S5V9, 
output V0toV9earlyOutReady,
output reg CNearlyOutReady1 = 0, //not used
output reg CNearlyOutReady2 = 0 //not used
);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_0(clk, reset, enable,
	inReady,
	A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, 
	B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, 
	V0toV9outReady,
	S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, 
	V0toV9earlyOutReady
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_1(clk, reset, enable,
	inReady,
	A0V1, A1V1, A2V1, A3V1, A4V1, A5V1, 
	B0V1, B1V1, B2V1, B3V1, B4V1, B5V1, 
	VA1OR, //not used
	S0V1, S1V1, S2V1, S3V1, S4V1, S5V1, 
	VA1EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_2(clk, reset, enable,
	inReady,
	A0V2, A1V2, A2V2, A3V2, A4V2, A5V2, 
	B0V2, B1V2, B2V2, B3V2, B4V2, B5V2, 
	VA2OR, //not used
	S0V2, S1V2, S2V2, S3V2, S4V2, S5V2, 
	VA2EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_3(clk, reset, enable,
	inReady,
	A0V3, A1V3, A2V3, A3V3, A4V3, A5V3, 
	B0V3, B1V3, B2V3, B3V3, B4V3, B5V3, 
	VA3OR, //not used
	S0V3, S1V3, S2V3, S3V3, S4V3, S5V3, 
	VA3EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_4(clk, reset, enable,
	inReady,
	A0V4, A1V4, A2V4, A3V4, A4V4, A5V4, 
	B0V4, B1V4, B2V4, B3V4, B4V4, B5V4, 
	VA4OR, //not used
	S0V4, S1V4, S2V4, S3V4, S4V4, S5V4, 
	VA4EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_5(clk, reset, enable,
	inReady,
	A0V5, A1V5, A2V5, A3V5, A4V5, A5V5, 
	B0V5, B1V5, B2V5, B3V5, B4V5, B5V5, 
	VA5OR, //not used
	S0V5, S1V5, S2V5, S3V5, S4V5, S5V5, 
	VA5EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_6(clk, reset, enable,
	inReady,
	A0V6, A1V6, A2V6, A3V6, A4V6, A5V6, 
	B0V6, B1V6, B2V6, B3V6, B4V6, B5V6, 
	VA6OR, //not used
	S0V6, S1V6, S2V6, S3V6, S4V6, S5V6, 
	VA6EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_7(clk, reset, enable,
	inReady,
	A0V7, A1V7, A2V7, A3V7, A4V7, A5V7, 
	B0V7, B1V7, B2V7, B3V7, B4V7, B5V7, 
	VA7OR, //not used
	S0V7, S1V7, S2V7, S3V7, S4V7, S5V7, 
	VA7EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_8(clk, reset, enable,
	inReady,
	A0V8, A1V8, A2V8, A3V8, A4V8, A5V8, 
	B0V8, B1V8, B2V8, B3V8, B4V8, B5V8, 
	VA8OR, //not used
	S0V8, S1V8, S2V8, S3V8, S4V8, S5V8, 
	VA8EOR //not used
	);

VectorAdd_6_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_9(clk, reset, enable,
	inReady,
	A0V9, A1V9, A2V9, A3V9, A4V9, A5V9, 
	B0V9, B1V9, B2V9, B3V9, B4V9, B5V9, 
	VA9OR, //not used
	S0V9, S1V9, S2V9, S3V9, S4V9, S5V9, 
	VA9EOR //not used
	);

endmodule
