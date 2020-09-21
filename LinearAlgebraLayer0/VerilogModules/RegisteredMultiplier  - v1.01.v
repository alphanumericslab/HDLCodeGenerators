`timescale 1ns / 1ps

module RegisteredMultiplier
#(
parameter IN_WIDTH = 10,
parameter INPUT_REG_DEPTH = 1,
parameter MULT_PIPE_DEPTH = 1
)(
input clk, reset, enable,
input inReady,
input signed [IN_WIDTH-1:0] A0,
input signed [IN_WIDTH-1:0] B0,
output outReady,
output signed [(2*IN_WIDTH)-1:0] DP,
output earlyOutReady
);

generate
if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH==0) begin

assign DP = A0*B0;
assign outReady = inReady;
assign earlyOutReady = 1'b0;

end
else begin

reg [0:INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1] OR = 0;

if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH!=0) begin

reg signed [(2*IN_WIDTH)-1:0] DPp [0:MULT_PIPE_DEPTH-1];
integer i;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(inReady)
			DPp[0] <= A0*B0;
		for(i=0;i<(MULT_PIPE_DEPTH-1);i=i+1) begin
			if(OR[i])
				DPp[i+1] <= DPp[i];
		end
	end
end
assign DP = DPp[MULT_PIPE_DEPTH-1];

end
else if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH==0) begin

reg signed [IN_WIDTH-1:0] A0r [0:INPUT_REG_DEPTH-1], B0r [0:INPUT_REG_DEPTH-1];
integer j;
reg signed [(2*IN_WIDTH)-1:0] DPp;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(inReady) begin
			A0r[0] <= A0;
			B0r[0] <= B0;
		end
		for(j=0;j<INPUT_REG_DEPTH-1;j=j+1) begin
			if(OR[j]) begin
				A0r[j+1] <= A0r[j];
				B0r[j+1] <= B0r[j];
			end
		end
	end
end
assign DP = A0r[INPUT_REG_DEPTH-1]*B0r[INPUT_REG_DEPTH-1];

end
else begin //if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH!=0)

reg signed [IN_WIDTH-1:0] A0r [0:INPUT_REG_DEPTH-1], B0r [0:INPUT_REG_DEPTH-1];
reg signed [(2*IN_WIDTH)-1:0] DPp [0:MULT_PIPE_DEPTH-1];
integer i, j;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(inReady)
			A0r[0] <= A0;
			B0r[0] <= B0;
		for(j=0;j<INPUT_REG_DEPTH-1;j=j+1) begin
			if(OR[j]) begin
				A0r[j+1] <= A0r[j];
				B0r[j+1] <= B0r[j];
			end
		end
		if(OR[INPUT_REG_DEPTH-1])
			DPp[0] <= A0r[INPUT_REG_DEPTH-1]*B0r[INPUT_REG_DEPTH-1];
		for(i=0;i<(MULT_PIPE_DEPTH-1);i=i+1) begin
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
