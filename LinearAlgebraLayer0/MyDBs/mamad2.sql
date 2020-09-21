-- MySQL dump 10.13  Distrib 5.5.59, for Win64 (AMD64)
--
-- Host: localhost    Database: LATool
-- ------------------------------------------------------
-- Server version	5.5.59-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `matrixmatrixmultiply_si_10`
--

DROP TABLE IF EXISTS `matrixmatrixmultiply_si_10`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `matrixmatrixmultiply_si_10` (
  `Project_Name` varchar(200) DEFAULT NULL,
  `Verilog_Parameters_name_append` varchar(200) DEFAULT NULL,
  `Random_Index_Result` int(11) DEFAULT NULL,
  `CPU_Core` int(11) DEFAULT NULL,
  `M` int(11) DEFAULT NULL,
  `N` int(11) DEFAULT NULL,
  `Q` int(11) DEFAULT NULL,
  `PR` int(11) DEFAULT NULL,
  `PC` int(11) DEFAULT NULL,
  `HRR` int(11) DEFAULT NULL,
  `MAMCS` int(11) DEFAULT NULL,
  `ADDER_SIZE` int(11) DEFAULT NULL,
  `ENABLE_INPUT_LATCH` int(11) DEFAULT NULL,
  `ENABLE_COLUMN_LATCH` int(11) DEFAULT NULL,
  `ENABLE_ROW_LATCH` int(11) DEFAULT NULL,
  `ENABLE_SIM_OUTPUT_REGISTERS` int(11) DEFAULT NULL,
  `IN_WIDTH` int(11) DEFAULT NULL,
  `INPUT_REG_DEPTH` int(11) DEFAULT NULL,
  `MULT_PIPE_DEPTH` int(11) DEFAULT NULL,
  `simulation_reported_values_present` tinyint(1) DEFAULT NULL,
  `result_simulate_run` int(11) DEFAULT NULL,
  `result_simulate` varchar(200) DEFAULT NULL,
  `syn_reported_values_present` tinyint(1) DEFAULT NULL,
  `result_synthesize` int(11) DEFAULT NULL,
  `syn_min_period` double DEFAULT NULL,
  `syn_max_freq` double DEFAULT NULL,
  `syn_min_input_arrival_time_before_clock` double DEFAULT NULL,
  `syn_max_output_required_time_after_clock` double DEFAULT NULL,
  `syn_max_comb_path_delay` varchar(200) DEFAULT NULL,
  `syn_slice_registers` int(11) DEFAULT NULL,
  `syn_slice_registers_percent` int(11) DEFAULT NULL,
  `syn_slice_LUTs` int(11) DEFAULT NULL,
  `syn_slice_LUTs_percent` int(11) DEFAULT NULL,
  `syn_LUT_FF_pairs` int(11) DEFAULT NULL,
  `syn_fully_used_LUT_FF_pairs` int(11) DEFAULT NULL,
  `syn_fully_used_LUT_FF_pairs_percent` int(11) DEFAULT NULL,
  `syn_IOs` int(11) DEFAULT NULL,
  `syn_bonded_IOBs` int(11) DEFAULT NULL,
  `syn_bonded_IOBs_percent` int(11) DEFAULT NULL,
  `syn_BUFG_BUFGCTRLs` int(11) DEFAULT NULL,
  `syn_BUFG_BUFGCTRLs_percent` int(11) DEFAULT NULL,
  `syn_DSP48s` int(11) DEFAULT NULL,
  `syn_DSP48s_percent` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `matrixmatrixmultiply_si_10`
--

LOCK TABLES `matrixmatrixmultiply_si_10` WRITE;
/*!40000 ALTER TABLE `matrixmatrixmultiply_si_10` DISABLE KEYS */;
INSERT INTO `matrixmatrixmultiply_si_10` VALUES ('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx9_C1_A0_IL_CL_RL_MR','_12_1_0',2,0,5,9,5,2,2,9,1,0,1,1,1,1,12,1,0,1,0,'Succeeded',1,0,6.519,153.395,1.903,3.597,'No path found',2069,1,1013,1,2737,345,12,569,569,98,1,6,4,2),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx9_C1_A0_IL_CL_RL_MR','_13_1_1',2,0,5,9,5,2,2,9,1,0,1,1,1,1,13,1,1,1,0,'Succeeded',1,0,5.696,175.575,1.974,3.597,'No path found',2270,1,1944,2,2980,1234,41,613,613,106,1,6,4,2),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C2_A0_IL_CL_RL_MR','_10_1_1',1,0,5,9,5,2,2,5,2,0,1,1,1,1,10,1,1,1,0,'Succeeded',1,0,4.692,213.111,1.974,3.597,'No path found',1774,0,1497,1,2269,1002,44,480,480,83,1,6,8,4),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C2_A0_IL_CL_RL_MR','_14_1_1',1,0,5,9,5,2,2,5,2,0,1,1,1,1,14,1,1,1,0,'Succeeded',1,0,4.607,217.052,2.007,3.597,'No path found',2428,1,2480,2,3143,1765,56,656,656,113,1,6,8,4),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C1_A2_IL_CL_RL_MR','_10_1_1',0,0,5,9,5,2,2,5,1,2,1,1,1,1,10,1,1,1,0,'Succeeded',1,0,4.692,213.111,1.974,3.597,'No path found',1774,0,1497,1,2268,1003,44,480,480,83,1,6,8,4),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C1_A2_IL_CL_RL_MR','_14_1_1',0,0,5,9,5,2,2,5,1,2,1,1,1,1,14,1,1,1,0,'Succeeded',1,0,4.607,217.052,2.007,3.597,'No path found',2428,1,2480,2,3142,1766,56,656,656,113,1,6,8,4);
/*!40000 ALTER TABLE `matrixmatrixmultiply_si_10` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `matrixmatrixmultiply_si_11`
--

DROP TABLE IF EXISTS `matrixmatrixmultiply_si_11`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `matrixmatrixmultiply_si_11` (
  `Project_Name` varchar(200) DEFAULT NULL,
  `Verilog_Parameters_name_append` varchar(200) DEFAULT NULL,
  `Random_Index_Result` int(11) DEFAULT NULL,
  `CPU_Core` int(11) DEFAULT NULL,
  `M` int(11) DEFAULT NULL,
  `N` int(11) DEFAULT NULL,
  `Q` int(11) DEFAULT NULL,
  `PR` int(11) DEFAULT NULL,
  `PC` int(11) DEFAULT NULL,
  `HRR` int(11) DEFAULT NULL,
  `MAMCS` int(11) DEFAULT NULL,
  `ADDER_SIZE` int(11) DEFAULT NULL,
  `ENABLE_INPUT_LATCH` int(11) DEFAULT NULL,
  `ENABLE_COLUMN_LATCH` int(11) DEFAULT NULL,
  `ENABLE_ROW_LATCH` int(11) DEFAULT NULL,
  `ENABLE_SIM_OUTPUT_REGISTERS` int(11) DEFAULT NULL,
  `IN_WIDTH` int(11) DEFAULT NULL,
  `INPUT_REG_DEPTH` int(11) DEFAULT NULL,
  `MULT_PIPE_DEPTH` int(11) DEFAULT NULL,
  `simulation_reported_values_present` tinyint(1) DEFAULT NULL,
  `result_simulate_run` int(11) DEFAULT NULL,
  `result_simulate` varchar(200) DEFAULT NULL,
  `syn_reported_values_present` tinyint(1) DEFAULT NULL,
  `result_synthesize` int(11) DEFAULT NULL,
  `syn_min_period` double DEFAULT NULL,
  `syn_max_freq` double DEFAULT NULL,
  `syn_min_input_arrival_time_before_clock` double DEFAULT NULL,
  `syn_max_output_required_time_after_clock` double DEFAULT NULL,
  `syn_max_comb_path_delay` varchar(200) DEFAULT NULL,
  `syn_slice_registers` int(11) DEFAULT NULL,
  `syn_slice_registers_percent` int(11) DEFAULT NULL,
  `syn_slice_LUTs` int(11) DEFAULT NULL,
  `syn_slice_LUTs_percent` int(11) DEFAULT NULL,
  `syn_LUT_FF_pairs` int(11) DEFAULT NULL,
  `syn_fully_used_LUT_FF_pairs` int(11) DEFAULT NULL,
  `syn_fully_used_LUT_FF_pairs_percent` int(11) DEFAULT NULL,
  `syn_IOs` int(11) DEFAULT NULL,
  `syn_bonded_IOBs` int(11) DEFAULT NULL,
  `syn_bonded_IOBs_percent` int(11) DEFAULT NULL,
  `syn_BUFG_BUFGCTRLs` int(11) DEFAULT NULL,
  `syn_BUFG_BUFGCTRLs_percent` int(11) DEFAULT NULL,
  `syn_DSP48s` int(11) DEFAULT NULL,
  `syn_DSP48s_percent` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `matrixmatrixmultiply_si_11`
--

LOCK TABLES `matrixmatrixmultiply_si_11` WRITE;
/*!40000 ALTER TABLE `matrixmatrixmultiply_si_11` DISABLE KEYS */;
INSERT INTO `matrixmatrixmultiply_si_11` VALUES ('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx9_C1_A0_IL_CL_RL_MR','_13_1_1',2,0,5,9,5,2,2,9,1,0,1,1,1,1,13,1,1,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx9_C1_A0_IL_CL_RL_MR','_11_1_0',2,0,5,9,5,2,2,9,1,0,1,1,1,1,11,1,0,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C2_A0_IL_CL_RL_MR','_11_1_0',1,0,5,9,5,2,2,5,2,0,1,1,1,1,11,1,0,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C2_A0_IL_CL_RL_MR','_10_1_0',1,0,5,9,5,2,2,5,2,0,1,1,1,1,10,1,0,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C1_A2_IL_CL_RL_MR','_11_1_1',0,0,5,9,5,2,2,5,1,2,1,1,1,1,11,1,1,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('MatrixMatrixMultiply_5_9_5_2PR_2PC_SI_HRx5_C1_A2_IL_CL_RL_MR','_10_1_1',0,0,5,9,5,2,2,5,1,2,1,1,1,1,10,1,1,0,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `matrixmatrixmultiply_si_11` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-02-20 16:21:10
