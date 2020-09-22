`timescale 1ns / 1ps

module VectorAdd_10_S5E_HRx2_NIR
#(
parameter IN_WIDTH = 10
)(
input clk, reset, enable,
output reg readyForNewDataSeries = 1,
output reg inSeries = 0,
input inReady,
input signed [IN_WIDTH-1:0] A0, A1, A2, A3, A4, 
input signed [IN_WIDTH-1:0] B0, B1, B2, B3, B4, 
output S0toS4outReady,
output reg SNoutReady=0, //not used
output reg outSeries = 1,
output signed [IN_WIDTH:0] S0, S1, S2, S3, S4, 
output S0toS4earlyOutReady,
output reg SNearlyOutReady=0 //not used
);

always @(posedge clk) begin
	if(reset) begin
		readyForNewDataSeries <= 1;
		inSeries <= 0;
	end
	else if(enable) begin
		if(inReady) begin
			if(inSeries==1) begin
				readyForNewDataSeries <= 1;
				inSeries <= 0;
			end
			else begin
				readyForNewDataSeries <= 0;
				inSeries <= 1;
			end
		end
	end
end

//(* use_dsp48 = "yes" *) //yes, no, AutoMax, Auto
Registered2to1Adder_NIR #(.IN_WIDTH(IN_WIDTH))
Add0 (clk, reset, enable,
inReady,
A0, B0,
S0toS4outReady,
S0,
S0toS4earlyOutReady);

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

always @(posedge clk) begin
	if(reset) begin
		outSeries <= 1;
	end
	else if(enable) begin
		if(S0toS4earlyOutReady) begin
			if(outSeries==1) begin
				outSeries <= 0;
			end
			else begin
				outSeries <= 1;
			end
		end
	end
end

endmodule
