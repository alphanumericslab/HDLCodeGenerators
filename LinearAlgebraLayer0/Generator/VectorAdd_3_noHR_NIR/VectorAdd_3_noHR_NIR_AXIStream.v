`timescale 1ns / 1ps

module VectorAdd_3_noHR_NIR_AXIStream
#(
parameter IN_DATA_LENGHT= 60, 
parameter OUT_DATA_LENGHT= 33 
)( 

input aclk,
input aresetn,
input enable,
input [IN_DATA_LENGHT-1:0]s_axi_data,
input s_axi_valid,
input m_axi_ready,
output reg[OUT_DATA_LENGHT-1:0]m_axi_data,
output reg m_axi_valid,
output reg s_axi_ready
);

reg inready;
reg [IN_DATA_LENGHT-1:0]indata;
wire outready;
wire earlyoutready;
wire [OUT_DATA_LENGHT-1:0]outdata;
////////////////instancing wrapping module///////////////////
VectorAdd_3_noHR_NIR VectorAdd_3_noHR_NIR_AXIStream(
.clk(aclk),
.reset(!aresetn),
.enable(enable),
.inready(inready),
.A0(indata[59:50]),
.A1(indata[49:40]),
.A2(indata[39:30]),
.B0(indata[29:20]),
.B1(indata[19:10]),
.B2(indata[9:0]),
.outready(outready),
.S0(outdata[32:22]),
.S1(outdata[21:11]),
.S2(outdata[10:0]),
earlyOutReady(earlyoutready)
);
/////////////////Main body/////////////
always @(posedge aclk)begin
 if(aresetn==0)begin
  m_axi_data<=0;
  m_axi_valid<=0;
 end
 else begin
  if(m_axi_ready==1 && m_axi_valid==1)begin
   m_axi_valid<=0;
  end
  else if(outready==1)begin
   m_axi_valid<=1;
   m_axi_data<=outdata;
  end
 end
end
always @(posedge aclk)begin
 if(aresetn==0)begin
  s_axi_ready<=1;
  inready<=0;
  indata<=0;
 end
 else begin
  inready<=0;
  if(s_axi_valid==1 && s_axi_ready==1)begin
   s_axi_ready<=0;
   inready<=1;
   indata<= s_axi_data;
  end
  else if(m_axi_valid==1 && m_axi_ready==1)begin
   s_axi_ready<=1;
  end
 end
end

endmodule
