var SurveyHelper = function () {

	/*
	 * RECEIVE A HTML AND RETURN AN ANSWER OBJECT
	 */
	var extractAnswer = function (answerContainer) {
		var answer = new Answer();
		
		answer.init2($(answerContainer.firstElementChild).attr("data-answer-id"),
			answerContainer.firstElementChild.value);
		return answer;
	};

	/*
	 * RECEIVE ANSWERS HTML AND RETURN AN ANSWERS OBJECT
	 */
	var extractAnswers = function (answersContainer) {
		var answers = [];

		for (var i = 0; i < answersContainer.length; i++) {
			answers.push(extractAnswer(answersContainer[i]));
		}

		return answers;
	};

	/*
	 * RECEIVE A HTML FROM QUESTION ITEM CONTAINER AND RETURN AN ANSWER OBJECT
	 * BUT IN THIS CASE THE QUESTION IS GOING TO BE EDITED
	 */
	var extractAnswerFromQuestionItem = function (answerContainer) {
		var answer = new Answer();
		answer.init2($(answerContainer).attr("data-answer-id"),answerContainer.firstElementChild.innerText);
		return answer;
	};

	/*
	 * RECEIVE AN ANSWERS HTML FROM QUESTION ITEM CONTAINER AND RETURN AN ANSWER OBJECT
	 * BUT IN THIS CASE THE QUESTION IS GOING TO BE EDITED
	 */
	var extractAnswersFromQuestionItem = function (answersContainer) {
		var answers = [];

		for (var i = 0; i < answersContainer.length; i++) {
			answers.push(extractAnswerFromQuestionItem(answersContainer[i]));
		}

		return answers;
	};

	/*
	 * RECEIVE A HTML AND RETURN AN QUESTION OBJECT
	 */
	var extractQuestion = function (questionContainer) {
		var question = new Question();

		question.init3($(questionContainer).attr("data-question-id"), $(questionContainer).attr("data-question-type"),
			$(questionContainer).children()[1].innerText.replace($(questionContainer).children()[1].firstChild.innerText, ""),
			$(questionContainer).children()[1].firstChild.innerText.replace(".", ""),
			extractAnswersFromQuestionItem($(questionContainer).children("#question-answers").children()));

		return question;
	};

	/*
	 * RECEIVE AN ANSWER OBJECT AND RETURN A HTML
	 */
	var getAnswerHtmlForCreating = function (answer) {
		return answerTemplateForCreation(answer);
	};

	/*
	 * RECEIVE AN QUESTION OBJECT AND RETURN A HTML
	 */
	var getQuestionHtml = function (question) {
		return questiontemplate(question);
	};

	/*
	 * RETURN A QUESTION TEMPLATE
	 */
	var questiontemplate = function (question) {

		var questionAnswers = "";

		for (var i = 0; i < question.Answers.length; i++) {
			questionAnswers = questionAnswers.concat(answerTemplate(question.Answers[i],question.Number, question.Type));
		}
		return `<div class="question-item" data-question-id="${question.Id}" data-question-type="${question.Type}">
							<div class="btn-group btn-edit-delete">
								<button type="button" class="btn btn-default btn-sm btn-edit">
									<i class="fas fa-pencil-alt"></i>
								</button>
								<button type="button" class="btn btn-default btn-sm btn-delete">
									<i class="fas fa-times"></i>
								</button>
							</div>
							<h4 class="question-title"><span class="answer-number">${question.Number}.</span>${question.Description}</h4>
							<div id="question-answers">${questionAnswers}</div>
						</div>`;
	};

	/*
	 * RETURN AN QUESTION TEMPLATE
	 */
	var answerTemplateForCreation = function (answer) {
		return `<div class="input-group">
							<input type="text" class="form-control input-answer" data-answer-id="${answer.Id}" aria-describedby="basic-addon2" disabled="true" value="${answer.Description}">
							<span class="input-group-addon" id="basic-addon2">
								<div class=input-group-btn btn-edit-delete">
									<button type="button" class="btn btn-default btn-sm btn-edit">
										<i class="fas fa-pencil-alt"></i>
									</button>
									<button type="button" class="btn btn-default btn-sm btn-delete">
										<i class="fas fa-times"></i>
									</button>
								</div>
							</span>
						</div>`;
	};

	/*
	 * RETURN AN ANSWER TEMPLATE IN THE PROCESS OF CREATING A QUESTION
	 */
	var answerTemplate = function (answer, number, type) {
		var answerType = "radio";

		if (type == 1) answerType = "checkbox";

		return `<div class="${answerType} answer" data-answer-id="${answer.Id}">
								<label><input type="${answerType}" name="${number}">${answer.Description}</label>
							</div>`;
	};

	return {
		extractAnswer: extractAnswer,
		extractAnswers: extractAnswers,
		extractAnswersFromQuestionItem:extractAnswersFromQuestionItem,
		extractAnswerFromQuestionItem:extractAnswerFromQuestionItem,
		extractQuestion: extractQuestion,
		answerHtml: getAnswerHtmlForCreating,
		questionHtml: getQuestionHtml,
		answerHtmlForRegistration: getAnswerHtmlForCreating
	};
}();