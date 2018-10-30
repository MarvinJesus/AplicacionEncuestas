/**************************************************************************************************
								STORE PROCEDURES FOR USER
**************************************************************************************************/

/************** REGISTER USER **************/
/******************************************/
CREATE PROCEDURE CRE_USER
	@P_PASSWORD	VARBINARY(MAX),
	@P_SALT		VARBINARY(MAX),
	@P_USERNAME VARCHAR(100)

AS
	BEGIN
		SET NOCOUNT ON;
		DECLARE @USER_ID UNIQUEIDENTIFIER = NEWID()
		INSERT INTO TBL_USER VALUES(@USER_ID,@P_USERNAME,@P_PASSWORD,@P_SALT)
		EXEC DBO.CRE_ROLE_BY_USER 2,@USER_ID
		EXEC DBO.RET_USER_BY_ID @USER_ID  
	END
GO



/************** RETIRVE USER ***************/
/*******************************************/
CREATE PROCEDURE RET_USER_BY_ID
	@P_USER_ID	UNIQUEIDENTIFIER

AS
	BEGIN
		SELECT * FROM TBL_USER WHERE @P_USER_ID = USER_ID
	END
GO



/************** RETIRVE USER BY USERNAME ***************/
/******************************************************/
CREATE PROCEDURE RET_USER_BY_USERNAME
	@P_USERNAME VARCHAR(100)
AS
	BEGIN
		SELECT * FROM TBL_USER WHERE USERNAME = @P_USERNAME
	END
GO




/**************************************************************************************************
								STORE PROCEDURES FOR USER CLAIMS
**************************************************************************************************/

/************ CREATE USER CLAIMS **************/
/*********************************************/
CREATE PROCEDURE CRE_USER_CLAIMS
	@P_USER_ID UNIQUEIDENTIFIER,
	@P_CLAIM_TYPE VARCHAR(30),
	@P_CLAIM_VALUE VARCHAR(30)
AS
	BEGIN
		INSERT INTO TBL_USER_CLAIM VALUES(@P_USER_ID, @P_CLAIM_VALUE,@P_CLAIM_TYPE)
		DECLARE @P_ID  INT = (SELECT IDENT_CURRENT('TBL_USER_CLAIM'))
		EXEC DBO.RET_USER_CLAIMS_BY_ID @P_ID
	END
GO



/************ RETRIVE USER CLAIMS BY ID **************/
/****************************************************/
CREATE PROCEDURE RET_USER_CLAIMS_BY_ID
	@P_ID INT
AS
	BEGIN
		SELECT * FROM TBL_USER_CLAIM WHERE ID = @P_ID
	END
GO



/************ RETRIVE USER CLAIMS **************/
/**********************************************/
CREATE PROCEDURE RET_USER_CLAIMS
	@P_USER_ID UNIQUEIDENTIFIER
AS
	BEGIN
		SELECT * FROM TBL_USER_CLAIM WHERE USER_ID = @P_USER_ID
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
	@P_IMG_URL				VARCHAR(2083),
	@P_USER_ID				UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CREATE_DATE DATE = GETDATE(),@PROFILE_ID UNIQUEIDENTIFIER = NEWID()
	
	INSERT INTO TBL_PROFILE VALUES (@PROFILE_ID,@P_IDENTIFICATION,@P_NAME,@P_EMAIL,
						@P_IMG_URL,@CREATE_DATE,1,@P_USER_ID);

	EXEC dbo.RET_PROFILE_BY_USER_ID @P_USER_ID;
END
GO



/********** RETRIVE PROFILE BY USER ID **********/
/***********************************************/
CREATE PROCEDURE RET_PROFILE_BY_USER_ID 
	@P_USER_ID			UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_PROFILE  WHERE @P_USER_ID = USER_ID;
END
GO



/********** RETRIVE PROFILE BY PROFILE ID **********/
/***********************************************/
CREATE PROCEDURE RET_PROFILE 
	@P_PROFILE_ID			UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_PROFILE  WHERE @P_PROFILE_ID = PROFILE_ID
END
GO



/********** UPDATE PROFILE PICTURE **********/
/*******************************************/
CREATE PROCEDURE UPD_PROFILE_PICTURE
	@P_IMG_URL VARCHAR(2083),
	@P_USER_ID UNIQUEIDENTIFIER
