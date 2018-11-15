/*-----------------------------------------------------------
 *  SUVERY OBJECT
 ------------------------------------------------------------*/
var Survey = function () {
    var Id;
    var Title;
    var Description;
    var TopicId;
    var Questions = [];

    var init = function (title, description, topicId, questions) {
        this.Title = title;
        this.Description = description;
        this.TopicId = topicId;
        this.Questions = questions;
    };

    var init2 = function (id, title, description, topicId, questions) {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.TopicId = topicId;
        this.Questions = questions;
    };

    return {
        init: init,
        init2: init2
    };
};

/*-----------------------------------------------------------
 *  QUESITON OBJECT
 ------------------------------------------------------------*/
var Question = function () {
    var Id = -1;
    var Type;
    var Description;
    var Number;
    var Answers = [];
    var init = function (type, description, number) {
        this.Type = type;
        this.Description = description;
        this.Number = number;
        this.Id = -1;
    };
    var init2 = function (id, type, description, number) {
        this.Type = type;
        this.Description = description;
        this.Number = number;
        this.Id = id;
    };
    var init3 = function (id, type, description, number, answers) {
        this.Type = type;
        this.Description = description;
        this.Number = number;
        this.Id = id;
        this.Answers = answers;
    };
    return {
        init: init,
        init2: init2,
        init3: init3
    };
};

/*-----------------------------------------------------------
 *  ANSWER OBJECT
 ------------------------------------------------------------*/
var Answer = function () {
    var Id;
    var Description;
    var init = function (description) {
        this.Description = description;
        this.Id = -1;
    };
    var init2 = function (id, description) {
        this.Description = description;
        this.Id = id;
    };
    return {
        init: init,
        init2: init2
    };
};