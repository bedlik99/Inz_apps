INSERT INTO employee_user VALUES (1,'$2a$12$7eBBtGtxPEHmoE3j2T2GOeAPsxlE92oyWL6cIjBI5YXWzzXE6TMsm','eiti');
INSERT INTO role VALUES (1,'EMPLOYEE');
INSERT INTO user_role VALUES(1,1);

INSERT INTO registered_user VALUES (1,'12345678@pw.edu.pl','t@#Dg3sa');
INSERT INTO registered_user VALUES (2,'30060023@pw.edu.pl','8vkAE&sd');
INSERT INTO registered_user VALUES (3,'30050064@pw.edu.pl','gK4L0##G');
INSERT INTO registered_user VALUES (4,'30040099@pw.edu.pl','Ku6aa!Pl');
INSERT INTO registered_user VALUES (5,'30030011@pw.edu.pl','fWuD31(j');
INSERT INTO registered_user VALUES (6,'30020039@pw.edu.pl','UUiW62$#');

INSERT INTO recorded_event VALUES (1,PARSEDATETIME('03-05-2021 13:00:56','dd-MM-yyyy HH:mm:ss'),'User Registered',1);
INSERT INTO recorded_event VALUES (2,PARSEDATETIME('14-01-2021 15:35:46','dd-MM-yyyy HH:mm:ss'),'User Registered',2);
INSERT INTO recorded_event VALUES (3,PARSEDATETIME('27-02-2021 14:47:33','dd-MM-yyyy HH:mm:ss'),'User Registered',3);
INSERT INTO recorded_event VALUES (4,PARSEDATETIME('19-03-2021 18:46:14','dd-MM-yyyy HH:mm:ss'),'User Registered',4);
INSERT INTO recorded_event VALUES (5,PARSEDATETIME('21-12-2021 22:20:01','dd-MM-yyyy HH:mm:ss'),'User Registered',5);
INSERT INTO recorded_event VALUES (6,PARSEDATETIME('16-08-2021 20:20:00','dd-MM-yyyy HH:mm:ss'),'User Registered',6);