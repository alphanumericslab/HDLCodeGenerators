`timescale 1ns / 1ps

module VectorAdd_11_noHR_NIR_AXIStream
#(
parameter IN_DATA_LENGHT= 220, 
parameter OUT_DATA_LENGHT= 121 
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
VectorAdd_11_noHR_NIR VectorAdd_11_noHR_NIR_AXIStream(
.clk(aclk),
.reset(!aresetn),
.enable(enable),
.inReady(inready),
.A0(indata[219:210]),
.A1(indata[209:200]),
.A2(indata[199:190]),
.A3(indata[189:180]),
.A4(indata[179:170]),
.A5(indata[169:160]),
.A6(indata[159:150]),
.A7(indata[149:140]),
.A8(indata[139:130]),
.A9(indata[129:120]),
.A10(indata[119:110]),
.B0(indata[109:100]),
.B1(indata[99:90]),
.B2(indata[89:80]),
.B3(indata[79:70]),
.B4(indata[69:60]),
.B5(indata[59:50]),
.B6(indata[49:40]),
.B7(indata[39:30]),
.B8(indata[29:20]),
.B9(indata[19:10]),
.B10(indata[9:0]),
.outReady(outready),
.S0(outdata[120:110]),
.S1(outdata[109:99]),
.S2(outdata[98:88]),
.S3(outdata[87:77]),
.S4(outdata[76:66]),
.S5(outdata[65:55]),
.S6(outdata[54:44]),
.S7(outdata[43:33]),
.S8(outdata[32:22]),
.S9(outdata[21:11]),
.S10(outdata[10:0]),
.earlyOutReady(earlyoutready)
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
