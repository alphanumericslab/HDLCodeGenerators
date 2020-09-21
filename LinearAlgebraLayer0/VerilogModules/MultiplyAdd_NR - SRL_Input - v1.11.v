`timescale 1ns / 1ps

module MultiplyAdd_NR
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
output signed [OUT_WIDTH-1:0] RES,
output earlyOutReady
);

generate
if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH==0) begin

assign RES = C + (A * B);
assign outReady = inReady;
assign earlyOutReady = 1'b0;

end
else begin

reg [0:INPUT_REG_DEPTH+MULT_PIPE_DEPTH-1] OR = 0;

if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH!=0) begin

reg signed [(2*IN_M_WIDTH)-1:0] mult [0:MULT_PIPE_DEPTH-1];
integer i;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		if(inReady)
			mult[0] <= A * B;
		for(i=0;i<(MULT_PIPE_DEPTH-1);i=i+1) begin
			if(OR[i])
				mult[i+1] <= mult[i];
		end
	end
end
assign RES = C + mult[MULT_PIPE_DEPTH-1];

end
else if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH==0) begin

reg signed [IN_M_WIDTH-1:0] Ar[1:INPUT_REG_DEPTH], Br[1:INPUT_REG_DEPTH];
integer j;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		Ar[1] <= A;
		Br[1] <= B;
		for(j=1;j<INPUT_REG_DEPTH;j=j+1) begin
			Ar[j+1] <= Ar[j];
			Br[j+1] <= Br[j];
		end
	end
end
assign RES = C + (Ar[INPUT_REG_DEPTH] * Br[INPUT_REG_DEPTH]);	

end
else begin //if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH!=0)

reg signed [IN_M_WIDTH-1:0] Ar[1:INPUT_REG_DEPTH], Br[1:INPUT_REG_DEPTH];
reg signed [(2*IN_M_WIDTH)-1:0] mult [0:MULT_PIPE_DEPTH-1];
integer i, j;
always @(posedge clk) begin
	if(reset) begin
		//Do Nothing
	end
	else if(enable) begin
		Ar[1] <= A;
		Br[1] <= B;
		for(j=1;j<INPUT_REG_DEPTH;j=j+1) begin
			Ar[j+1] <= Ar[j];
			Br[j+1] <= Br[j];
		end
		if(OR[INPUT_REG_DEPTH-1])
			mult[0] <= Ar[INPUT_REG_DEPTH] * Br[INPUT_REG_DEPTH];
		for(i=0;i<MULT_PIPE_DEPTH-1;i=i+1) begin
			if(OR[INPUT_REG_DEPTH+i])
				mult[i+1] <= mult[i];
		end
	end
end
assign RES = C + mult[MULT_PIPE_DEPTH-1];

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
