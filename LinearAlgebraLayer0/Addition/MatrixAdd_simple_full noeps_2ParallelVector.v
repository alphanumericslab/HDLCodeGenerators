`timescale 1ns / 1ps

module MatrixAddTest
#(
parameter IN_WIDTH = 16
)(
input clk, reset, enable,
output reg [2:0] vectorSetInNo = 0,
input inReady,
input signed [IN_WIDTH-1:0] A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, A6V0, A7V0, A8V0, A9V0, A10V0, A11V0, 
A0V1, A1V1, A2V1, A3V1, A4V1, A5V1, A6V1, A7V1, A8V1, A9V1, A10V1, A11V1, 
input signed [IN_WIDTH-1:0] B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, B6V0, B7V0, B8V0, B9V0, B10V0, B11V0, 
B0V1, B1V1, B2V1, B3V1, B4V1, B5V1, B6V1, B7V1, B8V1, B9V1, B10V1, B11V1, 
output V0toV1outReady,
output reg VNoutReady1 = 0, //not used
output reg VNoutReady2 = 0, //not used
output reg [2:0] vectorSetOutNo = 4,
output signed [IN_WIDTH:0] S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, S6V0, S7V0, S8V0, S9V0, S10V0, S11V0, 
S0V1, S1V1, S2V1, S3V1, S4V1, S5V1, S6V1, S7V1, S8V1, S9V1, S10V1, S11V1, 
output V0toV1earlyOutReady,
output reg CNearlyOutReady1 = 0, //not used
output reg CNearlyOutReady2 = 0 //not used
);

always @(posedge clk) begin
	if(reset) begin
		vectorSetInNo <= 0;
	end
	else if(enable) begin
		if(inReady) begin
			if(vectorSetInNo==4) begin
				vectorSetInNo <= 0;
			end
			else begin
				vectorSetInNo <= vectorSetInNo+1;
			end
		end
	end
end

VectorAdd_12_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_0(clk, reset, enable,
	inReady,
	A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, A6V0, A7V0, A8V0, A9V0, A10V0, A11V0, 
	B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, B6V0, B7V0, B8V0, B9V0, B10V0, B11V0, 
	V0toV1outReady,
	S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, S6V0, S7V0, S8V0, S9V0, S10V0, S11V0, 
	V0toV1earlyOutReady
	);

VectorAdd_12_noHR_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_1(clk, reset, enable,
	inReady,
	A0V1, A1V1, A2V1, A3V1, A4V1, A5V1, A6V1, A7V1, A8V1, A9V1, A10V1, A11V1, 
	B0V1, B1V1, B2V1, B3V1, B4V1, B5V1, B6V1, B7V1, B8V1, B9V1, B10V1, B11V1, 
	VA1OR, //not used
	S0V1, S1V1, S2V1, S3V1, S4V1, S5V1, S6V1, S7V1, S8V1, S9V1, S10V1, S11V1, 
	VA1EOR //not used
	);

always @(posedge clk) begin
	if(reset) begin
		vectorSetOutNo <= 4;
	end
	else if(enable) begin
		if(V0toV1earlyOutReady) begin
			if(vectorSetOutNo==4) begin
				vectorSetOutNo <= 0;
			end
			else begin
				vectorSetOutNo <= vectorSetOutNo+1;
			end
		end
	end
end

endmodule