AS
	BEGIN
		UPDATE TBL_PROFILE
		SET IMG_URL = @P_IMG_URL
		WHERE USER_ID = @P_USER_ID	  
	END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR ROLE BY PROFILE
**************************************************************************************************/

/********** CREATE ROLE BY TOPIC ************/
/*********************************************/
CREATE PROCEDURE CRE_ROLE_BY_PROFILE

	@P_ROLE_ID		INT,
	@P_PROFILE_ID	INT

AS
	BEGIN
		
		SET NOCOUNT ON;

		INSERT INTO TBL_ROLE_BY_USER VALUES(@P_ROLE_ID,@P_PROFILE_ID)
	END
GO



/********** REGISTER ROLE BY USER **********/
/******************************************/
CREATE PROCEDURE CRE_ROLE_BY_USER
	@P_ROLE_ID	INT,
	@P_USER_ID	UNIQUEIDENTIFIER
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO TBL_ROLE_BY_USER VALUES(@P_ROLE_ID,@P_USER_ID)
	END
GO



/********** RETRIEVE ROLE BY USER **********/
/******************************************/
CREATE PROCEDURE RET_ROLE_BY_USER
	@P_USER_ID	UNIQUEIDENTIFIER
AS
	BEGIN
		SET NOCOUNT ON;
		SELECT ROL.ROLE_ID, ROL.ROLE_NAME
		FROM TBL_ROLE_BY_USER AS USER_ROL
		INNER JOIN TBL_ROLE AS ROL ON
		ROL.ROLE_ID = USER_ROL.ROLE_ID
		INNER JOIN TBL_USER AS TUSER ON
		TUSER.USER_ID = USER_ROL.USER_ID
		WHERE USER_ROL.USER_ID = @P_USER_ID
	END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR TOPIC
**************************************************************************************************/

/********** RETRIVE TOPIC BY ID ************/
/******************************************/
CREATE PROCEDURE RET_TOPIC
	@P_TOPIC_ID			UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_TOPIC  WHERE @P_TOPIC_ID = TOPIC_ID;
END
GO



/********** RETRIVE TOPIC BY USER ID ************/
/***********************************************/
CREATE PROCEDURE RET_TOPIC_BY_USER_ID
	@P_USER_ID			UNIQUEIDENTIFIER
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
	@P_USER_ID				UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @P_CREATE_DATE DATE = GETDATE(), @P_TOPIC_ID UNIQUEIDENTIFIER = NEWID();
	INSERT INTO TBL_TOPIC VALUES (@P_TOPIC_ID,@P_TITLE,@P_TOPIC_DESCRIPTION,
						@P_IMG_URL,@P_CREATE_DATE,@P_USER_ID);

	EXEC dbo.RET_TOPIC @P_TOPIC_ID;
END
GO



/********** SEARCH TOPIC **********/
/***********************************/
CREATE PROCEDURE SEARCH_TOPIC
	@P_SEARCH VARCHAR(100)
AS
	BEGIN
		SELECT *
		FROM TBL_TOPIC AS TOPIC
		WHERE TOPIC.TITLE LIKE '%' + @P_SEARCH +'%'
	END
GO



/********** UPDATE TOPIC **********/
/***********************************/
CREATE PROCEDURE UPD_TOPIC
	@P_TOPIC_ID				UNIQUEIDENTIFIER,
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



/************ DELETE TOPIC ************/
/************************************/
CREATE PROCEDURE DEL_TOPIC
	@P_TOPIC_ID				UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM TBL_TOPIC
	WHERE TOPIC_ID = @P_TOPIC_ID

END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR CATEGORY
**************************************************************************************************/

/********** RETRIEVE CATEGORIES **********/
/****************************************/
CREATE PROCEDURE RET_CATEGORIES
AS
	BEGIN
		SELECT * FROM TBL_CATEGORY
	END
GO



/********** RETRIEVE CATEGORIES BY TOPIC**********/
/****************************************/
CREATE PROCEDURE RET_CATEGORIES_BY_TOPIC
	@P_TOPIC_ID	UNIQUEIDENTIFIER

