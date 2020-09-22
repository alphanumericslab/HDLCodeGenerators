`timescale 1ns / 1ps

module MatrixAddTest
#(
parameter IN_WIDTH = 16
)(
input clk, reset, enable,
output readyForNewVectorStart,
output vectorInSeries,
output reg [3:0] vectorSetInNo = 0,
input inReady,
input signed [IN_WIDTH-1:0] A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, 
input signed [IN_WIDTH-1:0] B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, 
output V0S0toS5outReady,
output VSNoutReady1, //not used
output reg VNoutReady1 = 0, //not used
output reg VNoutReady2 = 0, //not used
output vectorOutSeries,
output reg [3:0] vectorSetOutNo = 9,
output signed [IN_WIDTH:0] S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, 
output V0S0toS5earlyOutReady,
output CSNearlyOutReady1, //not used
output reg CNearlyOutReady1 = 0, //not used
output reg CNearlyOutReady2 = 0 //not used
);

always @(posedge clk) begin
	if(reset) begin
		vectorSetInNo <= 0;
	end
	else if(enable) begin
		if(inReady) begin
			if(vectorInSeries==1) begin
				if(vectorSetInNo==9) begin
					vectorSetInNo <= 0;
				end
				else begin
					vectorSetInNo <= vectorSetInNo+1;
				end
			end
		end
	end
end

VectorAdd_12_S6E_HRx2_NIR#( .IN_WIDTH(IN_WIDTH) )
	VA_0(clk, reset, enable,
	readyForNewVectorStart,
	vectorInSeries,
	inReady,
	A0V0, A1V0, A2V0, A3V0, A4V0, A5V0, 
	B0V0, B1V0, B2V0, B3V0, B4V0, B5V0, 
	V0S0toS5outReady, VSNoutReady1, //not used
	vectorOutSeries,
	S0V0, S1V0, S2V0, S3V0, S4V0, S5V0, 
	V0S0toS5earlyOutReady, VSNearlyOutReady1 //not used
	);

always @(posedge clk) begin
	if(reset) begin
		vectorSetOutNo <= 9;
	end
	else if(enable) begin
		if(V0S0toS5earlyOutReady) begin
			if(vectorOutSeries==1) begin
				if(vectorSetOutNo==9) begin
					vectorSetOutNo <= 0;
				end
				else begin
					vectorSetOutNo <= vectorSetOutNo+1;
				end
			end
		end
	end
end

endmodule
