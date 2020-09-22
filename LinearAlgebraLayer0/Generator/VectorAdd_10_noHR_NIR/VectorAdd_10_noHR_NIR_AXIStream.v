`timescale 1ns / 1ps

module VectorAdd_10_noHR_NIR_AXIStream
#(
parameter IN_DATA_LENGHT= 300, 
parameter OUT_DATA_LENGHT= 160 
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
VectorAdd_10_noHR_NIR VectorAdd_10_noHR_NIR_AXIStream(
.clk(aclk),
.reset(!aresetn),
.enable(enable),
.inReady(inready),
.A0(indata[299:285]),
.A1(indata[284:270]),
.A2(indata[269:255]),
.A3(indata[254:240]),
.A4(indata[239:225]),
.A5(indata[224:210]),
.A6(indata[209:195]),
.A7(indata[194:180]),
.A8(indata[179:165]),
.A9(indata[164:150]),
.B0(indata[149:135]),
.B1(indata[134:120]),
.B2(indata[119:105]),
.B3(indata[104:90]),
.B4(indata[89:75]),
.B5(indata[74:60]),
.B6(indata[59:45]),
.B7(indata[44:30]),
.B8(indata[29:15]),
.B9(indata[14:0]),
.outReady(outready),
.S0(outdata[159:144]),
.S1(outdata[143:128]),
.S2(outdata[127:112]),
.S3(outdata[111:96]),
.S4(outdata[95:80]),
.S5(outdata[79:64]),
.S6(outdata[63:48]),
.S7(outdata[47:32]),
.S8(outdata[31:16]),
.S9(outdata[15:0]),
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
