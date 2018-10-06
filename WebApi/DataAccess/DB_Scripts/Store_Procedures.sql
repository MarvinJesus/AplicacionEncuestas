/**************************************************************************************************
								STORE PROCEDURES FOR EXCEPTION
**************************************************************************************************/

/********** RETRIVE ALL EXCEPTIONS ************/
/*********************************************/
CREATE PROCEDURE RET_ALL_EXCEPTIONS

AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_EXCEPTION;
END
GO



/********** REGISTER EXCEPTION **********/
/***************************************/
CREATE PROCEDURE CRE_EXCEPTION
	@P_MESSAGE		VARCHAR(500),
	@P_EXC_HOUR		TIME,
	@P_EXC_DAY		DATE,
	@P_STACK_TRACE	VARCHAR(MAX)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO TBL_UNCONTROLLED_EXCEPTION VALUES(@P_MESSAGE,@P_EXC_HOUR,@P_EXC_DAY,@P_STACK_TRACE);
	END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR TOPIC
**************************************************************************************************/

/********** RETRIVE TOPIC BY ID ************/
/******************************************/
CREATE PROCEDURE RET_TOPIC
	@P_TOPIC_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_TOPIC  WHERE @P_TOPIC_ID = TOPIC_ID;
END
GO



/********** RETRIVE TOPIC BY USER ID ************/
/***********************************************/
CREATE PROCEDURE RET_TOPIC_BY_USER_ID
	@P_USER_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_TOPIC  WHERE @P_USER_ID = USER_ID;
END
GO



/********** RETRIVE ALL TOPICS ************/
/*****************************************/
CREATE PROCEDURE RET_ALL_TOPICS

AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_TOPIC;
END
GO



/********** REGISTER TOPIC **********/
/***********************************/
CREATE PROCEDURE CRE_TOPIC
	@P_TITLE				VARCHAR(100),
	@P_TOPIC_DESCRIPTION	VARCHAR(500),
	@P_IMG_URL				VARCHAR(2083),
	@P_USER_ID				INT

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @P_CREATE_DATE DATE = GETDATE();
	INSERT INTO TBL_TOPIC VALUES (@P_TITLE,@P_TOPIC_DESCRIPTION,
						@P_IMG_URL,@P_CREATE_DATE,@P_USER_ID);

	DECLARE @P_TOPIC_ID INT = (SELECT IDENT_CURRENT( 'TBL_TOPIC' ));
	EXEC dbo.RET_TOPIC @P_TOPIC_ID;
END
GO



/********** UPDATE TOPIC **********/
/***********************************/
CREATE PROCEDURE UPD_TOPIC
	@P_TOPIC_ID				INT,
	@P_TITLE				VARCHAR(100),
	@P_TOPIC_DESCRIPTION	VARCHAR(500),
	@P_IMG_URL				VARCHAR(2083)

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @P_CREATE_DATE DATE = GETDATE();

	UPDATE TBL_TOPIC
	SET TITLE = @P_TITLE , TOPIC_DESCRIPTION = @P_TOPIC_DESCRIPTION , IMG_URL = @P_IMG_URL,
			CREATE_DATE = @P_CREATE_DATE
	WHERE TOPIC_ID = @P_TOPIC_ID

END
GO



/************ DELTE TOPIC ************/
/************************************/
CREATE PROCEDURE DEL_TOPIC
	@P_TOPIC_ID				INT

AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM TBL_TOPIC
	WHERE TOPIC_ID = @P_TOPIC_ID

END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR PROFILE
**************************************************************************************************/

