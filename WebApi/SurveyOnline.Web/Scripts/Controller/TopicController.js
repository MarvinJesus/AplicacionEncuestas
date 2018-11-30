var TopicController = function(){

    var displayTopicDetails = function(){
        var parent = $(this).parent();
        window.location.href = $(parent).attr("data-topic-link");
    };
    var init = function(topicContainer){
        $(topicContainer).on("click",".overlay",displayTopicDetails);
    };
    return{
        init:init
    }; 
}();