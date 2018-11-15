var SurveyController = function (surveyHelper, surveyService) {

	/*
	 * REGISTER SURVEY
	 */
	var registerSurvey = function () {
		var questionsHtml = $("#question-container").children();
		var survey = new Survey();
		var questions = [];

		for (var i = 0; i < questionsHtml.length; i++) {
			questions.push(surveyHelper.extractQuestion(questionsHtml[i]));
		}

		survey.init($("#txtTitle").val(),$("#txtDescription").val(),
			$("#topic-id").val(), questions);

		surveyService.registerSurvey(survey,fail);
		
	};

	/*
	 * IT IS EXECUTE WHEN WE TRY TO REGISTER THE SURVEY
	 * IF THE REQUEST IS ERROR CALL THIS METHOD
	 */
	var fail = function () {
		alert("Upps! something fail");
	};

	/*
	 * EDIT THE QUESTION
	 */
	var editQuestion = function (e) {

		var question = surveyHelper.extractQuestion($(this).parents()[1]);
		$("#question-title").val(question.Description);
		$("#question-type").val(question.Type);
		$("#answer-container").attr("data-question-number", question.Number);

		for (var i = 0; i < question.Answers.length; i++) {
			$("#answer-container").append(surveyHelper.answerHtml(question.Answers[i]));
		}

		showQuestionForm();
	};

	/*
	 * CONFIRMATION OF THE QUESTION TO BE ELIMINATED
	 */
	var deleteQuestion = function (e) {
		var questionElement = $(this).parents()[1];
		bootbox.confirm({
			message: "Esta seguro que quiere eliminar esta pregunta?",
			title: "Confirmación",
			buttons: {
				cancel: {
					label: "Cancelar",
					className: "btn-default"
				},
				confirm: {
					label: "Eliminar",
					className: "btn-danger"
				}
			},
			callback: function (result) {
				if (result) {
					removeQuestionItem(questionElement);
				}
			}
		});
	};

	/*
	 * REMOVE ITEM QUESTION FROM THE QUESTION_CONTAINER
	 */
	var removeQuestionItem = function (questionItem) {
		orderTheQuestionsPosition($(questionItem).children()[1].
			firstChild.innerText.replace(".", ""));

		$(questionItem).fadeOut("slow", function () {
			$(questionItem).remove();
		});
	};

	/*
	 * ORDER THE QUESTIONS POSITION FROM THE ITEM TO ELIMINATE AND RECEIVE THE POSITION NUMBER
	 */
	var orderTheQuestionsPosition = function (number) {
		var questions = $('#question-container').children();

		for (var i = parseInt(number); i < questions.length; i++) {
			$($(questions[i]).children()[1].firstChild).text(i + ".");
		}
	};

	/*
	 * EDIT ANSWER IN THE PROCESS OF CREATING A QUESTION
	 */
	var editAnswer = function (e) {
		$($(this).parents()[1]).prev()
			.removeAttr("disabled");
	};

	/*
	 * DELETE THE ANSWER IN THE PROCESS OF CREATING A QUESTION
	 */
	var deleteAnswer = function (e) {
		$($(this).parents()[2]).fadeOut("slow",function () {
			$(this).remove();
		});
	};

	/*
	 * DISABLE INPUT TO EDIT THE INFO
	 */
	var disableInput = function (e) {
		$(this).attr("disabled", "true");
	};


	/*
	 * ADD NEW ANSWER IN THE PROCESS OF CREATING A QUESTION
	 */
	var addAnswer = function () {
		var answer = new Answer();
		answer.init($("#input-answer").val());

		$("#answer-container").append(surveyHelper.answerHtmlForRegistration(answer));
	};


	/*
	 * ADD NEW QUESTION
	 */
	var addQuestion = function () {
		var question = new Question();
		var number = 0;

		if ($("#answer-container").attr("data-question-number") == "-1") {
			number = $("#question-container").children().length + 1;
		} else {
			number = $("#answer-container").attr("data-question-number");
		}

		question.init($("#question-type").val(), $("#question-title").val(), number);
		question.Answers = getAnswers();

		addQuestionToTheContainer(question);
	};

	/*
	 * GET THE ANSWERS FROM ANSWER_CONTAINER
	 */
	var getAnswers = function () {
		var answersInHtmlFormat = $("#answer-container").children();

		var answers = [];

		for (var i = 0; i < answersInHtmlFormat.length; i++) {
			answers.push(surveyHelper.extractAnswer(answersInHtmlFormat[i]));
		}

		return answers;
	};

	/*
	 * ADD QUESTION TO THE QUESTION_CONTAINER AND VALIDATES THE QUESTION POSITION
	 */
	var addQuestionToTheContainer = function (question) {

		var questions = $("#question-container").children();
		var container = $("#question-container");
		var template = surveyHelper.questionHtml(question);

		if (question.Number > questions.length) {
			$(container).append(template);
		} else {
			$(questions[question.Number - 1]).remove();
			if (question.Number == 1) {
				$(container).prepend(template);
			} else {
				$(template).insertAfter(questions[question.Number - 2]);
			}
		}
	};

	/*
	 * SHOW THE QUESTION FORM
	 */
	var showQuestionForm = function(){
		$("#survey-form").addClass("hide-container");
		$("#question-form").removeClass("hide-container");
	};

	/*
	 * HIDE THE QUESTION FORM
	 */
	var hideQuestionForm = function () {
		$("#question-form").addClass("hide-container");
		$("#survey-form").removeClass("hide-container");
	};

	/*
	 *CLEAR THE QUESTION FORM 
	 */
	var clearQuestionForm = function () {
		$(".question-error-message").hide();
		$("#question-title").val("");
		$("#question-type").val("0");
		$("#answer-container").empty();
		$("#answer-container").attr("data-question-number", "-1");
	};

	var init = function (questionContainer, answerContainer) {
		$(questionContainer).on("click", ".btn-edit", editQuestion);
		$(questionContainer).on("click", ".btn-delete", deleteQuestion);
		$(answerContainer).on("click", ".btn-edit", editAnswer);
		$(answerContainer).on("focusout", ".input-answer", disableInput);
		$(answerContainer).on("click", ".btn-delete", deleteAnswer);
	};
	return {
		init: init,
		addAnswer: addAnswer,
		addQuestion: addQuestion,
		registerSurvey: registerSurvey,
		showQuestionForm: showQuestionForm,
		hideQuestionForm: hideQuestionForm,
		clearQuestionForm: clearQuestionForm
	};
}(SurveyHelper, SurveyService);