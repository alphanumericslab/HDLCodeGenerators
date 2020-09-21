`timescale 1ns / 1ps

module MultiplyAdd
#(
parameter IN_M_WIDTH = 10,
parameter IN_A_WIDTH = 20,
parameter OUT_WIDTH = 21,
parameter INPUT_REG_DEPTH = 0,
parameter MULT_PIPE_DEPTH = 0 //0,1 or 2; 0: no pipelining, 1: 1 level pipelining, 2: 2 level pipelining
)(
input clk, enable,
input signed [IN_M_WIDTH-1:0] A, B,
input signed [IN_A_WIDTH-1:0] C,
output reg signed [OUT_WIDTH-1:0] RES
);

generate
if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH==0) begin

always @(posedge clk) begin
	if(enable) begin
		RES <= C + (A * B);	
	end
end

end
else if(INPUT_REG_DEPTH==0 && MULT_PIPE_DEPTH!=0) begin

reg signed [(2*IN_M_WIDTH)-1:0] mult [0:MULT_PIPE_DEPTH-1];
integer i;
always @(posedge clk) begin
	if(enable) begin
		mult[0] <= A * B;
		for(i=0;i<MULT_PIPE_DEPTH-1;i=i+1) begin
			mult[i+1] <= mult[i];
		end
		RES <= C + mult[MULT_PIPE_DEPTH-1];
	end
end

end
else if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH==0) begin

reg signed [IN_M_WIDTH-1:0] Ar[1:INPUT_REG_DEPTH], Br[1:INPUT_REG_DEPTH];
integer j;
always @(posedge clk) begin
	if(enable) begin
		Ar[1] <= A;
		Br[1] <= B;
		for(j=1;j<INPUT_REG_DEPTH;j=j+1) begin
			Ar[j+1] <= Ar[j];
			Br[j+1] <= Br[j];
		end
		RES <= C + (Ar[INPUT_REG_DEPTH] * Br[INPUT_REG_DEPTH]);	
	end
end

end
else begin //if(INPUT_REG_DEPTH!=0 && MULT_PIPE_DEPTH!=0)

reg signed [IN_M_WIDTH-1:0] Ar[1:INPUT_REG_DEPTH], Br[1:INPUT_REG_DEPTH];
reg signed [(2*IN_M_WIDTH)-1:0] mult [0:MULT_PIPE_DEPTH-1];
integer i, j;
always @(posedge clk) begin
	if(enable) begin
		Ar[1] <= A;
		Br[1] <= B;
		for(j=1;j<INPUT_REG_DEPTH;j=j+1) begin
			Ar[j+1] <= Ar[j];
			Br[j+1] <= Br[j];
		end
		mult[0] <= Ar[INPUT_REG_DEPTH] * Br[INPUT_REG_DEPTH];
		for(i=0;i<MULT_PIPE_DEPTH-1;i=i+1) begin
			mult[i+1] <= mult[i];
		end
		RES <= C + mult[MULT_PIPE_DEPTH-1];
	end
end

end
endgenerate

endmodule
