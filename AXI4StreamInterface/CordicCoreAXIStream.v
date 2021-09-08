`timescale 1ns / 1ps
//this primitive module is a single iterator cordic core with just x and y for hyperbolic_vectoring mode.(calculating Square Root)
module CORDICCore#(
parameter IN_WIDTH=10,
parameter OUT_WIDTH=IN_WIDTH,
parameter MAX_ITERATION_WIDTH=10
)
(
input aclk,
input aresetn,
input enable,
input signed[2*IN_WIDTH+MAX_ITERATION_WIDTH-1:0]s_axi_data,//={Xin,Yin,iteration}
input s_axi_valid,
input m_axi_ready,
output signed [2*OUT_WIDTH-1:0]m_axi_data,//={Xout,Yout}
output reg s_axi_ready,
output reg m_axi_valid
);
wire signed [IN_WIDTH-1:0]Xin,Yin;
reg signed [OUT_WIDTH-1:0]Xout,Yout;
wire signed [MAX_ITERATION_WIDTH-1:0]iteration;

assign m_axi_data={Xout,Yout};
assign {Xin,Yin,iteration}=s_axi_data;

reg [1:0]state;
always@(posedge aclk)begin
	if (enable)begin
		if(aresetn==0)begin
			s_axi_ready<=1;
			Xout<=0;
			Yout<=0;
			m_axi_valid<=0;
			state<=0;
		end
		else begin
			case(state)
				0:begin
					if(s_axi_valid==1)begin
						if(Yin<0)begin
							Xout <= Xin + (Yin >>> iteration);
							Yout <= Yin + (Xin >>> iteration);
						end
						else begin 
							Xout <= Xin - (Yin >>> iteration);
							Yout <= Yin - (Xin >>> iteration);
						end
						state<=1;
						m_axi_valid<=1;
					end
				end
				1:begin
					if(m_axi_ready==1)begin
						s_axi_ready<=1;
						m_axi_valid<=0;	
						state<=0;
					end
				end
			endcase	
		end
	end
end
endmodule 