AS
	BEGIN
		SELECT CATEGORY.CATEGORY_ID, CATEGORY.CATEGORY_NAME 
		FROM TBL_CATEGORY_BY_TOPIC AS CAT_BY_TOPIC
		INNER JOIN TBL_CATEGORY AS CATEGORY ON
		CAT_BY_TOPIC.CATEGORY_ID = CATEGORY.CATEGORY_ID
		INNER JOIN TBL_TOPIC AS TOPIC ON
		CAT_BY_TOPIC.TOPIC_ID = TOPIC.TOPIC_ID
		WHERE CAT_BY_TOPIC.TOPIC_ID = @P_TOPIC_ID
	END
GO



/********** CREATE TOPICS CATEGORIES**********/
/****************************************/
CREATE PROCEDURE CRE_TOPICS_CATEGORIES
	@P_TOPIC_ID		UNIQUEIDENTIFIER,
	@P_CATEGORY_ID	INT

AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO TBL_CATEGORY_BY_TOPIC VALUES(@P_CATEGORY_ID,@P_TOPIC_ID)
		SELECT * FROM TBL_CATEGORY WHERE CATEGORY_ID = @P_CATEGORY_ID
	END
GO



/********** DELETE TOPICS CATEGORIES**********/
/****************************************/
CREATE PROCEDURE DEL_TOPICS_CATEGORIES
	@P_TOPIC_ID		UNIQUEIDENTIFIER

AS
	BEGIN
		SET NOCOUNT ON
		DELETE 
		FROM TBL_CATEGORY_BY_TOPIC 
		WHERE TOPIC_ID = @P_TOPIC_ID
	END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR SURVEY
**************************************************************************************************/

/********** RETRIVE SURVEY BY ID ************/
/******************************************/
CREATE PROCEDURE RET_SURVEY
	@P_SURVEY_ID			UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_SURVEY  WHERE @P_SURVEY_ID = SURVEY_ID;
END
GO



/********** RETRIVE SURVEY BY TOPIC ID ************/
/***********************************************/
CREATE PROCEDURE RET_SURVEY_BY_TOPIC_ID
	@P_TOPIC_ID			UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_SURVEY  WHERE @P_TOPIC_ID = TOPIC_ID;
END
GO



/********** RETRIVE ALL SURVEYS ************/
/*****************************************/
CREATE PROCEDURE RET_ALL_SURVEYS

AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_SURVEY;
END
GO



/********** REGISTER SURVEY **********/
/************************************/
CREATE PROCEDURE CRE_SURVEY
	@P_TITLE				VARCHAR(100),
	@P_SURVEY_DESCRIPTION	VARCHAR(500),
	@P_IMG_URL				VARCHAR(2083),
	@P_TOPIC_ID			UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @P_CREATE_DATE DATE = GETDATE(), @P_SURVEY_ID UNIQUEIDENTIFIER = NEWID();
	INSERT INTO TBL_TOPIC VALUES (@P_SURVEY_ID,@P_TITLE,@P_SURVEY_DESCRIPTION,
						@P_IMG_URL,@P_CREATE_DATE,@P_TOPIC_ID);

	EXEC dbo.RET_SURVEY @P_SURVEY_ID;
END
GO



/*********** UPDATE SURVEY ***********/
/************************************/
CREATE PROCEDURE UPD_SURVEY
	@P_SURVEY_ID				UNIQUEIDENTIFIER,
	@P_TITLE				VARCHAR(100),
	@P_SURVEY_DESCRIPTION	VARCHAR(500),
	@P_IMG_URL				VARCHAR(2083)

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @P_CREATE_DATE DATE = GETDATE();

	UPDATE TBL_SURVEY
	SET TITLE = @P_TITLE , SURVEY_DESCRIPTION = @P_SURVEY_DESCRIPTION , IMG_URL = @P_IMG_URL,
			CREATE_DATE = @P_CREATE_DATE
	WHERE SURVEY_ID = @P_SURVEY_ID

END
GO



/************ DELETE SURVEY ************/
/*************************************/
CREATE PROCEDURE DEL_SURVEY
	@P_SURVEY_ID		UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM TBL_SURVEY
	WHERE SURVEY_ID = @P_SURVEY_ID

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
/*******************************************/
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
	@P_SURVEY_ID				INT

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TBL_QUESTION VALUES (@P_QUESTION_DESCRIPTION, @P_SURVEY_ID);

	DECLARE @P_QUESTION_ID INT = (SELECT IDENT_CURRENT( 'TBL_QUESTION' ));
	EXEC dbo.RET_QUESTION @P_QUESTION_ID;
