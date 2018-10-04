CREATE DATABASE DB_ENCUESTA
GO

USE DB_ENCUESTA
GO



/************ TABLE USER ************/
/***********************************/
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE 
TABLE_NAME = 'TBL_PROFILE'))
BEGIN

  CREATE TABLE TBL_PROFILE(
    PROFILE_ID			INT NOT NULL IDENTITY(1,1),
	IDENTIFICATION		VARCHAR(15) NOT NULL UNIQUE,
	NAME				VARCHAR(100) NOT NULL,
	EMAIL				VARCHAR(100) NOT NULL UNIQUE,
	PASSWORD			VARBINARY(MAX),
	SALT				VARBINARY(MAX),
	IMG_URL				VARCHAR(2083),
	CREATE_DATE			DATETIME NOT NULL,
	ACTIVE				BIT NOT NULL,			
    PRIMARY KEY(PROFILE_ID)
  )

END;



/************ TABLE TOPIC ************/
/************************************/
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE 
TABLE_NAME = 'TBL_TOPIC'))
BEGIN

  CREATE TABLE TBL_TOPIC(
    TOPIC_ID			INT NOT NULL IDENTITY(1,1),
	TITLE				VARCHAR(100) NOT NULL,
	TOPIC_DESCRIPTION	VARCHAR(500) NOT NULL,
	IMG_URL				VARCHAR(2083),
	CREATE_DATE			DATE NOT NULL,
	USER_ID				INT NOT NULL,
    PRIMARY KEY(TOPIC_ID)
  )

END;



/************ TABLE EXCEPTION ************/
/****************************************/
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE 
TABLE_NAME = 'TBL_EXCEPTION'))
BEGIN

  CREATE TABLE TBL_EXCEPTION(
    EXCEPTION_ID		INT NOT NULL IDENTITY(1,1),
	CODE				INT NOT NULL UNIQUE,
	MESSAGE				VARCHAR(500) NOT NULL,
    PRIMARY KEY(EXCEPTION_ID)
  )

END;



/************ TABLE UNCONTROLLED EXCEPTION ************/
/*****************************************************/
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE 
TABLE_NAME = 'TBL_UNCONTROLLED_EXCEPTION'))
BEGIN

  CREATE TABLE TBL_UNCONTROLLED_EXCEPTION(
    UNCONTROLLED_EXCEPTION_ID	INT NOT NULL IDENTITY(1,1),
	MESSAGE						VARCHAR(500) NOT NULL,
	EXC_HOUR					TIME NOT NULL,
	EXC_DAY						DATE NOT NULL,
	STACK_TRACE					VARCHAR(MAX),
    PRIMARY KEY(UNCONTROLLED_EXCEPTION_ID)
  )
END;

/********************************************************************/
/**************************FOREIGN KEYS*****************************/
/******************************************************************/

ALTER TABLE TBL_TOPIC ADD FOREIGN KEY (USER_ID)
REFERENCES TBL_PROFILE(PROFILE_ID) ON UPDATE CASCADE


/***************************************************************************/
/**************************REGISTROS REQUERIDO*****************************/
/*************************************************************************/
INSERT INTO TBL_EXCEPTION VALUES(1,'Upps algo fallo');
INSERT INTO TBL_EXCEPTION VALUES(2,'Por favor completar los campos requeridos');
INSERT INTO TBL_EXCEPTION VALUES(3,'Por favor ingrese un correo o numero de cedula diferente, ya se encuentran registrado');
INSERT INTO TBL_EXCEPTION VALUES(4,'El usuario no existe');