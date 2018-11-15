var SurveyService = function () {

    var registerSurvey = function (survey,fail) {
        var serealizedSurvey = JSON.stringify(survey);

        $.ajax({
            type: 'POST',
            url: '/Survey/RegisterSurvey',
            data: serealizedSurvey,
            contentType: 'application/json',
        })
        .done(function (data) {
            window.location.href = data;
        })
        .fail(fail);

    }

    return {
        registerSurvey: registerSurvey
    };
}();