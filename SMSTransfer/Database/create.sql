
DROP TABLE IF EXISTS SMSTELEPHONES;
CREATE TABLE SMSTELEPHONES(
   ID INTEGER PRIMARY KEY   AUTOINCREMENT,
   TELEPHONE      TEXT      NOT NULL,
   AREA      TEXT      NOT NULL,
   CITY      TEXT      NOT NULL
);

DROP TABLE IF EXISTS SMSUsers;
CREATE TABLE SMSUsers(
   ID INTEGER PRIMARY KEY   AUTOINCREMENT,
   USERKEY    TEXT      NOT NULL,
   LIMITEDQTY INTEGER NOT NULL,
   STATUS INTEGER NOT NULL
);


DROP TABLE IF EXISTS SMSLogs;
CREATE TABLE SMSLogs(
   ID INTEGER PRIMARY KEY   AUTOINCREMENT,
   USERKEY    TEXT      NOT NULL,
   TELEPHONE      TEXT      NOT NULL,
   COUNTOFSENT INTEGER NOT NULL,
   CREATETIME DATETIME NOT NULL,
   LASTMODTIME DATETIME NOT NULL
);


INSERT INTO SMSUsers(USERKEY, LIMITEDQTY,STATUS)VALUES('649cf2e3',10,0);
INSERT INTO SMSTELEPHONES(TELEPHONE, AREA,CITY)VALUES('15958847895','广西','桂林');

--随机取不重复号码
--SELECT TEL.* FROM SMSTELEPHONES TEL WHERE NO EXISTS(SELECT 1 FROM SMSLogs LOG WHERE USERKEY = {} AND TEL.TELEPHONE =LOG.TELEPHONE AND LASTMODTIME = {} ) AND AREA={} AND CITY = {} LIMIT 1;

--指定取已取号码
--SELECT TEL.* FROM SMSTELEPHONES TEL WHERE EXISTS(SELECT 1 FROM SMSLogs LOG WHERE LOG.USERKEY=@USERKEY AND LOG.CREATETIME=@CREATETIME AND LOG.TELEPHONE=@TELEPHONE) LIMIT 1;

--INSERT INTO SMSLogs(USERKEY,TELEPHONE,COUNTOFSENT,CREATETIME,LASTMODTIME)VALUES(@USERKEY,@TELEPHONE,0,@CREATETIME,'@LASTMODTIME');
--SELECT * FROM SMSLogs WHERE USERKEY=@USERKEY AND TELEPHONE=@TELEPHONE AND LASTMODTIME>DATE(@LASTMODTIME);


--UPDATE SMSLogs SET COUNTOFSENT = COUNTOFSENT+1,LASTMODTIME = @LASTMODTIME WHERE CREATETIME=@CREATETIME  AND USERKEY = @USERKEY;

