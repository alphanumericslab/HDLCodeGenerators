`timescale 1ns / 1ps

module SItoSoE_10_HRx1_NIL_NOR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
output reg newInSeriesStart = 1,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, 
output O0toO9OutReady,
output reg ONOutReady=0, //not used
output ,
output reg signed [IN_WIDTH-1:0] O0, O1, O2, O3, O4, O5, O6, O7, O8, O9, 
output reg O0toO9earlyOutReady = 0, //not used
output reg ONearlyOutReady=0 //not used
);

assign O0toO9OutReady = inReady;
always @(posedge clk) begin
	if(reset) begin
		newInSeriesStart <= 1;
		inSeries <= 0;
	end
	else if(enable & inReady) begin
		if(inSeries==0) begin
			newInSeriesStart <= 1;
			inSeries <= 0;
		end
		else begin
			newInSeriesStart <= 0;
			inSeries <= inSeries+1;
		end
	end
end

always @(*) begin
	case(inSeries)
		0: begin
			O0 = A0; O1 = A1; O2 = A2; O3 = A3; O4 = A4; O5 = A5; O6 = A6; O7 = A7; O8 = A8; O9 = A9; 
		end
	endcase
end

assign outSeries = inSeries;

endmodule