/********** REGISTER PROFILE **********/
/*************************************/
CREATE PROCEDURE CRE_PROFILE
	@P_IDENTIFICATION		VARCHAR(15),
	@P_NAME					VARCHAR(100),
	@P_EMAIL				VARCHAR(100),
	@P_PASSWORD				VARBINARY(MAX),
	@P_SALT					VARBINARY(MAX),
	@P_IMG_URL				VARCHAR(2083)

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @P_CREATE_DATE DATE = GETDATE();
	INSERT INTO TBL_PROFILE VALUES (@P_IDENTIFICATION,@P_NAME,@P_EMAIL,
						@P_PASSWORD,@P_SALT,@P_IMG_URL,@P_CREATE_DATE,1);

	DECLARE @P_PROFILE_ID INT = (SELECT IDENT_CURRENT( 'TBL_PROFILE' ));
	EXEC dbo.RET_PROFILE @P_PROFILE_ID;
END
GO



/********** RETRIVE PROFILE BY ID **********/
/******************************************/
CREATE PROCEDURE RET_PROFILE 
	@P_PROFILE_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_PROFILE  WHERE @P_PROFILE_ID = PROFILE_ID;
END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR ANSWER
**************************************************************************************************/

/********** RETRIVE ANSWER BY ID ************/
/******************************************/
CREATE PROCEDURE RET_ANSWER
	@P_ANSWER_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_ANSWER  WHERE @P_ANSWER_ID = ANSWER_ID;
END
GO



/********** RETRIVE ANSWERS BY QUESTION ID ************/
/***********************************************/
CREATE PROCEDURE RET_ANSWERS_BY_QUESTION_ID
	@P_QUESTION_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_ANSWER  WHERE @P_QUESTION_ID = QUESTION_ID;
END
GO



/********** RETRIVE ALL ANSWERS ************/
/*****************************************/
CREATE PROCEDURE RET_ALL_ANSWERS

AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_ANSWER;
END
GO



/********** REGISTER ANSWER **********/
/***********************************/
CREATE PROCEDURE CRE_ANSWER
	@P_ANSWER_DESCRIPTION	VARCHAR(500),
	@P_QUESTION_ID			INT

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TBL_ANSWER VALUES (@P_ANSWER_DESCRIPTION, @P_QUESTION_ID);

	DECLARE @P_ANSWER_ID INT = (SELECT IDENT_CURRENT( 'TBL_ANSWER' ));
	EXEC dbo.RET_ANSWER @P_ANSWER_ID;
END
GO



/********** UPDATE ANSWER **********/
/***********************************/
CREATE PROCEDURE UPD_ANSWER
	@P_ANSWER_ID			INT,
	@P_ANSWER_DESCRIPTION	VARCHAR(500)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE TBL_ANSWER
	SET ANSWER_DESCRIPTION = @P_ANSWER_DESCRIPTION
	WHERE ANSWER_ID = @P_ANSWER_ID

END
GO



/************ DELETE ANSWER ************/
/**************************************/
CREATE PROCEDURE DEL_ANSWER
	@P_ANSWER_ID		INT

AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM TBL_ANSWER
	WHERE ANSWER_ID = @P_ANSWER_ID

END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR QUESTION
**************************************************************************************************/

/********** RETRIVE QUESTION BY ID ************/
/*********************************************/
CREATE PROCEDURE RET_QUESTION
	@P_QUESTION_ID		INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_QUESTION  WHERE @P_QUESTION_ID = QUESTION_ID;
END
GO



/********** RETRIVE ALL QUESTION ************/
/*****************************************/
CREATE PROCEDURE RET_ALL_QUESTIONS

AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_QUESTION;
END
GO



/********** REGISTER QUESTION **********/
/**************************************/
CREATE PROCEDURE CRE_QUESTION
	@P_QUESTION_DESCRIPTION	VARCHAR(500),
	@P_TOPIC_ID				INT

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TBL_QUESTION VALUES (@P_QUESTION_DESCRIPTION, @P_TOPIC_ID);

	DECLARE @P_QUESTION_ID INT = (SELECT IDENT_CURRENT( 'TBL_QUESTION' ));
	EXEC dbo.RET_QUESTION @P_QUESTION_ID;
END
GO