# Memstate tutorial

This is material for a 2-day course. If you are doing a 3 hour workshop jump in at task 3, examine what's already there and run the unit tests.

## Prerequisites
* Development environment for .NET Core such as VS2017 on Windows, VS for mac or VS Code on any platform
* Docker for some of the later modules
* Experienced C# developer with an open mind


## Task 0 : Clone the repository
Clone this repository, or even better fork it to your own GH account and clone from there. If you don't have an account create one now!
```
git clone  https://github.com/DevrexLabs/memstate-tutorial
```

Each step of the tutorial is tagged so that you can easily jump around and look at the code. To start working on a task checkout a new branch from the tag associated with that task. 

```
git checkout -b dev-1 task-1
```

This will create and a switch to a branch named dev-1. Go ahead and do this now then you are all set for task 1.


## Task 1 : Creating the domain model
Time required: 40-60 minutes

In the Twitter.Core library, add a `Tweet` class, a `User` class and a `Twitter` class.

The Twitter class should have 
* an ordered collection of tweets
* a collection of users keyed by id (string)

and methods to:
* post a tweet: `int PostTweet(string user, string message)`
* retrieve a range of users tweets
* retrieve a range of all tweets
* Follow a user: `void Follow(string follower, string followee)`

Keep it simple and just do the bare minimum at this point, you will add more state and behavior later. Try to use the ubiquitous language of the domain. Some additional rules:
 
* The User entity is created when posting the first tweet of the account.
* The user entity should have an ordered collection of tweets
* The user entity should have ordered collections of followers and followees
* Prefer passing basic arguments instead of entities, example: `PostTweet(string user, string message, Datetime when)` instead of `PostTweet(Tweet tweet)`

## Task 2 : Testing the domain model
Time required: 10-20 minutes

Add a unit test project using your preferred unit testing library.
Write some of the tests below, you might need to add behavior to your domain model to make them pass. Don't get carried away with details, this is just to demonstrate how you would do TDD if you're into that kind of thing.

 * Tweets are assigned unique int ids in an ever increasing sequence
 * Tweets are added to the users list of tweets
 * Tweets are added to the all tweets collection
 

## Task 3 : Adding command and queries
Time required: 10-20 minutes

Add the `Memstate.All` package to the `Twitter.Core` project. Create command and query classes:
* `class PostTweet : Command<Twitter, long>` (returns the assigned id
* `class ReadTimeLine : Query<Twitter, Tweet[]>` (retrieve a range of tweets for a specific user)
* `class AllTweets : Query<Twitter, Tweet[]>`
* `class Follow : Command<Twitter>`

override the `Execute(Twitter state)` method adding the correct behavior.

## Task 4 : Testing commands and queries
Time required: 15-30 minutes

Add some tests for the commands and queries by creating an instance of Twitter, executing the command/query then verifying return values and possible state changes.


## Task 5: Integration tests
Time required: 15-30 minutes

Repeat the tests from task 4 but running through the Memstate engine. The test setup should:
```
var cfg = Config.Current;cfg.UseInMemoryFileSystem();
var settings = cfg.GetSettings<EngineSettings>();
settings.WithRandomSuffixAppendedToStreamName();
var engine = Engine.Start<Twitter>();
```

and then in your test methods
`engine.Execute(command or query)`

## Task 6: Implementing ASP.NET Controllers
Time required: 10-20 minutes

In the web project, add a reference to the domain model project and the `Memstate.All` nuget package.
Add a controller named `TwitterController` 
In the constructor create a client using `Client.For<Twitter>()` or if you have the skills configure it for dependency injection as a singleton.

Create action methods for the following operations:
* `GET UserTweets(userName, skip, take)`
* `GET AllTweets(skip, take)`
* `POST Tweet(userName, message)`
* `POST Follow(userName, followee)`

The UI is a single page and already hooked up. It uses jquery ajax to talk to the backend, examine the `Twitter.cshtml` view for details. Test manually that you can tweet, follow, refresh to see all.

## Task 7: Reactive
Time: 
Bugs: If you upodat
Define the events `Tweeted` and `Followed`. They should inherit from the `Event` base class. Publish events from within the commands `Execute()` by calling `RaiseEvent()` on the `Command` base class. 
Note: The tests from Task 4 might fail when you raise events. This bug is fixed with memstate version 0.6.6

(Optional) Add tests to verify that the correct events are published. The events are passed as an argument to the `Engine.CommandExecuted` event.

To subscribe to events use `Client.Subscribe<TEvent>(Action<TEvent> handler)` or `Client.Subscribe<TEvent>(Action<TEvent> handler, IEventFilter filter)`

This tutorial on SignalR should 
https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr

Add a SignalR hub that keeps track of connected users, subscribes to events and relays them to clients.

## Task 8: Running a standalone server
Run a standalone server by adding a .NET Core console application named `Twitter.Host`, add the `Memstate.Host` nuget package and call the `Host.Run<Twitter>()` from your main method.
Configure the web application to connect to the external server.
`Client.For<Twitter>()` should now return a `RemoteClient<Twitter>` instead of a `LocalClient<Twitter`.

## Task 9: Using Event Store
Install and launch eventstore or run it in a docker container.
Configure the server to use the eventstore storageprovider.
 
## Task 10: Create a docker image
Dockerize the web and/or host app created in task 8.
