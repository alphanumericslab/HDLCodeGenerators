`timescale 1ns / 1ns

////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer:
//
// Create Date:   19:48:01 07/03/2009
// Design Name:   top
// Module Name:   E:/bs/BSc Project/Nikahd project(Last Version)/Nikahd project(Last Version)/FixedMedianFilter/testBench.v
// Project Name:  FixedMedianFilter
// Target Device:  
// Tool versions:  
// Description: 
//
// Verilog Test Fixture created by ISE for module: top
//
// Dependencies:
// 
// Revision:
// Revision 0.01 - File Created
// Additional Comments:
// 
////////////////////////////////////////////////////////////////////////////////
`timescale 1ns / 1ps
`include "macro.vh"

module testBench;

	// Inputs
	reg clk;
	reg reset;
	reg [`DATA_LENGTH-1:0] X;
	reg [`LOG_WMAX-1:0] W;
	// Outputs
	wire [`DATA_LENGTH-1:0] median;
	

	// Instantiate the Unit Under Test (UUT)
	top uut (
		.clk(clk), 
		.reset(reset), 
		.X(X), 	 
		.W(W),
		.median(median)
	);
	
	initial begin
		clk =0;
		forever 
			#100 clk = ~clk;   
	end
	initial begin
		// Initialize Inputs
		//clk = 0;	   
		W = `LOG_WMAX'd5;
		reset = 1;
		X = 16'd0;
		
		// Wait 100 ns for global reset to finish
		#200;
		reset = 0;
		X = 16'd64;
		#200;
		X = 16'd62;
		#200;
		X = 16'd76;
		#200;
		X = 16'd76;
		#200;
		X = 16'd121;
		#200;
		X = 16'd79;
		#200;
		X = 16'd83;
		#200;
		X = 16'd80;
		#200;
		X = 16'd48;
		#200;
		X = 16'd88;
		#200;
		X = 16'd63;
		#200;
		X = 16'd91;
		#200;
		X = 16'd90;
		#200;
		X = 16'd23;
		#200;
		X = 16'd20;
		#200;
		X = 16'd59;
		#200;
		X = 16'd67;
		#200;
		X = 16'd78;
		#200;
		X = 16'd83;
		#200;
		X = 16'd96;
		#200;
		X = 16'd114;
		#200;
		X = 16'd104;
		#200;
		X = 16'd128;
		#200;
		X = 16'd123;
		#200;
		X = 16'd123;
		#200;
		X = 16'd134;
		#200;
		X = 16'd136;
		#200;
		X = 16'd157;
		#200;
		X = 16'd147;
		#200;
		X = 16'd144;
		#200;
		X = 16'd169;
		#200;
		X = 16'd175;
		#200;
		X = 16'd154;
		#200;
		X = 16'd151;
		#200;
		X = 16'd171;
		#200;
		X = 16'd180;
		#200;
		X = 16'd179;
		#200;
		X = 16'd166;
		#200;
		X = 16'd195;
		#200;
		X = 16'd231;
		#200;
		X = 16'd200;
		#200;
		X = 16'd213;
		#200;	
		// Add stimulus here
		
	end
      
endmodule

