`timescale 1ns / 1ps

module MatrixAddTest
#(
parameter IN_WIDTH = 16
)(
input clk, reset, enable,
output reg [3:0] vectorSetInNo = 0,
input inReady,
input signed [IN_WIDTH-1:0] A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, A6V0, A7V0, A8V0, A9V0, A10V0, A11V0, 
input signed [IN_WIDTH-1:0] B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, B6V0, B7V0, B8V0, B9V0, B10V0, B11V0, 
output V0outReady,
output reg VNoutReady1 = 0, //not used
output reg VNoutReady2 = 0, //not used
output reg [3:0] vectorSetOutNo = 9,
output signed [IN_WIDTH:0] S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, S6V0, S7V0, S8V0, S9V0, S10V0, S11V0, 
output V0earlyOutReady,
output reg CNearlyOutReady1 = 0, //not used
output reg CNearlyOutReady2 = 0 //not used
);

always @(posedge clk) begin
	if(reset) begin
		vectorSetInNo <= 0;
	end
	else if(enable) begin
		if(inReady) begin
			if(vectorSetInNo==9) begin
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
	V0outReady,
	S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, S6V0, S7V0, S8V0, S9V0, S10V0, S11V0, 
	V0earlyOutReady
	);

always @(posedge clk) begin
	if(reset) begin
		vectorSetOutNo <= 9;
	end
	else if(enable) begin
		if(V0earlyOutReady) begin
			if(vectorSetOutNo==9) begin
				vectorSetOutNo <= 0;
			end
			else begin
				vectorSetOutNo <= vectorSetOutNo+1;
			end
		end
	end
end

endmodule
