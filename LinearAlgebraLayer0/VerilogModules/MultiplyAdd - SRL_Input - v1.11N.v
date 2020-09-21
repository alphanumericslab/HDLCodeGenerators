`timescale 1ns / 1ps

module MultiplyAdd
#(
parameter IN_M_WIDTH = 10,
parameter IN_A_WIDTH = 20,
parameter OUT_WIDTH = 21,
parameter INPUT_REG_DEPTH = 1,
parameter MULT_PIPE_DEPTH = 1 //0,1 or 2; 0: no pipelining, 1: 1 level pipelining, 2: 2 level pipelining
)(
input clk, reset, enable,
input inReady,
input signed [IN_M_WIDTH-1:0] A, B,
input signed [IN_A_WIDTH-1:0] C,
output outReady,
output reg signed [OUT_WIDTH-1:0] RES,
output earlyOutReady
);

wire signed [IN_M_WIDTH-1:0] AI, BI;
wire signed [(2*IN_M_WIDTH)-1:0] multF = AI * BI;
wire ORMS;

reg [0:INPUT_REG_DEPTH+MULT_PIPE_DEPTH] OR = 0;
integer k;
always @(posedge clk) begin
	if(reset) begin
		OR <= 0;
	end
	else if(enable) begin
		OR[0] <= inReady;
		for(k=0;k<INPUT_REG_DEPTH+MULT_PIPE_DEPTH;k=k+1) begin
			OR[k+1] <= OR[k];
		end
	end
end

assign outReady = OR[INPUT_REG_DEPTH+MULT_PIPE_DEPTH];

generate

if(INPUT_REG_DEPTH==0) begin

assign AI = A;
assign BI = B;
assign ORMS = inReady;

end
else begin

reg signed [IN_M_WIDTH-1:0] Ar [0:INPUT_REG_DEPTH-1], Br [0:INPUT_REG_DEPTH-1];
integer j;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		Ar[0] <= A;
		Br[0] <= B;
		for(j=0;j<INPUT_REG_DEPTH-1;j=j+1) begin
			Ar[j+1] <= Ar[j];
			Br[j+1] <= Br[j];
		end
	end
end
assign AI = Ar[INPUT_REG_DEPTH-1];
assign BI = Br[INPUT_REG_DEPTH-1];
assign ORMS = OR[INPUT_REG_DEPTH-1];

end

if(MULT_PIPE_DEPTH==0) begin

always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(ORMS) begin
			RES <= C + multF;	
		end
	end
end

end
else begin

reg signed [(2*IN_M_WIDTH)-1:0] mult [0:MULT_PIPE_DEPTH-1];
integer i;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(ORMS)
			mult[0] <= multF;
		for(i=0;i<MULT_PIPE_DEPTH-1;i=i+1) begin
			if(OR[INPUT_REG_DEPTH+i])
				mult[i+1] <= mult[i];
		end
		if(OR[INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1])
			RES <= C + mult[MULT_PIPE_DEPTH-1];
	end
end

end

if(INPUT_REG_DEPTH+MULT_PIPE_DEPTH==0) begin
assign earlyOutReady = inReady;
end
else begin
assign earlyOutReady = OR[INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1];
end

endgenerate

endmodule
