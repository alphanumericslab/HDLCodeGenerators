//
// 4-bit Unsigned Up Accumulator with sync Reset
//
module v_accumulators_1 (clk, rst, D, Q);
input clk, rst;
input [3:0] D;
output [3:0] Q;
reg [3:0] tmp;
always @(posedge clk)
	begin
		if (rst)
			tmp <= 0;
		else
			tmp <= tmp + D;
	end
assign Q = tmp;
endmodule