END
GO



/********** RETRIVE QUESTIONS BY SURVEY ID ************/
/***********************************************/
CREATE PROCEDURE RET_QUESTIONS_BY_SURVEY_ID
	@P_SURVEY_ID			INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TBL_QUESTION  WHERE @P_SURVEY_ID = SURVEY_ID;
END
GO



/********** UPDATE QUESTION **********/
/***********************************/
CREATE PROCEDURE UPD_QUESTION
	@P_QUESTION_ID			INT,
	@P_QUESTION_DESCRIPTION	VARCHAR(500)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE TBL_QUESTION
	SET QUESTION_DESCRIPTION = @P_QUESTION_DESCRIPTION
	WHERE QUESTION_ID = @P_QUESTION_ID

END
GO



/************ DELETE QUESTION ************/
/**************************************/
CREATE PROCEDURE DEL_QUESTION
	@P_QUESTION_ID		INT

AS
	BEGIN
		SET NOCOUNT ON;

		DELETE
		FROM TBL_QUESTION
		WHERE QUESTION_ID = @P_QUESTION_ID

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
								STORE PROCEDURES FOR CATEGORY
**************************************************************************************************/

/********** RETRIVE CATEGORY BY ID ************/
/*********************************************/
CREATE PROCEDURE RET_CATEGORY

	@P_CATEGORY_ID	INT

AS
	BEGIN
		
		SET NOCOUNT ON;

		SELECT * FROM TBL_CATEGORY WHERE CATEGORY_ID = @P_CATEGORY_ID
	END
GO



/********** RETRIVE CATEGORY BY TOPIC ID ************/
/***************************************************/
CREATE PROCEDURE RET_CATEGORY_BY_TOPIC

	@P_TOPIC_ID	INT

AS
	BEGIN
		
		SET NOCOUNT ON;

		SELECT CATEGORY.CATEGORY_NAME
		FROM TBL_CATEGORY_BY_TOPIC AS CAT_BY_TOP

		INNER JOIN  TBL_CATEGORY AS CATEGORY ON
		CAT_BY_TOP.CATEGORY_ID = CATEGORY.CATEGORY_ID

		INNER JOIN TBL_TOPIC AS TOPIC ON
		CAT_BY_TOP.TOPIC_ID = TOPIC.TOPIC_ID

		WHERE CAT_BY_TOP.TOPIC_ID = @P_TOPIC_ID

	END
GO



/********** RETRIVE ALL CATEGORY ************/
/*******************************************/
CREATE PROCEDURE RET_ALL_CATEGORY
AS
	BEGIN
		SELECT * FROM TBL_CATEGORY
	END
GO



/********** DELETE CATEGORY ************/
/**************************************/
CREATE PROCEDURE DEL_CATEGORY
	@P_CATEGORY_ID INT
AS
	BEGIN
		DELETE FROM TBL_CATEGORY WHERE CATEGORY_ID = @P_CATEGORY_ID
	END
GO



/********** UPDATE CATEGORY ************/
/**************************************/
CREATE PROCEDURE UPD_CATEGORY
	@P_CATEGORY_ID		INT,
	@P_CATEGORY_NAME	VARCHAR(20)
AS
	BEGIN
		UPDATE TBL_CATEGORY
		SET CATEGORY_NAME = @P_CATEGORY_NAME
		WHERE CATEGORY_ID = @P_CATEGORY_ID
	END
GO



/********** CREATE CATEGORY ************/
/**************************************/
CREATE PROCEDURE CRE_CATEGORY
	@P_CATEGORY_NAME	VARCHAR(20)
AS
	BEGIN
		INSERT INTO TBL_CATEGORY VALUES(@P_CATEGORY_NAME)
	END
GO



/**************************************************************************************************
								STORE PROCEDURES FOR CATEGORY BY TOPIC
**************************************************************************************************/

/********** CREATE CATEGORY BY TOPIC ************/
/*********************************************/
CREATE PROCEDURE CRE_CATEGORY_BY_TOPIC

	@P_CATEGORY_ID	INT,
	@P_TOPIC_ID		INT

AS
	BEGIN
		
		SET NOCOUNT ON;

		INSERT INTO TBL_CATEGORY_BY_TOPIC VALUES(@P_CATEGORY_ID,@P_TOPIC_ID)
	END
GO



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