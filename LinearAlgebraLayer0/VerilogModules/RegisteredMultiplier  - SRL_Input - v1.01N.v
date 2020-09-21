`timescale 1ns / 1ps

module RegisteredMultiplier
#(
parameter IN_WIDTH = 10,
parameter INPUT_REG_DEPTH = 16,
parameter MULT_PIPE_DEPTH = 1
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A,
input signed [IN_WIDTH-1:0] B,
output outReady,
output signed [(2*IN_WIDTH)-1:0] DP,
output earlyOutReady
);


generate
if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH==0) begin

assign outReady = inReady;
assign earlyOutReady = 1'b0;
assign DP = A*B;

end
else begin

reg [0:INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1] OR = 0;

wire signed [IN_WIDTH-1:0] AI, BI;
wire ORMS;

if(INPUT_REG_DEPTH==0) begin

assign AI = A;
assign BI = B;
assign ORMS = inReady;

end
else begin

reg signed [IN_WIDTH-1:0] Ar [0:INPUT_REG_DEPTH-1], Br [0:INPUT_REG_DEPTH-1];
integer j;
reg signed [(2*IN_WIDTH)-1:0] DPp;
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

assign DP = AI*BI;

end
else begin

reg signed [(2*IN_WIDTH)-1:0] DPp [0:MULT_PIPE_DEPTH-1];
integer i;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(ORMS)
			DPp[0] <= AI*BI;
		for(i=0;i<MULT_PIPE_DEPTH-1;i=i+1) begin
			if(OR[INPUT_REG_DEPTH+i])
				DPp[i+1] <= DPp[i];
		end
	end
end
assign DP = DPp[MULT_PIPE_DEPTH-1];

end

integer k;
always @(posedge clk) begin
	if(reset) begin
		OR <= 0;
	end
	else if(enable) begin
		OR[0] <= inReady;
		for(k=0;k<INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1;k=k+1) begin
			OR[k+1] <= OR[k];
		end
	end
end

assign outReady = OR[INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1];

if(INPUT_REG_DEPTH+MULT_PIPE_DEPTH==1) begin
assign earlyOutReady = inReady;
end
else begin
assign earlyOutReady = OR[INPUT_REG_DEPTH+MULT_PIPE_DEPTH-2];
end

end
endgenerate

endmodule
