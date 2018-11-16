"use strict";

var userName = null;
var tweetTemplate = "<li><b><a class='user' href='#'>@%s</a></b><br/>%s</li>";

function refresh(tweets, target) {
    let targetElement = $(target);
    targetElement.empty();
    tweets.forEach(tweet => {
        let childHtml = render(tweetTemplate, [tweet.userName, tweet.message]);
        
        $(childHtml).prependTo(targetElement);
    });
}

function render(template, args) {
    for(let i = 0; i < args.length; i++) {
        template = template.replace("%s", args[i]);
    }
    return template;
}

$(function(){

   $("#loginButton").click(function(){
        userName = $("#userName").val();
        $("#user").text("User: " + userName);
        $("#loginContainer").hide();
        $("#postButton").removeAttr('disabled');
        connection.invoke("Subscribe", userName);
   });

   $("#postButton").click(function(){
       var message = $("#message").val();
       $.post("/twitter/tweet", {user: userName, message: message}, function(data){
           $("#message").val("");
       })
   });

   $("#refreshTimeline").click(function(){
      $.get("/twitter/usertweets", {user:userName}, function(data){
          refresh(data, "#timeline")
      });
   });

   $("#refreshAll").click(function(){
      $.get("/twitter/alltweets/", function(data){
          refresh(data, "#firehose");
      });
   });

});   

var connection = new signalR.HubConnectionBuilder()
        .withUrl("/twitter-hub").build();

connection.on("tweet", function (tweet) {

    let childElement = $(render(tweetTemplate, [tweet.user, tweet.message]));
    
    childElement.prependTo($("#firehose"));
    // Add to notifications
    if (tweet.isMention) {
        childElement.prependTo($("#notifications"));
    }
    // We follow this user so add to timeline
    if (tweet.isTimeline || tweet.user === userName) {
        childElement.prependTo("#timeline");
    }
        
        
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});