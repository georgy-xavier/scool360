USE `winschooldb`;


INSERT INTO `tblaction` VALUES (5,'Create Exam','This page allows user to create exams for each classes. The exam subjects can be selected from the left subject list.','CreateExam.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (6,'Schedule Exam','This page allows user to schedule exams. User can also edit the details ','ScheduleExam.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (7,'Enter Marks','If the exam is scheduled then user can enter each subject marks','EnterMark.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (11,'Student performance','This page allows user to view the performance of a student if exists. To print the performance click the \'print\' button. A new window will appear and performance can be printed from that window. If new window is not coming then check the pop-up setting of your internet browser','StudentPerform.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (23,'Create Exam Type','This page allows user to create the exam types. If exams are not scheduled the user can delete the exam types.','CreateExamType.aspx','Link',NULL);
INSERT INTO `tblaction` VALUES (28,'Exam Report','This page allows user to generate the exam report','ExamReport.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (30,'Remove Exam','This page allows user to remove the exam if it is not scheduled.','RemoveExam.aspx','SubLink',NULL);
INSERT INTO `tblaction` VALUES (41,'Manage All Mark',NULL,NULL,'Special',NULL);
INSERT INTO `tblaction` VALUES (49,'Search Exam','This page allows user to search the exam reports. Generate exam reports before searching.','SearchExam.aspx','Link',NULL);

INSERT INTO `tblmodule` VALUES (3,'Exam Manager','This Module will have actions for Exam Manager','Exam Manager','ViewExams.aspx',3);

INSERT INTO `tblmoduleactionmap` VALUES (3,5);
INSERT INTO `tblmoduleactionmap` VALUES (3,6);
INSERT INTO `tblmoduleactionmap` VALUES (3,7);
INSERT INTO `tblmoduleactionmap` VALUES (2,11);
INSERT INTO `tblmoduleactionmap` VALUES (3,23);
INSERT INTO `tblmoduleactionmap` VALUES (3,28);
INSERT INTO `tblmoduleactionmap` VALUES (3,30);
INSERT INTO `tblmoduleactionmap` VALUES (3,41);
INSERT INTO `tblmoduleactionmap` VALUES (4,49);


INSERT INTO `tblgrade` VALUES (1,'A+',NULL,75,NULL,1,'Distinction');
INSERT INTO `tblgrade` VALUES (2,'A',NULL,60,NULL,1,'1st Class ');
INSERT INTO `tblgrade` VALUES (3,'B+',NULL,50,NULL,0,NULL);
INSERT INTO `tblgrade` VALUES (4,'B',NULL,50,NULL,1,'2st Class');
INSERT INTO `tblgrade` VALUES (5,'C+',NULL,0,NULL,0,NULL);
INSERT INTO `tblgrade` VALUES (6,'C',NULL,40,NULL,1,'Passed');
INSERT INTO `tblgrade` VALUES (7,'D+',NULL,0,NULL,0,NULL);
INSERT INTO `tblgrade` VALUES (8,'D',NULL,35,NULL,1,'Passed');
INSERT INTO `tblgrade` VALUES (9,'E',NULL,0,NULL,0,NULL);
INSERT INTO `tblgrade` VALUES (10,'F',NULL,0,NULL,1,'Failed');
