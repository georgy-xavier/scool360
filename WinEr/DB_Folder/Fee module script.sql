USE `winschooldb`;

INSERT INTO `tblaccount` VALUES (1,'Fee',0);
INSERT INTO `tblaccount` VALUES (2,'Student',0);
INSERT INTO `tblaccount` VALUES (3,'Discount',0);
INSERT INTO `tblaccount` VALUES (4,'Fine',0);

INSERT INTO `tblaction` VALUES (1,'Create Fee Account','This page allows user to create a fee account. While creating a fee account user needs to specify the frequency type.','CreateFeeAccount.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (2,'Collect Fee','This page allows user to collect fee and generate bill. For viewing a student fee user need to select the class and select the student name or rollno from list and then click show fee button. For paying the fee user need to check the corresponding checkbox and enter the paying amount in the textbox. For paying a fee the balance need to 0. After paying the fee click generate bill button to generate the bill','CollectFeeAccount.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (8,'Search Fee',NULL,'SearchFeeAccount.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (12,'Manage Fee','This page allows user to select the fee which needs to be viewed,scheduled,edit schedule and removed ','ManageFeeAccount.aspx','No Link',NULL);
INSERT INTO `tblaction` VALUES (38,'Schedule Fee',NULL,'ScheduleFeeAccount.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (39,'Remove Fee','This page enables you to remove fee. To remove a fee certain condition should be followed.If a fee is scheduled it should not be removed in the current Batch.In this condition you have to wait for the next batch for fee removal of fee. ','RemoveFee.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (40,'View Fee Bills','This page allows user to view all the Bills ','ViewBill.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (42,'View Collected Amount','This page enable you to view the amount collected and transactions for a period of time. You can view amount accountwise',NULL,'Button',NULL);
INSERT INTO `tblaction` VALUES (43,'Edit Fee Schedule',NULL,'EditFeeSchdule.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (45,'Printer Settings','This page allows user to change the printer settings','PrinterSettings.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (15,'Vew Fee Schedule Details',NULL,NULL,'No Link',NULL);
INSERT INTO `tblaction` VALUES (21,'View Fee Report','This page helps user to view a detailed report of the fee.This page will display all the informations the current year fee.','ViewFeeReport.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (44,'Fee Details','This page allowes user to view the fee transactions for the selected student. The first tab displays the paid fee transactions. Last six months transactions are displayed by default. The second tab displays the unpaid fee details. Using the export button user can export the details to an excel.','ViewStudFeeDetails.aspx','SubLink',NULL);

INSERT INTO `tblmodule` VALUES (1,'Fee Manager','This Module will have actions for managing fee.','Fee Manager','ManageFeeAccount.aspx',4);


INSERT INTO `tblmoduleactionmap` VALUES (1,1);
INSERT INTO `tblmoduleactionmap` VALUES (1,2);
INSERT INTO `tblmoduleactionmap` VALUES (1,8);
INSERT INTO `tblmoduleactionmap` VALUES (1,12);
INSERT INTO `tblmoduleactionmap` VALUES (1,38);
INSERT INTO `tblmoduleactionmap` VALUES (1,39);
INSERT INTO `tblmoduleactionmap` VALUES (1,40);
INSERT INTO `tblmoduleactionmap` VALUES (1,42);
INSERT INTO `tblmoduleactionmap` VALUES (1,43);
INSERT INTO `tblmoduleactionmap` VALUES (12,45);
INSERT INTO `tblmoduleactionmap` VALUES (1,15);
INSERT INTO `tblmoduleactionmap` VALUES (1,21);
INSERT INTO `tblmoduleactionmap` VALUES (2,44);

INSERT INTO `tblfeeasso` VALUES (1,'Class');
INSERT INTO `tblfeeasso` VALUES (2,'Student');
INSERT INTO `tblfeefrequencytype` VALUES (1,'Yearly');
INSERT INTO `tblfeefrequencytype` VALUES (2,'Half Yearly');
INSERT INTO `tblfeefrequencytype` VALUES (3,'Quarterly');
INSERT INTO `tblfeefrequencytype` VALUES (4,'Monthly');
INSERT INTO `tblfeefrequencytype` VALUES (5,'Single Payment');
INSERT INTO `tblfeefrequencytype` VALUES (6,'Semester');